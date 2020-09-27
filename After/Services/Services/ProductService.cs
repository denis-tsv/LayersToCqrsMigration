using AutoMapper;
using Domain;
using Infrastructure.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class ProductService : Service<Product, ProductDto>, IProductService
    {
        public ProductService(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }
    }
}
