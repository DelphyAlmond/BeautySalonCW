using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;
using System.Text.RegularExpressions;

namespace BSUcontrmodels.DataModels;

public class CustomerDM(string id, string username, string phonenumber,double bonuses) : IValidation
{
    public string ID { get; private set; } = id;
    public string Username { get; private set; } = username;
    public string Phonenumber { get; private set; } = phonenumber;
    public double Bonuses { get; private set; } = bonuses;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Customer: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Customer: ID is not valid, not GUID >");
        if (Username.IsEmpty()) throw new ValidationException("< Customer: Username is empty >");
        if (Phonenumber.IsEmpty()) throw new ValidationException("< Phone number is empty >");
        if (!Regex.IsMatch(Phonenumber, @"^(?:\+7|8)[\s\-]*(?:\(\d{3}\)|\d{3})[\s\-]*\d{3}[\s\-]*\d{2}[\s\-]*\d{2}$"))
            throw new ValidationException("< Phone number is not valid >");
        if (Bonuses < 0) throw new ValidationException("< Bonuses are negative >");
    }
}
