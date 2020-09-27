using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using MediatR;
using Services;

namespace UseCases.Common.Queries.GetEntity
{
    public abstract class GetEntityQueryHandler<TRequest, TEntity, TDto> : IRequestHandler<TRequest, TDto>
        where TRequest : GetEntityQuery<TDto>
        where TEntity : Entity
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        protected GetEntityQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public virtual async Task<TDto> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(request.Id);
            if (entity == null) throw new EntityNotFoundException();

            return _mapper.Map<TDto>(entity);
        }
    }
}
