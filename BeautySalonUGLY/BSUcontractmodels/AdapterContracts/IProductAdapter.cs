using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface IProductAdapter
{
    ProductOR GetList();
    ProductOR GetElement(string id);
    ProductOR GetByManufacturer(string manufacturerId); // << extra
    ProductOR RegisterProduct(ProductBM model);
    ProductOR ChangeProductInfo(ProductBM model);
    ProductOR RemoveProduct(string id);
}