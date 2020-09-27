using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;
using Services;
using Services.Interfaces;
using UseCases.Common.Queries.GetEntity;

namespace UseCases.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler : GetEntityQueryHandler<GetOrderQuery, Domain.Order, OrderDto>
    {
        public GetOrderQueryHandler(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
