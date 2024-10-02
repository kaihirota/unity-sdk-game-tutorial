using System;
using System.Collections.Generic;
using System.Net.Http;
using Cysharp.Threading.Tasks;
using HyperCasual.Core;
using Immutable.Passport;
using UnityEngine;
using Xsolla.Core;
using TMPro;
using Immutable.Search.Client;
using Immutable.Search.Model;
using Immutable.Search.Api;

namespace HyperCasual.Runner
{
    /// <summary>
    ///     The inventory view which displays the player's assets (e.g. skins).
    /// </summary>
    public class ShopScreen : View
    {
        [SerializeField] private HyperCasualButton m_BackButton;
        [SerializeField] private HyperCasualButton m_AddButton;
        [SerializeField] private AbstractGameEvent m_BackEvent;
        [SerializeField] private BalanceObject m_Balance;
        [SerializeField] private PackListObject m_PackObj;
        [SerializeField] private Transform m_ListParent;
        [SerializeField] private InfiniteScrollGridView m_ScrollView;
        [SerializeField] private AddFunds m_AddFunds;

        // Pagination
        private bool m_IsLoadingMore;
        private PageModel m_Page;

        /// <summary>
        ///     Sets up the inventory list and fetches the player's assets.
        /// </summary>
        private async void OnEnable()
        {
            // Hide pack template item
            m_PackObj.gameObject.SetActive(false);

            m_BackButton.RemoveListener(OnBackButtonClick);
            m_BackButton.AddListener(OnBackButtonClick);

            m_AddButton.RemoveListener(OnAddFundsButtonClick);
            m_AddButton.AddListener(OnAddFundsButtonClick);

            if (Passport.Instance != null)
            {
                // Setup infinite scroll view and load packs
                m_ScrollView.OnCreateItemView += OnCreateItemView;
                m_ScrollView.TotalItemCount = 1;

                // Gets the player's balance
                m_Balance.UpdateBalance();
            }
        }

        /// <summary>
        ///     Configures the asset list item view
        /// </summary>
        private void OnCreateItemView(int index, GameObject item)
        {
            if (index < 1)
            {
                var itemComponent = item.GetComponent<PackListObject>();
                itemComponent.Initialise(
                        "Survivor pack", 
                        "Get 5 shields that provide temporary immunity to obstacles like fire and trees. Stay safe and keep running!", 
                        "https://cyan-electric-peafowl-878.mypinata.cloud/ipfs/QmSA7X4Jxq2k8oTAricFrYrTrgXajLBLKvVoSfZoM6z4pF"
                    );
                // Set up click listener
                var clickable = item.GetComponent<ClickableView>();
                if (clickable != null)
                {
                    clickable.ClearAllSubscribers();
                    clickable.OnClick += () =>
                    {
                        var view = UIManager.Instance.GetView<AssetDetailsView>();
                        UIManager.Instance.Show(view);
                        // view.Initialise(asset);
                    };
                }
            }
        }

        /// <summary>
        ///     Cleans up views and handles the back button click
        /// </summary>
        private void OnBackButtonClick()
        {
            // Reset pagination information
            m_Page = null;

            // Reset the InfiniteScrollView
            m_ScrollView.TotalItemCount = 0;
            m_ScrollView.Clear();

            // Trigger back button event
            m_BackEvent.Raise();
        }

        /// <summary>
        ///     handles the add funds button click
        /// </summary>
        private void OnAddFundsButtonClick()
        {
            m_AddFunds.Show();
        }
    }
}