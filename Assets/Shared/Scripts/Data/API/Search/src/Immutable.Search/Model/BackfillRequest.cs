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
    ///     Backfill request
    /// </summary>
    [DataContract(Name = "BackfillRequest")]
    public class BackfillRequest
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BackfillRequest" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected BackfillRequest()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BackfillRequest" /> class.
        /// </summary>
        /// <param name="entity">The entity to be backfilled (required).</param>
        /// <param name="apiUrl">The indexer url for the given chain and entity (required).</param>
        public BackfillRequest(string entity = default, string apiUrl = default)
        {
            // to ensure "entity" is required (not null)
            if (entity == null)
                throw new ArgumentNullException("entity is a required property for BackfillRequest and cannot be null");
            Entity = entity;
            // to ensure "apiUrl" is required (not null)
            if (apiUrl == null)
                throw new ArgumentNullException("apiUrl is a required property for BackfillRequest and cannot be null");
            ApiUrl = apiUrl;
        }

        /// <summary>
        ///     The entity to be backfilled
        /// </summary>
        /// <value>The entity to be backfilled</value>
        [DataMember(Name = "entity", IsRequired = true, EmitDefaultValue = true)]
        public string Entity { get; set; }

        /// <summary>
        ///     The indexer url for the given chain and entity
        /// </summary>
        /// <value>The indexer url for the given chain and entity</value>
        /// <example>https://indexer-mr.dev.imtbl.com</example>
        [DataMember(Name = "api-url", IsRequired = true, EmitDefaultValue = true)]
        public string ApiUrl { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BackfillRequest {\n");
            sb.Append("  Entity: ").Append(Entity).Append("\n");
            sb.Append("  ApiUrl: ").Append(ApiUrl).Append("\n");
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