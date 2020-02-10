/*
    NoviArtWallet.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 10 feb 2020
*/

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Klasse voor de wallet van een user
/// </summary>
namespace NoviKunstuitleen.Data
{
    public class NoviArtWallet
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
