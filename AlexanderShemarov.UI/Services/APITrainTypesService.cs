using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;
using static AlexanderShemarov.UI.Services.APITrainTypesService;

namespace AlexanderShemarov.UI.Services
{
    public class APITrainTypesService(HttpClient httpClient) : ITrainTypesService
    {
        public async Task<ResponseData<List<TrainTypes>>> GetTrainTypesListAsync()
        {
            var result = await httpClient.GetAsync(httpClient.BaseAddress);
            if (result.IsSuccessStatusCode)
            {
                var trainTypes = await result.Content.ReadFromJsonAsync<List<TrainTypes>>();
                return new ResponseData<List<TrainTypes>>
                {
                    Data = trainTypes,
                    Success = true
                };
            };

            var response = new ResponseData<List<TrainTypes>>
            {
                Success = false,
                ErrorMessage = "TrainTypesAPI Data Reading Error"
            };
            return response;
        }
    }
}
