namespace AlexanderShemarov.Blazor.Services
{
    public interface ITrainService<T> where T : class
    {
        event Action ListChanged;

        // Trains' Array
        IEnumerable<T> Trains { get; }

        // Current Page Number
        int CurrentPage { get; }

        // Common Amount of Pages
        int TotalPages { get; }


        // Getting Trains' Array
        Task GetTrains(int pageNo = 1, int pageSize = 3);
    }
}
