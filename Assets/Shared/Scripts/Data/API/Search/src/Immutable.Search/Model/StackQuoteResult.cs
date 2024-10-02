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
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Immutable.Search.Model
{
    /// <summary>
    ///     Stack quote result
    /// </summary>
    [DataContract(Name = "StackQuoteResult")]
    public class StackQuoteResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StackQuoteResult" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected StackQuoteResult()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StackQuoteResult" /> class.
        /// </summary>
        /// <param name="chain">chain (required).</param>
        /// <param name="stackId">stackId (required).</param>
        /// <param name="marketStack">marketStack (required).</param>
        /// <param name="marketCollection">marketCollection (required).</param>
        public StackQuoteResult(Chain chain = default, Guid stackId = default, Market marketStack = default,
            Market marketCollection = default)
        {
            // to ensure "chain" is required (not null)
            if (chain == null)
                throw new ArgumentNullException("chain is a required property for StackQuoteResult and cannot be null");
            Chain = chain;
            StackId = stackId;
            // to ensure "marketStack" is required (not null)
            if (marketStack == null)
                throw new ArgumentNullException(
                    "marketStack is a required property for StackQuoteResult and cannot be null");
            MarketStack = marketStack;
            // to ensure "marketCollection" is required (not null)
            if (marketCollection == null)
                throw new ArgumentNullException(
                    "marketCollection is a required property for StackQuoteResult and cannot be null");
            MarketCollection = marketCollection;
        }

        /// <summary>
        ///     Gets or Sets Chain
        /// </summary>
        [DataMember(Name = "chain", IsRequired = true, EmitDefaultValue = true)]
        public Chain Chain { get; set; }

        /// <summary>
        ///     Gets or Sets StackId
        /// </summary>
        [DataMember(Name = "stack_id", IsRequired = true, EmitDefaultValue = true)]
        public Guid StackId { get; set; }

        /// <summary>
        ///     Gets or Sets MarketStack
        /// </summary>
        [DataMember(Name = "market_stack", IsRequired = true, EmitDefaultValue = true)]
        public Market MarketStack { get; set; }

        /// <summary>
        ///     Gets or Sets MarketCollection
        /// </summary>
        [DataMember(Name = "market_collection", IsRequired = true, EmitDefaultValue = true)]
        public Market MarketCollection { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class StackQuoteResult {\n");
            sb.Append("  Chain: ").Append(Chain).Append("\n");
            sb.Append("  StackId: ").Append(StackId).Append("\n");
            sb.Append("  MarketStack: ").Append(MarketStack).Append("\n");
            sb.Append("  MarketCollection: ").Append(MarketCollection).Append("\n");
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