using System.Linq;
using System.Threading.Tasks;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementations;

public class OrderAppService(IOrderAppRepository _orderAppRepository) : IOrderAppService
{

    public TableOrderAppViewModel GetTableList()
    {

        var sectionListModel = _orderAppRepository.GetTableList();
        var allTables = sectionListModel.SelectMany(x => x.TableDetails).Where(x => x.IsDeleted == false).ToList();
        // var temp = sectionListModel.Select(x => x.TableDetails).SelectMany(y => y.).ToList();
        var viewModel = new TableOrderAppViewModel
        {
            SectionList = new List<SectionsViewModel>()
        };

        foreach (var section in sectionListModel)
        {
            var tablesInSection = allTables.Where(t => t.SectionId == section.SectionId).Select(t => new TableViewModel
            {
                TableId = t.TableId,
                TableName = t.TableName,
                SectionId = t.SectionId,
                Capacity = t.Capacity,
                Status = t.Status,
                CreatedAt = t.TableOrderMappings.Count != 0 ? t.TableOrderMappings.First().Order.CreatedAt : DateTime.Now,
                OrderId = t.TableOrderMappings.Count != 0 ? t.TableOrderMappings.Where(x => x.IsDeleted == false).Select(o => o.OrderId).FirstOrDefault() : 0,
                TableAmount = t.TableOrderMappings.Count != 0 ? t.TableOrderMappings.First().Order.TotalAmount : 0f,
                // TableAmount = _orderAppRepository.TableAmount(OrderId ?? 0),
                // TableAmount = (int)t.TableOrderMappings.Select(o => o.Order.TotalAmount).FirstOrDefault(),
            }).ToList();

            var sectionViewModel = new SectionsViewModel
            {
                SectionId = section.SectionId,
                SectionName = section.SectionName,
                TableDetails = tablesInSection
            };

            viewModel.SectionList.Add(sectionViewModel);
        }

        // var CreatedAt = _orderAppRepository.GetCreatedAtCustomer().FirstOrDefault().CreatedAt ?? DateTime.Now;

        return viewModel;
    }

    public List<Section> GetSections()
    {
        return _orderAppRepository.GetSections();
    }
    public List<TableDetail> GetTables(int sectionId)
    {
        return _orderAppRepository.GetTableDetail(sectionId);
    }
    public List<TableDetail> GetTablesBySectionId(int sectionId)
    {
        return _orderAppRepository.GetTablesBySectionId(sectionId);
    }

    public void AddWaitingList(WaitingListViewModel model)
    {
        _orderAppRepository.AddWaitingList(model);
    }

    public AddAssignTableViewModel AddAssign(int sectionId, List<int> tableIds)
    {


        var waitingList = _orderAppRepository.GetWaitingListBySectionId();

        var sectionList = _orderAppRepository.GetSections();

        var model = new AddAssignTableViewModel
        {
            WaitingUserList = new List<WaitingListViewModel>()
        };

        foreach (var item in waitingList)
        {
            var waitingListModel = new WaitingListViewModel
            {
                SectionId = item.SectionId,
                UserName = item.UserName,
                PhoneNumber = item.Phone,
                Email = item.Email,
                WaitingId = item.WaitingId,
                NoOfPerson = item.TotalPerson,

            };
            model.WaitingUserList.Add(waitingListModel);
        }


        return model;
    }

