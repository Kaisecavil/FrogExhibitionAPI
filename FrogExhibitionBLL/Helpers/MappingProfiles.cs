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
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.Interfaces.IProvider;

namespace FrogExhibitionBLL.Helpers
{
    public class MappingProfiles : Profile
    {
        private readonly IFrogPhotoService _frogPhotoService;

        public MappingProfiles(IFrogPhotoService frogPhotoService)
        {
            _frogPhotoService = frogPhotoService;

            CreateMap<FrogDtoForCreate, Frog>();
            CreateMap<FrogDtoForUpdate, Frog>();

            CreateMap<Frog, FrogDetailViewModel>()
                .ForMember(
                    dest => dest.Sex,
                    opt => opt.MapFrom(src => src.Sex.ToString())
                )
                .ForMember(
                    dest => dest.Comments,
                    opt => opt.MapFrom(src =>
                        ConcatenateLists(src.FrogsOnExhibitions
                            .Select(foe => foe.Comments).ToList()))
                )
                .ForMember(
                    dest => dest.PhotoPaths,
                    opt => opt.MapFrom(src => _frogPhotoService.GetFrogPhotoPaths(src.Id).ToList())
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
            CreateMap<Vote, VoteReportViewModel>();
            CreateMap<VoteDtoForCreate, Vote>();
            CreateMap<VoteDtoForUpdate, Vote>();


            CreateMap<ApplicationUser, ApplicationUserDetailViewModel>();
            CreateMap<ApplicationUser, ApplicationUserGeneralViewModel>();
            CreateMap<ApplicationUserDtoForUpdate, ApplicationUser>();

            CreateMap<FrogStarRating, FrogStarRatingGeneralViewModel>();
            CreateMap<FrogStarRatingDtoForUpdate, FrogStarRating>();
            CreateMap<FrogStarRatingDtoForCreate, FrogStarRating>();

            CreateMap<Comment, CommentGeneralViewModel>();
            CreateMap<Comment, CommentReportViewModel>();
            CreateMap<CommentDtoForUpdate, Comment>();
            CreateMap<CommentDtoForCreate, Comment>();
        }

        public static List<T> ConcatenateLists<T>(List<List<T>> listOfLists)
        {
            List<T> concatenatedList = new List<T>();

            foreach (List<T> innerList in listOfLists)
            {
                concatenatedList.AddRange(innerList);
            }

            return concatenatedList;
        }
    }
}
