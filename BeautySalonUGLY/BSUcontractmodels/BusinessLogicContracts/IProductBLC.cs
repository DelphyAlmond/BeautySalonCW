using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface IProductBLC
{
    List<ProductDM> GetAllProducts(bool onlyActive = true);
    List<ProductDM> GetAllProductsByManufacturer(string manufacturerID, bool onlyActive = true);
    ProductDM GetProductByData(string data);
    void InsertPitem(ProductDM product);
    void UpdatePitem(ProductDM product);
    void DeletePitem(string id);
}