    public int AddAsCustomer(AddAssignTableViewModel model)
    {
        var addCustomer = new Customer();
        var check = _orderAppRepository.CheckExistingUser(model.Email);
        if (check == null)
        {
            var newCustomer = new Customer
            {
                Email = model.Email,
                Phone = model.Phone,
                CustomerName = model.UserName ?? "Unknown",
            };
            addCustomer = _orderAppRepository.AddCustomer(newCustomer);
        }
        else
        {
            addCustomer = check;
        }

        var newOrder = new Order
        {
            CustomerId = addCustomer.CustomerId,
            Status = "Pending",
            TotalAmount = 0,
        };
        var addOrder = _orderAppRepository.AddOrder(newOrder);


        if (model.SelectedTable != null)
        {
            foreach (var table in model.SelectedTable)
            {
                var allTable = _orderAppRepository.GetTableListByTableId(table);
                allTable.Status = "occupied";
                _orderAppRepository.UpdateTable(allTable);

                var tableMapping = new TableOrderMapping
                {
                    OrderId = addOrder.OrderId,
                    TableId = table,
                };
                _orderAppRepository.AddTableMapping(tableMapping);
            }

        }

        var removeWaitingList = _orderAppRepository.removeCustomerFromWaiting(model.Email);

        var Payment = new Payment
        {
            OrderId = addOrder.OrderId,
            PaymentStatus = "Pending",

        };
        _orderAppRepository.AddPayment(Payment);


        return addOrder.OrderId;
    }
    public int AddAsCustomerWaitingList(int waitingId, int sectionId, List<int> tableIds)
    {
        var result = _orderAppRepository.AddAsCustomerWaitingList(waitingId, sectionId, tableIds);
        return result;

        // var model = _orderAppRepository.GetWaitingByWaitingId(waitingId);

        // var SelectedTable = tableIds;

        // var addCustomer = new Customer();
        // var check = _orderAppRepository.CheckExistingUser(model.Email);
        // if (check == null)
        // {
        //     var newCustomer = new Customer
        //     {
        //         Email = model.Email,
        //         Phone = model.Phone,
        //         CustomerName = model.UserName ?? "Unknown",
        //     };
        //     addCustomer = _orderAppRepository.AddCustomer(newCustomer);
        // }
        // else
        // {
        //     addCustomer = check;
        // }



        // var newOrder = new Order
        // {
        //     CustomerId = addCustomer.CustomerId,
        //     Status = "Pending",
        //     TotalAmount = 0,
        // };
        // var addOrder = _orderAppRepository.AddOrder(newOrder);


        // if (SelectedTable != null)
        // {
        //     foreach (var table in SelectedTable)
        //     {
        //         var allTable = _orderAppRepository.GetTableListByTableId(table);
        //         allTable.Status = "occupied";
        //         _orderAppRepository.UpdateTable(allTable);

        //         var tableMapping = new TableOrderMapping
        //         {
        //             OrderId = addOrder.OrderId,
        //             TableId = table,
        //         };
        //         _orderAppRepository.AddTableMapping(tableMapping);
        //     }

        // }

        // var removeWaitingList = _orderAppRepository.removeCustomerFromWaiting(model.Email);

        // var Payment = new Payment
        // {
        //     OrderId = addOrder.OrderId,
        //     PaymentStatus = "Pending",

        // };
        // _orderAppRepository.AddPayment(Payment);

        // return addOrder.OrderId;
    }


    public OrderAppWaitingListViewModel GetSectionsForWaitingList()
    {
        var sectionListModel = _orderAppRepository.GetTableList();

        var viewModel = new OrderAppWaitingListViewModel
        {
            SectionList = new List<SectionsViewModel>()
        };

        foreach (var section in sectionListModel)
        {
            var sectionViewModel = new SectionsViewModel
            {
                SectionId = section.SectionId,
                SectionName = section.SectionName
            };

            viewModel.SectionList.Add(sectionViewModel);
        }

        return viewModel;
    }


    public OrderAppWaitingListViewModel GetWaitingList(int sectionId)
    {


        List<WaitingList> list = new List<WaitingList>();
        if (sectionId != 0)
        {
            list = _orderAppRepository.GetWaitingListBySectionId(sectionId);
        }
        else
        {
            list = _orderAppRepository.GetWaitingListBySectionId();
        }

        var viewModel = new OrderAppWaitingListViewModel
        {
            WaitingList = new List<WaitingListViewModel>()
        };

        foreach (var WList in list)
        {
            var waitingList = new WaitingListViewModel
            {
                WaitingId = WList.WaitingId,
                UserName = WList.UserName,
                CreatedAt = WList.CreatedAt ?? DateTime.Now,
                Email = WList.Email,
                PhoneNumber = WList.Phone,
                NoOfPerson = WList.TotalPerson,
                SectionId = WList.SectionId
            };
            viewModel.WaitingList.Add(waitingList);
        }
        return viewModel;
    }


