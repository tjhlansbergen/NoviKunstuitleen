using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Data
{

    /// <summary>
    /// Enumeratie voor de verschillende typen gebruiker
    /// </summary>
    public enum NoviUserType
    {
        Student, Docent, Admin
    };

    
    /// <summary>
    /// Aangepast implementatie van ASP IdentityUser met de extra velden voor het Novi nummer en gebruikerstype
    /// nb. de generieke velden zoals email en wachtwoord komen uit de superklasse IdentityUser
    /// </summary>
    public class NoviUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string NoviNumber { get; set; }
        public NoviUserType Type { get; set; }
    }

}
