using PizzaShop.Entity.Data;
using PizzaShop.Entity.Models;
using PizzaShop.Entity.ViewModel;
using PizzaShop.Repository.Interfaces;

namespace PizzaShop.Repository.Implementations;

public class TaxesAndFeesRepository(PizzaShopDbContext _context) : ITaxesAndFeesRepository
{
    public List<TaxesAndFee> GetTaxesAndFees()
    {
        return _context.TaxesAndFees
        .Where(x => x.IsDeleted == false)
        .OrderBy(x => x.TaxId).ToList();
    }

    public List<TaxesAndFee> GetTaxesAndFees(string search)
{
        return _context.TaxesAndFees
            .Where(x => (x.TaxName.Contains(search) || x.TaxType.Contains(search)) && x.IsDeleted == false)
            .ToList();
}

    public TaxesAndFee GetTaxFeeById(int id){

        var tax =  _context.TaxesAndFees
        .Where(x => x.IsDeleted == false)
        .FirstOrDefault(c => c.TaxId == id);

        return tax;
    }

    public void AddTax(TaxesAndFeesViewModel model)
    {
        var tax = new TaxesAndFee
        {
            TaxName = model.TaxName,
            TaxType = model.TaxType,
            TaxValue = model.TaxValue,
            IsDefault = model.IsDefault,
            IsEnabled = model.IsEnabled

        };
        _context.TaxesAndFees.Add(tax);
        _context.SaveChanges();
    }

    public void UpdateTax(TaxesAndFee taxesAndFee)
    {
        _context.TaxesAndFees.Update(taxesAndFee);
        _context.SaveChanges();
    }

    public void DeleteTaxFee(int id){
        var tax = _context.TaxesAndFees.FirstOrDefault(x => x.TaxId == id);
        if(tax !=null){
            tax.IsDeleted = true;
            _context.TaxesAndFees.Update(tax);
            _context.SaveChanges();
        }
    }
}
