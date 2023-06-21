using AutoMapper;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionDAL.Models;

namespace FrogExhibitionBLL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Frog, FrogDtoDetail>();
            CreateMap<FrogDtoDetail, Frog>();
            CreateMap<Frog, FrogDtoForCreate>();
            CreateMap<FrogDtoForCreate, Frog>();
            CreateMap<Frog, FrogDtoForUpdate>();
            CreateMap<FrogDtoForUpdate, Frog>();
            CreateMap<Frog, FrogDtoGeneral>();
            CreateMap<FrogDtoGeneral, Frog>();
            CreateMap<Frog, FrogDtoRating>();
            CreateMap<FrogDtoRating, Frog>();

            CreateMap<Exhibition, ExhibitionDtoDetail>();
            CreateMap<ExhibitionDtoDetail, Exhibition>();
            CreateMap<Exhibition, ExhibitionDtoForCreate>();
            CreateMap<ExhibitionDtoForCreate, Exhibition>();

            CreateMap<FrogOnExhibition, FrogOnExhibitionDtoDetail>();
            CreateMap<FrogOnExhibitionDtoDetail, FrogOnExhibition>();
            CreateMap<FrogOnExhibition, FrogOnExhibitionDtoForCreate>();
            CreateMap<FrogOnExhibitionDtoForCreate, FrogOnExhibition>();

            CreateMap<Vote, VoteDtoDetail>();
            CreateMap<VoteDtoDetail, Vote>();
            CreateMap<Vote, VoteDtoForCreate>();
            CreateMap<VoteDtoForCreate, Vote>();

            CreateMap<ApplicationUser, ApplicationUserDtoDetail>();
            CreateMap<ApplicationUserDtoDetail, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDtoForUpdate>();
            CreateMap<ApplicationUserDtoForUpdate, ApplicationUser>();

        }
    }
}
