using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Services.Interfaces;

namespace UseCases.Product.Queries
{
    public class GetProductQuery : IRequest<ProductDto>
    {
        public int Id { get; set; }
    }
}
