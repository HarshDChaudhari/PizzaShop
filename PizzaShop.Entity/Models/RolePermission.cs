using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class RolePermission
{
    public int RolePermissionId { get; set; }

    public int? PermissionId { get; set; }

    public int? RoleId { get; set; }

    public bool CanView { get; set; }

    public bool CanAddEdit { get; set; }

    public bool CanDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Role? Role { get; set; }
}
