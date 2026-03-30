using AutoMapper;
using AutoMapper.QueryableExtensions;
using guest_house_management_backend.Data;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.DTOs.Paging;
using Microsoft.EntityFrameworkCore;

namespace guest_house_management_backend.Repositories.AuditRepo
{
    public class AuditRepository : IAuditRepository
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public AuditRepository(DBContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Paging<AuditResponceDto>> fetchAduitLogs(int pageSize , int currentPage ,string EntityName, string id)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(EntityName))
            {
                query = query.Where(entry => entry.EntityName.Contains(EntityName));
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(entry => entry.EntityId.Equals(id));
            }

            int totalCount = query.Count();
            var res = await query.Include(entry=>entry.User)
                .OrderByDescending(entery=>entery.Timestamp)
                .Skip((currentPage-1)*pageSize)
                .Take(pageSize)
                .ProjectTo<AuditResponceDto>(_mapper.ConfigurationProvider).ToListAsync();


            return new Paging<AuditResponceDto>
            {
                Data = res,
                MetaData =
                {   
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = currentPage
                }
            };
        }
    }
}
