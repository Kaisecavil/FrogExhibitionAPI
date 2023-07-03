using AutoMapper;
using FrogExhibitionBLL.DTO.ExhibitionDTOs;
using FrogExhibitionBLL.Exceptions;
using FrogExhibitionBLL.Interfaces.IHelper;
using FrogExhibitionBLL.Interfaces.IService;
using FrogExhibitionBLL.ViewModels.CommentViewModels;
using FrogExhibitionBLL.ViewModels.ExhibitionViewModels;
using FrogExhibitionBLL.ViewModels.FrogViewModels;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrogExhibitionBLL.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExhibitionService> _logger;
        private readonly IMapper _mapper;
        private readonly ISortHelper<Exhibition> _sortHelper;
        private readonly IFrogPhotoService _frogPhotoService;
        private readonly IExcelHelper _excelHelper;
        private readonly IFileHelper _fileHelper;

        public ExhibitionService(IUnitOfWork unitOfWork,
            ILogger<ExhibitionService> logger,
            IMapper mapper,
            ISortHelper<Exhibition> sortHelper,
            IFrogPhotoService frogPhotoService,
            IExcelHelper excelHelper,
            IFileHelper fileHelper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _sortHelper = sortHelper;
            _frogPhotoService = frogPhotoService;
            _excelHelper = excelHelper;
            _fileHelper = fileHelper;
        }

        public async Task<Guid> CreateExhibitionAsync(ExhibitionDtoForCreate exebition)
        {
            try
            {
                var mappedExhibition = _mapper.Map<Exhibition>(exebition);
                await _unitOfWork.Exhibitions.CreateAsync(mappedExhibition);
                _logger.LogInformation("Exhibition Created");
                await _unitOfWork.SaveAsync();
                return mappedExhibition.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, exebition);
                throw;
            };

        }

        public async Task<IEnumerable<ExhibitionGeneralViewModel>> GetAllExhibitionsAsync()
        {
            var result = await _unitOfWork.Exhibitions.GetAllAsync(true);
            return _mapper.Map<IEnumerable<ExhibitionGeneralViewModel>>(result);
        }

        public async Task<ExhibitionDetailViewModel> GetExhibitionAsync(Guid id)
        {
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var mappedExhibition = _mapper.Map<ExhibitionDetailViewModel>(exebition);
            mappedExhibition.Frogs = _mapper.Map<List<FrogExhibitionViewModel>>(exebition.Frogs);
            mappedExhibition.Frogs
                .ForEach(f => f.Comments = 
                    _mapper.Map<List<CommentGeneralViewModel>>
                    (
                        exebition.FrogsOnExhibitions.First(foe => foe.FrogId == f.Id).Comments
                        .OrderBy(c => c.CreationDate))
                    ); 
            return mappedExhibition;
        }

        public async Task UpdateExhibitionAsync(ExhibitionDtoForUpdate exebition)
        {
            try
            {
                if (await _unitOfWork.Exhibitions.EntityExistsAsync(exebition.Id))
                {
                    var mappedExhibition = _mapper.Map<Exhibition>(exebition);
                    await _unitOfWork.Exhibitions.UpdateAsync(mappedExhibition);
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    throw new NotFoundException("Entity not found");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.Exhibitions.EntityExistsAsync(exebition.Id))
                {
                    throw new NotFoundException("Entity not found due to possible concurrency");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteExhibitionAsync(Guid id)
        {
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            await _unitOfWork.Exhibitions.DeleteAsync(exebition.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<FrogRatingViewModel>> GetRatingAsync(Guid id)
        {
            var exebition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var frogsOnExhibition = exebition.FrogsOnExhibitions;
            var res = frogsOnExhibition.Select(foe => new FrogRatingViewModel
            {
                Id = foe.Frog.Id,
                VotesCount = foe.Votes.Count,
                VotesSum = foe.Votes.Sum(v => (int)v.ApplicationUser.KnowledgeLevel),
                AverageRating = foe.Frog.FrogStarRatings.Average(r => r.Rating),
                Color = foe.Frog.Color,
                HouseKeepable = foe.Frog.HouseKeepable,
                CurrentAge = foe.Frog.CurrentAge,
                MaxAge = foe.Frog.MaxAge,
                PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(foe.Frog.Id).ToList(),
                Genus = foe.Frog.Genus,
                Habitat = foe.Frog.Habitat,
                Weight = foe.Frog.Weight,
                Sex = foe.Frog.Sex.ToString(),
                Poisonous = foe.Frog.Poisonous,
                Size = foe.Frog.Size,
                Species = foe.Frog.Species
            })
            .OrderByDescending(f => f.VotesSum)
            .ThenByDescending(f => f.VotesCount)
            .ThenByDescending(f => f.AverageRating);
            return res.ToList();
        }

        public IEnumerable<FrogRatingViewModel> GetRating(Guid id)
        {
            var exebition = _unitOfWork.Exhibitions.Get(id);
            if (exebition == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var frogsOnExhibition = exebition.FrogsOnExhibitions;
            var res = frogsOnExhibition.Select(foe => new FrogRatingViewModel
            {
                Id = foe.Frog.Id,
                VotesCount = foe.Votes.Count,
                VotesSum = foe.Votes.Sum(v => (int)v.ApplicationUser.KnowledgeLevel),
                AverageRating = foe.Frog.FrogStarRatings.Average(r => r.Rating),
                Color = foe.Frog.Color,
                HouseKeepable = foe.Frog.HouseKeepable,
                CurrentAge = foe.Frog.CurrentAge,
                MaxAge = foe.Frog.MaxAge,
                PhotoPaths = _frogPhotoService.GetFrogPhotoPaths(foe.Frog.Id).ToList(),
                Genus = foe.Frog.Genus,
                Habitat = foe.Frog.Habitat,
                Weight = foe.Frog.Weight,
                Sex = foe.Frog.Sex.ToString(),
                Poisonous = foe.Frog.Poisonous,
                Size = foe.Frog.Size,
                Species = foe.Frog.Species
            })
            .OrderByDescending(f => f.VotesSum)
            .ThenByDescending(f => f.VotesCount)
            .ThenByDescending(f => f.AverageRating);
            return res.ToList();
        }

        public async Task<IEnumerable<FrogRatingViewModel>> GetBestFrogsHistoryAsync()
        {
            var exhibitions = (await _unitOfWork.Exhibitions.GetAllAsync()).ToList();
            List<FrogRatingViewModel> res = new();
            foreach (var exhibition in exhibitions)
            {
                res.Add((await GetRatingAsync(exhibition.Id)).FirstOrDefault());
            }
            
            return res.ToList();
        }

        public async Task<IEnumerable<ExhibitionGeneralViewModel>> GetAllExhibitionsAsync(string sortParams)
        {
            var exhibitions = _unitOfWork.Exhibitions.GetAllQueryable(true);
            var sortedExhibitions = await _sortHelper.ApplySort(exhibitions, sortParams).ToListAsync();
            return _mapper.Map<IEnumerable<ExhibitionGeneralViewModel>>(sortedExhibitions);
        }

        public async Task<FileContentResult> GetExhibitionExcelReportAsync(Guid id)
        {
            var exhibition = await _unitOfWork.Exhibitions.GetAsync(id);
            if (exhibition == null)
            {
                throw new NotFoundException("Entity not found");
            }

            string filePath = _fileHelper.GetExhibitionReportFilePath(exhibition.Name);
            try
            {
                _excelHelper.CreateSpreadsheetFromObjects((await GetExhibitionExcelReportDataAsync(exhibition)).ToList<object>(), filePath, "Frogs");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            if (!File.Exists(filePath))
            {
                throw new NotFoundException("a");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string contentType = "application/octet-stream";
            string fileName = Path.GetFileName(filePath);
            var fileContentResult = new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName
            };

            return fileContentResult;
        }

        public int GetFrogsPlaceOnExhibition(Guid frogid, Guid exhibitionId) 
        {
            var exhibition = _unitOfWork.Exhibitions.Get(exhibitionId);
            var frog = _unitOfWork.Frogs.Get(frogid);
            if (exhibition == null || frog == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var rating = GetRating(exhibitionId);
            return rating.ToList().IndexOf(rating.FirstOrDefault(f => f.Id == frogid)) + 1;
        }

        public async Task<int> GetFrogsPlaceOnExhibitionAsync(Guid frogid, Guid exhibitionId)
        {
            var exhibition = await _unitOfWork.Exhibitions.GetAsync(exhibitionId);
            var frog = await _unitOfWork.Frogs.GetAsync(frogid);
            if (exhibition == null || frog == null)
            {
                throw new NotFoundException("Entity not found");
            }
            var rating = await GetRatingAsync(exhibitionId);
            return rating.ToList().IndexOf(rating.FirstOrDefault(f => f.Id == frogid)) + 1;
        }

        private async Task<IEnumerable<FrogExcelReportViewModel>> GetExhibitionExcelReportDataAsync(Exhibition exhibition)
        {
            var frogsOnExhibition = exhibition.FrogsOnExhibitions;
            var res = frogsOnExhibition.Select(foe => new FrogExcelReportViewModel
            {
                Id = foe.Frog.Id,
                VotesCount = foe.Votes.Count,
                VotesSum = foe.Votes.Sum(v => (int)v.ApplicationUser.KnowledgeLevel),
                AverageRating = foe.Frog.FrogStarRatings.Average(r => r.Rating),
                CommentsCount = foe.Comments.Count,
                Color = foe.Frog.Color,
                HouseKeepable = foe.Frog.HouseKeepable,
                CurrentAge = foe.Frog.CurrentAge,
                MaxAge = foe.Frog.MaxAge,
                Genus = foe.Frog.Genus,
                Habitat = foe.Frog.Habitat,
                Weight = foe.Frog.Weight,
                Sex = foe.Frog.Sex.ToString(),
                Poisonous = foe.Frog.Poisonous,
                Size = foe.Frog.Size,
                Species = foe.Frog.Species
            })
            .OrderByDescending(f => f.VotesSum)
            .ThenByDescending(f => f.VotesCount)
            .ThenByDescending(f => f.AverageRating);
            return res.ToList();
        }
    }
}
