/*
    IEmailSender.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System.Threading.Tasks;

namespace NoviKunstuitleen.Services
{

    /// <summary>
    /// Interface voor functionaliteit tbv verzenden email
    /// </summary>
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}