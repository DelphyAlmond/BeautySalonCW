using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class CustomerBM
{
    [Required(ErrorMessage = "> ID обязателен")]
    public string? ID { get; set; }

    [Required(ErrorMessage = "> Требуется имя пользователя")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "(должно быть от 3 до 50 символов)")]
    public string? Username { get; set; }

    [Phone(ErrorMessage = "> Некорректный формат телефона")]
    public string? Phonenumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "> Бонусы не могут быть отрицательными")]
    public double Bonuses { get; set; }
}