    public WaitingListViewModel GetWaitingListByWaitingId(int waitingId)
    {

        var WList = _orderAppRepository.GetWaitingListByWaitingId(waitingId);

        var model = new WaitingListViewModel();

        foreach (var list in WList)
        {
            model.NoOfPerson = list.TotalPerson;
            model.Email = list.Email;
            model.PhoneNumber = list.Phone;
            model.UserName = list.UserName;
            model.WaitingId = list.WaitingId;
            model.SectionId = list.SectionId;
        }

        return model;
    }

    public void UpdateWaitingList(WaitingListViewModel model)
    {
        var waitingList = _orderAppRepository.GetWaitingListByWaitingId(model.WaitingId);

        if (waitingList != null)
        {
            // category.CategoryName = model.CategoryName;
            foreach (var items in waitingList)
            {
                items.Email = model.Email;
                items.Phone = model.PhoneNumber;
                items.UserName = model.UserName;
                items.SectionId = model.SectionId;
                items.TotalPerson = model.NoOfPerson;

                _orderAppRepository.UpdateWaitingList(items);
            }
        }

    }

    public void DeleteWaiting(int waitingId)
    {
        var waitingList = _orderAppRepository.GetWaitingListByWaitingId(waitingId);

        if (waitingList != null)
        {
            // category.CategoryName = model.CategoryName;
            foreach (var items in waitingList)
            {
                items.IsDeleted = true;

                _orderAppRepository.DeleteWaitingName(items);
            }
        }

    }


    public OrderAppMenuViewModel AllCategories()
    {

        var category = _orderAppRepository.AllCategories();

        var model = new OrderAppMenuViewModel
        {
            Categories = new List<CategoryViewModel>()
        };

        foreach (var cat in category)
        {
            var Categories = new CategoryViewModel
            {
                CategoryId = cat.CategoryId,
                CategoryName = cat.CategoryName
            };

            model.Categories.Add(Categories);
        }
        return model;

    }


    public OrderAppMenuViewModel GetMenuDetails(int categoryId, string search)
    {
        var category = _orderAppRepository.AllCategories();

        var categoryViewModels = category.Select(i => new CategoryViewModel
        {
            CategoryId = i.CategoryId,
            CategoryName = i.CategoryName
        }).ToList();

        List<CategoryItem> items;
        if (categoryId == -1)
        {
            items = _orderAppRepository.GetFavoriteItems(search);
        }
        else if (categoryId == 0)
        {
            items = _orderAppRepository.GetAllItem(search);
        }
        else
        {
            items = _orderAppRepository.GetItemsByCategoryId(categoryId, search);
        }

        var ItemOrderMenu = items.Select(x => new ItemOrderMenu
        {
            ItemId = x.CategoryItemId,
            ItemName = x.ItemName,
            ImgUrl = x.ImageUrl,
            ItemPrice = x.Price,
            ItemType = x.ItemType,
            IsFavorite = x.IsFavourite,
            TaxPercentage = x.TaxPercentage,


        }).ToList();

        return new OrderAppMenuViewModel
        {
            Categories = categoryViewModels,
            Items = ItemOrderMenu,
            SelectedCategoryId = categoryId,
            SearchQuery = search,

        };

    }

    public CategoryItem ToogleFavourite(int itemId)
    {

        var items = _orderAppRepository.GetFavoriteItemsByItemId(itemId);

        if (items.IsFavourite == true)
        {
            items.IsFavourite = false;
        }
        else
        {
            items.IsFavourite = true;
        }



        var item = _orderAppRepository.UpdateToggleFavouriteItem(items);

        return item;
    }

