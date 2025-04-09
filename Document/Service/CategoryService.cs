using Microsoft.EntityFrameworkCore;
using Document.Dto;
using Document.Entity;
using Document.Data;
using AutoMapper;

namespace Document.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(CategoryDto dto)
        {
            if (await _context.Categories.AnyAsync(c => c.Name == dto.Name))
                throw new Exception("Bu nomdagi kategoriya allaqachon mavjud");

            var category = _mapper.Map<Category>(dto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateAsync(int id, CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _mapper.Map(dto, category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
