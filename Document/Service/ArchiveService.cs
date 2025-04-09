using AutoMapper;
using Document.Data;
using Document.Dto;
using Document.Entity;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Document.Service;

public class ArchiveService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public ArchiveService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
    {
        _context = context;
        _mapper = mapper;
        _env = env;
    }

public async Task<ArchiveDto?> ArchiveDocumentAsync(int documentId)
{
    var document = await _context.Documents
        .Include(d => d.Category)
        .FirstOrDefaultAsync(d => d.Id == documentId);

    if (document == null)
        throw new Exception("Fayl topilmadi.");

    if (document.IsFile)
        throw new Exception("Bu fayl allaqachon papkada saqlangan, arxivlab bo'lmaydi.");

    if (document.FileContent == null)
        throw new Exception("Fayl mazmuni topilmadi.");

    string archiveDir = Path.Combine(_env.WebRootPath, "Archives");
    Directory.CreateDirectory(archiveDir);

    string zipName = $"{Guid.NewGuid()}.zip";
    string zipFullPath = Path.Combine(archiveDir, zipName);

    using (var zip = ZipFile.Open(zipFullPath, ZipArchiveMode.Create))
    {
        var entry = zip.CreateEntry(document.Name + document.FileType);
        using var entryStream = entry.Open();
        await entryStream.WriteAsync(document.FileContent, 0, document.FileContent.Length);
    }

    var archive = new Archive
    {
        DocumentId = documentId,
        FilePath = Path.Combine("Archives", zipName).Replace("\\", "/"),
        ArchivedAt = DateTime.UtcNow
    };

    _context.Archives.Add(archive);
    _context.Documents.Remove(document);

    await _context.SaveChangesAsync();

    return _mapper.Map<ArchiveDto>(archive);
}


    public async Task<List<ArchiveDto>> GetAllAsync()
    {
        var archives = await _context.Archives.Include(a => a.Document).ToListAsync();
        return _mapper.Map<List<ArchiveDto>>(archives);
    }
}
