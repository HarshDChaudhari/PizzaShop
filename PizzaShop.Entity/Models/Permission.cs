using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string ModuleName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
