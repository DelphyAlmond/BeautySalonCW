using BSUcontrmodels.Enums;
using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;
using System.Text.RegularExpressions;

namespace BSUcontrmodels.DataModels;

public class WorkerDM(string id, string fName, Post postType, string password, DateTime birthDate,
    DateTime employmentDate, bool isDel) : IValidation
{
    public string ID { get; private set; } = id;
    public string FullName { get; private set; } = fName;
    public Post PostType { get; private set; } = postType;
    public string Password { get; private set; } = password;
    public bool IsDeleted { get; private set; } = isDel;

    public DateTime BirthDate { get; private set; } = birthDate;
    public DateTime EmploymentDate { get; private set; } = employmentDate;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Worker: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Worker: ID is not valid, not GUID >");
        if (FullName.IsEmpty()) throw new ValidationException("< Worker: Username is empty >");
        if (Password.IsEmpty()) throw new ValidationException("< Password is not stated >");
        if (BirthDate.Date > DateTime.Now.AddYears(-16).Date || (EmploymentDate - BirthDate).TotalDays/365 < 16)
            throw new ValidationException("< Minors CANNOT BE HIRED >");
    }
}
