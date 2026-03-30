using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;

namespace guest_house_management_backend.Repositories.AuditRepo
{
    public interface IAuditRepository
    {
        Task<Paging<AuditResponceDto>> fetchAduitLogs(int pageSize, int currentPage, string EntityName, string id);
    }
}
