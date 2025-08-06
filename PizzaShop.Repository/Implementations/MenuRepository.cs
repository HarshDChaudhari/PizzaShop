using Microsoft.EntityFrameworkCore;
using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class MenuRepository(PizzaShopDbContext _context) : IMenuRepository
{

    #region Category
    public List<Category> CategoryList()
    {
        var items = _context.Categories.Where(x => x.IsDeleted == false).OrderBy(c => c.CategoryName).ToList();
        return items;
    }
    public List<CategoryItem> CategoryItemList(int categoryId)
    {
        var items = _context.CategoryItems
            .Where(x => x.CategoryId == categoryId && x.IsDeleted == false)
            .OrderBy(x => x.CategoryItemId)
            .ToList();
        return items;
    }
    public List<CategoryItem> AllCategoryItemList()
    {
        var items = _context.CategoryItems
            .Where(x => x.IsDeleted == false)
            .OrderBy(x => x.CategoryItemId)
            .ToList();
        return items;
    }

    public CategoryItem GetItemById(int itemId)
    {
        return _context.CategoryItems
        .Where(x => x.IsDeleted == false)
        .Include(x => x.CategoryModifierMappings)
        .ThenInclude(x => x.Modifier)
        // .ThenInclude(x => x.MappingItemModifiers!)
        // .ThenInclude(x => x.Modifier)

        // .Include(x => x.)
        .FirstOrDefault(x => x.CategoryItemId == itemId)!;
    }
    public ModifierItem GetModifierItemById(int itemId)
    {
        return _context.ModifierItems.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.ModifierItemId == itemId)!;
    }
    public List<ModifierGroup> GetAllModifierById()
    {
        return _context.ModifierGroups.Where(x => x.IsDeleted == false)
                .ToList();
    }
    public MappingItemModifier GetMappingModifierItemById(int itemId)
    {
        return _context.MappingItemModifiers.FirstOrDefault(x => x.ModifierId == itemId)!;
    }
    // Delete category item from the database
    public void DeleteCategoryItem(int itemId)
    {
        var categoryItem = _context.CategoryItems.FirstOrDefault(x => x.CategoryItemId == itemId);
        if (categoryItem != null)
        {
            categoryItem.IsDeleted = true;
            _context.CategoryItems.Update(categoryItem);
            _context.SaveChanges();
        }

    }
    public void AddCategory(MenuViewModel model)
    {
        var category = new Category
        {
            CategoryName = model.CategoryName,
            Description = model.Description
        };
        _context.Categories.Add(category);
        _context.SaveChanges();
    }
    public CategoryItem AddCategoryItem(CategoryListViewModel model)
    {

        var categoryItem = new CategoryItem
        {
            CategoryId = model.CategoryId,
            ItemName = model.ItemName,
            Description = model.Description,
            Quantity = model.Quantity,
            Price = model.Price,
            DefaultTax = model.DefaultTax,
            UnitId = model.UnitId,
            IsAvailable = model.IsAvailable,
            ItemType = model.ItemType,
            ShortCode = model.ShortCode,
            ImageUrl = model.ImageUrl,
            TaxPercentage = model.TaxPercentage,
        };
        _context.CategoryItems.Add(categoryItem);
        _context.SaveChanges();

        return categoryItem;



    }
    public Category GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(p => p.CategoryId == id);
    }
    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }
    public void DeleteCategory(int id)
    {
        var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
        if (category != null)
        {
            category.IsDeleted = true;
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
    public void DeleteMultipleCategoryItem(List<int> dataId)
    {
        try
        {
            var itemToDelete = _context.CategoryItems.Where(item => dataId.Contains(item.CategoryItemId)).ToList();
            foreach (var item in itemToDelete)
            {
                item.IsDeleted = true;
                _context.CategoryItems.Update(item);
                _context.SaveChanges();


            }
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"DbUpdateException: {ex.Message}\n{ex.InnerException?.Message}");
            // throw;
            return;
        }



    }


    public CategoryItem GetCategoryItemById(int id)
    {
        return _context.CategoryItems.FirstOrDefault(ci => ci.CategoryItemId == id);
    }

    public void UpdateCategoryItem(CategoryItem categoryItem)
    {
        _context.CategoryItems.Update(categoryItem);
        _context.SaveChanges();
    }




    #endregion
    #region Modifier

    public List<ModifierGroup> ModifierList()
    {
        var item = _context.ModifierGroups.Where(x => x.IsDeleted == false).OrderBy(c => c.ModifierName).ToList();
        return item;
    }

    public List<ModifierItem> ModifierItemsList()
    {
        var item = _context.ModifierItems
            .Where(x => x.IsDeleted == false)
                    .Include(x => x.Unit)
                    .OrderBy(x => x.ModifierItemId).ToList();
        return item;
    }
    public List<MappingItemModifier> GetModifierItemIdFromModifierGroupId(int ModifierGroupId)
    {
        var item = _context.MappingItemModifiers
            .Where(x => x.ModifierGroupId == ModifierGroupId).ToList();
        return item;
    }

    public List<Unit> UnitList()
    {
        return _context.Units.ToList();
    }


    public async Task<List<ModifierItem>> ModifierItemList(int ModifierGroupId)
    {
        try
        {
            var modifiers = _context.MappingItemModifiers.Where(c => c.ModifierGroupId == ModifierGroupId).Select(c => c.ModifierId).ToList();
            var result = await _context.ModifierItems.Where(c => modifiers.Contains(c.ModifierItemId) && c.IsDeleted == false).Include(c => c.Unit).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public MappingItemModifier GetMappingByIds(int ModifierItemId, int ModifierGroupId){
        return _context.MappingItemModifiers.FirstOrDefault(x => x.ModifierId == ModifierItemId && x.ModifierGroupId == ModifierGroupId);
    }





    public void DeleteModifierItem(int itemId)
    {
        var modifierItem = _context.ModifierItems.FirstOrDefault(x => x.ModifierItemId == itemId);
        if (modifierItem != null)
        {
            modifierItem.IsDeleted = true;
            _context.ModifierItems.Update(modifierItem);
            _context.SaveChanges();
        }
    }

    public ModifierGroup AddModifier(ModifierGroup modifier)
    {
        try
        {
            _context.ModifierGroups.Add(modifier);
            _context.SaveChanges();
            return modifier;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public MappingItemModifier AddModifierMapping(MappingItemModifier modifier)
    {
        try
        {
            _context.MappingItemModifiers.Add(modifier);
            _context.SaveChanges();
            return modifier;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public CategoryModifierMapping UpdateCategoryModifierMapping(CategoryModifierMapping modifier)
    {
        try
        {
            _context.CategoryModifierMappings.Update(modifier);
            _context.SaveChanges();
            return modifier;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public CategoryModifierMapping AddCategoryModifierMapping(CategoryModifierMapping categoryModifierMapping)
    {
        try
        {
            _context.CategoryModifierMappings.Add(categoryModifierMapping);
            _context.SaveChanges();
            return categoryModifierMapping;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"DbUpdateException: {ex.Message}\n{ex.InnerException?.Message}");
            throw;
        }
    }
    public List<CategoryModifierMapping> GetAllCategoryModifierMapping(int CategoryItemId)
    {
        try
        {
            return _context.CategoryModifierMappings
                .Include(x => x.CategoryItem)
                .Include(x => x.Modifier)
                .Where(x => x.CategoryItemId == CategoryItemId)
                .ToList();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"DbUpdateException: {ex.Message}\n{ex.InnerException?.Message}");
            throw;
        }
    }
    public CategoryModifierMapping EditCategoryModifierMapping(int ModifierId, int CategoryItemId)
    {
        var categoryModifierMapping = _context.CategoryModifierMappings
            .Include(x => x.Modifier)
            .Where(x => x.CategoryItemId == CategoryItemId)
            .FirstOrDefault(x => x.ModifierId == ModifierId);
        return categoryModifierMapping;
    }
    public CategoryModifierMapping DeleteCategoryModifierMapping(int ModifierId)
    {
        var categoryModifierMapping = _context.CategoryModifierMappings.FirstOrDefault(x => x.CategoryModifierMappingId == ModifierId);
        if (categoryModifierMapping != null)
        {

            _context.CategoryModifierMappings.Remove(categoryModifierMapping);
            _context.SaveChanges();
            return categoryModifierMapping;
        }
        return null;
    }




    public ModifierItem AddModifierItem(ModifierListViewModel model)
    {
        // var modifierMapping = new MappingItemModifier{
        //     ModifierGroupId = model.ModifierGroupId,
        //     ModifierId = model.ModifierItemId

        // };

        var modifierItem = new ModifierItem
        {

            ModifierItemName = model.ModifierItemName,
            Description = model.Description,
            Quantity = model.Quantity,
            Rate = model.Rate,
            UnitId = model.UnitId,
        };
        _context.ModifierItems.Add(modifierItem);
        _context.SaveChanges();

        var ModifierItemList = _context.ModifierItems.FirstOrDefault(c => c.ModifierItemName == model.ModifierItemName);

        return ModifierItemList;
    }



    public ModifierGroup GetModifierGroupById(int id)
    {
        var name = _context.ModifierGroups
        .Where(x => x.IsDeleted == false)
        .Include(x => x.CategoryModifierMappings)
        .FirstOrDefault(p => p.ModifierGroupId == id);
        return name;
    }
    public Unit unitNameById(int id)
    {
        return _context.Units.FirstOrDefault(p => p.UnitId == id);

    }


    public void UpdateModifier(ModifierGroup modifier)
    {
        _context.ModifierGroups.Update(modifier);
        _context.SaveChanges();
    }


    public void DeleteModifier(int id)
    {
        var modifier = _context.ModifierGroups.FirstOrDefault(p => p.ModifierGroupId == id);

        if (modifier != null)
        {
            modifier.IsDeleted = true;
            _context.ModifierGroups.Update(modifier);
            _context.SaveChanges();
        }
    }

    public void UpdateModifierItem(ModifierItem modifierItem)
    {
        _context.ModifierItems.Update(modifierItem);
        _context.SaveChanges();
    }

    public void DeleteMultipleModifierItem(List<int> dataId)
    {
        // var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
        var itemToDelete = _context.ModifierItems.Where(item => dataId.Contains(item.ModifierItemId)).ToList();

        foreach (var item in itemToDelete)
        {   
            item.IsDeleted = true;
            _context.ModifierItems.Update(item);
            _context.SaveChanges();
        }

        
    }

    public List<MappingItemModifier> GetAllModifierItemsByModifierGroupId(int modifierGroupId)
    {
        return _context.MappingItemModifiers
        .Include(y => y.Modifier)
            .Where(x => x.ModifierGroupId == modifierGroupId && x.Modifier.IsDeleted == false)
            .ToList();
    }

    public List<MappingItemModifier> GetAllMappingItemModifier(int modifierGroupId)
    {
        return _context.MappingItemModifiers
            .Include(x => x.Modifier)
            .Where(x => x.ModifierGroupId == modifierGroupId)
            .ToList();
    }

    public List<MappingItemModifier> DeleteModifierModifierItemMapping(int modifierId)
    {
        var mappingItemModifier = _context.MappingItemModifiers
            .Where(x => x.ModifierId == modifierId)
            .ToList();

        if (mappingItemModifier != null)
        {
            _context.MappingItemModifiers.RemoveRange(mappingItemModifier);
            _context.SaveChanges();
        }
        return mappingItemModifier;
    }


    public MappingItemModifier DeleteModifierItemMapping(MappingItemModifier mappingItemModifier){
        _context.MappingItemModifiers.Remove(mappingItemModifier);
        _context.SaveChanges();
        return mappingItemModifier;
    }

    #endregion


    public bool CheckCategory(string categoryName)
    {
        var category = _context.Categories.Where(x => x.IsDeleted == false).Any(c => c.CategoryName.ToLower() == categoryName.ToLower());
        return category;
    }



}

