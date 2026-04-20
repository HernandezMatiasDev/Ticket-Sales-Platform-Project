using System;

namespace TicketingSystem.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; private set; }
        public int? UserId { get; private set; }
        public string Action { get; private set; }
        public string EntityType { get; private set; }
        public string EntityId { get; private set; }
        public string Details { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public AuditLog(Guid id, int? userId, string action, string entityType, string entityId, string details, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Action = action;
            EntityType = entityType;
            EntityId = entityId;
            Details = details;
            CreatedAt = createdAt;
        }
    }
}