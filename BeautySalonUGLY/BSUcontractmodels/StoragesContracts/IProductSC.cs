using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IProductSC
{
    List<ProductDM> GetProducts(bool onlyActive = true, string? manufacturerID = null);
    ProductDM? GetItemByID(string id);
    ProductDM? GetItemByName(string name);

    void AddItem(ProductDM item);
    void UpdItem(ProductDM item);
    void DelItem(string id);
}