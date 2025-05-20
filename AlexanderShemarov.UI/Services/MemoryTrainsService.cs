using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;

namespace AlexanderShemarov.UI.Services
{
    public class MemoryTrainsService : ITrainsService
    {
        List<Trains> _trains;
        List<TrainTypes> _trainTypes;

        public MemoryTrainsService(ITrainTypesService trainTypesService)
        {
            _trainTypes = trainTypesService.GetTrainTypesListAsync().Result.Data;

            SetupData();
        }
        private void SetupData()
        {
            _trains =
            [
                new Trains
                {
                    ID = 1,
                    Name = "Князь Вітаўт",
                    Description = "Пасажырскі цягнік сям'і Stadler Flirt, " +
                    "які выкарыстоўваецца па хуткасных кірунках Менск – Варшава, " +
                    "Гомель – Гданьск і Менск – Маладзечна – Гародня – Кракаў.",
                    Speed = 160,
                    Cost = 4250000.00M,
                    Image = "images/Stadler_Flirt.jpg",
                    TrainTypesId = _trainTypes.Find(tt => tt.NormalizedName.Equals("passenger")).ID,
                },
                new Trains
                {
                    ID = 2,
                    Name = "Laminatka",
                    Description = "Грузавы цягнік Škoda, выраблены з ламінату, " +
                    "ад чаго атрымаў мянушку Ламінатка; выкарыстоўваецца для " +
                    "грузавых перавозак у межах Чэхіі і Славакіі.",
                    Speed = 120,
                    Cost = 1000000.00M,
                    Image = "images/Laminatka_E230.jpg",
                    TrainTypesId = _trainTypes.Find(tt => tt.NormalizedName.Equals("cargo")).ID,
                },
                new Trains
                {
                    ID = 3,
                    Name = "Návrat Tomáše Masarykovi",
                    Description = "Рэтра паравы цягнік чэхаславацкага выробцы ČKD 1915 году, " +
                    "выкарыстоўваецца падчас гістарычных рэканструкцыяў, адна з якіх адбылася " +
                    "28 касірычніка ў 2018 годзе: віншаванне стварэння незалежнай Чэхаславацкай " +
                    "рэспубліцы і вяртанне першага презыдэнту Чэхаславакіі з эміграцыі " +
                    "Томаша Гарыка Масарыка.",
                    Speed = 100,
                    Cost = 2760000.00M,
                    Image = "images/Parnik310.23.jpg",
                    TrainTypesId = _trainTypes.Find(tt => tt.NormalizedName.Equals("retro")).ID,
                },
                new Trains
                {
                    ID = 4,
                    Name = "Гетьман Богдан Хмельницький",
                    Description = "Пасажырскі цягнік тыпу ДЕ1 выкарыстоўваецца " +
                    "на адным з самых знакаміцейшых кірунках цягніковага свету: " +
                    "'Цягнік 7 сталіцаў' – і закранае гарады: Вена, Будапешт, " +
                    "Кіяў, Менск, Вільня, Рыга і Талін.",
                    Speed = 100,
                    Cost = 1450000.00M,
                    Image = "images/DE1.jpg",
                    TrainTypesId = _trainTypes.Find(tt => tt.NormalizedName.Equals("passenger")).ID,
                },
                new Trains
                {
                    ID = 5,
                    Name = "Freccia Bianca Dynamica",
                    Description = "Пад звычайнай назвай хуткаснага цягніку таварыства " +
                    "TrenItalia хаваецца палову пасажырскі, палову дынамітрычны цягнік: " +
                    "падчас вандроўцы ад Неапалі да Мілана ён скануе і падлічвае асаблівасці " +
                    "чыгуначных шэрагаў для хуткасных цягнікоў сям'і Freccia, а таксама " +
                    "перавозіць пасажыраў, ня робічы дзірак у раскладзе кірунку.",
                    Speed = 300,
                    Cost = 5127000.00M,
                    Image = "images/Freccia_Bianca.jpg",
                    TrainTypesId = _trainTypes.Find(tt => tt.NormalizedName.Equals("special")).ID,
                },
            ];
        }
        public Task<ResponseData<ListModel<Trains>>> GetTrainsListAsync(string? trainTypesNormalizedName, int pageNo = 1)
        {
            //var model = new ListModel<Trains>() { Items = _trains };
            //var result = new ResponseData<ListModel<Trains>>()
            //{
            //    Data = model,
            //};

            var result = new ResponseData<ListModel<Trains>>();
            int? trainTypesID = null;

            if(trainTypesNormalizedName != null)
            {
                trainTypesID = _trainTypes.Find(tt => tt.NormalizedName.Equals(trainTypesNormalizedName))?.ID;
            }

            var data = _trains.Where(train => trainTypesID == null || train.TrainTypesId.Equals(trainTypesID))?.ToList();
            result.Data = new ListModel<Trains>() { Items = data };

            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Няма аб'ектаў адабранай катэгорыі!";
            }

            return Task.FromResult(result);
        }

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

        public Task UpdateTrainAsync(int id, Trains train, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