    public OrderAppMenuViewModel GetModifierDetails(int itemid)
    {

        var modifierGroupInItem = _orderAppRepository.GetModifierByItem(itemid);

        var model = new OrderAppMenuViewModel
        {
            SelectedModifierGroups = new List<ModifierListViewModel>()
        };

        foreach (var MList in modifierGroupInItem)
        {
            var selectedModifier = new ModifierListViewModel
            {
                ModifierGroupId = MList.Modifier.ModifierGroupId,
                ModifierGroupName = MList.Modifier!.ModifierName,
                MaxValue = MList.MaxValue,
                MinValue = MList.MinValue,
                ModifierLists = MList.Modifier.MappingItemModifiers.Select(c => new ModifierList
                {
                    ModifierItemId = c.ModifierId,
                    ModifierItemName = c.Modifier.ModifierItemName,
                    Rate = c.Modifier.Rate
                }).ToList()

            };
            model.SelectedModifierGroups.Add(selectedModifier);
        }

        return model;

    }
    public OrderAppOrderDetailsViewModel GetOrderDetails(int orderId)
    {
        var ordered = _orderAppRepository.GetOrderDetails(orderId);

        var model = new OrderAppOrderDetailsViewModel
        {
            // InvoiceId = ordered.Invoices.FirstOrDefault()?.InvoiceId ?? 0,

            OrderId = ordered.OrderId,
            // StartDate = ordered.CreatedAt,
            EndDate = ordered.ModifiedAt ?? DateTime.Now,
            OrderStatus = ordered.Status ?? string.Empty,
            CustomerId = ordered.CustomerId,
            CustomerName = ordered.Customer.CustomerName,
            CustomerEmail = ordered.Customer.Email ?? string.Empty,
            CustomerPhone = ordered.Customer.Phone,
            TotalPerson = ordered.NoOfPerson ?? 0,
            OrderAmount = ordered.TotalAmount ?? 0f,

            SectionName = ordered.TableOrderMappings.Select(o => o.Table!.Section!.SectionName).First(),
            TableList = ordered.TableOrderMappings.Where(x => x.IsDeleted == false).Select(o => o.Table!.TableName).ToList(),

            OrderItem = ordered.OrderItems.Select(m => new OrderManyItem
            {
                OrderDetailId = m.OrderItemId,
                ItemName = m.CategoryItem!.ItemName,
                ItemId = m.CategoryItemId ?? 0,
                Quantity = m.Quantity,
                Price = m.CategoryItem.Price ?? 0f,
                TaxPercentage = m.CategoryItem.TaxPercentage ?? 0f,


                modifier = m.OrderItemModifiers.Select(m => new ModifierOrderAppViewModel
                {
                    ModifierName = m.ModifierItem!.ModifierItemName,
                    ModifierId = m.ModifierItemId,
                    ModifierPrice = (int)m.ModifierItem.Rate!,
                }).ToList(),
            }).ToList(),

            // ManyTableList = _repository.TableMappping(id),

            tax = _orderAppRepository.GetTaxFeesList().Select(t => new TaxesAndFeesViewModel
            {
                TaxId = t.TaxId,
                TaxName = t.TaxName,
                TaxType = t.TaxType,
                TaxValue = t.TaxValue,
                IsEnabled = t.IsEnabled,
                IsDefault = t.IsDefault,
                IsDeleted = t.IsDeleted,
            }).ToList(),

            // PaymentMode = _orderAppRepository.checkPaymentStatus(orderId).PaymentMethod ?? "",
            PaymentMode = ordered.Payments.Any() ? ordered.Payments.FirstOrDefault().PaymentMethod : "",
        };

        return model;
        // return null;
    }


    // public OrderAppOrderDetailsViewModel SaveOrder(int orderId, List<OrderAppOrderItemViewModel> orderItems, float TotalAmount)
    // {
    //     var order = _orderAppRepository.GetOrderDetails(orderId);
    //     var tableIdList = _orderAppRepository.GetAllTableByOrderId(orderId);
    //     var tableList = new List<TableDetail>();
    //     foreach (var table in tableIdList)
    //     {
    //         var tableDetails = _orderAppRepository.GetAllTableNameByTableId(table.TableId ?? 0);
    //         foreach (var tableDetail in tableDetails)
    //         {
    //             tableDetail.Status = "running";
    //             _orderAppRepository.UpdateTable(tableDetail);
    //         }
    //     }

    //     var existingTax = _orderAppRepository.GetExistingTaxFeesList(orderId);

    //     var currentTax = _orderAppRepository.GetTaxFeesList();



