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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Immutable.Ts.Model
{
    /// <summary>
    ///     V1TsSdkOrderbookPrepareOrderCancellationsPostRequest
    /// </summary>
    [DataContract(Name = "_v1_ts_sdk_orderbook_prepareOrderCancellations_post_request")]
    public class V1TsSdkOrderbookPrepareOrderCancellationsPostRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="V1TsSdkOrderbookPrepareOrderCancellationsPostRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected V1TsSdkOrderbookPrepareOrderCancellationsPostRequest()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="V1TsSdkOrderbookPrepareOrderCancellationsPostRequest" /> class.
        /// </summary>
        /// <param name="orderIds">orderIds (required).</param>
        public V1TsSdkOrderbookPrepareOrderCancellationsPostRequest(List<string> orderIds = default)
        {
            // to ensure "orderIds" is required (not null)
            if (orderIds == null)
                throw new ArgumentNullException(
                    "orderIds is a required property for V1TsSdkOrderbookPrepareOrderCancellationsPostRequest and cannot be null");
            OrderIds = orderIds;
        }

        /// <summary>
        ///     Gets or Sets OrderIds
        /// </summary>
        [DataMember(Name = "orderIds", IsRequired = true, EmitDefaultValue = true)]
        public List<string> OrderIds { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class V1TsSdkOrderbookPrepareOrderCancellationsPostRequest {\n");
            sb.Append("  OrderIds: ").Append(OrderIds).Append("\n");
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