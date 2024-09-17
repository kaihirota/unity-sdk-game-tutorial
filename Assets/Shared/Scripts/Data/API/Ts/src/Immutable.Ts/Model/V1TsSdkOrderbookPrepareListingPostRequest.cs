/*
 * TS SDK API
 *
 * running ts sdk as an api
 *
 * The version of the OpenAPI document: 1.0.0
 * Contact: contact@immutable.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Immutable.Ts.Model
{
    /// <summary>
    ///     V1TsSdkOrderbookPrepareListingPostRequest
    /// </summary>
    [DataContract(Name = "_v1_ts_sdk_orderbook_prepareListing_post_request")]
    public class V1TsSdkOrderbookPrepareListingPostRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="V1TsSdkOrderbookPrepareListingPostRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected V1TsSdkOrderbookPrepareListingPostRequest()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="V1TsSdkOrderbookPrepareListingPostRequest" /> class.
        /// </summary>
        /// <param name="buy">buy (required).</param>
        /// <param name="makerAddress">makerAddress (required).</param>
        /// <param name="orderExpiry">orderExpiry.</param>
        /// <param name="sell">sell (required).</param>
        public V1TsSdkOrderbookPrepareListingPostRequest(V1TsSdkOrderbookPrepareListingPostRequestBuy buy = default,
            string makerAddress = default, DateTime orderExpiry = default,
            V1TsSdkOrderbookPrepareListingPostRequestSell sell = default)
        {
            // to ensure "buy" is required (not null)
            if (buy == null)
                throw new ArgumentNullException(
                    "buy is a required property for V1TsSdkOrderbookPrepareListingPostRequest and cannot be null");
            Buy = buy;
            // to ensure "makerAddress" is required (not null)
            if (makerAddress == null)
                throw new ArgumentNullException(
                    "makerAddress is a required property for V1TsSdkOrderbookPrepareListingPostRequest and cannot be null");
            MakerAddress = makerAddress;
            // to ensure "sell" is required (not null)
            if (sell == null)
                throw new ArgumentNullException(
                    "sell is a required property for V1TsSdkOrderbookPrepareListingPostRequest and cannot be null");
            Sell = sell;
            OrderExpiry = orderExpiry;
        }

        /// <summary>
        ///     Gets or Sets Buy
        /// </summary>
        [DataMember(Name = "buy", IsRequired = true, EmitDefaultValue = true)]
        public V1TsSdkOrderbookPrepareListingPostRequestBuy Buy { get; set; }

        /// <summary>
        ///     Gets or Sets MakerAddress
        /// </summary>
        [DataMember(Name = "makerAddress", IsRequired = true, EmitDefaultValue = true)]
        public string MakerAddress { get; set; }

        /// <summary>
        ///     Gets or Sets OrderExpiry
        /// </summary>
        [DataMember(Name = "orderExpiry", EmitDefaultValue = false)]
        public DateTime OrderExpiry { get; set; }

        /// <summary>
        ///     Gets or Sets Sell
        /// </summary>
        [DataMember(Name = "sell", IsRequired = true, EmitDefaultValue = true)]
        public V1TsSdkOrderbookPrepareListingPostRequestSell Sell { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class V1TsSdkOrderbookPrepareListingPostRequest {\n");
            sb.Append("  Buy: ").Append(Buy).Append("\n");
            sb.Append("  MakerAddress: ").Append(MakerAddress).Append("\n");
            sb.Append("  OrderExpiry: ").Append(OrderExpiry).Append("\n");
            sb.Append("  Sell: ").Append(Sell).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        ///     Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}