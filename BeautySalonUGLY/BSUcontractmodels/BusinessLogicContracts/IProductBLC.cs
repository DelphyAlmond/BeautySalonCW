using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface IProductBLC
{
    List<ProductDM> getAllProducts(bool onlyActive = true);
    List<ProductDM> getAllProductsByManufacturer(string manufacturerID, bool onlyActive = true);
    ProductDM GetProductByData(string data);
    void InsertPitem(ProductDM product);
    void UpdatePitem(ProductDM product);
    void DeletePitem(string id);
}
