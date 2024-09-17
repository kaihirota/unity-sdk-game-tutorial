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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Immutable.Search.Model
{
    /// <summary>
    ///     Stack bundle includes stacks, markets and listings
    /// </summary>
    [DataContract(Name = "StackBundle")]
    public class StackBundle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StackBundle" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected StackBundle()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StackBundle" /> class.
        /// </summary>
        /// <param name="stack">stack (required).</param>
        /// <param name="stackCount">Total count of NFTs in the stack matching the filter params (required).</param>
        /// <param name="market">market (required).</param>
        /// <param name="listings">List of open listings for the stack. (required).</param>
        public StackBundle(Stack stack = default, int stackCount = default, Market market = default,
            List<Listing> listings = default)
        {
            // to ensure "stack" is required (not null)
            if (stack == null)
                throw new ArgumentNullException("stack is a required property for StackBundle and cannot be null");
            Stack = stack;
            StackCount = stackCount;
            // to ensure "market" is required (not null)
            if (market == null)
                throw new ArgumentNullException("market is a required property for StackBundle and cannot be null");
            Market = market;
            // to ensure "listings" is required (not null)
            if (listings == null)
                throw new ArgumentNullException("listings is a required property for StackBundle and cannot be null");
            Listings = listings;
        }

        /// <summary>
        ///     Gets or Sets Stack
        /// </summary>
        [DataMember(Name = "stack", IsRequired = true, EmitDefaultValue = true)]
        public Stack Stack { get; set; }

        /// <summary>
        ///     Total count of NFTs in the stack matching the filter params
        /// </summary>
        /// <value>Total count of NFTs in the stack matching the filter params</value>
        /// <example>1</example>
        [DataMember(Name = "stack_count", IsRequired = true, EmitDefaultValue = true)]
        public int StackCount { get; set; }

        /// <summary>
        ///     Gets or Sets Market
        /// </summary>
        [DataMember(Name = "market", IsRequired = true, EmitDefaultValue = true)]
        public Market Market { get; set; }

        /// <summary>
        ///     List of open listings for the stack.
        /// </summary>
        /// <value>List of open listings for the stack.</value>
        [DataMember(Name = "listings", IsRequired = true, EmitDefaultValue = true)]
        public List<Listing> Listings { get; set; }

        [DataMember(Name = "notListed", IsRequired = false, EmitDefaultValue = true)]
        public List<Listing>? NotListed { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class StackBundle {\n");
            sb.Append("  Stack: ").Append(Stack).Append("\n");
            sb.Append("  StackCount: ").Append(StackCount).Append("\n");
            sb.Append("  Market: ").Append(Market).Append("\n");
            sb.Append("  Listings: ").Append(Listings).Append("\n");
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