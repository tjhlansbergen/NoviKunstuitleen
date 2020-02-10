/*
    EmailSender.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 10 feb 2020
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace NoviKunstuitleen.Services
{

    /// <summary>
    /// Klasse voor verwerken van betalingen met de Ethereum coin gebruikmakend van de Infura API
    /// Implementeert IPaymentService
    /// </summary>
    public class EthereumPaymentService : IPaymentService
    {
        private IConfiguration _configuration { get; set; }

        public EthereumPaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public decimal GetBalance(int userid)
        {
            throw new NotImplementedException();
        }

        public string GetAddress(int userid)
        {
            throw new NotImplementedException();
        }

        public void SendFunds(decimal amount, int userid)
        {
            throw new NotImplementedException();
        }

        public void SendFunds(decimal amount, string address)
        {
            throw new NotImplementedException();
        }
    }
}
