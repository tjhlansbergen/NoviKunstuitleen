/*
    Localization.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 02 jan 2020
*/

using System;

namespace NoviKunstuitleen
{

    /// <summary>
    /// Statische klasse voor interfacetekst
    /// </summary>
    public static class Localization
    {
        public const string MSG_LOGIN_FAILED = "Inlogpoging mislukt";
        public const string MSG_IMAGE_FORMAT = "Ongeldige foto, het bestand mag maximaal 2mb groot zijn, en moet van het type 'gif', 'png' of 'jpg' zijn!";
        public const string MSG_UNAUTHORIZED_REMOVAL = "U bent niet gemachtigd dit object te verwijderen.";
        public const string MSG_ARTPIECE_LOCKED = "Het kunstwerk wat u wilt verwijderen is momenteel verhuurd, u kunt deze pas verwijderen als de huurperiode verstreken is.";
        public const string MSG_LESSEE_LOCKED = "De gebruiker die u wilt verwijderen huurt op dit moment één of meerdere kunstwerken. U kunt deze gebruiker pas verwijderen als de huurperiode verstreken is.";
        public const string MSG_LESSER_LOCKED = "De gebruiker die u wilt verwijderen biedt nog kunstwerken te leen aan. U kunt de gebruiker pas verwijderen als al zijn/haar kunstwerken verwijderd zijn.";
        public const string MSG_PASSWORD_CHANGED = "Uw wachtwoord is gewijzigd";
        public const string MSG_ETHADDRESS_NOT_VALID = "U heeft een ongeldig ethereum adres opgegeven, probeer het opnieuw met een geldig adres.";
        public const string MSG_ETHAMOUNT_NOT_VALID = "U heeft een ongeldig bedrag opgegeven, probeer het opnieuw met een geldig bedrag met punt als scheidingsteken.";
        public const string MSG_INSUFFICIENT_ETH = "Uw heeft onvoldoende saldo voor deze transactie. Let op, voor iedere transactie worden transactiekosten berekent.";
        public const string MSG_PAYMENT_SUCCEEDED = "Uw transactie is succesvol verwerkt.";
        public const string MSG_PAYMENT_FAILED = "Uw is NIET geslaagd, ga terug om het opnieuw te proberen.";
        public const string MSG_ORDER_SUCCEEDED = "Uw transactie is succesvol verwerkt. U kunt contact opnemen met de aanbieder van het kunstobject via onderstaand emailadres voor het maken van een afspraak voor de overdracht.";

        public const string EML_CONFIRM_ACCOUNT = "Bevestig uw account";
        public const string EML_CONFIRM_CLICK = "Bevestig uw account door op de volgende link te klikken: ";
        public const string EML_CONFIRM = "bevestig";

        public const string VLD_REQUIRED = "Dit veld is verplicht";
        public const string VLD_MAX_CHARS_25 = "Maximaal 25 karakters";
        public const string VLD_MAX_CHARS_140 = "Maximaal 140 karakters";
        public const string VLD_USER_PASSWORD_LENGTH_6 = "Het wachtwoord moet minstens 6 tekens lang zijn.";
        public const string VLD_USER_PASSWORD_NOMATCH = "De wachtwoorden komen niet overeen";
        public const string VLD_MIN_MAX_MONTHS_3_12 = "Minimaal 3, maximaal 12 maanden";
        public const string VLD_ETH_ADDRESS_LENGTH = "Ongeldig adres.";
        public const string VLD_ARTPIECE_PRICE_FORMAT = "Ongeldige invoer, geef bedrag in ethereum gescheiden door een punt";

        public const string FLD_USER_EMAIL = "E-mail adres";
        public const string FLD_USER_DISPLAYNAME = "Weergavenaam";
        public const string FLD_USER_NUMBER = "Novi student/medewerker nummer";
        public const string FLD_USER_TYPE = "Ik ben een";
        public const string FLD_USER_PASSWORD = "Wachtwoord";
        public const string FLD_USER_PASSWORD_NEW = "Nieuw wachtwoord";
        public const string FLD_USER_PASSWORD_CONFIRM = "Bevestig wachtwoord";
        public const string FLD_USER_PASSWORD_CURRENT = "Huidig wachtwoord";

        public const string FLD_ARTPIECE_TITLE = "Titel";
        public const string FLD_ARTPIECE_ARTIST = "Kunstenaar";
        public const string FLD_ARTPIECE_PRICE = "Huurprijs";
        
        public const string FLD_ARTPIECE_MEASUREMENTS = "Afmetingen";
        public const string FLD_ARTPIECE_FRAME = "Type lijst";
        public const string FLD_ARTPIECE_IMAGE = "Foto";
        public const string FLD_ARTPIECE_DESCRIPTION = "Omschrijving";

        public const string FLD_WITHDRAWAL_AMOUNT = "Bedrag:";
        public const string FLD_WITHDRAWAL_ADDRESS = "Ontvanger:";

        public const string FLD_ORDER_MONTHS = "Aantal maanden";
    }
}
