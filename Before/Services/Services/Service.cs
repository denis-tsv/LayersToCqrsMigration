using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using Services.Interfaces;

namespace Services
{
    public abstract class Service<TEntity, TDto> : IService<TDto>
        where TEntity : Entity, new()
    {
        protected readonly IDbContext _dbContext;
        protected readonly IMapper _mapper;

        protected Service(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> GetAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null) throw new EntityNotFoundException();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task UpdateAsync(int id, TDto dto)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity == null) throw new EntityNotFoundException();
            _mapper.Map(dto, entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            _dbContext.Set<TEntity>().Remove(new TEntity {Id = id});
            await _dbContext.SaveChangesAsync();
        }
    }
}
