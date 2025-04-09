using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Document.Dto;
using Document.Entity;
using Document.Data;

namespace Document.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(UserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Bu email allaqachon ishlatilgan");

            var user = _mapper.Map<User>(dto);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(int id, UserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                throw new Exception("Bu email allaqachon boshqa user tomonidan ishlatilgan");

            _mapper.Map(dto, user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
