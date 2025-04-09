using Document.Data;
using Document.Dto;
using Document.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Document.Service;

public class DocumentService
{
    private readonly AppDbContext _context;
    private static readonly string[] _allowedExtensions = new[] { ".jpg", ".png", ".pdf", ".docx", ".zip" };
    private const long MaxFileSize = 10 * 1024 * 1024;

    public DocumentService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
    }
    public async Task<int> UploadAsync([FromForm] CreateDocumentDto dto, IFormFile file)
    {
        if (file.Length > MaxFileSize)
            throw new Exception("Fayl hajmi 10MB dan katta bo'lmasligi kerak");

        var ext = Path.GetExtension(file.FileName);
        if (!_allowedExtensions.Contains(ext))
            throw new Exception("Fayl turi qo‘llab-quvvatlanmaydi");

        // Category nomini olish (categoryId asosida)
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null)
            throw new Exception("Kategoriya topilmadi");

        var categoryName = category.Name;

        if (dto.IsFile)
        {
            // Faylni yuklash joyini tayyorlash
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file", categoryName);

            // Agar folder mavjud bo'lmasa, yaratamiz
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, Path.GetFileName(file.FileName));

            // Faylni saqlash
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Entity.Document
            {
                Name = dto.Name ?? Path.GetFileNameWithoutExtension(file.FileName),
                FileType = ext,
                Size = file.Length,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                LastAccessedDate = DateTime.UtcNow,
                IsArchived = false,
                IsFile = true
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document.Id;
        }
        else
        {
            // Faylni database'ga saqlash
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            var document = new Entity.Document
            {
                Name = dto.Name ?? Path.GetFileNameWithoutExtension(file.FileName),
                FileContent = fileData,
                FileType = ext,
                Size = file.Length,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                LastAccessedDate = DateTime.UtcNow,
                IsArchived = false,
                IsFile = false
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document.Id;
        }
    }


    public async Task<List<DocumentDto>> GetUserDocumentsAsync(int userId, DocumentFilterDto filter)
    {
        var query = _context.Documents.Where(d => d.UserId == userId);
        query = ApplyFilters(query, filter);
        return await ProjectToDto(query).ToListAsync();
    }

    public async Task<List<DocumentDto>> GetAllDocumentsAsync(DocumentFilterDto filter)
    {
        var query = ApplyFilters(_context.Documents.AsQueryable(), filter);
        return await ProjectToDto(query).ToListAsync();
    }

    public async Task<bool> DeleteAsync(int documentId, int userId, bool isAdmin)
    {
        var doc = await _context.Documents.FindAsync(documentId);
        if (doc == null || (!isAdmin && doc.UserId != userId))
            return false;

        _context.Documents.Remove(doc);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<DocumentDto?> GetDocumentByIdAsync(int id, int userId, bool isAdmin)
    {
        var document = await _context.Documents
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id && (isAdmin || d.UserId == userId));

        if (document == null)
            return null;

        document.LastAccessedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new DocumentDto
        {
            Id = document.Id,
            Name = document.Name,
            Size = document.Size,
            FileType = document.FileType,
            CreatedAt = document.CreatedAt,
            IsArchived = document.IsArchived,
            UserId = document.UserId,
            CategoryId = document.CategoryId,
            Category = document.Category.Name
        };
    }

    public async Task<bool> RenameAsync(int documentId, int userId, string newName, bool isAdmin)
    {
        var doc = await _context.Documents.FindAsync(documentId);
        if (doc == null || (!isAdmin && doc.UserId != userId))
            return false;

        doc.Name = newName;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<byte[]> DownloadAsZipAsync(List<int> documentIds, int userId, bool isAdmin)
    {
        var docs = await _context.Documents
            .Where(d => documentIds.Contains(d.Id) && (isAdmin || d.UserId == userId))
            .ToListAsync();

        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var doc in docs)
            {
                if (doc.FileContent == null || doc.FileContent.Length == 0)
                    continue;

                var entry = archive.CreateEntry($"{doc.Name}{doc.FileType}");
                using var entryStream = entry.Open();
                using var fileStream = new MemoryStream(doc.FileContent);
                await fileStream.CopyToAsync(entryStream);
            }
        }

        return memoryStream.ToArray();
    }

    private IQueryable<DocumentDto> ProjectToDto(IQueryable<Entity.Document> query)
    {
        return query.Select(d => new DocumentDto
        {
            Id = d.Id,
            Name = d.Name,
            Size = d.Size,
            FileType = d.FileType,
            CreatedAt = d.CreatedAt,
            IsArchived = d.IsArchived,
            UserId = d.UserId,
            CategoryId = d.CategoryId,
            Category = d.Category.Name
        });
    }

    private IQueryable<Entity.Document> ApplyFilters(IQueryable<Entity.Document> query, DocumentFilterDto filter)
    {
        if (filter.MinSize.HasValue)
            query = query.Where(d => d.Size >= filter.MinSize.Value);
        if (filter.MaxSize.HasValue)
            query = query.Where(d => d.Size <= filter.MaxSize.Value);
        if (!string.IsNullOrWhiteSpace(filter.FileType))
            query = query.Where(d => d.FileType == filter.FileType);
        if (filter.CreatedFrom.HasValue)
            query = query.Where(d => d.CreatedAt >= filter.CreatedFrom.Value);
        if (filter.CreatedTo.HasValue)
            query = query.Where(d => d.CreatedAt <= filter.CreatedTo.Value);
        return query;
    }
}
