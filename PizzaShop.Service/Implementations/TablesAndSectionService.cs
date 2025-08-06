using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interfaces;

namespace PizzaShop.Service.Implementations;

public class TablesAndSectionService(ITablesAndSectionRepository _tablesAndSectionRepository) : ITablesAndSectionService
{


    
    public async Task<PaginationViewModel<TableViewModel>> GetTable(int SectionId, int page, int pageSize, string search = "")
    {

        List<TableDetail> tableList = _tablesAndSectionRepository.TableListById(SectionId);
        List<TableViewModel> tableListViews = new List<TableViewModel>();
        int tableCount ;

        foreach (TableDetail item in tableList)
        {
            tableListViews.Add(new TableViewModel
            {
                TableId = item.TableId,
                TableName = item.TableName,
                Capacity = item.Capacity,
                SectionId = item.SectionId,
                Status = item.Status,
            });
        }
        if (!string.IsNullOrEmpty(search))
        {
            tableListViews = tableListViews.Where(u => u.TableName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        tableCount = tableListViews.Count();
        tableListViews = tableListViews.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return new PaginationViewModel<TableViewModel>
        {
            Items = tableListViews,
            TotalItems = tableCount,
            CurrentPage = page,
            PageSize = pageSize,
        };
    }

    public List<Section> GetSections()
    {
        return _tablesAndSectionRepository.GetSections();
    }

    public List<TableDetail> GetAllTable()
    {
        return _tablesAndSectionRepository.GetAllTable();
    }


    public void AddSection(SectionsViewModel model)
    {
        _tablesAndSectionRepository.AddSection(model);
    }
    public void EditSection(SectionsViewModel model)
    {
        var section = _tablesAndSectionRepository.GetSectionById(model.SectionId);
        if (section != null)
        {
            section.SectionName = model.SectionName;
            section.Description = model.Description;
            _tablesAndSectionRepository.UpdateSection(section);
        }
    }
    public bool DeleteSection(int itemId)
    {
        if (itemId != null)
        {
            _tablesAndSectionRepository.DeleteSection(itemId);
            return true;
        }
        return false;
    }



    public void AddTable(TableViewModel model)
    {
        _tablesAndSectionRepository.AddTable(model);
    }
    // public void AddTable(int SectionId, string TableName, int Capacity , string Status)
    // {
    //     _tablesAndSectionRepository.AddTable(SectionId,TableName, Capacity, Status);
    // }

    public TableViewModel GetTableById(int itemId)
    {
        var item = _tablesAndSectionRepository.GetTableById(itemId);
        if (item == null)
            return null;

        // Map data from entity to ViewModel
        return new TableViewModel
        {
            TableId = item.TableId,
            TableName = item.TableName,
            Capacity = item.Capacity,
            SectionId = item.SectionId,
            Status = item.Status,
        };
    }

    public void EditTable(TableViewModel model)
    {
        var tableItem = _tablesAndSectionRepository.GetTableById(model.TableId);

        if (tableItem != null)
        {
            tableItem.SectionId = model.SectionId;
            tableItem.TableName = model.TableName;
            tableItem.Capacity = model.Capacity;
            tableItem.Status = model.Status;

            _tablesAndSectionRepository.UpdateTable(tableItem);
        }
    }
    public bool DeleteTable(int itemId)
    {
        if (itemId != null)
        {
            _tablesAndSectionRepository.DeleteTable(itemId);
            return true;
        }
        return false;
    }

    public IEnumerable<RolePermission> GetPermissionByroleId(int roleId)
    {
        return _tablesAndSectionRepository.GetPermissionByroleId(roleId);
    }

    public bool DeleteMultipleTable(List<int> dataId)
    {


        _tablesAndSectionRepository.DeleteMultipleTable(dataId);
        return true;

    }

}
