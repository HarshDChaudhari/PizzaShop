using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Entity.Data;

public partial class PizzaShopDbContext : DbContext
{
    public PizzaShopDbContext()
    {
    }

    public PizzaShopDbContext(DbContextOptions<PizzaShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryItem> CategoryItems { get; set; }

    public virtual DbSet<CategoryModifierMapping> CategoryModifierMappings { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<MappingItemModifier> MappingItemModifiers { get; set; }

    public virtual DbSet<ModifierGroup> ModifierGroups { get; set; }

    public virtual DbSet<ModifierItem> ModifierItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderItemModifier> OrderItemModifiers { get; set; }

    public virtual DbSet<OrderTaxMapping> OrderTaxMappings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<TableDetail> TableDetails { get; set; }

    public virtual DbSet<TableOrderMapping> TableOrderMappings { get; set; }

    public virtual DbSet<TaxesAndFee> TaxesAndFees { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<OrderItemFlat> OrderItemFlat { get; set; }

    public virtual DbSet<WaitingList> WaitingLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Database=PizzaShop;User id=postgres;Password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OrderItemFlat>(entity =>
        {
            entity.HasNoKey(); // Mark as keyless
        });

        modelBuilder.Entity<OrderFlatDto>(entity =>
        {
            entity.HasNoKey();
            // Optional: map column names if different from property names here

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.OrderInstruction).HasColumnName("order_instruction");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.TableId).HasColumnName("table_id");
            entity.Property(e => e.TableName).HasColumnName("table_name");
            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.SectionName).HasColumnName("section_name");
            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReadyQuantity).HasColumnName("ready_quantity");
            entity.Property(e => e.OrderItemInstruction).HasColumnName("order_item_instruction");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
            entity.Property(e => e.OrderItemModifierId).HasColumnName("order_item_modifier_id");
            entity.Property(e => e.ModifierItemName).HasColumnName("modifier_item_name");
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsDeleted1)
                .HasDefaultValue(false)
                .HasColumnName("Is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
        });

        modelBuilder.Entity<CategoryItem>(entity =>
        {
            entity.HasKey(e => e.CategoryItemId).HasName("category_item_pkey");

            entity.ToTable("category_item");

            entity.Property(e => e.CategoryItemId).HasColumnName("category_item_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.DefaultTax)
                .HasDefaultValue(true)
                .HasColumnName("default_tax");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.IsAvailable)
                .HasDefaultValue(true)
                .HasColumnName("is_available");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsFavourite)
                .HasDefaultValue(false)
                .HasColumnName("is_favourite");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .HasColumnName("item_name");
            entity.Property(e => e.ItemType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'veg'::character varying")
                .HasColumnName("item_type");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShortCode)
                .HasMaxLength(50)
                .HasColumnName("short_code");
            entity.Property(e => e.TaxPercentage).HasColumnName("tax_percentage");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryItems)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("category_item_category_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.CategoryItems)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("category_item_unit_id_fkey");
        });

        modelBuilder.Entity<CategoryModifierMapping>(entity =>
        {
            entity.HasKey(e => e.CategoryModifierMappingId).HasName("category_modifier_mapping_pkey");

            entity.ToTable("category_modifier_mapping");

            entity.Property(e => e.CategoryModifierMappingId).HasColumnName("category_modifier_mapping_id");
            entity.Property(e => e.CategoryItemId).HasColumnName("category_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.MaxValue).HasColumnName("max_value");
            entity.Property(e => e.MinValue).HasColumnName("min_value");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");

            entity.HasOne(d => d.CategoryItem).WithMany(p => p.CategoryModifierMappings)
                .HasForeignKey(d => d.CategoryItemId)
                .HasConstraintName("category_modifier_mapping_category_item_id_fkey");

            entity.HasOne(d => d.Modifier).WithMany(p => p.CategoryModifierMappings)
                .HasForeignKey(d => d.ModifierId)
                .HasConstraintName("category_modifier_mapping_modifier_id_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("cities_pkey");

            entity.ToTable("cities");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(255)
                .HasColumnName("city_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.StateId).HasColumnName("state_id");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("cities_state_id_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("countries_pkey");

            entity.ToTable("countries");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(10)
                .HasColumnName("country_code");
            entity.Property(e => e.CountryName)
                .HasMaxLength(255)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.Email, "customer_email_key").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(255)
                .HasColumnName("customer_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("feedback_pkey");

            entity.ToTable("feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.AmbienceRating).HasColumnName("ambience_rating");
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FoodRating).HasColumnName("food_rating");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ServiceRating).HasColumnName("service_rating");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("feedback_customer_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("feedback_order_id_fkey");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FinalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("final_amount");
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("issued_at");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TableId).HasColumnName("table_id");
            entity.Property(e => e.TaxId).HasColumnName("tax_id");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("invoice_customer_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("invoice_order_id_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("invoice_table_id_fkey");

            entity.HasOne(d => d.Tax).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("invoice_tax_id_fkey");
        });

        modelBuilder.Entity<MappingItemModifier>(entity =>
        {
            entity.HasKey(e => e.MappingItem).HasName("mapping_item_modifier_pkey");

            entity.ToTable("mapping_item_modifier");

            entity.Property(e => e.MappingItem).HasColumnName("mapping_item");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.ModifierGroupId).HasColumnName("modifier_group_id");
            entity.Property(e => e.ModifierId).HasColumnName("modifier_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MappingItemModifierCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("mapping_item_modifier_created_by_fkey");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MappingItemModifierModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("mapping_item_modifier_modified_by_fkey");

            entity.HasOne(d => d.ModifierGroup).WithMany(p => p.MappingItemModifiers)
                .HasForeignKey(d => d.ModifierGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mapping_item_modifier_modifier_group_id_fkey");

            entity.HasOne(d => d.Modifier).WithMany(p => p.MappingItemModifiers)
                .HasForeignKey(d => d.ModifierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mapping_item_modifier_modifier_id_fkey");
        });

        modelBuilder.Entity<ModifierGroup>(entity =>
        {
            entity.HasKey(e => e.ModifierGroupId).HasName("modifier_group_pkey");

            entity.ToTable("modifier_group");

            entity.Property(e => e.ModifierGroupId).HasColumnName("modifier_group_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifierName)
                .HasMaxLength(50)
                .HasColumnName("modifier_name");
        });

        modelBuilder.Entity<ModifierItem>(entity =>
        {
            entity.HasKey(e => e.ModifierItemId).HasName("modifier_items_pkey");

            entity.ToTable("modifier_items");

            entity.Property(e => e.ModifierItemId).HasColumnName("modifier_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifierItemName)
                .HasMaxLength(255)
                .HasColumnName("modifier_item_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Unit).WithMany(p => p.ModifierItems)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("modifier_items_unit_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Instruction)
                .HasColumnType("character varying")
                .HasColumnName("instruction");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.NoOfPerson).HasColumnName("no_of_person");
            entity.Property(e => e.OrderType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Dine-In'::character varying")
                .HasColumnName("order_type");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_customer_id_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("order_item_pkey");

            entity.ToTable("order_item");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.CategoryItemId).HasColumnName("category_item_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Instruction).HasColumnName("instruction");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ReadyQuantity)
                .HasDefaultValue(0)
                .HasColumnName("ready_quantity");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.CategoryItem).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.CategoryItemId)
                .HasConstraintName("order_item_category_item_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_item_order_id_fkey");
        });

        modelBuilder.Entity<OrderItemModifier>(entity =>
        {
            entity.HasKey(e => e.OrderItemModifierId).HasName("order_item_modifier_pkey");

            entity.ToTable("order_item_modifier");

            entity.Property(e => e.OrderItemModifierId).HasColumnName("order_item_modifier_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifierItemId).HasColumnName("modifier_item_id");
            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");

            entity.HasOne(d => d.ModifierItem).WithMany(p => p.OrderItemModifiers)
                .HasForeignKey(d => d.ModifierItemId)
                .HasConstraintName("order_item_modifier_modifier_item_id_fkey");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.OrderItemModifiers)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("order_item_modifier_order_item_id_fkey");
        });

        modelBuilder.Entity<OrderTaxMapping>(entity =>
        {
            entity.HasKey(e => e.OrderTaxId).HasName("order_tax_mapping_pkey");

            entity.ToTable("order_tax_mapping");

            entity.Property(e => e.OrderTaxId).HasColumnName("order_tax_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TaxId).HasColumnName("tax_id");
            entity.Property(e => e.TaxType)
                .HasColumnType("character varying")
                .HasColumnName("tax_type");
            entity.Property(e => e.TaxValue).HasColumnName("tax_value");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderTaxMappings)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_tax_mapping_order_id_fkey");

            entity.HasOne(d => d.Tax).WithMany(p => p.OrderTaxMappings)
                .HasForeignKey(d => d.TaxId)
                .HasConstraintName("order_tax_mapping_tax_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("payment_order_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModuleName)
                .HasMaxLength(255)
                .HasColumnName("module_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("role_permission_pkey");

            entity.ToTable("role_permission");

            entity.Property(e => e.RolePermissionId).HasColumnName("role_permission_id");
            entity.Property(e => e.CanAddEdit)
                .HasDefaultValue(false)
                .HasColumnName("can_add_edit");
            entity.Property(e => e.CanDelete)
                .HasDefaultValue(false)
                .HasColumnName("can_delete");
            entity.Property(e => e.CanView)
                .HasDefaultValue(false)
                .HasColumnName("can_view");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("role_permission_permission_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("role_permission_role_id_fkey");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("sections_pkey");

            entity.ToTable("sections");

            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.SectionName)
                .HasMaxLength(255)
                .HasColumnName("section_name");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("states_pkey");

            entity.ToTable("states");

            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.StateName)
                .HasMaxLength(255)
                .HasColumnName("state_name");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("states_country_id_fkey");
        });

        modelBuilder.Entity<TableDetail>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("table_details_pkey");

            entity.ToTable("table_details");

            entity.Property(e => e.TableId).HasColumnName("table_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'available'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.TableName)
                .HasMaxLength(255)
                .HasColumnName("table_name");

            entity.HasOne(d => d.Section).WithMany(p => p.TableDetails)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("table_details_section_id_fkey");
        });

        modelBuilder.Entity<TableOrderMapping>(entity =>
        {
            entity.HasKey(e => e.TableOrderMappingId).HasName("table_order_mapping_pkey");

            entity.ToTable("table_order_mapping");

            entity.Property(e => e.TableOrderMappingId).HasColumnName("table_order_mapping_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TableId).HasColumnName("table_id");

            entity.HasOne(d => d.Order).WithMany(p => p.TableOrderMappings)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("table_order_mapping_order_id_fkey");

            entity.HasOne(d => d.Table).WithMany(p => p.TableOrderMappings)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("table_order_mapping_table_id_fkey");
        });

        modelBuilder.Entity<TaxesAndFee>(entity =>
        {
            entity.HasKey(e => e.TaxId).HasName("taxes_and_fees_pkey");

            entity.ToTable("taxes_and_fees");

            entity.Property(e => e.TaxId).HasColumnName("tax_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(true)
                .HasColumnName("is_default");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("is_enabled");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.TaxName)
                .HasMaxLength(255)
                .HasColumnName("tax_name");
            entity.Property(e => e.TaxType)
                .HasMaxLength(50)
                .HasColumnName("tax_type");
            entity.Property(e => e.TaxValue)
                .HasPrecision(5, 2)
                .HasColumnName("tax_value");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("unit_pkey");

            entity.ToTable("unit");

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.UnitName)
                .HasMaxLength(50)
                .HasColumnName("unit_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("img_url");
            entity.Property(e => e.Isdeleted).HasDefaultValue(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
            entity.Property(e => e.ZipCode).HasColumnName("zip_code");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("users_city_id_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("users_country_id_fkey");

            entity.HasOne(d => d.State).WithMany(p => p.Users)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("users_state_id_fkey");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .HasConstraintName("users_user_role_fkey");
        });

        modelBuilder.Entity<WaitingList>(entity =>
        {
            entity.HasKey(e => e.WaitingId).HasName("waiting_list_pkey");

            entity.ToTable("waiting_list");

            entity.Property(e => e.WaitingId).HasColumnName("waiting_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasColumnName("created_by");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .HasColumnName("modified_by");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.SectionId).HasColumnName("section_id");
            entity.Property(e => e.TotalPerson).HasColumnName("total_person");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");

            entity.HasOne(d => d.Section).WithMany(p => p.WaitingLists)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("waiting_list_section_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
