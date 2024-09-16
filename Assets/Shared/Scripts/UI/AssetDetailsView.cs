using System;
using System.Collections.Generic;
using System.Numerics;
using System.Net.Http;
using System.Text;
using System.Linq;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using Immutable.Passport;
using Immutable.Passport.Model;
using Immutable.Search.Model;
// using Immutable.Ts.Client;
// using Immutable.Ts.Api;
// using Immutable.Ts.Model;
using Immutable.Search.Api;
using Immutable.Search.Client;
using Immutable.Search.Model;

namespace HyperCasual.Runner
{
    public class AssetDetailsView : View
    {
        [SerializeField] private HyperCasualButton m_BackButton;
        [SerializeField] private BalanceObject m_Balance;
        [SerializeField] private ImageUrlObject m_Image;
        [SerializeField] private TextMeshProUGUI m_NameText;
        [SerializeField] private TextMeshProUGUI m_AmountText;
        // Market
        [SerializeField] private TextMeshProUGUI m_FloorPriceText = null;
        [SerializeField] private TextMeshProUGUI m_LastTradePriceText = null;
        // Details
        [SerializeField] private TextMeshProUGUI m_TokenIdText;
        [SerializeField] private TextMeshProUGUI m_CollectionText;
        // Attributes
        [SerializeField] private Transform m_AttributesListParent;
        [SerializeField] private AttributeView m_AttributeObj;
        // Actions
        [SerializeField] private HyperCasualButton m_SellButton;
        [SerializeField] private HyperCasualButton m_CancelButton;
        [SerializeField] private GameObject m_Progress = null;

        // Not listed
        [SerializeField] private GameObject m_EmptyNotListed;
        [SerializeField] private Transform m_NotListedParent = null;
        private List<AssetNotListedObject> m_NotListedViews = new List<AssetNotListedObject>();
        [SerializeField] private AssetNotListedObject m_NotListedObj = null;

        // Listings
        [SerializeField] private GameObject m_EmptyListing;
        [SerializeField] private Transform m_ListingParent = null;
        private List<AssetListingObject> m_ListingViews = new List<AssetListingObject>();
        [SerializeField] private AssetListingObject m_ListingObj = null;

        [SerializeField] private CustomDialog m_CustomDialog;

        private List<AttributeView> m_Attributes = new List<AttributeView>();
        private AssetModel m_Asset;
        private OldListing m_Listing;

        private void OnEnable()
        {
            m_AttributeObj.gameObject.SetActive(false); // Disable the template attribute object
            m_NotListedObj.gameObject.SetActive(false); // Hide not listed template object
            m_ListingObj.gameObject.SetActive(false); // Hide listing template object

            m_BackButton.RemoveListener(OnBackButtonClick);
            m_BackButton.AddListener(OnBackButtonClick);
            m_SellButton.RemoveListener(OnSellButtonClicked);
            m_SellButton.AddListener(OnSellButtonClicked);
            m_CancelButton.RemoveListener(OnCancelButtonClicked);
            m_CancelButton.AddListener(OnCancelButtonClicked);

            // Gets the player's balance
            m_Balance.UpdateBalance();
        }

