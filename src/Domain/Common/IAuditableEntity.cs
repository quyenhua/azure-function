﻿using Domain.Common;

namespace Domain.Common;

public interface IAuditableEntity : IEntity
{
    public DateTime Created { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string LastModifiedBy { get; set; }
}
