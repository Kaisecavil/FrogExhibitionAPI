using FrogExhibitionDAL.Database;
using FrogExhibitionDAL.Interfaces;
using FrogExhibitionDAL.Interfaces.IRepository;
using FrogExhibitionDAL.Models;
using FrogExhibitionDAL.Repositories;

namespace FrogExhibitionDAL.UoW
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationContext _db;
        private IBaseRepository<Frog> _frogRepository;
        private IBaseRepository<Exhibition> _exebitionRepository;
        private IBaseRepository<FrogOnExhibition> _frogOnExhibitionRepository;
        private IBaseRepository<Vote> _voteRepository;
        private IBaseRepository<FrogPhoto> _frogPhotoRepository;
        private IBaseRepository<Comment> _commentRepository;
        private IBaseRepository<FrogStarRating> _frogStarRatingRepository;

        public UnitOfWork(ApplicationContext db)
        {
            _db = db;
        }

        public IBaseRepository<Frog> Frogs
        {
            get
            {
                if (_frogRepository == null)
                    _frogRepository = new BaseRepository<Frog>(_db);
                return _frogRepository;
            }
        }
        public IBaseRepository<Exhibition> Exhibitions
        {
            get
            {
                if (_exebitionRepository == null)
                    _exebitionRepository = new BaseRepository<Exhibition>(_db);
                return _exebitionRepository;
            }
        }
        public IBaseRepository<FrogOnExhibition> FrogOnExhibitions
        {
            get
            {
                if (_frogOnExhibitionRepository == null)
                    _frogOnExhibitionRepository = new BaseRepository<FrogOnExhibition>(_db);
                return _frogOnExhibitionRepository;
            }
        }
        public IBaseRepository<Vote> Votes
        {
            get
            {
                if (_voteRepository == null)
                    _voteRepository = new BaseRepository<Vote>(_db);
                return _voteRepository;
            }
        }
        public IBaseRepository<FrogPhoto> FrogPhotos
        {
            get
            {
                if (_frogPhotoRepository == null)
                    _frogPhotoRepository = new BaseRepository<FrogPhoto>(_db);
                return _frogPhotoRepository;
            }
        }

        public IBaseRepository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new BaseRepository<Comment>(_db);
                return _commentRepository;
            }
        }

        public IBaseRepository<FrogStarRating> FrogStarRatings
        {
            get
            {
                if(_frogStarRatingRepository == null)
                    _frogStarRatingRepository = new BaseRepository<FrogStarRating>(_db);
                return _frogStarRatingRepository;
            }
        }

        public int Save()
        {
            return _db.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
