using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;

namespace AlexanderShemarov.UI.Services
{
    public interface ITrainTypesService
    {
        /// <summary> 
        /// Getting a list of all train types 
        /// </summary> 
        /// <returns></returns>
        public Task<ResponseData<List<TrainTypes>>> GetTrainTypesListAsync();
    }
}
