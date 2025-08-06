using System;
using System.Collections.Generic;

namespace PizzaShop.Entity.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string? Status { get; set; }

    public float? TotalAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? OrderType { get; set; }

    public string? Instruction { get; set; }

    public int? NoOfPerson { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderTaxMapping> OrderTaxMappings { get; set; } = new List<OrderTaxMapping>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<TableOrderMapping> TableOrderMappings { get; set; } = new List<TableOrderMapping>();
}