        /// <summary>
        /// Initialises the UI based on the asset.
        /// </summary>
        /// <param name="asset">The asset to display.</param>
        public async void Initialise(AssetModel asset)
        {
            m_Asset = asset;

            m_NameText.text = m_Asset.name;
            m_TokenIdText.text = $"Token ID: {m_Asset.token_id}";
            m_CollectionText.text = $"Collection: {m_Asset.contract_address}";
            m_AmountText.text = "-";
            m_FloorPriceText.text = $"Floor price: -";
            m_LastTradePriceText.text = $"Last trade price: -";

            // Clear existing attributes
            ClearAttributes();

            // Populate attributes
            foreach (AssetAttribute a in m_Asset.attributes)
            {
                NFTMetadataAttribute attribute = new(traitType: a.trait_type, value: new NFTMetadataAttributeValue(a.value));
                AttributeView newAttribute = Instantiate(m_AttributeObj, m_AttributesListParent);
                newAttribute.gameObject.SetActive(true);
                newAttribute.Initialise(attribute);
                m_Attributes.Add(newAttribute);
            }

            // Download and display the image
            m_Image.LoadUrl(m_Asset.image);

            // Check if asset is listed
            m_Listing = await GetActiveListingId();
            m_SellButton.gameObject.SetActive(m_Listing == null);
            m_CancelButton.gameObject.SetActive(m_Listing != null);

            // Price if it's listed
            if (m_Listing != null)
            {
                string amount = m_Listing.buy[0].amount;
                decimal quantity = (decimal)BigInteger.Parse(amount) / (decimal)BigInteger.Pow(10, 18);
                m_AmountText.text = $"{quantity} IMR";
            }
            else
            {
                m_AmountText.text = "Not listed";
            }

            // Get market data
            GetMarketData();
        }

        private async void GetMarketData()
        {
            Configuration config = new Configuration();
            config.BasePath = Config.SEARCH_BASE_URL;
            var apiInstance = new SearchApi(config);

            try
            {
                QuotesForStacksResult response = await apiInstance.QuotesForStacksAsync(Config.CHAIN_NAME, Contract.SKIN, stackId: new List<string> { m_Asset.metadata_id });
                if (response.Result.Count > 0)
                {
                    StackQuoteResult quote = response.Result[0];
                    Immutable.Search.Model.Market? market = quote.MarketStack;

                    if (market?.FloorListing != null)
                    {
                        decimal quantity = (decimal)BigInteger.Parse(market.FloorListing.PriceDetails.Amount.Value) / (decimal)BigInteger.Pow(10, 18);
                        m_FloorPriceText.text = $"Floor price: {quantity} IMR";
                    }
                    else
                    {
                        m_FloorPriceText.text = $"Floor price: N/A";
                    }

                    if (market?.LastTrade?.PriceDetails?.Count > 0)
                    {
                        decimal quantity = (decimal)BigInteger.Parse(market.LastTrade.PriceDetails[0].Amount.Value) / (decimal)BigInteger.Pow(10, 18);
                        m_LastTradePriceText.text = $"Last trade price: {quantity} IMR";
                    }
                    else
                    {
                        m_LastTradePriceText.text = $"Last trade price: N/A";
                    }
                }
            }
            catch (ApiException e)
            {
                Debug.LogError("Exception when calling: " + e.Message);
                Debug.LogError("Status Code: " + e.ErrorCode);
                Debug.LogError(e.StackTrace);
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to get market data: {ex.Message}");
            }
        }

