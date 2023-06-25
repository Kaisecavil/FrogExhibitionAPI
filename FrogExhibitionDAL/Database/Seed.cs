using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.Extensions.Logging;
using FrogExhibitionDAL.Enums;

namespace FrogExhibitionDAL.Database
{
    public class Seed
    {
        private readonly IUnitOfWork _unitOfWork;
        public Seed(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    Sex = FrogSex.Male,
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
                    Sex = FrogSex.Female,
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
                    Sex = FrogSex.Female,
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
                    Sex = FrogSex.Hermaphrodite,
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

            await _unitOfWork.Exhibitions.CreateRangeAsync(exhibitions);
            await _unitOfWork.Frogs.CreateRangeAsync(frogs);

            var frogsOnExhibitions = new List<FrogOnExhibition>();
            frogs
                .ForEach(f => exhibitions
                .ForEach(e => frogsOnExhibitions
                .Add(new FrogOnExhibition
                {
                    FrogId = f.Id,
                    ExhibitionId = e.Id
                })));
            await _unitOfWork.FrogOnExhibitions.CreateRangeAsync(frogsOnExhibitions);
            await _unitOfWork.SaveAsync();
        }
    }
}
