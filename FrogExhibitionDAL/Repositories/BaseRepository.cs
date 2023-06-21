using FrogExhibitionDAL.Database;
using FrogExhibitionDAL.Interfaces;
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
            _context.SaveChanges();
        }

        public IEnumerable<DbModel> GetAll(bool asNoTraking = false)
        {
            return asNoTraking ?
                await _context.Set<DbModel>().ToList() :
                await _context.Set<DbModel>().AsNoTracking().ToList();
        }

        public void Update(DbModel model)
        {
            //var toUpdate = _context.Set<DbModel>().FirstOrDefault(m => m.Id == model.Id);
            //if (toUpdate != null)
            //{
            //    toUpdate = model;
            //}
            //_context.Update(toUpdate);
            _context.Entry(model).State = EntityState.Modified;
        }

        public DbModel Get(Guid id)
        {
            return _context.Set<DbModel>().FirstOrDefault(m => m.Id == id);
        }

        public async Task<IEnumerable<DbModel>> GetAllAsync(bool asNoTraking = false)
        {
            return asNoTraking?
                await _context.Set<DbModel>().ToListAsync() :
                await _context.Set<DbModel>().AsNoTracking().ToListAsync();
        }

        public async Task<DbModel> GetAsync(Guid id, bool asNoTraking = false)
        {    
            return asNoTraking?
                await _context.Set<DbModel>()?.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) :
                await _context.Set<DbModel>()?.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateAsync(DbModel model)
        {
            _context.Set<DbModel>().Add(model);
        }

        public async Task UpdateAsync(DbModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
            //var toUpdate = await GetAsync(model.Id, true);
            //if (toUpdate != null)
            //{
            //    toUpdate = model;
            //    _context.Update(toUpdate);
            //    await _context.SaveChangesAsync();
            //    return toUpdate;
            //}
            //return null;

        }

        public async Task DeleteAsync(Guid id)
        {
            var toDelete = await GetAsync(id);
            _context.Set<DbModel>().Remove(toDelete);
        }

        public async Task<bool> EntityExists(Guid id)
        {
            return await _context.Set<DbModel>().AsNoTracking().AnyAsync(e => e.Id == id);
        }

        public async Task<bool> IsEmpty()
        {
            return !await _context.Set<DbModel>().AsNoTracking().AnyAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<DbModel> models)
        {
            await _context.Set<DbModel>().AddRangeAsync(models);
        }
        public async Task DeleteRangeAsync(IEnumerable<DbModel> models)
        {
            _context.Set<DbModel>().RemoveRange(models);
        }
    }
}
