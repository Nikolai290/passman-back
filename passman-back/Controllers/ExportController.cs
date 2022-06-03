using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Interfaces.Services;
using passman_back.Infrastructure.Business.Enums;
using System;
using System.IO;
using System.Threading.Tasks;

namespace passman_back.Controllers {

    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/export")]
    public class ExportController : ControllerBase {
        private readonly IExportService exportService;
        public ExportController(
            IExportService exportService
        ) {
            this.exportService = exportService;
        }

        [HttpGet("{type}")]
        public async Task<ActionResult> ExportAsync(string type = "csv") {
            try {
                var stream = new MemoryStream();
                await exportService.ExportAsync(type, stream);

                return new FileStreamResult(stream, Export.GetContentType(type)) {
                    FileDownloadName = "exportFile." + type
                };
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("hierarchy/passman")]
        public async Task<ActionResult> ExportHierarchyAsync([FromQuery] bool encrypted) {
            try {
                var type = "json";
                var stream = new MemoryStream();
                await exportService.ExportHierarchyPassmanAsync(stream, encrypted);

                return new FileStreamResult(stream, Export.GetContentType(type)) {
                    FileDownloadName = "exportFile." + type
                };
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
