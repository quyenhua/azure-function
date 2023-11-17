﻿
using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class AuditableEntity : IAuditableEntity
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string LastModifiedBy { get; set; }

    public List<DomainEvent> DomainEvents { get; set; }
}
