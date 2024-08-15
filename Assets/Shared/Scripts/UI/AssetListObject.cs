using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;
using Immutable.Passport;
using TMPro;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Represents an asset in the player's inventory
    /// </summary>
    public class AssetListObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;
        [SerializeField] private TextMeshProUGUI m_TokenIdText = null;
        [SerializeField] private TextMeshProUGUI m_CollectionText = null;
        [SerializeField] private TextMeshProUGUI m_StatusText = null;
        [SerializeField] private RawImage m_Image = null;

        private TokenModel asset;

        /// <summary>
        /// Initialises the asset object with relevant data and updates the UI.
        /// </summary>
        public async void Initialise(TokenModel asset)
        {
            this.asset = asset;
            UpdateData();

            // Fetch sale status
            bool isOnSale = await IsListed(asset.token_id);
            m_StatusText.text = $"Status: {(isOnSale ? "Listed" : "Not listed")}";

            // Download and display the image
            if (!string.IsNullOrEmpty(asset.image))
            {
                StartCoroutine(DownloadImage(asset.image));
            }
        }

        /// <summary>
        /// Updates the text fields with asset data.
        /// </summary>
        private void UpdateData()
        {
            m_NameText.text = $"Name: {asset.name}";
            m_TokenIdText.text = $"Token ID: {asset.token_id}";
            m_CollectionText.text = $"Contract: {asset.contract_address}";
        }

        /// <summary>
        /// Checks if the asset is listed for sale.
        /// </summary>
        /// <param name="tokenId">The token ID of the asset.</param>
        /// <returns>True if the asset is listed, otherwise false.</returns>
        private async UniTask<bool> IsListed(string tokenId)
        {
            try
            {
                using var client = new HttpClient();
                string url = $"https://api.sandbox.immutable.com/v1/chains/imtbl-zkevm-testnet/orders/listings?sell_item_contract_address={Contract.SKIN_CONTRACT}&sell_item_token_id={tokenId}&status=ACTIVE";

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ListingResponse listingResponse = JsonUtility.FromJson<ListingResponse>(responseBody);

                    // Check if the listing exists
                    return listingResponse.result.Length > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to check sale status: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Downloads the image from the given URL and displays it.
        /// </summary>
        private IEnumerator DownloadImage(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            // Wait for the web request to complete
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                m_Image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                m_Image.gameObject.SetActive(true);
            }
            else
            {
                m_Image.gameObject.SetActive(false);
            }
        }
    }
}