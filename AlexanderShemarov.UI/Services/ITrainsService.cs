using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;


namespace AlexanderShemarov.UI.Services
{
    public interface ITrainsService
    {
        /// <summary> 
        /// Getting all objects
        /// </summary> 
        /// <param name="trainTypesNormalizedName"></param>
        /// <param name="pageNo">List Page Number</param> 
        /// <returns></returns> 
        public Task<ResponseData<ListModel<Trains>>> GetTrainsListAsync(string? trainTypesNormalizedName, int pageNo = 1);

        /// <summary> 
        /// Looking for an object by ID
        /// </summary> 
        /// <param name="id"></param> 
        /// <returns>Found Object, or null (if an object isn't found)</returns> 
        public Task<ResponseData<Trains>> GetTrainByIdAsync(int id);

        /// <summary> 
        /// Object Updating
        /// </summary> 
        /// <param name="id"></param> 
        /// <param name="train">object with new parameters</param> 
        /// <param name="formFile">image file</param> 
        /// <returns></returns> 
        public Task<ResponseData<Trains>> UpdateTrainAsync(int id, Trains train, IFormFile? formFile);
        
        /// <summary> 
        /// Object Removing 
        /// </summary> 
        /// <param name="id"></param> 
        /// <returns></returns> 
        public Task<ResponseData<bool>> DeleteTrainAsync(int id);

        /// <summary> 
        /// Object Creation 
        /// </summary> 
        /// <param name="train">a new object</param> 
        /// <param name="formFile">image file</param> 
        /// <returns>Созданный объект</returns> 
        public Task<ResponseData<Trains>> CreateTrainAsync(Trains train, IFormFile? formFile);
    }
}
