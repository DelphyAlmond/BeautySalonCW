using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class ProductBLC(IProductSC productSC) : IProductBLC
{
    private readonly IProductSC _productSC = productSC;

    public List<ProductDM> GetAllProducts(bool onlyActive = true)
    {
        return _productSC.GetList(onlyActive) ?? throw new NullListException();
    }

    public List<ProductDM> GetAllProductsByManufacturer(string manufacturerID, bool onlyActive = true)
    {
        if (manufacturerID.IsEmpty())
            throw new ValidationException("< Product BLC: ManufacturerID is empty >");
        return _productSC.GetList(onlyActive, manufacturerID);
    }

    public ProductDM GetProductByData(string data)
    {
        if (data.IsEmpty())
            throw new ArgumentNullException($"< Product BLC: search data - {nameof(data)}, is empty >");

        if (data.IsGuid())
        {
            return _productSC.GetItemByID(data) ?? throw new ElementNotFoundException(null, data);
        }
        return _productSC.GetItemByName(data) ?? throw new ElementNotFoundException($"< Product with data '{data}' not found >", data);
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
        if (id.IsEmpty() || !id.IsGuid())
            throw new ValidationException("< Product BLC: ID for deletion is empty or not valid >");
        _productSC.DelItem(id);
    }
}