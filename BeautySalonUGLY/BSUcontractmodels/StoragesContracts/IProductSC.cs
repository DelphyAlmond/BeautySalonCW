using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IProductSC
{
    List<ProductDM> GetList(bool onlyActive = true, string? manufacturerId = null);
    ProductDM? GetItemByID(string id);
    ProductDM? GetItemByName(string name);

    void AddItem(ProductDM item);
    void UpdateItem(ProductDM item);
    void DeleteItem(string id);
}