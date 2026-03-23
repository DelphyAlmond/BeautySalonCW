using BSUcontrmodels.Enums;
using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class ProductDM(string id, string prodTitle, ProductType prType, string manufacturerId,
    double price, string description, bool isDeleted) : IValidation
{
    // [ + ] т.к. во View модели оно теперь числится:
    private ManufacturerDM? _manufacturer { get; set; }

    public string ID { get; private set; } = id;
    public string ProductNaming { get; private set; } = prodTitle;
    public ProductType ProductType { get; private set; } = prType;
    public string ManufacturerID { get; private set; } = manufacturerId;
    public double Price { get; private set; } = price;
    public string Description { get; private set; } = description;
    public bool IsDeleted { get; private set; } = isDeleted;

    // Переход из DM во VM затруднён -> [ + ] (ref. Product VM)

    public string? ManufacturerName => _manufacturer?.Manufacturer ?? string.Empty;

    public ProductDM(string id, string prodTitle, ProductType prType, string manufacturerId,
    double price, string description, bool isDeleted, ManufacturerDM manufacturer) :
        this(id, prodTitle, prType, manufacturerId, price, description, isDeleted)
    {
        _manufacturer = manufacturer;
    }

    // [ + ] Также преобразование из BM в DM затруднено отсутствием флага "удалённого продукта":
    public ProductDM(string id, string prodTitle, ProductType prType, string manufacturerId,
    double price, string description) :
        this(id, prodTitle, prType, manufacturerId, price, description, false) { }

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Product: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Product: ID is not valid, not GUID >");
        if (ProductNaming.IsEmpty()) throw new ValidationException("< Naming of the product is empty >");
        if (ProductType == ProductType.None) throw new ValidationException("< Type of the product is not stated >");
        if (ManufacturerID.IsEmpty()) throw new ValidationException("< Product: Manufacturer ID is empty >");
        if (!ManufacturerID.IsGuid()) throw new ValidationException("< Product: Manufacturer ID is not valid >");
        if (Price <= 0) throw new ValidationException("< Product: Price is negative or not stated >");
    }
}