        // TODO not required one we have the NFT search endpoint
        private async UniTask<OldListing?> GetActiveListingId()
        {
            try
            {
                using var client = new HttpClient();
                string url = $"{Config.BASE_URL}/v1/chains/{Config.CHAIN_NAME}/orders/listings?sell_item_contract_address={Contract.SKIN}&sell_item_token_id={m_Asset.token_id}&status=ACTIVE";
                Debug.Log($"GetActiveListingId URL: {url}");

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ListingsResponse listingResponse = JsonUtility.FromJson<ListingsResponse>(responseBody);

                    // Check if the listing exists
                    if (listingResponse.result.Count > 0 && listingResponse.result[0].status.name == "ACTIVE")
                    {
                        return listingResponse.result[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to check sale status: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Handles the click event for the sell button.
        /// </summary>
        private async void OnSellButtonClicked()
        {
            m_SellButton.gameObject.SetActive(false);
            m_Progress.gameObject.SetActive(true);

            (bool result, string price) = await m_CustomDialog.ShowDialog(
                            $"List {m_Asset.name} for sale",
                            $"Enter your price below (in IMR):",
                            "Confirm",
                            negativeButtonText: "Cancel",
                            showInputField: true
                        );

            if (result)
            {
                decimal amount = Math.Floor(decimal.Parse(price) * (decimal)BigInteger.Pow(10, 18));
                string listingId = await PrepareListing($"{amount}");

                m_SellButton.gameObject.SetActive(listingId == null);
                m_CancelButton.gameObject.SetActive(listingId != null);
                m_Progress.gameObject.SetActive(false);

                if (listingId != null)
                {
                    // TODO update to use get stack bundle by stack ID endpoint instead
                    m_AmountText.text = $"{price} IMR";

                    return;// true;
                }
            }

            m_SellButton.gameObject.SetActive(true);
            m_Progress.gameObject.SetActive(false);

            return;// false;
        }

        /// <summary>
        /// Gets the details for the listing
        /// </summary>
        private async UniTask<Listing> GetListing(string listingId) // TODO To replace with get stack by ID endpoint
        {
            try
            {
                using var client = new HttpClient();
                string url = $"{Config.BASE_URL}/v1/chains/{Config.CHAIN_NAME}/orders/listings/{listingId}";
                Debug.Log($"Get listing URL: {url}");

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    OrderResponse orderResponse = JsonUtility.FromJson<OrderResponse>(responseBody);

                    return new Listing(

                        listingId: orderResponse.result.id,
                        priceDetails: new PriceDetails
                        (
                            token: new PriceDetailsToken(new ERC20Token(symbol: "IMR", contractAddress: Contract.TOKEN, decimals: 18)),
                            amount: new PaymentAmount(orderResponse.result.buy[0].amount, orderResponse.result.buy[0].amount),
                            feeInclusiveAmount: new PaymentAmount(orderResponse.result.buy[0].amount, orderResponse.result.buy[0].amount), // Mocked
                            fees: orderResponse.result.fees.Select(fee => new Immutable.Search.Model.Fee(
                                fee.amount, Immutable.Search.Model.Fee.TypeEnum.ROYALTY, fee.recipient_address)).ToList()
                        ),
                        tokenId: orderResponse.result.sell[0].token_id,
                        creator: orderResponse.result.account_address,
                        amount: "1"
                    );
                }
                else
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log($"Failed to get listing: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to get listing: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Prepares the listing for the asset.
        /// </summary>
        /// <param name="price">The price of the asset in smallest unit.</param>
        /// <returns>The listing ID is asset was successfully listed</returns>
        private async UniTask<string> PrepareListing(string price)
        {
            string address = SaveManager.Instance.WalletAddress;

            var data = new PrepareListingRequest
            {
                makerAddress = address,
                sell = new PrepareListingERC721Item
                {
                    contractAddress = Contract.SKIN,
                    tokenId = m_Asset.token_id,
                },
                buy = new PrepareListingERC20Item
                {
                    amount = price,
                    contractAddress = Contract.TOKEN,
                }
            };

            // Configuration config = new Configuration();
            // config.BasePath = Config.TS_BASE_URL;
            // var apiInstance = new DefaultApi(config);

            try
            {
                // V1TsSdkV1OrderbookPrepareListingPost200Response response = await apiInstance.V1TsSdkV1OrderbookPrepareListingPostAsync(new V1TsSdkV1OrderbookPrepareListingPostRequest
                // (
                //     makerAddress: address,
                //     sell: new V1TsSdkV1OrderbookPrepareListingPostRequestSell(
                //         new PrepareListingReqBodyERC721Item(contractAddress: Contract.SKIN, tokenId: asset.TokenId, type: PrepareListingReqBodyERC721Item.TypeEnum.ERC721)),
                //     buy: new V1TsSdkV1OrderbookPrepareListingPostRequestBuy(
                //         new PrepareListingReqBodyERC20Item(amount: price, contractAddress: Contract.TOKEN, type: PrepareListingReqBodyERC20Item.TypeEnum.ERC20))
                // ));

                // var transactionAction = response.Actions.FirstOrDefault(action => action.GetPrepareListingResBodyTransactionAction() != null);
                // if (transactionAction != null)
                // {
                //     PrepareListingResBodyTransactionAction tx = transactionAction.GetPrepareListingResBodyTransactionAction();
                //     var transactionResponse = await Passport.Instance.ZkEvmSendTransactionWithConfirmation(new TransactionRequest
                //     {
                //         to = tx.PopulatedTransactions.To,
                //         data = tx.PopulatedTransactions.Data,
                //         value = "0"
                //     });

                //     if (transactionResponse.status != "1")
                //     {
                //         await m_CustomDialog.ShowDialog("Error", "Failed to prepare listing.", "OK");
                //         return null;
                //     }
                // }

                // // Sign payload
                // var signableAction = response.Actions.FirstOrDefault(action => action.GetPrepareListingResBodySignableAction() != null);
                // if (signableAction != null)
                // {
                //     PrepareListingResBodySignableActionMessage message = signableAction.GetPrepareListingResBodySignableAction().Message;

                //     var eip712TypedData = new TsEIP712TypedData
                //     {
                //         domain = message.Domain,
                //         types = message.Types,
                //         message = message.Value,
                //         primaryType = "OrderComponents"
                //     };

                //     eip712TypedData.message.Add("EIP712Domain", new List<PrepareListingResBodyRecordStringTypedDataFieldValueInner>
                //         {
                //             new PrepareListingResBodyRecordStringTypedDataFieldValueInner(name: "name", type: "string"),
                //             new PrepareListingResBodyRecordStringTypedDataFieldValueInner(name: "version", type: "string"),
                //             new PrepareListingResBodyRecordStringTypedDataFieldValueInner(name: "chainId", type: "uint256"),
                //             new PrepareListingResBodyRecordStringTypedDataFieldValueInner(name: "verifyingContract", type: "address")
                //         });

                //     Debug.Log($"EIP712TypedData: {JsonUtility.ToJson(eip712TypedData)}");
                //     string signature = await Passport.Instance.ZkEvmSignTypedDataV4(JsonUtility.ToJson(eip712TypedData));
                //     Debug.Log($"Signature: {signature}");

                //     // (bool result, string signature) = await m_CustomDialog.ShowDialog(
                //     //     "Confirm listing",
                //     //     "Enter signed payload:",
                //     //     "Confirm",
                //     //     negativeButtonText: "Cancel",
                //     //     showInputField: true
                //     // );
                //     // if (result)
                //     // {
                //     return await ListAsset(signature, response, address);
                //     // }
                // }

                var json = JsonUtility.ToJson(data);
                Debug.Log($"json = {json}");

                using var client = new HttpClient();
                using var req = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:6060/v1/ts-sdk/v1/orderbook/prepareListing")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                using var res = await client.SendAsync(req);

                if (!res.IsSuccessStatusCode)
                {
                    await m_CustomDialog.ShowDialog("Error", "Failed to prepare listing.", "OK");
                    return null;
                }

                string responseBody = await res.Content.ReadAsStringAsync();
                PrepareListingResponse response = JsonUtility.FromJson<PrepareListingResponse>(responseBody);

                // Send transaction if required
                var transaction = response.actions.FirstOrDefault(action => action.type == "TRANSACTION");
                if (transaction != null)
                {
                    var transactionResponse = await Passport.Instance.ZkEvmSendTransactionWithConfirmation(new TransactionRequest
                    {
                        to = transaction.populatedTransactions.to,
                        data = transaction.populatedTransactions.data,
                        value = "0"
                    });

                    if (transactionResponse.status != "1")
                    {
                        await m_CustomDialog.ShowDialog("Error", "Failed to prepare listing.", "OK");
                        return null;
                    }
                }

                // Sign payload
                var signable = response.actions.FirstOrDefault(action => action.type == "SIGNABLE");
                if (signable != null)
                {
                    Debug.Log($"Sign: {JsonUtility.ToJson(signable.message)}");

                    signable.message.types.EIP712Domain = new List<NameType>
                    {
                        new NameType { name = "name", type = "string" },
                        new NameType { name = "version", type = "string" },
                        new NameType { name = "chainId", type = "uint256" },
                        new NameType { name = "verifyingContract", type = "address" }
                    };

                    var eip712TypedData = new EIP712TypedData
                    {
                        domain = signable.message.domain,
                        types = signable.message.types,
                        message = signable.message.value,
                        primaryType = "OrderComponents"
                    };

                    Debug.Log($"EIP712TypedData: {JsonUtility.ToJson(eip712TypedData)}");
                    string signature = await Passport.Instance.ZkEvmSignTypedDataV4(JsonUtility.ToJson(eip712TypedData));
                    Debug.Log($"Signature: {signature}");

                    // (bool result, string signature) = await m_CustomDialog.ShowDialog(
                    //     "Confirm listing",
                    //     "Enter signed payload:",
                    //     "Confirm",
                    //     negativeButtonText: "Cancel",
                    //     showInputField: true
                    // );
                    // if (result)
                    // {
                    return await ListAsset(signature, response, address);
                    // }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to sell: {ex.Message}");
                await m_CustomDialog.ShowDialog("Error", "Failed to prepare listing", "OK");
            }

            return null;
        }

        /// <summary>
        /// Finalises the listing of the asset.
        /// </summary>
        /// <param name="signature">The signature for the listing.</param>
        /// <param name="preparedListing">The prepared listing data.</param>
        /// <param name="address">The wallet address of the user.</param>
        private async UniTask<string?> ListAsset(string signature, PrepareListingResponse preparedListing, string address)
        {
            var data = new CreateListingRequest
            {
                makerFees = new List<CreateListingFeeValue>(),
                orderComponents = preparedListing.orderComponents,
                orderHash = preparedListing.orderHash,
                orderSignature = signature
            };

            try
            {

                // V1TsSdkV1OrderbookCreateListingPost200Response response = await apiInstance.V1TsSdkV1OrderbookCreateListingPostAsync(new V1TsSdkV1OrderbookCreateListingPostRequest
                // (
                //     makerAddress: address,
                //     orderComponents: new CreateListingReqBodyOrderComponents(
                //         conduitKey: preparedListing.OrderComponents.ConduitKey,
                //         conduitKey: preparedListing.OrderComponents.ConduitKey,
                //     ),
                //     orderHash: preparedListing.OrderHash,
                //     orderSignature: signature
                // ));

                var json = JsonUtility.ToJson(data);
                Debug.Log($"json = {json}");

                using var client = new HttpClient();
                using var req = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:6060/v1/ts-sdk/v1/orderbook/createListing")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                using var res = await client.SendAsync(req);

                if (!res.IsSuccessStatusCode)
                {
                    string errorBody = await res.Content.ReadAsStringAsync();
                    Debug.Log($"Error: {errorBody}");
                    await m_CustomDialog.ShowDialog("Error", "Failed to list", "OK");
                    return null;
                }
                else
                {
                    string responseBody = await res.Content.ReadAsStringAsync();
                    CreateListingResponse response = JsonUtility.FromJson<CreateListingResponse>(responseBody);
                    Debug.Log($"Listing ID: {response.result.id}");

                    // Validate that listing is active
                    await ConfirmListingStatus(response.result.id, "ACTIVE");
                    return response.result.id;
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to list: {ex.Message}");
                await m_CustomDialog.ShowDialog("Error", "Failed to list", "OK");
                return null;
            }
        }

        /// <summary>
        /// Cancels the listing of the asset.
        /// </summary>
        private async void OnCancelButtonClicked()
        {
            Debug.Log($"Cancel listing {m_Listing.id}");

            m_CancelButton.gameObject.SetActive(false);
            m_Progress.gameObject.SetActive(true);

            string address = SaveManager.Instance.WalletAddress;
            var data = new CancelListingRequest
            {
                accountAddress = address,
                orderIds = new List<string> { m_Listing.id }
            };

            try
            {
                var json = JsonUtility.ToJson(data);
                Debug.Log($"json = {json}");

                using var client = new HttpClient();
                using var req = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:6060/v1/ts-sdk/v1/orderbook/cancelOrdersOnChain")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                using var res = await client.SendAsync(req);

                if (!res.IsSuccessStatusCode)
                {
                    await m_CustomDialog.ShowDialog("Error", "Failed to cancel listing", "OK");
                    return;// false;
                }

                string responseBody = await res.Content.ReadAsStringAsync();
                Debug.Log($"responseBody = {responseBody}");

                CancelListingResponse response = JsonUtility.FromJson<CancelListingResponse>(responseBody);
                if (response?.cancellationAction.populatedTransaction.to != null)
                {
                    var transactionResponse = await Passport.Instance.ZkEvmSendTransactionWithConfirmation(new TransactionRequest()
                    {
                        to = response.cancellationAction.populatedTransaction.to, // Immutable seaport contract
                        data = response.cancellationAction.populatedTransaction.data, // fd9f1e10 cancel
                        value = "0"
                    });

                    if (transactionResponse.status == "1")
                    {
                        // Validate that listing has been cancelled
                        await ConfirmListingStatus(m_Listing.id, "CANCELLED");

                        // TODO update to use get stack bundle by stack ID endpoint instead

                        m_SellButton.gameObject.SetActive(true);
                        m_Progress.gameObject.SetActive(false);
                        m_AmountText.text = "Not listed";

                        return;// true;
                    }
                    else
                    {
                        m_Progress.gameObject.SetActive(false);
                        m_CancelButton.gameObject.SetActive(true);
                        await m_CustomDialog.ShowDialog("Error", "Failed to cancel listing", "OK");
                        return;// false;
                    }
                }

                m_Progress.gameObject.SetActive(false);
                m_CancelButton.gameObject.SetActive(true);
                return;// false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                m_Progress.gameObject.SetActive(false);
                m_CancelButton.gameObject.SetActive(true);
                await m_CustomDialog.ShowDialog("Error", "Failed to cancel listing", "OK");
                return;// false;
            }
        }

        /// <summary>
        /// Polls the listing status until it transitions to the given status or the operation times out after 1 minute.
        /// </summary>
        private async UniTask ConfirmListingStatus(string listingId, string status)
        {
            Debug.Log($"Confirming listing {listingId} is {status}...");

            bool conditionMet = await PollingHelper.PollAsync(
                $"https://api.dev.immutable.com/v1/chains/imtbl-zkevm-devnet/orders/listings/{listingId}",
                (responseBody) =>
                {
                    ListingResponse listingResponse = JsonUtility.FromJson<ListingResponse>(responseBody);
                    return listingResponse.result?.status.name == status;
                });

            if (conditionMet)
            {
                await m_CustomDialog.ShowDialog("Success", $"Listing is {status.ToLower()}.", "OK");
            }
            else
            {
                await m_CustomDialog.ShowDialog("Error", $"Failed to confirm if listing is {status.ToLower()}.", "OK");
            }
        }

        private void OnBackButtonClick()
        {
            UIManager.Instance.GoBack();
        }

        /// <summary>
        /// Removes all the attribute views
        /// </summary>
        private void ClearAttributes()
        {
            foreach (AttributeView attribute in m_Attributes)
            {
                Destroy(attribute.gameObject);
            }
            m_Attributes.Clear();
        }

        /// <summary>
        /// Cleans up data
        /// </summary>
        private void OnDisable()
        {
            m_NameText.text = "";
            m_TokenIdText.text = "";
            m_CollectionText.text = "";
            m_AmountText.text = "";
            m_FloorPriceText.text = "";
            m_LastTradePriceText.text = "";

            m_Asset = null;
            ClearAttributes();
        }
    }
}
