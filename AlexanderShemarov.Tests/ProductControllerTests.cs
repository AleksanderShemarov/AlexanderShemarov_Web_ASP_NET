using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;
using AlexanderShemarov.UI.Controllers;
using AlexanderShemarov.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;


namespace AlexanderShemarov.Tests
{
    public class ProductControllerTests
    {
        ITrainsService _trainsService;
        ITrainTypesService _trainTypesService;
        public ProductControllerTests()
        {
            SetupData();
        }

        [Fact]
        public async void IndexPutsTrainTypesToViewData()
        {
            var controller = new ProductController(_trainTypesService, _trainsService);

            var response = await controller.Index(null);

            var view = Assert.IsType<ViewResult>(response);
            var traintypes = Assert.IsType<List<TrainTypes>>(view.ViewData["trainTypes"]);
            Assert.Equal(4, traintypes.Count);
            Assert.Equal("All", view.ViewData["currentTrainType"]);
        }

        [Fact]
        public async void IndexSetsCorrectCurrentTrainType()
        {
            var traintypes = await _trainTypesService.GetTrainTypesListAsync();
            var currentTrainType = traintypes.Data[0];
            var controller = new ProductController(_trainTypesService, _trainsService);

            var response = await controller.Index(currentTrainType.NormalizedName);

            var view = Assert.IsType<ViewResult>(response);
            Assert.Equal(currentTrainType.Name, view.ViewData["currentTrainType"]);
        }

        [Fact]
        public async void IndexReturnsNotFound()
        {
            string errorMessage = "Test Error";
            var trainTypesResponse = new ResponseData<List<TrainTypes>>();
            trainTypesResponse.Success = false;
            trainTypesResponse.ErrorMessage = errorMessage;

            _trainTypesService.GetTrainTypesListAsync().Returns(Task.FromResult(trainTypesResponse));
            var controller = new ProductController(_trainTypesService, _trainsService);

            var response = await controller.Index(null);

            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(errorMessage, result.Value.ToString());
        }

        void SetupData()
        {
            _trainTypesService = Substitute.For<ITrainTypesService>();
            var trainTypesResponse = new ResponseData<List<TrainTypes>>();
            trainTypesResponse.Data = new List<TrainTypes>
            {
                new TrainTypes { ID = 1, Name = "Passenger Train", NormalizedName = "passenger" },
                new TrainTypes { ID = 2, Name = "Cargo Train", NormalizedName = "cargo" },
                new TrainTypes { ID = 3, Name = "Special Train", NormalizedName = "special" },
                new TrainTypes { ID = 4, Name = "Retro Train", NormalizedName = "retro" }
            };
            _trainTypesService.GetTrainTypesListAsync().Returns(Task.FromResult(trainTypesResponse));
        
            _trainsService = Substitute.For<ITrainsService>();
            var trains = new List<Trains>
            {
                new Trains { TrainTypesId = 1 },
                new Trains { TrainTypesId = 2 },
                new Trains { TrainTypesId = 4 },
                new Trains { TrainTypesId = 1 },
                new Trains { TrainTypesId = 3 },
            };
            var trainsResponse = new ResponseData<ListModel<Trains>>();
            trainsResponse.Data = new ListModel<Trains>
            {
                Items = trains,
            };
            _trainsService.GetTrainsListAsync(Arg.Any<string?>(), Arg.Any<int>())
                .Returns(trainsResponse);
        }
    }
}
