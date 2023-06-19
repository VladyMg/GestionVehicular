using Microsoft.AspNetCore.Identity;

namespace GestionVehicular.Models;

public class Login : IdentityUser
{
    public string Cedula { get; set; }

    public string Contrasenia { get; set; }
}