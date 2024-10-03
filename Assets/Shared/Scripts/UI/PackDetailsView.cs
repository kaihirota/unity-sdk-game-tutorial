#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using Cysharp.Threading.Tasks;
using HyperCasual.Core;
using Immutable.Orderbook.Api;
using Immutable.Orderbook.Client;
using Immutable.Orderbook.Model;
using Immutable.Passport;
using Immutable.Passport.Model;
using Immutable.Search.Api;
using Immutable.Search.Model;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using ApiException = Immutable.Search.Client.ApiException;

namespace HyperCasual.Runner
{
    public class PackDetailsView : View
    {
        [SerializeField] private HyperCasualButton m_BackButton;
        [SerializeField] private BalanceObject m_Balance;
        [SerializeField] private ImageUrlObject m_Image;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_DescriptionText;
        [SerializeField] private TextMeshProUGUI m_AmountText;
        
        // Attributes
        [SerializeField] private Transform m_ItemsListParent;

        [SerializeField] private PackItemView m_ItemObj;

        // Actions
        [SerializeField] private HyperCasualButton m_BuyButton;
        [SerializeField] private GameObject m_Progress;

        [SerializeField] private CustomDialog m_CustomDialog;
        
        private readonly List<PackItemView> m_Items = new();

        private Pack? m_Pack = null;

        private void OnEnable()
        {
            m_ItemObj.gameObject.SetActive(false); // Disable the templateitem object
            
            m_BackButton.RemoveListener(OnBackButtonClick);
            m_BackButton.AddListener(OnBackButtonClick);
            m_BuyButton.RemoveListener(OnBuyButtonClicked);
            m_BuyButton.AddListener(OnBuyButtonClicked);

            // Gets the player's balance
            m_Balance.UpdateBalance();
        }

        /// <summary>
        ///     Cleans up data
        /// </summary>
        private void OnDisable()
        {
            m_NameText.text = "";
            m_DescriptionText.text = "";
            m_AmountText.text = "";

            m_Pack = null;
            ClearItems();
        }

        public async void Initialise(Pack pack)
        {
            m_Pack = pack;
            m_NameText.text = pack.name;
            m_DescriptionText.text = pack.description;
            m_Image.LoadUrl(pack.image);
            
            var quantity = (decimal)BigInteger.Parse(pack.price) / (decimal)BigInteger.Pow(10, 18);
            m_AmountText.text = $"{quantity} IMR";
            
            // Clear existing items
            ClearItems();

            // Populate items
            foreach (var item in m_Pack.items)
            {
                var newItem = Instantiate(m_ItemObj, m_ItemsListParent);
                newItem.gameObject.SetActive(true);
                newItem.Initialise(item);
                m_Items.Add(newItem);
            }
        }

        /// <summary>
        ///     Handles the click event for the sell button.
        /// </summary>
        private async void OnBuyButtonClicked()
        {
        }

        private void OnBackButtonClick()
        {
            UIManager.Instance.GoBack();
        }
        
        /// <summary>
        ///     Removes all the item views
        /// </summary>
        private void ClearItems()
        {
            foreach (var attribute in m_Items) Destroy(attribute.gameObject);
            m_Items.Clear();
        }
    }
}