using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionDAL.Database
{
    public class Seed
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<Seed> _logger;
        public Seed(IUnitOfWork unitOfWork,
            ILogger<Seed> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task SeedApplicationContextAsync()
        {
            var exhibitions = new List<Exhibition>
            {
                new Exhibition
                {
                    Name = "Exhibition of Beautifull frogs",
                    Date = DateTime.Now.AddDays(7),
                    Country = "Gernamy",
                    City = "Berlin",
                    Street = "Kharnchahte 34",
                    House = "17/b"
                },
                new Exhibition
                {
                    Name = "FrogFEST",
                    Date = DateTime.Now.AddDays(14),
                    Country = "Poland",
                    City = "Krakov",
                    Street = "Pervolino 21",
                    House = "19"
                }
            // Add more exhibitions if needed
            };

            var frogs = new List<Frog>
            {
                new Frog
                {
                    Genus = "Red Frog",
                    Species = "Bull-frog",
                    Color = "Red",
                    Habitat = "North America",
                    Poisonous = false,
                    Sex = "Male",
                    HouseKeepable = true,
                    Size = 5.0f,
                    Weight = 10.0f,
                    CurrentAge = 3,
                    MaxAge = 12,
                    Features = "Bulls everyone around",
                    Diet = "Insects"
                },
                new Frog
                {
                    Genus = "Green Frog",
                    Species = "Jumping frog",
                    Color = "Green",
                    Habitat = "Asia",
                    Poisonous = true,
                    Sex = "Female",
                    HouseKeepable = false,
                    Size = 4.5f,
                    Weight = 8.0f,
                    CurrentAge = 2,
                    MaxAge = 10,
                    Features = "Can do triple backflip",
                    Diet = "flies"
                },
                new Frog
                {
                    Genus = "Purple Frog",
                    Species = "Programmer frog",
                    Color = "Purple",
                    Habitat = "Dark offices",
                    Poisonous = false,
                    Sex = "Female",
                    HouseKeepable = false,
                    Size = 8.5f,
                    Weight = 8.0f,
                    CurrentAge = 2,
                    MaxAge = 10,
                    Features = "She is capable of anything",
                    Diet = "Code bugs"
                },
                new Frog
                {
                    Genus = "Yellow Frog",
                    Species = "Toxic frog",
                    Color = "Yellow",
                    Habitat = "Africa",
                    Poisonous = true,
                    Sex = "Hermaphrodite",
                    HouseKeepable = false,
                    Size = 4.5f,
                    Weight = 8.0f,
                    CurrentAge = 2,
                    MaxAge = 10,
                    Features = "Peolpe say biological weapon isn't toxic as he/she is",
                    Diet = "Your suffering"
                }
                // Add more frogs if needed
            };



            foreach (var exhibition in exhibitions)
            {
                _unitOfWork.Exhibitions.Create(exhibition);
            }

            foreach (var frog in frogs)
            {
                _unitOfWork.Frogs.Create(frog);
            }

            _unitOfWork.Save();

            var exe = _unitOfWork.Exhibitions.GetAll();
            var frgs = _unitOfWork.Frogs.GetAll();
            foreach (var exhibition in exe)
            {
                foreach (var frog in frgs)
                {
                    var temp = new FrogOnExhibition()
                    {
                        FrogId = frog.Id,
                        ExhibitionId = exhibition.Id
                    };

                    try
                    {
                        _unitOfWork.FrogOnExhibitions.Create(temp);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Db update exception");
                        continue;
                    }
                }
            }

            _unitOfWork.Save();


        }
    }
}
