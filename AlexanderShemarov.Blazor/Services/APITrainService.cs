using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;


namespace AlexanderShemarov.Blazor.Services
{
    public class APITrainService(HttpClient Http) : ITrainService<Trains>
    {
        List<Trains> _trains;
        int _currentPage = 1;
        int _totalPages = 1;

        public IEnumerable<Trains> Trains => _trains;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;
        

        public event Action ListChanged;


        public async Task GetTrains(int pageNo, int pageSize)
        {
            var uri = Http.BaseAddress?.AbsoluteUri;
            
            var queryData = new Dictionary<string, string?>
            {
                { "pageNo", pageNo.ToString() },
                { "pageSize", pageSize.ToString() }
            };
            var query = QueryString.Create(queryData);

            var result = await Http.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                var responseData = await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Trains>>>();

                _currentPage = responseData.Data.CurrentPage;
                _totalPages = responseData.Data.TotalPages;
                _trains = responseData.Data.Items;

                ListChanged?.Invoke();
            }
            else
            {
                _trains = null;
                _currentPage = 1;
                _totalPages = 1;
            }
        }
    }
}
