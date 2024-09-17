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
using Newtonsoft.Json.Converters;

namespace Immutable.Search.Model
{
    /// <summary>
    ///     APIError404AllOf
    /// </summary>
    [DataContract(Name = "APIError404_allOf")]
    public class APIError404AllOf
    {
        /// <summary>
        ///     Error Code
        /// </summary>
        /// <value>Error Code</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CodeEnum
        {
            /// <summary>
            ///     Enum RESOURCENOTFOUND for value: RESOURCE_NOT_FOUND
            /// </summary>
            [EnumMember(Value = "RESOURCE_NOT_FOUND")]
            RESOURCENOTFOUND = 1
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="APIError404AllOf" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected APIError404AllOf()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="APIError404AllOf" /> class.
        /// </summary>
        /// <param name="code">Error Code (required).</param>
        /// <param name="details">Additional details to help resolve the error (required).</param>
        public APIError404AllOf(CodeEnum code = default, object details = default)
        {
            Code = code;
            // to ensure "details" is required (not null)
            if (details == null)
                throw new ArgumentNullException(
                    "details is a required property for APIError404AllOf and cannot be null");
            Details = details;
        }


        /// <summary>
        ///     Error Code
        /// </summary>
        /// <value>Error Code</value>
        /// <example>RESOURCE_NOT_FOUND</example>
        [DataMember(Name = "code", IsRequired = true, EmitDefaultValue = true)]
        public CodeEnum Code { get; set; }

        /// <summary>
        ///     Additional details to help resolve the error
        /// </summary>
        /// <value>Additional details to help resolve the error</value>
        [DataMember(Name = "details", IsRequired = true, EmitDefaultValue = true)]
        public object Details { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class APIError404AllOf {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Details: ").Append(Details).Append("\n");
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