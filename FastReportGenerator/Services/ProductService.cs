using FastReportGenerator.Context;
using FastReportGenerator.Models;

namespace FastReportGenerator.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            var products = _context.Products.ToList();
            return products;
        }
    }
}
