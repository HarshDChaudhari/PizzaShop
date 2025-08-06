using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Repository.Interfaces;

public interface IOrderAppRepository
{
  List<Category> GetCategoryList();
  List<Section> GetTableList();
  List<Section> GetSections();
  List<TableDetail> GetTableDetail(int sectionId);
  List<TableDetail> GetTablesBySectionId(int sectionId);
  void AddWaitingList(WaitingListViewModel model);
  List<Customer> GetCreatedAtCustomer();

  List<WaitingList> GetWaitingListBySectionId();
  List<WaitingList> GetWaitingListBySectionId(int sectionId);
  Customer AddCustomer(Customer customer);
  WaitingList removeCustomerFromWaiting(string Email);
  TableDetail GetTableListByTableId(int tableId);

  TableDetail UpdateTable(TableDetail table);

  Order AddOrder(Order order);

  TableOrderMapping AddTableMapping(TableOrderMapping tableOrderMapping);
  TableOrderMapping UpdateTableOrderMapping(TableOrderMapping tableOrderMapping);

  List<WaitingList> GetWaitingListByWaitingId(int waitingId);
  WaitingList GetWaitingByWaitingId(int waitingId);
  void UpdateWaitingList(WaitingList waitingList);

  void DeleteWaitingName(WaitingList waitingList);
  Feedback CustomerReview(int orderId);
  int AvgRating(int FoodRating, int ServiceRating, int AmbienceRating);
  Feedback UpdateFeedBack(Feedback feedback);
  Feedback AddFeedBack(Feedback feedback);
  float? TableAmount(int orderId);

  Customer CheckExistingUser(string email);

  List<Category> AllCategories();

  List<CategoryItem> GetAllItem(string search);
  List<CategoryItem> GetItemsByCategoryId(int categoryId, string search);
  List<CategoryItem> GetFavoriteItems(string search);
  CategoryItem GetFavoriteItemsByItemId(int itemId);

  CategoryItem UpdateToggleFavouriteItem(CategoryItem categoryItem);

  List<CategoryModifierMapping> GetModifierByItem(int itemId);
  List<Order> GetTableByOrderId(int orderId);
  List<TableDetail> GetAllTableNameByTableId(int tableId);


  Order GetOrderDetails(int orderId);

  OrderItem AddOrderItem(OrderItem orderItem);

  OrderItemModifier AddOrderModifier(OrderItemModifier orderItemModifier);

  Order UpdateOrder(Order order);

  List<OrderItem> GetOrderItemsByOrderId(int orderId);

  OrderItem RemoveOrderItem(OrderItem orderItem);

  OrderItemModifier RemoveOrderItemModifier(int OrderItemId);

  OrderItem UpdateOrderItem(OrderItem orderItem);

  List<TaxesAndFee> GetTaxFeesList();

  List<TableOrderMapping> GetAllTableNameByOrderId(int tableId);
  List<TableOrderMapping> GetAllTableByOrderId(int orderId);
  List<OrderTaxMapping> GetExistingTaxFeesList(int orderId);
  OrderTaxMapping UpdateTaxOrder(int taxId, int orderId);
  OrderTaxMapping AddTaxOrder(OrderTaxMapping orderTaxMapping);

  List<string> GetAllCategories();
  List<Order> GetOrdersByCategoryAndStatus(string category, string status);

  List<OrderItem> GetOrdersByOrderId(string category, int orderId, string status);
  Response UpdateOrderItemStatus(List<int> itemIds, int orderId, string newStatus, Dictionary<int, int> updatedQuantities);

  Payment checkPaymentStatus(int orderId);

  Payment UpdatePaymentStatus(Payment payment);

  Payment AddPayment(Payment payment);
  Customer GetCustomerDetail(int customerId);
  void UpdateCustomerDetail(Customer customer);
  OrderItem GetItemWiseComment(int itemId, int orderId);
  OrderItem GetOrderItemById(int orderItemId);


  int AddAsCustomerWaitingList(int waitingId, int sectionId, List<int> tableIds);

Task SaveOrderUsingProcAsync(int orderId, List<OrderAppOrderItemViewModel> orderItems, float totalAmount);  

}
