using BSUcontractmodels.AdapterContracts.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface IManufacturerAdapter
{
    // same*>
    ManufacturerOR GetList();
    ManufacturerOR GetElement(string id);
    ManufacturerOR RegisterManufacturer(ManufacturerBM model);
    ManufacturerOR ChangeManufacturerInfo(ManufacturerBM model);
    ManufacturerOR RemoveManufacturer(string id);
}