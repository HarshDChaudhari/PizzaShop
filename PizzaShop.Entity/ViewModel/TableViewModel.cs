using PizzaShop.Entity.Models;

namespace PizzaShop.Entity.ViewModel;

public class TableViewModel
{
     public int TableId { get; set; }
     public float? TableAmount { get; set; }

    public int? SectionId { get; set; }

    public string TableName { get; set; } = null!;
    public string SectionName { get; set; } = null!;

    public int? Capacity { get; set; }
    public int? OrderId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt {get; set;}

    public TableOrderMapping? TableOrderMappings { get; set; } = null!;

}
