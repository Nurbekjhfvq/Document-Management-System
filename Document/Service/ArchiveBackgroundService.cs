// ArchiveJob.cs

using Document.Data;
using Document.Service;
using Microsoft.EntityFrameworkCore;

public class ArchiveJob
{
    private readonly AppDbContext _dbContext;
    private readonly ArchiveService _archiveService;
    private readonly ILogger<ArchiveJob> _logger;

    public ArchiveJob(AppDbContext dbContext, ArchiveService archiveService, ILogger<ArchiveJob> logger)
    {
        _dbContext = dbContext;
        _archiveService = archiveService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var outdatedDocuments = await _dbContext.Documents
            .Where(d =>
                d.LastAccessedDate <= DateTime.UtcNow.AddDays(-30) &&
                !d.Archives.Any() &&
                !d.IsFile) // Faqat databasega saqlangan fayllar
            .ToListAsync();

        foreach (var document in outdatedDocuments)
        {
            var result = await _archiveService.ArchiveDocumentAsync(document.Id);
            if (result != null)
            {
                _logger.LogInformation($"Document {document.Id} archived successfully.");
            }
            else
            {
                _logger.LogWarning($"Failed to archive document {document.Id}.");
            }
        }
    }


}
