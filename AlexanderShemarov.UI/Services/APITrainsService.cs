using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;

namespace AlexanderShemarov.UI.Services
{
    public class APITrainsService(HttpClient httpClient) : ITrainsService
    {
        public Task<ResponseData<Trains>> CreateTrainAsync(Trains train, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
        public Task DeleteTrainAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseData<Trains>> GetTrainByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseData<ListModel<Trains>>> GetTrainsListAsync(string? trainTypesNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;

            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString());
            if (!String.IsNullOrEmpty(trainTypesNormalizedName))
            {
                queryData.Add("trainType", trainTypesNormalizedName);
            }
            var query = QueryString.Create(queryData);

            var result = await httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Trains>>>();
            };

            var response = new ResponseData<ListModel<Trains>>
            {
                Success = false,
                ErrorMessage = "TrainsAPI Data Reading Error"
            };
            return response;
        }
        public Task UpdateTrainAsync(int id, Trains train, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
