using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;

namespace PizzaShop.Repository.Interfaces;

public interface ITablesAndSectionRepository
{
    List<Section> GetSections();
    List<TableDetail> TableListById(int tableId);
    TableDetail GetTableById(int itemId);
    void AddSection(SectionsViewModel model);
    List<TableDetail> GetAllTable();
    Section GetSectionById(int id);
    void UpdateSection(Section section);
    void DeleteSection(int id);
    void AddTable(TableViewModel model);
    void UpdateTable(TableDetail tableDetail);
    void DeleteTable(int itemId);

    IEnumerable< RolePermission> GetPermissionByroleId(int roleId);

    void DeleteMultipleTable(List<int> dataId);
}
