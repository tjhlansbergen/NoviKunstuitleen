/*
    ErrorViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 24 dec 2019
*/

using System;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public string ReturnToController { get; set; }
        public string ReturnToAction { get; set; }
    }
}
