using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Services.Interfaces;
using UseCases.Common.Queries.GetEntity;

namespace UseCases.Product.Queries
{
    public class GetProductQuery : GetEntityQuery<ProductDto> 
    {
    }
}
