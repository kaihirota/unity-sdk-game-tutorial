using System;
using Cysharp.Threading.Tasks;
using HyperCasual.Core;
using Immutable.Search.Model;
using TMPro;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class AssetNotListedObject : View
    {
        [SerializeField] private TextMeshProUGUI m_TokenIdText;
        [SerializeField] private HyperCasualButton m_SellButton;
        [SerializeField] private GameObject m_Progress;
        private Listing m_Asset;
        private Func<Listing, UniTask<bool>> m_OnSell;

        private async void OnEnable()
        {
        }

        /// <summary>
        ///     Cleans up data
        /// </summary>
        private void OnDisable()
        {
            m_TokenIdText.text = "";

            m_Asset = null;
        }

        /// <summary>
        ///     Initialises the UI based on the order
        /// </summary>
        public async void Initialise(Listing listing, Func<Listing, UniTask<bool>> onSell)
        {
            m_Asset = listing;
            m_OnSell = onSell;
            m_TokenIdText.text = $"Token ID: {m_Asset.TokenId}";

            // Hide progress
            m_Progress.SetActive(false);

            // Set listener to button
            m_SellButton.RemoveListener(OnSellButtonClick);
            m_SellButton.AddListener(OnSellButtonClick);
        }

        /// <summary>
        ///     Handles the sell button click event.
        /// </summary>
        private async void OnSellButtonClick()
        {
            m_SellButton.gameObject.SetActive(false);
            m_Progress.SetActive(true);

            var success = await m_OnSell(m_Asset);

            m_SellButton.gameObject.SetActive(!success);
            m_Progress.SetActive(false);
        }
    }
}