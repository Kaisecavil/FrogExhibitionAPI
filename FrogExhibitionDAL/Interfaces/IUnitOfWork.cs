﻿using FrogExhibitionDAL.Interfaces.IRepository;
using FrogExhibitionDAL.Models;

namespace FrogExhibitionDAL.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<Exhibition> Exhibitions { get; }
        IBaseRepository<Frog> Frogs { get; }
        IBaseRepository<FrogOnExhibition> FrogOnExhibitions { get; }
        IBaseRepository<Vote> Votes { get; }
        IBaseRepository<FrogPhoto> FrogPhotos { get; }
        IBaseRepository<Comment> Comments { get; }
        IBaseRepository<FrogStarRating> FrogStarRatings { get; }

        int Save();
        Task<int> SaveAsync();
    }
}