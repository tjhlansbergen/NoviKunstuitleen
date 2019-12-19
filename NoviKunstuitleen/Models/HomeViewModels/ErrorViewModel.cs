using System;

namespace NoviKunstuitleen.Models.HomeViewModels
{
    /* 
    <summary>
    Action voor een te tonen melding, gebruik:
        <a asp-controller="Home" asp-action="Error">Lege error</a>
        <a asp-controller="Home" asp-action="Error" asp-route-message="Berichttekst">Error met breichttekst</a>
        <a asp-controller="Home" asp-action="Error" asp-route-message="Berichttekst" asp-route-returncontroller="Home" asp-route-returnaction="Create">Error met message en return route</a>
    </summary>
    */
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public string ReturnToController { get; set; }
        public string ReturnToAction { get; set; }
    }
}
