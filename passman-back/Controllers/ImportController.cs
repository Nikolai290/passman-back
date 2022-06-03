using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using passman_back.Business.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace passman_back.Controllers {

    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/import")]
    public class ImportController : ControllerBase {
        private readonly IImportService importService;
        public ImportController(
            IImportService importService
        ) {
            this.importService = importService;

        }

        [HttpPost]
        public async Task<ActionResult> ImportAsync(IFormFile file) {
            try {
                await importService.ImportAsync(file);
                return Ok(file.FileName);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("hierarchy/passman")]
        public async Task<ActionResult> ImportHierarchyPassmanAsync(IFormFile file) {
            try {
                await importService.ImportHierarchyPassmanAsync(file);
                return Ok(file.FileName);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("hierarchy/bitwarden")]
        public async Task<ActionResult> ImportHierarchyBitwardenAsync(IFormFile file) {
            try {
                await importService.ImportHierarchyBitwardenAsync(file);
                return Ok(file.FileName);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
    }
}
