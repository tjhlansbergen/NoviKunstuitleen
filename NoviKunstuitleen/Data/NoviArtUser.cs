/*
    NoviArtUser.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using Microsoft.AspNetCore.Identity;

namespace NoviKunstuitleen.Data
{
    /// <summary>
    /// Enumeratie voor de verschillende typen gebruiker
    /// </summary>
    public enum NoviUserType
    {
        Student, Medewerker, Admin, Root
    };
   
    /// <summary>
    /// Aangepast implementatie van ASP IdentityUser met de extra velden voor het Novi nummer en gebruikerstype en int's ipv guid's
    /// nb. de generieke velden zoals email en wachtwoord komen uit de superklasse IdentityUser
    /// </summary>
    public class NoviArtUser : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public string NoviNumber { get; set; }
        public NoviUserType Type { get; set; }
    }
}
