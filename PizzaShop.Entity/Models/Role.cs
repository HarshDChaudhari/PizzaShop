using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PizzaShop.Entity.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
