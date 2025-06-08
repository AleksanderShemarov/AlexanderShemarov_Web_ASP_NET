using AlexanderShemarov.Domain.Entities;

namespace AlexanderShemarov.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Uri of the project
            var uri = "https://localhost:7002/";

            // Getting the database
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrainsApiDbContext>();

            // Database filling by data 
            if (!context.TrainTypesAPI.Any() && !context.TrainsAPI.Any())
            {
                var traintypes = new TrainTypes[]
                {
                    new() { Name = "Passenger Train", NormalizedName = "passenger" },
                    new() { Name = "Cargo Train", NormalizedName = "cargo" },
                    new() { Name = "Special Train", NormalizedName = "special" },
                    new() { Name = "Retro Train", NormalizedName = "retro" }
                };
                await context.TrainTypesAPI.AddRangeAsync(traintypes);
                await context.SaveChangesAsync();


                var trains = new List<Trains>
                {
                    new()
                    {
                        Name = "Князь Вітаўт",
                        Description = "Пасажырскі цягнік сям'і Stadler Flirt, " +
                        "які выкарыстоўваецца па хуткасных кірунках Менск – Варшава, " +
                        "Гомель – Гданьск і Менск – Маладзечна – Гародня – Кракаў.",
                        Speed = 160,
                        Cost = 4250000.00M,
                        Image = uri + "images/Stadler_Flirt.jpg",
                        TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("passenger")).ID
                    },
                    new()
                    {
                        Name = "Laminatka",
                        Description = "Грузавы цягнік Škoda, выраблены з ламінату, " +
                        "ад чаго атрымаў мянушку Ламінатка; выкарыстоўваецца для " +
                        "грузавых перавозак у межах Чэхіі і Славакіі.",
                        Speed = 120,
                        Cost = 1000000.00M,
                        Image = uri + "images/Laminatka_E230.jpg",
                        TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("cargo")).ID
                    },
                    new()
                    {
                        Name = "Návrat Tomáše Masarykovi",
                        Description = "Рэтра паравы цягнік чэхаславацкага выробцы ČKD 1915 году, " +
                        "выкарыстоўваецца падчас гістарычных рэканструкцыяў, адна з якіх адбылася " +
                        "28 касірычніка ў 2018 годзе: віншаванне стварэння незалежнай Чэхаславацкай " +
                        "рэспубліцы і вяртанне першага презыдэнту Чэхаславакіі з эміграцыі " +
                        "Томаша Гарыка Масарыка.",
                        Speed = 100,
                        Cost = 2760000.00M,
                        Image = uri + "images/Parnik310.23.jpg",
                        TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("retro")).ID
                    },
                    new()
                    {
                        Name = "Гетьман Богдан Хмельницький",
                        Description = "Пасажырскі цягнік тыпу ДЕ1 выкарыстоўваецца " +
                        "на адным з самых знакаміцейшых кірунках цягніковага свету: " +
                        "'Цягнік 7 сталіцаў' – і закранае гарады: Вена, Будапешт, " +
                        "Кіяў, Менск, Вільня, Рыга і Талін.",
                        Speed = 100,
                        Cost = 1450000.00M,
                        Image = uri + "images/DE1.jpg",
                        TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("passenger")).ID
                    },
                    new()
                    {
                        Name = "Freccia Bianca Dynamica",
                        Description = "Пад звычайнай назвай хуткаснага цягніку таварыства " +
                        "TrenItalia хаваецца палову пасажырскі, палову дынамітрычны цягнік: " +
                        "падчас вандроўцы ад Неапалі да Мілана ён скануе і падлічвае асаблівасці " +
                        "чыгуначных шэрагаў для хуткасных цягнікоў сям'і Freccia, а таксама " +
                        "перавозіць пасажыраў, ня робічы дзірак у раскладзе кірунку.",
                        Speed = 300,
                        Cost = 5127000.00M,
                        Image = uri + "images/Freccia_Bianca.jpg",
                        TrainTypesId = traintypes.FirstOrDefault(tt => tt.NormalizedName.Equals("special")).ID
                    },
                };
                await context.TrainsAPI.AddRangeAsync(trains);
                await context.SaveChangesAsync();
            }
        }
    }
}
