using AutoMapper;
using Document.Dto;
using Document.Entity;
using Document.Data;
using Microsoft.EntityFrameworkCore;
using Version = Document.Entity.Version;

namespace Document.Services
{
    public class VersionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VersionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VersionDto?> GetVersionByIdAsync(int versionId)
        {
            var version = await _context.Versions
                .FirstOrDefaultAsync(v => v.Id == versionId);

            return version != null ? _mapper.Map<VersionDto>(version) : null;
        }

        public async Task<List<VersionDto>> GetVersionsByDocumentIdAsync(int documentId)
        {
            var versions = await _context.Versions
                .Where(v => v.DocumentId == documentId)
                .ToListAsync();

            return _mapper.Map<List<VersionDto>>(versions);
        }

        public async Task<bool> DeleteVersionAsync(int versionId)
        {
            var version = await _context.Versions.FindAsync(versionId);
            if (version == null) return false;

            _context.Versions.Remove(version);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> AddVersionAsync(CreateVersionDto dto)
        {
            var document = await _context.Documents.FindAsync(dto.DocumentId);
            if (document == null)
                throw new Exception("Document topilmadi");

            var version = _mapper.Map<Version>(dto);

            _context.Versions.Add(version);
            await _context.SaveChangesAsync();

            return version.Id;
        }
    }
}
