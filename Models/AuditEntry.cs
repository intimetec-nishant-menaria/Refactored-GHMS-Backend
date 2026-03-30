using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Helpers
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry) => Entry = entry;

        public EntityEntry Entry { get; }
        public int UserId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAudit()
        {
            return new AuditLog
            {
                UserId = UserId,
                EntityName = EntityName,
                EntityId = EntityId,
                Action = Action,
                Timestamp = DateTime.UtcNow,
                OldValue = OldValues.Count == 0 ? "{}" : JsonSerializer.Serialize(OldValues),
                NewValue = NewValues.Count == 0 ? "{}" : JsonSerializer.Serialize(NewValues)
            };
        }
    }
}