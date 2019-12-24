/*
    AuthMessageSenderOptions.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System;

namespace NoviKunstuitleen.Services
{

    /// <summary>
    /// Gegevensklasse voor SendGrid API Credentials
    /// </summary>
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