    //     if (currentTax.Count >= existingTax.Count)
    //     {
    //         for (int i = 0; i < currentTax.Count; i++)
    //         {
    //             var tax = currentTax[i];
    //             var isPresent = existingTax.Any(x => x.TaxId == tax.TaxId);
    //             if (!isPresent)
    //             {
    //                 var orderTax = new OrderTaxMapping
    //                 {
    //                     OrderId = orderId,
    //                     TaxId = tax.TaxId,
    //                     TaxValue = (float?)tax.TaxValue,
    //                     TaxType = tax.TaxType,
    //                 };
    //                 _orderAppRepository.AddTaxOrder(orderTax);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < existingTax.Count; i++)
    //         {
    //             var tax = existingTax[i];
    //             var isPresent = currentTax.Any(x => x.TaxId == tax.TaxId);
    //             if (!isPresent)
    //             {
    //                 _orderAppRepository.UpdateTaxOrder(tax.TaxId ?? 0, orderId);
    //             }
    //         }
    //     }

    //     var existingOrderItems = _orderAppRepository.GetOrderItemsByOrderId(orderId);


    //     if (existingOrderItems != null)
    //     {
    //         // Remove existing order items if they are not in the new list
    //         foreach (var existingItem in existingOrderItems)
    //         {
    //             if (!orderItems.Any(i => i.ItemId == existingItem.CategoryItemId))
    //             {
    //                 _orderAppRepository.RemoveOrderItemModifier(existingItem.OrderItemId);
    //                 _orderAppRepository.RemoveOrderItem(existingItem);
    //             }
    //         }
    //         // Add or update order items
    //         foreach (var item in orderItems)
    //         {
    //             var existingOrderItem = existingOrderItems.FirstOrDefault(i => i.CategoryItemId == item.ItemId);

    //             if (existingOrderItem != null)
    //             {
    //                 // Update existing order item
    //                 existingOrderItem.Quantity = item.Quantity;

    //                 _orderAppRepository.UpdateOrderItem(existingOrderItem);
    //             }
    //             else
    //             {
    //                 // Add new order item
    //                 var orderItem = new OrderItem
    //                 {
    //                     CategoryItemId = item.ItemId,
    //                     Quantity = item.Quantity,
    //                     OrderId = order.OrderId,
    //                     // Price = item.Price,  
    //                     CreatedAt = DateTime.Now,
    //                     ModifiedAt = DateTime.Now,
    //                     ReadyQuantity = 0,

    //                 };

    //                 _orderAppRepository.AddOrderItem(orderItem);

    //                 foreach (var modifier in item.Modifiers)
    //                 {
    //                     var orderModifier = new OrderItemModifier
    //                     {
    //                         OrderItemId = orderItem.OrderItemId,
    //                         ModifierItemId = modifier.Id,
    //                         CreatedAt = DateTime.Now,
    //                         ModifiedAt = DateTime.Now
    //                     };

    //                     _orderAppRepository.AddOrderModifier(orderModifier);
    //                 }
    //             }
    //         }

    //         order.TotalAmount = TotalAmount;
    //         order.Status = "InProgress";
    //         _orderAppRepository.UpdateOrder(order);
    //     }
    //     else
    //     {
    //         if (order != null)
    //         {
    //             foreach (var item in orderItems)
    //             {
    //                 var orderItem = new OrderItem
    //                 {
    //                     CategoryItemId = item.ItemId,
    //                     Quantity = item.Quantity,
    //                     OrderId = order.OrderId,
    //                     ModifiedAt = DateTime.Now,
    //                     ReadyQuantity = 0,
    //                 };

    //                 _orderAppRepository.AddOrderItem(orderItem);

    //                 foreach (var modifier in item.Modifiers)
    //                 {
    //                     var orderModifier = new OrderItemModifier
    //                     {
    //                         OrderItemId = orderItem.OrderItemId,
    //                         ModifierItemId = modifier.Id,
    //                         CreatedAt = DateTime.Now,
    //                         ModifiedAt = DateTime.Now
    //                     };

    //                     _orderAppRepository.AddOrderModifier(orderModifier);
    //                 }
    //             }

