using FastReportGenerator.Context;
using FastReportGenerator.Models;

namespace FastReportGenerator.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public List<Category> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }
    }
}
