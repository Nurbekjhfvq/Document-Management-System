using Document.Dto;
using Document.Service;
using Microsoft.AspNetCore.Mvc;

namespace Document.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;

        public DocumentController(DocumentService documentService)
        {
            _documentService = documentService;
        }
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] CreateDocumentDto dto)
    {
        try
        {
            var id = await _documentService.UploadAsync(dto, dto.File);
            return Ok(new { DocumentId = id, Message = "Fayl muvaffaqiyatli yuklandi." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id, [FromQuery] bool isAdmin, [FromQuery] int userId)
        {
            var document = await _documentService.GetDocumentByIdAsync(id, userId, isAdmin);
            if (document == null)
                return NotFound();

            return Ok(document);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserDocuments(int userId, [FromQuery] DocumentFilterDto filter)
        {
            var docs = await _documentService.GetUserDocumentsAsync(userId, filter);
            return Ok(docs);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllDocuments([FromQuery] DocumentFilterDto filter)
        {
            var docs = await _documentService.GetAllDocumentsAsync(filter);
            return Ok(docs);
        }

        [HttpDelete("{documentId}")]
        public async Task<IActionResult> Delete(int documentId, [FromQuery] int userId, [FromQuery] bool isAdmin)
        {
            var result = await _documentService.DeleteAsync(documentId, userId, isAdmin);
            return result ? Ok() : NotFound("Fayl topilmadi yoki ruxsatsiz o‘chirishga urinish");
        }

        [HttpPut("rename/{documentId}")]
        public async Task<IActionResult> Rename(int documentId, [FromQuery] int userId, [FromQuery] string newName, [FromQuery] bool isAdmin)
        {
            var result = await _documentService.RenameAsync(documentId, userId, newName, isAdmin);
            return result ? Ok() : NotFound("Fayl topilmadi yoki ruxsatsiz o‘zgartirishga urinish");
        }

        [HttpPost("download-zip")]
        public async Task<IActionResult> DownloadAsZip([FromBody] List<int> documentIds, [FromQuery] int userId, [FromQuery] bool isAdmin)
        {
            var zipBytes = await _documentService.DownloadAsZipAsync(documentIds, userId, isAdmin);
            return File(zipBytes, "application/zip", "documents.zip");
        }
    }
}
