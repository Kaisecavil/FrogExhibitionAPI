﻿using FrogExhibitionDAL.Database;
using FrogExhibitionDAL.Interfaces.IRepository;
using FrogExhibitionDAL.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace FrogExhibitionDAL.Repositories
{
    public class BaseRepository<DbModel> : IBaseRepository<DbModel> where DbModel : BaseModel
    {
        protected readonly ApplicationContext _context;
        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Create(DbModel model)
        {
            _context.Set<DbModel>().Add(model);
        }

        public void CreateRange(IEnumerable<DbModel> models)
        {
            _context.Set<DbModel>().AddRange(models);
        }

        public void Delete(Guid id)
        {
            var toDelete = _context.Set<DbModel>().FirstOrDefault(m => m.Id == id);
            _context.Set<DbModel>().Remove(toDelete);
        }

        public IEnumerable<DbModel> GetAll(bool asNoTraking = false)
        {
            return asNoTraking ?
                _context.Set<DbModel>().AsNoTracking().Where(m => m.DeletedAt == null).ToList() :
                _context.Set<DbModel>().Where(m => m.DeletedAt == null).ToList();
        }

        public IQueryable<DbModel> GetAllQueryable(bool asNoTraking = false)
        {
            return asNoTraking ?
               _context.Set<DbModel>().AsNoTracking().Where(m => m.DeletedAt == null) :
               _context.Set<DbModel>().Where(m => m.DeletedAt == null);
        }

        public void Update(DbModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public DbModel Get(Guid id)
        {
            return _context.Set<DbModel>().Where(m => m.DeletedAt == null).FirstOrDefault(m => m.Id == id);
        }

        public async Task<IEnumerable<DbModel>> GetAllAsync(bool asNoTraking = false)
        {
            return asNoTraking ?
                await _context.Set<DbModel>().AsNoTracking()
                    .Where(m => m.DeletedAt == null).ToListAsync() :
                await _context.Set<DbModel>()
                    .Where(m => m.DeletedAt == null).ToListAsync() ;
        }

        public async Task<DbModel> GetAsync(Guid id, bool asNoTraking = false)
        {    
            return asNoTraking?
                await _context.Set<DbModel>().AsNoTracking()
                    .Where(m => m.DeletedAt == null)
                    .FirstOrDefaultAsync(m => m.Id == id) :
                await _context.Set<DbModel>()
                    .Where(m => m.DeletedAt == null)
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateAsync(DbModel model)
        {
            _context.Set<DbModel>().Add(model);
        }

        public async Task UpdateAsync(DbModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await GetAsync(id);
            _context.Set<DbModel>().Remove(toDelete);
        }

        public async Task<bool> EntityExistsAsync(Guid id)
        {
            return await _context.Set<DbModel>().AsNoTracking()
                .Where(m => m.DeletedAt == null)
                .AnyAsync(e => e.Id == id);
        }

        public async Task<bool> IsEmptyAsync()
        {
            return !await _context.Set<DbModel>().AsNoTracking()
                .Where(m => m.DeletedAt == null)
                .AnyAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<DbModel> models)
        {
            await _context.Set<DbModel>().AddRangeAsync(models);
        }
        public void DeleteRange(IEnumerable<DbModel> models)
        {
            _context.Set<DbModel>().RemoveRange(models);
        }
    }
}
