using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Services.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Ops")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLog _auditLogService;

        public AuditLogController(IAuditLog auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<ActionResult<Paging<AuditResponceDto>>> fetchAuditLogs([FromQuery] int pageSize , [FromQuery] int currentPage , [FromQuery] string entityName , [FromQuery] string id)
        {
            return await _auditLogService.fetchAduitLogs(pageSize, currentPage, entityName, id);
        }

    }
}
