using BSUcontractmodels.BusinessLogicContracts;
using BSUcontrmodels.DataModels;
using BSUcontractmodels.StoragesContracts;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class ProductBLC : IProductBLC
{
    private readonly IProductSC _productSC;

    public ProductBLC(IProductSC productSC)
    {
        _productSC = productSC;
    }

    public List<ProductDM> getAllProducts(bool onlyActive = true)
    {
        return _productSC.GetList(onlyActive);
    }

    public List<ProductDM> getAllProductsByManufacturer(string manufacturerID, bool onlyActive = true)
    {
        if (manufacturerID.IsEmpty())
            throw new ValidationException("< Product BLC: ManufacturerID is empty >");
        return _productSC.GetList(onlyActive, manufacturerID);
    }

    public ProductDM GetProductByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Product BLC: search data is empty >");

        if (data.IsGuid())
        {
            var byId = _productSC.GetItemByID(data);
            if (byId != null) return byId;
        }

        var byName = _productSC.GetItemByName(data);
        if (byName != null) return byName;

        throw new ValidationException($"< Product with data '{data}' not found >");
    }

    public void InsertPitem(ProductDM product)
    {
        product.Validate();
        _productSC.AddItem(product);
    }

    public void UpdatePitem(ProductDM product)
    {
        product.Validate();
        _productSC.UpdItem(product);
    }

    public void DeletePitem(string id)
    {
        if (id.IsEmpty())
            throw new ValidationException("< Product BLC: ID for deletion is empty >");
        _productSC.DelItem(id);
    }
}