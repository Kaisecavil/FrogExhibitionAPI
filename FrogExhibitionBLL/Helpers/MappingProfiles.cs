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
using FrogExhibitionBLL.ViewModels.FrogStarRatingViewModels;
using FrogExhibitionBLL.DTO.FrogStarRatingDTOs;
using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionBLL.DTO.CommentsDTOs;

namespace FrogExhibitionBLL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<FrogDtoForCreate, Frog>();
            CreateMap<FrogDtoForUpdate, Frog>();
            CreateMap<Frog, FrogDetailViewModel>()
                .ForMember(
                    dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex.ToString())
                );
            CreateMap<Frog, FrogGeneralViewModel>()
                .ForMember(
                    dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex.ToString())
                );
            CreateMap<Frog, FrogRatingViewModel>()
                .ForMember(
                    dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex.ToString())
                );
            CreateMap<Frog, FrogExhibitionViewModel>()
                .ForMember(
                    dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex.ToString())
                );

            CreateMap<Exhibition, ExhibitionGeneralViewModel>();
            CreateMap<Exhibition, ExhibitionDetailViewModel>();
            CreateMap<ExhibitionDtoForUpdate, Exhibition>();
            CreateMap<ExhibitionDtoForCreate, Exhibition>();

            CreateMap<FrogOnExhibition, FrogOnExhibitionDetailViewModel>();
            CreateMap<FrogOnExhibitionDtoForCreate, FrogOnExhibition>();
            CreateMap<FrogOnExhibitionDtoForUpdate, FrogOnExhibition>();


            CreateMap<Vote, VoteDetailViewModel>();
            CreateMap<VoteDtoForCreate, Vote>();
            CreateMap<VoteDtoForUpdate, Vote>();


            CreateMap<ApplicationUser, ApplicationUserDetailViewModel>();
            CreateMap<ApplicationUser, ApplicationUserGeneralViewModel>();
            CreateMap<ApplicationUserDtoForUpdate, ApplicationUser>();

            CreateMap<FrogStarRating, FrogStarRatingGeneralViewModel>();
            CreateMap<FrogStarRatingDtoForUpdate, FrogStarRating>();
            CreateMap<FrogStarRatingDtoForCreate, FrogStarRating>();

            CreateMap<Comment, CommentGeneralViewModel>();
            CreateMap<CommentDtoForUpdate, Comment>();
            CreateMap<CommentDtoForCreate, Comment>();
        }
    }
}
