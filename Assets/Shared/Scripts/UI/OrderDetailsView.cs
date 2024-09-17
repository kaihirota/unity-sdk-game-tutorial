using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using Cysharp.Threading.Tasks;
using HyperCasual.Core;
using Immutable.Passport;
using Immutable.Passport.Model;
using Immutable.Search.Model;
using TMPro;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class OrderDetailsView : View
    {
        [SerializeField] private HyperCasualButton m_BackButton;
        [SerializeField] private BalanceObject m_Balance;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_Count;
        [SerializeField] private TextMeshProUGUI m_CollectionText;
        [SerializeField] private TextMeshProUGUI m_FloorPriceText;
        [SerializeField] private TextMeshProUGUI m_LastTradePriceText;
        [SerializeField] private Transform m_AttributesListParent;
        [SerializeField] private AttributeView m_AttributeObj;
        [SerializeField] private ImageUrlObject m_Image;
        [SerializeField] private GameObject m_Progress;
        [SerializeField] private CustomDialog m_CustomDialog;

        [SerializeField] private Transform m_ListingParent;
        [SerializeField] private ListingObject m_ListingObj;
        [SerializeField] private GameObject m_EmptyListingText;
        private readonly List<AttributeView> m_AttributeViews = new();
        private Listing m_Listing;
        private readonly List<ListingObject> m_ListingViews = new();

        private StackBundle m_Order;

        private async void OnEnable()
        {
            m_AttributeObj.gameObject.SetActive(false); // Hide template attribute object
            m_ListingObj.gameObject.SetActive(false); // Hide listing template object

            m_BackButton.RemoveListener(OnBackButtonClick);
            m_BackButton.AddListener(OnBackButtonClick);

            // Gets the player's balance
            m_Balance.UpdateBalance();
        }

        /// <summary>
        ///     Cleans up data
        /// </summary>
        private void OnDisable()
        {
            m_NameText.text = "";
            m_CollectionText.text = "";
            m_FloorPriceText.text = "";
            m_LastTradePriceText.text = "";

            m_Order = null;
            ClearAttributes();
            ClearListings();
        }

        /// <summary>
        ///     Initialises the UI based on the order
        /// </summary>
        public async void Initialise(StackBundle order)
        {
            m_Order = order;
            UpdateData();

            // Hide progress
            m_Progress.SetActive(false);
        }

        /// <summary>
        ///     Updates the text fields with asset data.
        /// </summary>
        private async void UpdateData()
        {
            m_NameText.text = m_Order.Stack.Name;
            m_Count.text = $"{m_Order.StackCount} items";
            m_CollectionText.text = $"Collection: {m_Order.Stack.ContractAddress}";

            // Floor price
            if (m_Order.Market?.FloorListing != null)
            {
                var amount = m_Order.Market.FloorListing.PriceDetails.Amount.Value;

                var quantity = (decimal)BigInteger.Parse(amount) / (decimal)BigInteger.Pow(10, 18);
                m_FloorPriceText.text = $"Floor price: {quantity} IMR";
            }
            else
            {
                m_FloorPriceText.text = "Floor price: N/A";
            }

            // Last trade price
            if (m_Order.Market?.LastTrade?.PriceDetails?.Count > 0)
            {
                var amount = m_Order.Market.LastTrade.PriceDetails[0].Amount.Value;

                var quantity = (decimal)BigInteger.Parse(amount) / (decimal)BigInteger.Pow(10, 18);
                m_LastTradePriceText.text = $"Last trade price: {quantity} IMR";
            }
            else
            {
                m_LastTradePriceText.text = "Last trade price: N/A";
            }

            // Clears all existing attributes
            ClearAttributes();

            // Populate attributes
            foreach (var attribute in m_Order.Stack.Attributes)
            {
                var newAttribute = Instantiate(m_AttributeObj, m_AttributesListParent); // Create a new asset object
                newAttribute.gameObject.SetActive(true);
                newAttribute.Initialise(attribute); // Initialise the view with data
                m_AttributeViews.Add(newAttribute); // Add to the list of displayed attributes
            }

            // Download and display the image
            m_Image.LoadUrl(m_Order.Stack.Image);

            // Clear all existing listings
            ClearListings();

            // Populate listings
            foreach (var stackListing in m_Order.Listings)
            {
                var newListing = Instantiate(m_ListingObj, m_ListingParent);
                newListing.gameObject.SetActive(true);
                newListing.Initialise(stackListing, OnBuyButtonClick); // Initialise the view with data
                m_ListingViews.Add(newListing); // Add to the list of displayed attributes
            }

            m_EmptyListingText.SetActive(m_Order.Listings.Count == 0);
        }

        /// <summary>
        ///     Removes all the attribute views
        /// </summary>
        private void ClearAttributes()
        {
            foreach (var attribute in m_AttributeViews) Destroy(attribute.gameObject);
            m_AttributeViews.Clear();
        }

        /// <summary>
        ///     Removes all the listing views
        /// </summary>
        private void ClearListings()
        {
            foreach (var listing in m_ListingViews) Destroy(listing.gameObject);
            m_ListingViews.Clear();
        }

        /// <summary>
        ///     Handles the buy button click event. Sends a request to fulfil an order,
        ///     processes the response, and updates the UI accordingly.
        /// </summary>
        private async UniTask<bool> OnBuyButtonClick(Listing listing)
        {
            var address = SaveManager.Instance.WalletAddress;
            var data = new FulfullOrderRequest
            {
                takerAddress = address,
                listingId = listing.ListingId,
                takerFees = listing.PriceDetails.Fees.Select(fee => new FulfullOrderRequestFee
                {
                    amount = fee.Amount,
                    recipientAddress = fee.RecipientAddress
                }).ToArray()
            };

            try
            {
                var json = JsonUtility.ToJson(data);
                Debug.Log($"json = {json}");

                using var client = new HttpClient();
                using var req = new HttpRequestMessage(HttpMethod.Post,
                    "http://localhost:8080/v1/ts-sdk/v1/orderbook/fulfillOrder")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                using var res = await client.SendAsync(req);

                if (!res.IsSuccessStatusCode)
                {
                    var errorBody = await res.Content.ReadAsStringAsync();
                    Debug.Log($"errorBody = {errorBody}");
                    await m_CustomDialog.ShowDialog("Error", "Failed to buy", "OK");
                    return false;
                }

                var responseBody = await res.Content.ReadAsStringAsync();
                var response = JsonUtility.FromJson<FulfullOrderResponse>(responseBody);
                if (response.transactions != null)
                {
                    foreach (var transaction in response.transactions)
                    {
                        var transactionHash = await Passport.Instance.ZkEvmSendTransaction(new TransactionRequest
                        {
                            to = transaction.to, // Immutable seaport contract
                            data = transaction.data, // 87201b41 fulfillAvailableAdvancedOrders
                            value = "0"
                        });
                    }

                    // Validate that order is fulfilled
                    await ConfirmListingStatus();
                    m_Balance.UpdateBalance(); // Update user's balance on successful buy

                    // TODO update to use get stack bundle by stack ID endpoint later
                    // Locally update stack listing
                    var listingToRemove = m_Order.Listings.FirstOrDefault(l => l.ListingId == listing.ListingId);
                    if (listingToRemove != null) m_Order.Listings.Remove(listingToRemove);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to buy: {ex.Message}");
                await m_CustomDialog.ShowDialog("Error", "Failed to buy", "OK");
                return false;
            }
        }

        /// <summary>
        ///     Polls the order status until it transitions to FULFILLED or the operation times out after 1 minute.
        /// </summary>
        private async UniTask ConfirmListingStatus()
        {
            Debug.Log("Confirming order is filled...");

            var conditionMet = await PollingHelper.PollAsync(
                $"https://api.sandbox.immutable.com/v1/chains/imtbl-zkevm-testnet/orders/listings/{m_Order.Listings[0].ListingId}",
                responseBody =>
                {
                    var listingResponse = JsonUtility.FromJson<ListingResponse>(responseBody);
                    return listingResponse.result?.status.name == "FILLED";
                });

            if (conditionMet)
                await m_CustomDialog.ShowDialog("Success", "Order is filled.", "OK");
            else
                await m_CustomDialog.ShowDialog("Error", "Failed to confirm if order is filled.", "OK");
        }

        /// <summary>
        ///     Handles the back button click
        /// </summary>
        private void OnBackButtonClick()
        {
            UIManager.Instance.GoBack();
        }
    }
}