using AutoMapper;
using FrogExhibitionDAL.Models;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.DTO.FrogDTOs;
using FrogExhibitionBLL.DTO.FrogOnExhibitionDTOs;
using FrogExhibitionBLL.DTO.VoteDtos;
using FrogExhibitionBLL.DTO.ApplicatonUserDTOs;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogOnExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.VoteViewModels;
using FrogExhibitionBLL.ViewModels.ApplicatonUserViewModels;

namespace FrogExhibitionBLL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Frog, FrogDetailViewModel>();
            CreateMap<FrogDetailViewModel, Frog>();
            CreateMap<Frog, FrogDtoForCreate>();
            CreateMap<FrogDtoForCreate, Frog>();
            CreateMap<Frog, FrogDtoForUpdate>();
            CreateMap<FrogDtoForUpdate, Frog>();
            CreateMap<Frog, FrogGeneralViewModel>();
            CreateMap<FrogGeneralViewModel, Frog>();
            CreateMap<Frog, FrogRatingViewModel>();
            CreateMap<FrogRatingViewModel, Frog>();

            CreateMap<Exhibition, ExhibitionDetailViewModel>();
            CreateMap<ExhibitionDetailViewModel, Exhibition>();
            CreateMap<Exhibition, ExhibitionDtoForCreate>();
            CreateMap<ExhibitionDtoForCreate, Exhibition>();
            CreateMap<Exhibition, ExhibitionDtoForUpdate>();
            CreateMap<ExhibitionDtoForUpdate, Exhibition>();

            CreateMap<FrogOnExhibition, FrogOnExhibitionDetailViewModel>();
            CreateMap<FrogOnExhibitionDetailViewModel, FrogOnExhibition>();
            CreateMap<FrogOnExhibition, FrogOnExhibitionDtoForCreate>();
            CreateMap<FrogOnExhibitionDtoForCreate, FrogOnExhibition>();
            CreateMap<FrogOnExhibition, FrogOnExhibitionDtoForUpdate>();
            CreateMap<FrogOnExhibitionDtoForUpdate, FrogOnExhibition>();

            CreateMap<Vote, VoteDetailViewModel>();
            CreateMap<VoteDetailViewModel, Vote>();
            CreateMap<Vote, VoteDtoForCreate>();
            CreateMap<VoteDtoForCreate, Vote>();
            CreateMap<Vote, VoteDtoForUpdate>();
            CreateMap<VoteDtoForUpdate, Vote>();

            CreateMap<ApplicationUser, ApplicationUserDetailViewModel>();
            CreateMap<ApplicationUserDetailViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserGeneralViewModel>();
            CreateMap<ApplicationUserGeneralViewModel, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDtoForUpdate>();
            CreateMap<ApplicationUserDtoForUpdate, ApplicationUser>();

        }
    }
}
