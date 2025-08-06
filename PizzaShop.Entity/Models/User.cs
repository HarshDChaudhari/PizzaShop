using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? UserRole { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? CountryId { get; set; }

    public int? StateId { get; set; }

    public int? CityId { get; set; }

    public string? Address { get; set; }

    public int? ZipCode { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Phone { get; set; }

    public string? ImgUrl { get; set; }

    public virtual City? City { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<MappingItemModifier> MappingItemModifierCreatedByNavigations { get; set; } = new List<MappingItemModifier>();

    public virtual ICollection<MappingItemModifier> MappingItemModifierModifiedByNavigations { get; set; } = new List<MappingItemModifier>();

    public virtual State? State { get; set; }

    public virtual Role? UserRoleNavigation { get; set; }
}
