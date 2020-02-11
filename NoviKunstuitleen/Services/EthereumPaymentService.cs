﻿/*
    EmailSender.cs
    Auteur: Tako Lansbergen, Novi Hogeschool
    Studentnr.: 800009968
    Leerlijn: Praktijk 2
    Datum: 10 feb 2020
*/

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NBitcoin;
using Nethereum.Web3.Accounts;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.HdWallet;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace NoviKunstuitleen.Services
{

    /// <summary>
    /// Klasse voor verwerken van betalingen met de Ethereum coin en Infura API
    /// Implementeert IPaymentService
    /// </summary>
    public class EthereumPaymentService : IPaymentService
    {
        private IConfiguration _configuration { get; }
        private Wallet _wallet { get; }
        private string _apiURL { get; }

        // constructor
        public EthereumPaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            _wallet = new Wallet(_configuration.GetValue<string>("HDWallet:Mnemonic"), _configuration.GetValue<string>("HDWallet:Password"));
            _apiURL = _configuration.GetValue<string>("HDWallet:APIUrl");
        }

        /// <summary>
        /// Retourneert het saldo in Ethereum
        /// </summary>
        /// <returns>Het saldo als decimal</returns>
        public async Task<decimal> GetBalance(int userid)
        {
            // haal het saldo op in Wei (Ethereum 'cent')
            var account = _wallet.GetAccount(userid);
            var web3 = new Web3(_apiURL);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

            // en retourneer als decimal
            return Web3.Convert.FromWei(balance.Value);
        }


        /// <summary>
        /// Retourneert het unieke adres aan de hand van het user ID. Omdat er een HD-wallet gebruikt wordt is een unieke int voldoende om het adres te kunnen genereren
        /// </summary>
        /// <returns>Ethereum adres als string</returns>
        public string GetAddress(int userid)
        {
            var account = _wallet.GetAccount(userid);
            return account.Address;
        }

        /// <summary>
        /// Maak ethereum over naar een gebruiker binnen NoviKunstuitleen
        /// </summary>
        /// <returns>False indien er onvoldoende saldo was, anders True</returns>
        public async Task<bool> SendFunds(int userid, decimal amount, int recipientid)
        {
            var account = _wallet.GetAccount(userid);
            var recipient = _wallet.GetAccount(recipientid);
            var web3 = new Web3(_apiURL);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

            if (Web3.Convert.FromWei(balance.Value) >= amount)
            {

                var transaction = await web3.Eth.GetEtherTransferService()
                    .TransferEtherAndWaitForReceiptAsync(recipient.Address, amount);
                return true;
            }

            // onvoldoende saldo
            return false;
        }

        /// <summary>
        /// Maak ethereum over naar een adres buiten NoviKunstuitleen
        /// </summary>
        /// <returns>False indien er onvoldoende saldo was, anders True</returns>
        public async Task<bool> SendFunds(int userid, decimal amount, string address)
        {
            var account = _wallet.GetAccount(userid);
            var web3 = new Web3(_apiURL);
            var balance = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

            if (Web3.Convert.FromWei(balance.Value) >= amount)
            {
                var transaction = await web3.Eth.GetEtherTransferService()
                    .TransferEtherAndWaitForReceiptAsync(address, amount);
                return true;
            }

            // onvoldoende saldo
            return false;
        }
    }
}
