using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class OrderAppRepository(PizzaShopDbContext _context) : IOrderAppRepository
{

    // public List<Order> GetOrdersByCategoryAndStatus(string category, string status)
    // {
    //     try
    //     {
    //         var query = _context.Orders.Where(o => o.Status == "InProgress" || o.Status == "Ready")
    //             .Include(o => o.Customer)
    //         .Include(o => o.TableOrderMappings)
    //             .ThenInclude(ot => ot.Table)
    //                 .ThenInclude(t => t.Section)
    //         .Include(o => o.OrderItems)
    //             .ThenInclude(oi => oi.CategoryItem)
    //                 .ThenInclude(i => i.Category)
    //         .Include(o => o.OrderItems)
    //             .ThenInclude(oi => oi.OrderItemModifiers)
    //                 .ThenInclude(oim => oim.ModifierItem);

    //         var orders = query.ToList();

    //         if (category != "All")
    //         {
    //             foreach (var order in orders)
    //             {
    //                 order.OrderItems = order.OrderItems
    //                     .Where(oi => oi.CategoryItem.Category.CategoryName == category)
    //                     .ToList();
    //             }
    //             orders = orders.Where(o => o.OrderItems.Any()).ToList();
    //         }
    //         if (status == "InProgress")
    //         {

    //             foreach (var order in orders)
    //             {

    //                 order.OrderItems = order.OrderItems
    //                     .Where(oi => oi.Quantity - oi.ReadyQuantity > 0)
    //                     .ToList();
    //             }
    //             orders = orders.Where(o => o.OrderItems.Any()).ToList();
    //         }
    //         else
    //         {
    //             foreach (var order in orders)
    //             {
    //                 order.OrderItems = order.OrderItems
    //                     .Where(oi => oi.ReadyQuantity > 0)
    //                     .ToList();
    //             }
    //             orders = orders.Where(o => o.OrderItems.Any()).ToList();
    //         }

    //         return orders;

    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return new List<Order>();
    //     }
    // }

    public List<Order> GetOrdersByCategoryAndStatus(string category, string status)
    {
        try
        {
            // Use NpgsqlParameter for safety
            var paramCategory = new NpgsqlParameter("p_category", category ?? (object)DBNull.Value);
            var paramStatus = new NpgsqlParameter("p_status", status ?? (object)DBNull.Value);

            // Call the function - raw SQL query mapping to DTO
            var flatResults = _context.Set<OrderFlatDto>()
                .FromSqlRaw("SELECT * FROM GetOrdersByCategoryAndStatus(@p_category, @p_status)", paramCategory, paramStatus)
                .AsNoTracking()
                .ToList();

            // Group flat results into Order entities
            var orders = flatResults.GroupBy(r => r.OrderId).Select(g =>
            {
                var first = g.First();
                var order = new Order
                {
                    OrderId = first.OrderId,
                    Status = first.OrderStatus,
                    Instruction = first.OrderInstruction,
                    CreatedAt = first.CreatedAt,
                    Customer = new Customer
                    {
                        CustomerId = first.CustomerId,
                        CustomerName = first.CustomerName
                    },
                    TableOrderMappings = g
                        .Select(r => r.TableId)
                        .Distinct()
                        .Select(tableId =>
                        {
                            var t = g.First(x => x.TableId == tableId);
                            return new TableOrderMapping
                            {
                                Table = new TableDetail
                                {
                                    TableId = t.TableId,
                                    TableName = t.TableName,
                                    Section = new Section
                                    {
                                        SectionId = t.SectionId,
                                        SectionName = t.SectionName
                                    }
                                }
                            };
                        }).ToList(),
                    OrderItems = g.GroupBy(r => r.OrderItemId).Select(oiGroup =>
                    {
                        var oiFirst = oiGroup.First();
                        var orderItem = new OrderItem
                        {
                            OrderItemId = oiFirst.OrderItemId,
                            Quantity = oiFirst.Quantity,
                            ReadyQuantity = oiFirst.ReadyQuantity,
                            Instruction = oiFirst.OrderItemInstruction,
                            CategoryItem = new CategoryItem
                            {
                                ItemName = oiFirst.ItemName,
                                Category = new Category
                                {
                                    CategoryId = oiFirst.CategoryId,
                                    CategoryName = oiFirst.CategoryName
                                }
                            },
                            OrderItemModifiers = oiGroup
                                .Where(m => m.OrderItemModifierId.HasValue)
                                .Select(m => new OrderItemModifier
                                {
                                    OrderItemModifierId = m.OrderItemModifierId.Value,
                                    ModifierItem = new ModifierItem
                                    {
                                        ModifierItemName = m.ModifierItemName
                                    }
                                }).ToList()
                        };
                        return orderItem;
                    }).ToList()
                };
                return order;
            }).ToList();

            return orders;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<Order>();
        }
    }





    public List<string> GetAllCategories()
    {
        try
        {
            return _context.Categories
                // .Where(c => c.Isdeleted == false)
                .Select(c => c.CategoryName)
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<string>();
        }

    }

    // public List<OrderItem> GetOrdersByOrderId(string category, int orderId, string status)
    // {
    //     try
    //     {
    //         var query = _context.Orders
    //         .Include(o => o.OrderItems)
    //             .ThenInclude(oi => oi.CategoryItem)
    //                 .ThenInclude(i => i.Category)
    //         .Include(o => o.OrderItems)
    //             .ThenInclude(oi => oi.OrderItemModifiers)
    //                 .ThenInclude(oim => oim.ModifierItem)
    //                 .FirstOrDefault(o => o.OrderId == orderId);

    //         var items = query.OrderItems.AsQueryable();
    //         if (category != "All")
    //         {
    //             items = items.Where(oi => oi.CategoryItem.Category.CategoryName == category);
    //         }

    //         if (status == "InProgress")
    //         {
    //             items = items.Where(oi => oi.Quantity - oi.ReadyQuantity > 0);
    //         }
    //         else
    //         {
    //             items = items.Where(oi => oi.ReadyQuantity > 0);
    //         }

    //         return items.ToList();
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return new List<OrderItem>();
    //     }
    // }

    public List<OrderItem> GetOrdersByOrderId(string category, int orderId, string status)
    {
        try
        {
            var sql = "SELECT * FROM get_order_items_for_modal({0}, {1}, {2});";

            var flatItems = _context.OrderItemFlat
                .FromSqlRaw(sql, category, orderId, status)
                .ToList();

            var orderItems = flatItems
             .Select(f => new OrderItem
             {
                 OrderId = f.order_id,
                 CategoryItemId = f.order_item_id,
                 Quantity = f.quantity,
                 ReadyQuantity = f.ready_quantity,
                 Status = f.status,
                 CategoryItem = new CategoryItem
                 {
                     ItemName = f.item_name
                 },
                 OrderItemModifiers = string.IsNullOrEmpty(f.modifier_name)
                     ? new List<OrderItemModifier>()
                     : new List<OrderItemModifier>
                     {
                        new OrderItemModifier
                        {
                            ModifierItem = new ModifierItem
                            {
                                ModifierItemName = f.modifier_name
                            }
                        }
                     }
             })
             .ToList();

            return orderItems;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<OrderItem>();
        }
    }


    // public Response UpdateOrderItemStatus(List<int> itemIds, int orderId, string newStatus, Dictionary<int, int> updatedQuantities)
    // {
    //     try
    //     {
    //         var items = _context.OrderItems.Where(oi => oi.OrderId == orderId && itemIds.Contains((int)oi.CategoryItemId)).ToList();
    //         foreach (var item in items)
    //         {
    //             if (newStatus == "Ready")
    //             {

    //                 if (updatedQuantities.TryGetValue((int)item.CategoryItemId, out var qty))
    //                 {
    //                     if (item.ReadyQuantity + qty == item.Quantity)
    //                     {

    //                         item.Status = "ready";
    //                     }
    //                     if (item.ReadyQuantity + qty <= item.Quantity && item.ReadyQuantity + qty >= 0)
    //                     {
    //                         item.ReadyQuantity = item.ReadyQuantity + qty;
    //                     }
    //                 }
    //             }
    //             else
    //             {
    //                 item.Status = "inProgress";
    //                 if (updatedQuantities.TryGetValue((int)item.CategoryItemId, out var qty))
    //                 {
    //                     if (item.ReadyQuantity - qty <= item.Quantity && item.ReadyQuantity - qty >= 0)
    //                     {

    //                         item.ReadyQuantity = item.ReadyQuantity - qty;
    //                     }
    //                 }
    //             }

    //         }
    //         _context.SaveChanges();
    //         return new Response { Error = false, Message = "Order Item status updated successfully" };
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return new Response { Error = true, Message = ex.Message };
    //     }
    // }

    public Response UpdateOrderItemStatus(List<int> itemIds, int orderId, string newStatus, Dictionary<int, int> updatedQuantities)
    {
        try
        {
            // Convert itemIds to PostgreSQL array format
            string itemIdsArray = "{" + string.Join(",", itemIds) + "}";

            // Convert dictionary to JSON
            string updatedQuantitiesJson = JsonConvert.SerializeObject(updatedQuantities);

            // Build and execute the SQL command
            string sql = "CALL update_order_item_status(@item_ids, @order_id, @new_status, @updated_quantities)";

            var itemIdsParam = new Npgsql.NpgsqlParameter("@item_ids", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer)
            {
                Value = itemIds.ToArray()
            };
            var orderIdParam = new Npgsql.NpgsqlParameter("@order_id", orderId);
            var newStatusParam = new Npgsql.NpgsqlParameter("@new_status", newStatus);
            var updatedQuantitiesParam = new Npgsql.NpgsqlParameter("@updated_quantities", NpgsqlTypes.NpgsqlDbType.Json)
            {
                Value = updatedQuantitiesJson
            };

            _context.Database.ExecuteSqlRaw(sql, itemIdsParam, orderIdParam, newStatusParam, updatedQuantitiesParam);

            return new Response { Error = false, Message = "Order Item status updated successfully (via procedure)." };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response { Error = true, Message = ex.Message };
        }
    }



    // --------------------------------------------------------------- //
    // Waiting List //
    // public List<Section> GetTableList()
    // {
    //     var TableList = _context.Sections
    //                     .Include(x => x.WaitingLists)
    //                     .Include(x => x.TableDetails)
    //                     .ThenInclude(x => x.TableOrderMappings)
    //                     .ThenInclude(x => x.Order)
    //                     // .ThenInclude(x => x.Customer)
    //                     .Where(x => x.IsDeleted == false)
    //                     .ToList();
    //     return TableList;
    // }


    public List<Section> GetTableList()
    {
        // Raw SQL to call your PostgreSQL function
        var sql = "SELECT get_sections_with_all_details()";

        // Execute the SQL and get JSON string from the database
        var jsonResult = _context.Database
            .SqlQueryRaw<string>(sql)
            .AsEnumerable()
            .FirstOrDefault();

        if (string.IsNullOrEmpty(jsonResult))
            return new List<Section>();

        // Deserialize JSON to List<Section> - note the function returns jsonb array of sections
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var sections = System.Text.Json.JsonSerializer.Deserialize<List<Section>>(jsonResult, options);

        return sections ?? new List<Section>();
    }

    // public List<WaitingList> GetWaitingListBySectionId(int sectionId)
    // {
    //     return _context.WaitingLists
    //     .Where(x => !x.IsDeleted && x.SectionId == sectionId)
    //     .OrderBy(x => x.WaitingId)
    //     .ToList();

    // }
    public List<WaitingList> GetWaitingListBySectionId(int sectionId)
    {
        // Raw SQL to call the PostgreSQL function with parameter
        var sql = $"SELECT get_waiting_list_by_section_id({sectionId})";

        // Execute the SQL and get JSON string from the database
        var jsonResult = _context.Database
            .SqlQueryRaw<string>(sql)
            .AsEnumerable()
            .FirstOrDefault();

        if (string.IsNullOrEmpty(jsonResult))
            return new List<WaitingList>();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var waitingLists = System.Text.Json.JsonSerializer.Deserialize<List<WaitingList>>(jsonResult, options);

        return waitingLists ?? new List<WaitingList>();
    }

    // public List<WaitingList> GetWaitingListBySectionId()
    // {
    //     return _context.WaitingLists.Where(x => x.IsDeleted == false).OrderBy(x => x.WaitingId).ToList();
    // }


    public List<WaitingList> GetWaitingListBySectionId()
    {
        var sql = "SELECT get_waiting_list_all()";

        var jsonResult = _context.Database
            .SqlQueryRaw<string>(sql)
            .AsEnumerable()
            .FirstOrDefault();

        if (string.IsNullOrEmpty(jsonResult))
            return new List<WaitingList>();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var waitingLists = System.Text.Json.JsonSerializer.Deserialize<List<WaitingList>>(jsonResult, options);

        return waitingLists ?? new List<WaitingList>();
    }

    // public List<WaitingList> GetWaitingListByWaitingId(int waitingId)
    // {
    //     return _context.WaitingLists.Where(x => x.WaitingId == waitingId).ToList();
    // }
    public List<WaitingList> GetWaitingListByWaitingId(int waitingId)
    {
        var sql = "SELECT get_waiting_list_by_waiting_id(@waitingId)";

        var jsonResult = _context.Database
            .SqlQueryRaw<string>(sql, new NpgsqlParameter("waitingId", waitingId))
            .AsEnumerable()
            .FirstOrDefault();

        if (string.IsNullOrEmpty(jsonResult))
            return new List<WaitingList>();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var waitingLists = System.Text.Json.JsonSerializer.Deserialize<List<WaitingList>>(jsonResult, options);

        return waitingLists ?? new List<WaitingList>();
    }
    // public void UpdateWaitingList(WaitingList waitingList)
    // {
    //     _context.Update(waitingList);
    //     _context.SaveChanges();
    // }

    public void UpdateWaitingList(WaitingList waitingList)
    {
        var sql = "CALL update_waiting_list(@email, @phone, @total_person, @section_id, @user_name)";

        var parameters = new[]
        {
            new NpgsqlParameter("email", (object?)waitingList.Email ?? DBNull.Value),
            new NpgsqlParameter("phone", (object?)waitingList.Phone ?? DBNull.Value),
            new NpgsqlParameter("total_person", (object?)waitingList.TotalPerson ?? DBNull.Value),
            new NpgsqlParameter("section_id", (object?)waitingList.SectionId ?? DBNull.Value),
            new NpgsqlParameter("user_name", (object?)waitingList.UserName ?? DBNull.Value),
        };

        _context.Database.ExecuteSqlRaw(sql, parameters);
    }


    public void DeleteWaitingName(WaitingList waitingList)
    {
        _context.Update(waitingList);
        _context.SaveChanges();
    }


    public int AddAsCustomerWaitingList(int waitingId, int sectionId, List<int> tableIds)
    {

        int orderId;

        using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
        connection.Open();

        using var command = new NpgsqlCommand("add_assign_of_waiting_list", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Input parameters
        command.Parameters.AddWithValue("p_waiting_id", waitingId);
        command.Parameters.AddWithValue("p_section_id", sectionId);
        command.Parameters.AddWithValue("p_table_ids", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer)
            .Value = tableIds ?? (object)DBNull.Value;

        // Output parameter
        var outputParam = new NpgsqlParameter("p_order_id", NpgsqlTypes.NpgsqlDbType.Integer)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outputParam);

        command.ExecuteNonQuery();

        // Read output value
        orderId = (int)outputParam.Value;

        return orderId;
    }



    public WaitingList GetWaitingByWaitingId(int waitingId)
    {
        return _context.WaitingLists.FirstOrDefault(x => x.WaitingId == waitingId);
    }


    public Customer AddCustomer(Customer customer)
    {

        _context.Customers.Add(customer);
        _context.SaveChanges();

        return customer;
    }
    public Order AddOrder(Order order)
    {

        _context.Orders.Add(order);
        _context.SaveChanges();

        return order;
    }


    public WaitingList removeCustomerFromWaiting(string Email)
    {

        var customer = _context.WaitingLists.FirstOrDefault(x => x.Email == Email)!;

        customer.IsDeleted = true;
        customer.ModifiedAt = DateTime.Now;

        _context.WaitingLists.Update(customer);
        _context.SaveChanges();

        return null;
    }


    public TableDetail GetTableListByTableId(int tableId)
    {
        return _context.TableDetails.FirstOrDefault(x => x.TableId == tableId)!;
    }
    public TableDetail UpdateTable(TableDetail table)
    {
        _context.TableDetails.Update(table);
        _context.SaveChanges();
        return table;
    }

    public TableOrderMapping AddTableMapping(TableOrderMapping tableOrderMapping)
    {
        _context.TableOrderMappings.Add(tableOrderMapping);
        _context.SaveChanges();

        return tableOrderMapping;
    }

    public Payment AddPayment(Payment payment)
    {
        _context.Payments.Add(payment);
        _context.SaveChanges();
        return payment;
    }



    public void AddWaitingList(WaitingListViewModel model)
    {
        // var check = _context.WaitingLists.FirstOrDefault(x => x.Email == model.Email && x.IsDeleted == false);
        // if (check != null)
        // {
        //     throw new Exception("Email already exist");
        // }
        // var WaitingList = new WaitingList
        // {
        //     Email = model.Email,
        //     Phone = model.PhoneNumber,
        //     TotalPerson = model.NoOfPerson,
        //     SectionId = model.SectionId,
        //     UserName = model.UserName
        // };
        // _context.WaitingLists.Add(WaitingList);
        // _context.SaveChanges();
        var sql = "SELECT add_waiting_list(@email, @phone, @total_person, @section_id, @user_name)";

        var parameters = new[]
        {
            new NpgsqlParameter("email", model.Email ?? (object)DBNull.Value),
            new NpgsqlParameter("phone", model.PhoneNumber ?? (object)DBNull.Value),
            new NpgsqlParameter("total_person", model.NoOfPerson ?? (object)DBNull.Value),
            new NpgsqlParameter("section_id", model.SectionId ?? (object)DBNull.Value),
            new NpgsqlParameter("user_name", model.UserName ?? (object)DBNull.Value)
        };

        try
        {
            _context.Database.ExecuteSqlRaw(sql, parameters);
        }
        catch (PostgresException ex) when (ex.SqlState == "P0001") // Catch the RAISE EXCEPTION error
        {
            throw new Exception(ex.Message);
        }

    }




    //-------------------------------------------------------//

    //Menu --------------------------------------------//


        public async Task SaveOrderUsingProcAsync(int orderId, List<OrderAppOrderItemViewModel> orderItems, float totalAmount)
        {
           string orderItemsJson = JsonConvert.SerializeObject(orderItems);

            using var connection = new NpgsqlConnection(_context.Database.GetConnectionString());
            await connection.OpenAsync();

            using var command = new NpgsqlCommand("CALL save_order(@OrderId, @OrderItems::json, @TotalAmount)", connection);
            command.Parameters.AddWithValue("OrderId", orderId);
            command.Parameters.AddWithValue("OrderItems", NpgsqlTypes.NpgsqlDbType.Json, orderItemsJson);
            command.Parameters.AddWithValue("TotalAmount", totalAmount);

            await command.ExecuteNonQueryAsync();
        }


    // ---------------------------------------------------//


    public List<Category> GetCategoryList()
    {
        var CategoryList = _context.Categories.ToList();
        return CategoryList;
    }


    public List<Customer> GetCreatedAtCustomer()
    {
        var customer = _context.Customers
            .ToList();
        return customer;

    }

    public List<Section> GetSections()
    {
        return _context.Sections
        .Where(x => x.IsDeleted == false)
        .OrderBy(x => x.SectionId)
        .ToList();
    }
    public List<TableDetail> GetTableDetail(int sectionId)
    {
        return _context.TableDetails
        .Where(x => x.IsDeleted == false && x.SectionId == sectionId)
        .OrderBy(x => x.TableId)

        .ToList();
    }
    public List<TableDetail> GetTablesBySectionId(int sectionId)
    {
        return _context.TableDetails
        .Where(x => x.IsDeleted == false && x.SectionId == sectionId && x.Status == "available")
        .OrderBy(x => x.TableId)
        .ToList();
    }

    public List<Category> AllCategories()
    {
        return _context.Categories
                .OrderBy(x => x.CategoryId)
                .ToList();
    }

    public List<CategoryItem> GetAllItem(string search)
    {
        var item = _context.CategoryItems
                    .Where(x => (string.IsNullOrEmpty(search) || x.ItemName.Trim().ToLower().Contains(search.Trim().ToLower())) && x.IsAvailable == true)
                    .OrderBy(x => x.ItemName)
                    .ToList();
        return item;
    }
    public List<CategoryItem> GetFavoriteItems(string search)
    {
        var item = _context.CategoryItems.Where(x => x.IsFavourite == true && (string.IsNullOrEmpty(search) || x.ItemName.Trim().ToLower().Contains(search.Trim().ToLower())) && x.IsAvailable == true).OrderBy(x => x.ItemName).ToList();
        return item;
    }
    public CategoryItem GetFavoriteItemsByItemId(int itemId)
    {
        var item = _context.CategoryItems
                    .OrderBy(x => x.ItemName)
                    .FirstOrDefault(x => x.CategoryItemId == itemId && x.IsAvailable == true);
        return item;
    }

    public List<CategoryItem> GetItemsByCategoryId(int categoryId, string search)
    {
        var item = _context.CategoryItems.Where(x => x.CategoryId == categoryId && (string.IsNullOrEmpty(search) || x.ItemName.Trim().ToLower().Contains(search.Trim().ToLower())) && x.IsAvailable == true).ToList();
        return item;
    }

    public CategoryItem UpdateToggleFavouriteItem(CategoryItem categoryItem)
    {
        _context.Update(categoryItem);
        _context.SaveChanges();

        return categoryItem;
    }

    public List<CategoryModifierMapping> GetModifierByItem(int itemId)
    {
        return _context.CategoryModifierMappings
                .Where(x => x.CategoryItemId == itemId)
                .Include(x => x.Modifier)
                .ThenInclude(x => x!.MappingItemModifiers)
                .ThenInclude(x => x.Modifier)
                .ToList();
    }

    public List<Order> GetTableByOrderId(int orderId)
    {
        return _context.Orders
                        .Where(x => x.OrderId == orderId)
                        .Include(c => c.OrderItems)
                        .Include(c => c.TableOrderMappings)
                        .ThenInclude(x => x!.Table)
                        .ThenInclude(x => x.Section)
                        .ToList();
    }

    public List<TableDetail> GetAllTableNameByTableId(int tableId)
    {
        return _context.TableDetails.Where(x => x.TableId == tableId).ToList();
    }
    public List<TableOrderMapping> GetAllTableNameByOrderId(int tableId)
    {
        return _context.TableOrderMappings.Where(x => x.TableId == tableId).ToList();
    }
    public List<TableOrderMapping> GetAllTableByOrderId(int orderId)
    {
        return _context.TableOrderMappings.Where(x => x.OrderId == orderId).ToList();
    }

    public List<OrderTaxMapping> GetExistingTaxFeesList(int orderId)
    {
        return _context.OrderTaxMappings
            .Where(x => x.OrderId == orderId)
            .ToList();
    }

    public Order GetOrderDetails(int orderId)
    {
        return _context.Orders
                        .Include(x => x.Customer)
                        .Include(c => c.OrderItems)
                        .ThenInclude(x => x.CategoryItem)
                        .Include(c => c.OrderItems)
                        .ThenInclude(x => x.OrderItemModifiers)
                        .ThenInclude(x => x.ModifierItem)
                        .Include(c => c.TableOrderMappings)
                        .ThenInclude(x => x!.Table)
                        .ThenInclude(x => x.Section)
                        .Include(c => c.Payments)
                        .FirstOrDefault(x => x.OrderId == orderId)!;
    }

    public Feedback CustomerReview(int orderId)
    {
        var feedback = _context.Feedbacks
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .FirstOrDefault(x => x.OrderId == orderId);
        return feedback;
    }

    public Feedback UpdateFeedBack(Feedback feedback)
    {
        _context.Feedbacks.Update(feedback);
        _context.SaveChanges();

        return feedback;
    }
    public Feedback AddFeedBack(Feedback feedback)
    {
        _context.Feedbacks.Add(feedback);
        _context.SaveChanges();

        return feedback;
    }

    public int AvgRating(int FoodRating, int ServiceRating, int AmbienceRating)
    {
        var avgRating = (FoodRating + ServiceRating + AmbienceRating) / 3;
        return avgRating;
    }

    public OrderItem AddOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        _context.SaveChanges();

        return orderItem;
    }

    public OrderItemModifier AddOrderModifier(OrderItemModifier orderItemModifier)
    {
        _context.OrderItemModifiers.Add(orderItemModifier);
        _context.SaveChanges();

        return orderItemModifier;
    }

    public Order UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();

        return order;
    }

    public List<OrderItem> GetOrderItemsByOrderId(int orderId)
    {
        return _context.OrderItems
                       .Include(x => x.CategoryItem)
                       .Include(x => x.OrderItemModifiers)
                           .ThenInclude(x => x.ModifierItem)
                       .Where(x => x.OrderId == orderId)
                       .ToList();
    }

    public OrderItem RemoveOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Remove(orderItem);
        _context.SaveChanges();

        return orderItem;
    }

    public OrderItemModifier RemoveOrderItemModifier(int OrderItemId)
    {
        var orderItemModifier = _context.OrderItemModifiers.FirstOrDefault(x => x.OrderItemId == OrderItemId);
        if (orderItemModifier != null)
        {
            _context.OrderItemModifiers.Remove(orderItemModifier);
            _context.SaveChanges();
        }
        return orderItemModifier;

    }

    public OrderItem UpdateOrderItem(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        _context.SaveChanges();

        return orderItem;
    }

    public List<TaxesAndFee> GetTaxFeesList()
    {
        return _context.TaxesAndFees
            .Where(x => x.IsDeleted == false && x.IsEnabled == true)!
            .OrderBy(x => x.TaxId)
            .ToList();
    }

    public OrderTaxMapping UpdateTaxOrder(int taxId, int orderId)
    {
        var orderTaxMapping = _context.OrderTaxMappings.FirstOrDefault(x => x.OrderId == orderId && x.TaxId == taxId);

        _context.OrderTaxMappings.Remove(orderTaxMapping);
        _context.SaveChanges();

        return orderTaxMapping;
    }

    public OrderTaxMapping AddTaxOrder(OrderTaxMapping orderTaxMapping)
    {
        _context.OrderTaxMappings.Add(orderTaxMapping);
        _context.SaveChanges();

        return orderTaxMapping;
    }


    public Payment checkPaymentStatus(int orderId)
    {
        var payment = _context.Payments.FirstOrDefault(x => x.OrderId == orderId);
        return payment;
    }

    public Payment UpdatePaymentStatus(Payment payment)
    {
        _context.Payments.Update(payment);
        _context.SaveChanges();
        return payment;
    }


    public float? TableAmount(int orderId)
    {
        var order = _context.Orders
            .Include(x => x.TableOrderMappings)
            .ThenInclude(x => x.Table)
            .FirstOrDefault(x => x.OrderId == orderId);
        var tableAmount = order.TotalAmount;
        return tableAmount;
    }

    public Customer GetCustomerDetail(int customerId)
    {
        var customer = _context.Customers
            .Include(x => x.Orders)
            .ThenInclude(x => x.TableOrderMappings)
            .ThenInclude(x => x.Table)
            .ThenInclude(x => x.Section)
            .FirstOrDefault(x => x.CustomerId == customerId);
        return customer;
    }

    public void UpdateCustomerDetail(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public OrderItem GetItemWiseComment(int itemId, int orderId)
    {
        var orderItem = _context.OrderItems
            .Include(x => x.CategoryItem)
            .Include(x => x.OrderItemModifiers)
            .ThenInclude(x => x.ModifierItem)
            .FirstOrDefault(x => x.CategoryItemId == itemId && x.OrderId == orderId);
        return orderItem;
    }

    public OrderItem GetOrderItemById(int orderItemId)
    {
        var orderItem = _context.OrderItems
            .Include(x => x.CategoryItem)
            .Include(x => x.OrderItemModifiers)
            .ThenInclude(x => x.ModifierItem)
            .FirstOrDefault(x => x.OrderItemId == orderItemId);
        return orderItem;
    }

    public TableOrderMapping UpdateTableOrderMapping(TableOrderMapping tableOrderMapping)
    {
        _context.TableOrderMappings.Update(tableOrderMapping);
        _context.SaveChanges();
        return tableOrderMapping;
    }

    public Customer CheckExistingUser(string email)
    {
        var customer = _context.Customers.Where(c => c.Email.ToLower() == email.ToLower())
                        .Include(x => x.Orders.Where(x => x.Status == "Completed" || x.Status == "Cancelled"))
                        .FirstOrDefault();
        return customer;
    }




}
