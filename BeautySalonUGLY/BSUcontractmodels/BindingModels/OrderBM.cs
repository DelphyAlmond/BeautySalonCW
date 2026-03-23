using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BSUcontractmodels.BindingModels;

public class OrderBM
{
    // [ * ] Binding Model-и используются для привязки входящих данных из HTTP-запроса
    // (например, из тела запроса, формы или query-параметров).
    // Она содержит только те поля, которые клиент может передать при создании или обновлении сущности.

    // Контроллер принимает объект CustomerBM как параметр действия (например, [FromBody] BM*model).
    // Затем этот объект передаётся в адаптер, который преобразует его в Data Model (DM->VM)
    // для дальнейшейработы с бизнес-логикой (BLC) и базой данных (SC).

    public string? ID { get; set; }
    public string? CustomerID { get; set; }
    public string? WorkerID { get; set; }

    // * left for us, kept inside system: public DateTime DateTime Date { get; set; }

    // ID в Binding Models - nullable,
    // чтобы различать создание (null) и обновление (задан).
    // Списки позиций – это List<...BM>
    // - в адаптере они будут преобразованы в соответствующие
    // Data Models (ProdUnitOrderLinkDM \ ServUnitVisitLinkDM)

    public List<ProdUnitBM>? Cart { get; set; }

    public double Discount { get; set; }
    public int Status { get; set; } // enum allows *
}
