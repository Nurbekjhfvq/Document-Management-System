using Microsoft.AspNetCore.Mvc;
using Document.Services;
using Document.Dto;

namespace Document.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly VersionService _service;

        public VersionController(VersionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddVersion([FromBody] CreateVersionDto dto)
        {
            try
            {
                int id = await _service.AddVersionAsync(dto);
                return CreatedAtAction(nameof(GetVersionById), new { versionId = id }, new { VersionId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{versionId}")]
        public async Task<IActionResult> GetVersionById(int versionId)
        {
            var version = await _service.GetVersionByIdAsync(versionId);
            if (version == null) return NotFound();
            return Ok(version);
        }

        [HttpGet("document/{documentId}")]
        public async Task<IActionResult> GetVersionsByDocumentId(int documentId)
        {
            var versions = await _service.GetVersionsByDocumentIdAsync(documentId);
            return Ok(versions);
        }

        [HttpDelete("{versionId}")]
        public async Task<IActionResult> DeleteVersion(int versionId)
        {
            bool deleted = await _service.DeleteVersionAsync(versionId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
