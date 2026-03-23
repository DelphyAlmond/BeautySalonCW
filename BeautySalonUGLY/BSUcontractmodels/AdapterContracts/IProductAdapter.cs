using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface IProductAdapter
{
    ProductOR GetList(bool includeDelted);
    ProductOR GetElement(string id);
    ProductOR GetByManufacturer(string manufacturerId, bool includeDelted); // << extra
    ProductOR RegisterProduct(ProductBM model);
    ProductOR ChangeProductInfo(ProductBM model);
    ProductOR RemoveProduct(string id);
}