using Document.Service;
using Microsoft.AspNetCore.Mvc;

namespace Document.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveController : ControllerBase
    {
        private readonly ArchiveService _archiveService;

        public ArchiveController(ArchiveService archiveService)
        {
            _archiveService = archiveService;
        }

        [HttpPost("{documentId}")]
        public async Task<IActionResult> ArchiveDocument(int documentId)
        {
            var result = await _archiveService.ArchiveDocumentAsync(documentId);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _archiveService.GetAllAsync();
            return Ok(result);
        }
    }
}
