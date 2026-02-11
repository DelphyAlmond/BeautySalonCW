namespace BSUcontrmodels.Enums;
// Флаговое перечисление (бит.-е на основе степени 2-ки, возможны комбинации: *услуга)
public enum ProductType
{
    None = 0,
    Makeup = 2,
    Bodycare = 4,
    Accessories = 8,
    Service = 16
}
