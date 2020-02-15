/*
    MessageViewModel.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 15 feb 2020
*/

using System;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /// <summary>
    /// MVC Model voor bijbehorende view
    /// </summary>
    public class MessageViewModel
    {
        public string Message { get; set; }
        public string[] Messages { get; set; }
        public string ReturnToController { get; set; }
        public string ReturnToAction { get; set; }
    }
}
