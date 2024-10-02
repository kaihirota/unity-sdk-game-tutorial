/*
 * Indexer Search API
 *
 * This API implements endpoints to power data driven marketplace and game experiences
 *
 * The version of the OpenAPI document: 1.0
 * Contact: helpmebuild@immutable.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using OpenAPIDateConverter = Immutable.Search.Client.OpenAPIDateConverter;

namespace Immutable.Search.Model
{
    /// <summary>
    /// NFT quote result
    /// </summary>
    [DataContract(Name = "NFTQuoteResult")]
    public partial class NFTQuoteResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NFTQuoteResult" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected NFTQuoteResult() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NFTQuoteResult" /> class.
        /// </summary>
        /// <param name="chain">chain (required).</param>
        /// <param name="tokenId">tokenId (required).</param>
        /// <param name="marketStack">marketStack (required).</param>
        /// <param name="marketNft">marketNft (required).</param>
        /// <param name="marketCollection">marketCollection (required).</param>
        public NFTQuoteResult(Chain chain = default(Chain), string tokenId = default(string), Market marketStack = default(Market), Market marketNft = default(Market), Market marketCollection = default(Market))
        {
            // to ensure "chain" is required (not null)
            if (chain == null)
            {
                throw new ArgumentNullException("chain is a required property for NFTQuoteResult and cannot be null");
            }
            this.Chain = chain;
            // to ensure "tokenId" is required (not null)
            if (tokenId == null)
            {
                throw new ArgumentNullException("tokenId is a required property for NFTQuoteResult and cannot be null");
            }
            this.TokenId = tokenId;
            // to ensure "marketStack" is required (not null)
            if (marketStack == null)
            {
                throw new ArgumentNullException("marketStack is a required property for NFTQuoteResult and cannot be null");
            }
            this.MarketStack = marketStack;
            // to ensure "marketNft" is required (not null)
            if (marketNft == null)
            {
                throw new ArgumentNullException("marketNft is a required property for NFTQuoteResult and cannot be null");
            }
            this.MarketNft = marketNft;
            // to ensure "marketCollection" is required (not null)
            if (marketCollection == null)
            {
                throw new ArgumentNullException("marketCollection is a required property for NFTQuoteResult and cannot be null");
            }
            this.MarketCollection = marketCollection;
        }

        /// <summary>
        /// Gets or Sets Chain
        /// </summary>
        [DataMember(Name = "chain", IsRequired = true, EmitDefaultValue = true)]
        public Chain Chain { get; set; }

        /// <summary>
        /// Gets or Sets TokenId
        /// </summary>
        [DataMember(Name = "token_id", IsRequired = true, EmitDefaultValue = true)]
        public string TokenId { get; set; }

        /// <summary>
        /// Gets or Sets MarketStack
        /// </summary>
        [DataMember(Name = "market_stack", IsRequired = true, EmitDefaultValue = true)]
        public Market MarketStack { get; set; }

        /// <summary>
        /// Gets or Sets MarketNft
        /// </summary>
        [DataMember(Name = "market_nft", IsRequired = true, EmitDefaultValue = true)]
        public Market MarketNft { get; set; }

        /// <summary>
        /// Gets or Sets MarketCollection
        /// </summary>
        [DataMember(Name = "market_collection", IsRequired = true, EmitDefaultValue = true)]
        public Market MarketCollection { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class NFTQuoteResult {\n");
            sb.Append("  Chain: ").Append(Chain).Append("\n");
            sb.Append("  TokenId: ").Append(TokenId).Append("\n");
            sb.Append("  MarketStack: ").Append(MarketStack).Append("\n");
            sb.Append("  MarketNft: ").Append(MarketNft).Append("\n");
            sb.Append("  MarketCollection: ").Append(MarketCollection).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

    }

}
