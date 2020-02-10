/*
    IPaymentService.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 10 feb 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviKunstuitleen.Services
{
    /// <summary>
    /// Interface voor functionaliteit tbv doen van crypto-currency betalingen
    /// </summary>
    interface IPaymentService
    {
        public decimal GetBalance(int userid);
        public string GetAddress(int userid);
        public void SendFunds(decimal amount, int userid);
        public void SendFunds(decimal amount, string address);
    }
}
