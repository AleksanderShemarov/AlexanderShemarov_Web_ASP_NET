using AlexanderShemarov.API.Controllers;
using AlexanderShemarov.API.Data;
using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Data.Common;


namespace AlexanderShemarov.Tests
{
    public class TrainsControllerTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<TrainsApiDbContext> _contextOptions;
        private readonly IWebHostEnvironment _environment;

        public TrainsControllerTests()
        {
            _environment = Substitute.For<IWebHostEnvironment>();

            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<TrainsApiDbContext>()
                .UseSqlite(_connection)
                .Options;

            using var context = new TrainsApiDbContext(_contextOptions);
            context.Database.EnsureCreated();

            var traintypes = new TrainTypes[]
            {
                new TrainTypes { Name = "", NormalizedName = "passenger" },
                new TrainTypes { Name = "", NormalizedName = "cargo" },
                new TrainTypes { Name = "", NormalizedName = "special" },
                new TrainTypes { Name = "", NormalizedName = "retro" }
            };
            context.TrainTypesAPI.AddRange(traintypes);
            context.SaveChanges();

            var trains = new List<Trains>
            {
                new Trains
                {
                    Name = "",
                    Description = "",
                    Speed = 0,
                    Cost = 0.00M,
                    Image = "",
                    TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("passenger")).ID,
                },
                new Trains
                {
                    Name = "",
                    Description = "",
                    Speed = 0,
                    Cost = 0.00M,
                    Image = "",
                    TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("cargo")).ID,
                },
                new Trains
                {
                    Name = "",
                    Description = "",
                    Speed = 0,
                    Cost = 0.00M,
                    Image = "",
                    TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("retro")).ID,
                },
                new Trains
                {
                    Name = "",
                    Description = "",
                    Speed = 0,
                    Cost = 0.00M,
                    Image = "",
                    TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("passenger")).ID,
                },
                new Trains
                {
                    Name = "",
                    Description = "",
                    Speed = 0,
                    Cost = 0.00M,
                    Image = "",
                    TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("special")).ID,
                },
            };
            context.AddRange(trains);
            context.SaveChanges();
        }

        public void Dispose() => _connection?.Dispose();
        TrainsApiDbContext CreateContext() => new TrainsApiDbContext(_contextOptions);

        [Fact]
        public async void ControllerFiltersTrainTypes()
        {
            using var context = CreateContext();
            var trainType = context.TrainTypesAPI.First();

            var controller = new TrainsController(context, _environment);

            var response = await controller.GetTrainsAPI(trainType.NormalizedName);
            ResponseData<ListModel<Trains>> responseData = response.Value;
            var trainsList = responseData.Data.Items;

            Assert.True(trainsList.All(t => t.TrainTypesId == trainType.ID));
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async void ControllerReturnsCorrectPagesCount(int size, int qty)
        {
            using var context = CreateContext();
            var controller = new TrainsController(context, _environment);

            var response = await controller.GetTrainsAPI(null, 1, size);
            ResponseData<ListModel<Trains>> responseData = response.Value;
            var totalPages = responseData.Data.TotalPages;

            Assert.Equal(qty, totalPages);
        }

        [Fact]
        public async void ControllerReturnsCorrectPage()
        {
            using var context = CreateContext();
            var controller = new TrainsController(context, _environment);

            int itemsInPage = 2;
            Trains firstItem = context.TrainsAPI.ToArray()[3];

            var response = await controller.GetTrainsAPI(null, 2);
            ResponseData<ListModel<Trains>> responseData = response.Value;
            var trainsList = responseData.Data.Items;
            var currentPage = responseData.Data.CurrentPage;

            Assert.Equal(2, currentPage);
            Assert.Equal(2, trainsList.Count);
            Assert.Equal(firstItem.ID, trainsList[0].ID);
        }
    }
}
