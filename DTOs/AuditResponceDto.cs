namespace guest_house_management_backend.DTOs
{
    public class AuditResponceDto
    {
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string EntityName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