    //             // Update the total amount of the order
    //             order.TotalAmount = TotalAmount;
    //             order.Status = "InProgress";
    //             _orderAppRepository.UpdateOrder(order);
    //         }

    //     }
    //     return null;
    // }


    public async Task<OrderAppOrderDetailsViewModel> SaveOrder(int orderId, List<OrderAppOrderItemViewModel> orderItems, float TotalAmount){

        await _orderAppRepository.SaveOrderUsingProcAsync(orderId, orderItems, TotalAmount);


        return null;
    }

    public int CheckCompleteOrder(int orderId)
    {
        var orderItem = _orderAppRepository.GetOrderItemsByOrderId(orderId);

        foreach (var item in orderItem)
        {
            if (item.ReadyQuantity != item.Quantity)
            {
                return -1;
            }
        }


        return 1;


    }
    public int CheckOrder(int orderId)
    {
        var orderItem = _orderAppRepository.GetOrderItemsByOrderId(orderId);

        foreach (var item in orderItem)
        {
            if (item.ReadyQuantity >= 1)
            {
                return -1;
            }
        }
        return 1;


    }

    public bool CompleteOrder(int orderId)
    {
        var order = _orderAppRepository.GetOrderDetails(orderId);
        var tableList = _orderAppRepository.GetAllTableByOrderId(orderId);
        var payment = _orderAppRepository.checkPaymentStatus(orderId);
        if (payment != null)
        {
            payment.PaymentStatus = "Completed";
            _orderAppRepository.UpdatePaymentStatus(payment);
        }

        foreach (var table in tableList)
        {
            table.IsDeleted = true;
            _orderAppRepository.UpdateTableOrderMapping(table);
            var tableDetails = _orderAppRepository.GetAllTableNameByTableId(table.TableId ?? 0);
            foreach (var tableDetail in tableDetails)
            {
                tableDetail.Status = "available";
                _orderAppRepository.UpdateTable(tableDetail);
            }
        }



        order.Status = "Completed";
        _orderAppRepository.UpdateOrder(order);

        return true;

    }

    public bool CancelOrder(int orderId)
    {
        var tableList = _orderAppRepository.GetAllTableByOrderId(orderId);
        foreach (var table in tableList)
        {
            table.IsDeleted = true;
            _orderAppRepository.UpdateTableOrderMapping(table);
            var tableDetails = _orderAppRepository.GetAllTableNameByTableId(table.TableId ?? 0);
            foreach (var tableDetail in tableDetails)
            {
                tableDetail.Status = "available";
                _orderAppRepository.UpdateTable(tableDetail);
            }
        }

        var order = _orderAppRepository.GetOrderDetails(orderId);
        order.Status = "Cancelled";
        _orderAppRepository.UpdateOrder(order);

        var payment = _orderAppRepository.checkPaymentStatus(orderId);
        if (payment != null)
        {
            payment.PaymentStatus = "Cancelled";
            _orderAppRepository.UpdatePaymentStatus(payment);
        }

        return true;

    }

    public bool CustomerReview(CustomerReviewViewModel model)
    {
        var feedback = _orderAppRepository.CustomerReview(model.OrderId);

        if (feedback == null)
        {
            feedback = new Feedback
            {
                OrderId = model.OrderId,
                CustomerId = model.CustomerId,
                Comments = model.OrderComment,
                FoodRating = model.FoodRating,
                ServiceRating = model.ServiceRating,
                AmbienceRating = model.AmbienceRating,
                Rating = _orderAppRepository.AvgRating(model.FoodRating ?? 0, model.ServiceRating ?? 0, model.AmbienceRating ?? 0),
            };
            _orderAppRepository.AddFeedBack(feedback);
            return true;
        }
        else
        {
            feedback.Comments = model.ReviewText;
            feedback.FoodRating = model.FoodRating;
            feedback.ServiceRating = model.ServiceRating;
            feedback.AmbienceRating = model.AmbienceRating;
            feedback.Rating = _orderAppRepository.AvgRating(feedback.FoodRating ?? 0, feedback.ServiceRating ?? 0, feedback.AmbienceRating ?? 0);
            _orderAppRepository.UpdateFeedBack(feedback);
            return true;
        }
    }


