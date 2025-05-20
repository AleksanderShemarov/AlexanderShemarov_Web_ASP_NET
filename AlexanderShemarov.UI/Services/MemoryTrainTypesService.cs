using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;

namespace AlexanderShemarov.UI.Services
{
    public class MemoryTrainTypesService : ITrainTypesService
    {
        public Task<ResponseData<List<TrainTypes>>> GetTrainTypesListAsync()
        {
            var trainTypes = new List<TrainTypes>
            {
                new TrainTypes { ID = 1, Name = "Passenger Train", NormalizedName = "passenger" },
                new TrainTypes { ID = 2, Name = "Cargo Train", NormalizedName = "cargo" },
                new TrainTypes { ID = 3, Name = "Special Train", NormalizedName = "special" },
                new TrainTypes { ID = 4, Name = "Retro Train", NormalizedName = "retro" }
            };

            var result = new ResponseData<List<TrainTypes>>();
            result.Data = trainTypes;

            return Task.FromResult(result);
        }
    }
}
