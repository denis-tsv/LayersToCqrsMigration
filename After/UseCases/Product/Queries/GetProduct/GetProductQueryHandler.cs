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
using UseCases.Common.Queries.GetEntity;

namespace UseCases.Product.Queries.GetProduct
{
    public class GetProductQueryHandler : GetEntityQueryHandler<GetProductQuery, Domain.Product, ProductDto>
    {
        public GetProductQueryHandler(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
