using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class UserRegistrationInformationsDTO
{
    public string emailAdress { get; set; }

    public string telefonNumber { get; set; }

    public string password { get; set; }
}