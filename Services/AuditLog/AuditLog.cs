using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using guest_house_management_backend.Repositories.AuditRepo;

namespace guest_house_management_backend.Services.AuditLog
{
    public class AuditLog : IAuditLog
    {
        private readonly IAuditRepository _auditRepository;

        public AuditLog(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<Paging<AuditResponceDto>> fetchAduitLogs(int pageSize, int currentPage, string EntityName, string id)
        {
            return await _auditRepository.fetchAduitLogs(pageSize, currentPage, EntityName, id);
        }
    }
}
