using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;
using Services;
using Services.Interfaces;

namespace UseCases.Product.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ProductDto> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Products.FindAsync(query.Id);
            if (entity == null) throw new EntityNotFoundException();
            return _mapper.Map<ProductDto>(entity);
        }
    }
}
