using Microsoft.EntityFrameworkCore;
using Project.Entities;

namespace Project.Repository
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity> where TEntity : AbstractEntity where TContext : DbContext
    {
        private readonly TContext _dbContext;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository(TContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public async Task<TEntity> GetById(int id)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var createdEntity = await DbSet.AddAsync(entity);
            return createdEntity.Entity;
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            DbSet.Remove(entity);
        }
    }
}