    public KOTMainViewModel GetKotOrderDetails(string category, string status, int page)
    {
        try
        {
            var orders = _orderAppRepository.GetOrdersByCategoryAndStatus(category, status);
            if (orders == null)
            {
                return null;
            }

            int totalOrders = orders.Count;
            int displayCount = 4;
            if (totalOrders == 0)
            {
                return new KOTMainViewModel
                {
                    Categories = _orderAppRepository.GetAllCategories(),
                    Orders = new List<KOTOrderCardViewModel>(),
                    CurrentPage = 1,
                    TotalPage = 0,
                    SelectedCategory = category,
                    SelectedStatus = status
                };
            }

            var pagedOrders = orders.ToList();
            var AllCategories = _orderAppRepository.GetAllCategories();
            if (AllCategories == null)
            {
                return null;
            }
            return new KOTMainViewModel
            {
                Categories = AllCategories,
                SelectedCategory = category,
                SelectedStatus = status,
                CurrentPage = page,
                TotalPage = orders.Count(),
                Orders = pagedOrders.Select(o => new KOTOrderCardViewModel
                {
                    OrderId = o.OrderId,
                    TableName = o.TableOrderMappings.Select(ot => ot.Table.TableName).ToList(),
                    SectionName = o.TableOrderMappings.FirstOrDefault()?.Table?.Section?.SectionName,
                    OrderInstruction = o.Instruction,
                    CreatedAt = (DateTime)o.CreatedAt,
                    Items = o.OrderItems.Select(oi => new KOTOrderItemViewModel
                    {
                        ItemName = oi.CategoryItem.ItemName,
                        Quantity = GetQuantity(status, (int)oi.Quantity, (int)oi.ReadyQuantity),
                        Instruction = oi.Instruction,
                        Modifiers = oi.OrderItemModifiers.Select(m => new KOTOrderItemModifierViewModel
                        {
                            ModifierName = m.ModifierItem.ModifierItemName,
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new KOTMainViewModel();
        }
    }

    private int GetQuantity(string status, int Quantity, int ReadyQuantity)
    {
        if (status == "InProgress")
        {
            return Quantity - ReadyQuantity;
        }
        else
        {
            return ReadyQuantity;
        }
    }

    public KOTOrderCardViewModel GetOrderForModal(string category, int orderId, string status)
    {
        try
        {
            var order = _orderAppRepository.GetOrdersByOrderId(category, orderId, status);
            if (order == null)
            {
                return null;
            }
            var firstOrderItem = order.FirstOrDefault();

            if (firstOrderItem == null)
            {
                return null;
            }
            // return new KOTOrderCardViewModel
            // {
            //     OrderId = (int)order.FirstOrDefault().OrderId,
            //     CreatedAt = (DateTime)order.FirstOrDefault().CreatedAt,
            //     Items = order.Select(o => new KOTOrderItemViewModel
            //     {
            //         ItemName = o.CategoryItem.ItemName,
            //         Quantity = GetQuantity(status, (int)o.Quantity, (int)o.ReadyQuantity),
            //         ReadyQuantity = (int)o.ReadyQuantity,
            //         Status = o.Status,
            //         OrderItemId = (int)o.CategoryItemId,
            //         Modifiers = o.OrderItemModifiers.Select(m => new KOTOrderItemModifierViewModel
            //         {
            //             ModifierName = m.ModifierItem.ModifierItemName,
            //         }).ToList()
            //     }).ToList()
            // };
            return new KOTOrderCardViewModel
            {
                OrderId = firstOrderItem.OrderId ?? 0, // or handle null case appropriately
                CreatedAt = firstOrderItem.CreatedAt ?? DateTime.MinValue,
                Items = order.Select(o => new KOTOrderItemViewModel
                {
                    ItemName = o.CategoryItem.ItemName,
                    Quantity = GetQuantity(status, o.Quantity, o.ReadyQuantity ?? 0),
                    ReadyQuantity = o.ReadyQuantity ?? 0,
                    Status = o.Status,
                    OrderItemId = o.CategoryItemId ?? 0,
                    Modifiers = o.OrderItemModifiers.Select(m => new KOTOrderItemModifierViewModel
                    {
                        ModifierName = m.ModifierItem.ModifierItemName,
                    }).ToList()
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new KOTOrderCardViewModel();
        }
    }

    public Response UpdateOrderItem(List<int> itemIds, int orderId, string newStatus, Dictionary<int, int> updatedQuantities)
    {
        try
        {
            return _orderAppRepository.UpdateOrderItemStatus(itemIds, orderId, newStatus, updatedQuantities);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response { Error = false, Message = ex.Message };
        }
    }

    public bool UpdatePaymentMethod(int orderId, string paymentMethod)
    {
        try
        {
            var result = _orderAppRepository.checkPaymentStatus(orderId);
            if (result != null)
            {
                result.PaymentMethod = paymentMethod;
                _orderAppRepository.UpdatePaymentStatus(result);
                return true;
            }
            else
            {
                var Payment = new Payment
                {
                    OrderId = orderId,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = "Pending",
                };
                _orderAppRepository.AddPayment(Payment);
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public CustomerViewModel GetCustomerDetail(int customerId)
    {
        var customer = _orderAppRepository.GetCustomerDetail(customerId);
        if (customer == null)
        {
            return null;
        }

        var customerViewModel = new CustomerViewModel
        {
            CustomerId = customer.CustomerId,
            Orderid = customer.Orders.FirstOrDefault()?.OrderId ?? 0,
            TotalPerson = customer.Orders.FirstOrDefault()?.NoOfPerson ?? 0,
            CustomerName = customer.CustomerName,
            Email = customer.Email,
            Phone = customer.Phone
        };

        return customerViewModel;
    }

    public OrderViewModel GetOrderWiseComment(int orderId)
    {
        var order = _orderAppRepository.GetOrderDetails(orderId);
        var model = new OrderViewModel
        {
            OrderComment = order.Instruction
        };
        return model;
    }

    public bool UpdateOrderWiseComment(OrderViewModel model)
    {
        var order = _orderAppRepository.GetOrderDetails(model.OrderId);
        if (order != null)
        {
            order.Instruction = model.OrderComment;
            _orderAppRepository.UpdateOrder(order);
            return true;
        }
        return false;
    }

    public bool UpdateCustomerDetail(CustomerViewModel model)
    {
        var customer = _orderAppRepository.GetCustomerDetail(model.CustomerId);
        var table = _orderAppRepository.GetAllTableByOrderId(model.Orderid);
        var TotalCapacity = 0;
        foreach (var tableDetail in table)
        {
            TotalCapacity += tableDetail.Table.Capacity ?? 0;
        }

        if (model.TotalPerson > TotalCapacity)
        {
            return false;
        }
        if (customer != null)
        {
            customer.CustomerName = model.CustomerName;
            customer.Email = model.Email;
            customer.Phone = model.Phone;
            customer.Orders.FirstOrDefault().NoOfPerson = model.TotalPerson;

            _orderAppRepository.UpdateCustomerDetail(customer);
        }
        return true;

    }

    public OrderViewModel GetItemWiseComment(int itemId, int orderId)
    {
        var order = _orderAppRepository.GetItemWiseComment(itemId, orderId);
        var model = new OrderViewModel
        {
            OrderComment = order.Instruction
        };
        return model;
    }

    public int GetReadyQuantity(int orderDetailId)
    {
        var orderItem = _orderAppRepository.GetOrderItemById(orderDetailId);
        if (orderItem != null)
        {
            return (int)orderItem.ReadyQuantity;
        }
        return 0;
    }

    public string checkOrderStatus(int orderId)
    {
        var order = _orderAppRepository.GetOrderDetails(orderId);
        if (order != null)
        {
            return order.Status;
        }
        return string.Empty;
    }

    public WaitingListViewModel CheckExistingUser(string email)
    {

        var customerUser = _orderAppRepository.CheckExistingUser(email);

        if (customerUser != null)
        {
            var customer = new WaitingListViewModel
            {

                Email = customerUser.Email,
                PhoneNumber = customerUser.Phone,
                UserName = customerUser.CustomerName
            };
            return customer;
        }
        return null;
    }


}
