using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;
using Immutable.Passport;
using Cysharp.Threading.Tasks;
using System.Net.Http;
using TMPro;

namespace HyperCasual.Runner
{
    /// <summary>
    /// The inventory view which displays the user's assets (e.g. skins).
    /// </summary>
    public class InventoryScreen : View
    {
        [SerializeField] private HyperCasualButton m_BackButton;
        [SerializeField] private AbstractGameEvent m_BackEvent;
        [SerializeField] private AssetListObject m_AssetObj = null;
        [SerializeField] private Transform m_ListParent = null;
        [SerializeField] private TextMeshProUGUI m_ErrorMessage = null;

        private List<AssetListObject> m_ListedAssets = new List<AssetListObject>();

        /// <summary>
        /// Sets up the inventory list and fetches the user's assets.
        /// </summary>
        private async void OnEnable()
        {
            m_AssetObj.gameObject.SetActive(false); // Disable the template asset object initially
            m_BackButton.AddListener(OnBackButtonClick); // Listen for back button click

            if (Passport.Instance != null)
            {
                // Get user's assets
                List<TokenModel> assets = await GetAssets();

                // Clear existing assets from the list before populating the list with new assets
                ClearListedAssets();
                PopulateAssetList(assets);
            }
        }

        /// <summary>
        /// Clears all currently listed assets from the list.
        /// </summary>
        private void ClearListedAssets()
        {
            foreach (AssetListObject uiAsset in m_ListedAssets)
            {
                Destroy(uiAsset.gameObject);
            }
            m_ListedAssets.Clear();
        }

        /// <summary>
        /// Populates the list with the given assets.
        /// </summary>
        private void PopulateAssetList(List<TokenModel> assets)
        {
            foreach (TokenModel asset in assets)
            {
                AssetListObject newAsset = Instantiate(m_AssetObj, m_ListParent); // Create a new asset object
                newAsset.gameObject.SetActive(true);
                newAsset.Initialise(asset); // Initialise the asset with data
                m_ListedAssets.Add(newAsset); // Add to the list of displayed assets
            }
        }

        /// <summary>
        /// Retrieves the users's wallet address.
        /// </summary>
        private async UniTask<string> GetWalletAddress()
        {
            List<string> accounts = await Passport.Instance.ZkEvmRequestAccounts();
            return accounts.Count > 0 ? accounts[0] : string.Empty; // Return the first wallet address
        }

        /// <summary>
        /// Fetches the user's skins from the API based on the wallet address.
        /// </summary>
        /// <returns>A list of user's assets.</returns>
        private async UniTask<List<TokenModel>> GetAssets()
        {
            Debug.Log("Fetching user assets...");
            List<TokenModel> tokens = new List<TokenModel>();

            try
            {
                string address = await GetWalletAddress();
                if (!string.IsNullOrEmpty(address))
                {
                    using var client = new HttpClient();
                    string url = $"https://api.sandbox.immutable.com/v1/chains/imtbl-zkevm-testnet/accounts/{address}/nfts?contract_address={Contract.SKIN_CONTRACT}&page_size=10";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Debug.Log($"Response: {responseBody}");

                        ListTokenResponse tokenResponse = JsonUtility.FromJson<ListTokenResponse>(responseBody);
                        tokens.AddRange(tokenResponse.result);
                    }
                    else
                    {
                        m_ErrorMessage.gameObject.SetActive(true);
                        m_ErrorMessage.text = "Failed to fetch assets";
                    }
                }
                else
                {
                    Debug.Log($"Failed to the user's wallet address");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to fetch assets: {ex.Message}");
            }

            return tokens;
        }

        /// <summary>
        /// Cleans up listeners and UI elements.
        /// </summary>
        private void OnDisable()
        {
            m_BackButton.RemoveListener(OnBackButtonClick);
            ClearListedAssets();
        }

        /// <summary>
        /// Handles the back button click
        /// </summary>
        private void OnBackButtonClick()
        {
            m_BackEvent.Raise();
        }
    }
}
