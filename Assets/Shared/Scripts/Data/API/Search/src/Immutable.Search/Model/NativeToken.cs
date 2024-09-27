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
    ///     NativeToken
    /// </summary>
    [DataContract(Name = "NativeToken")]
    public class NativeToken
    {
        /// <summary>
        ///     Token type user is offering, which in this case is the native IMX token
        /// </summary>
        /// <value>Token type user is offering, which in this case is the native IMX token</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            /// <summary>
            ///     Enum NATIVE for value: NATIVE
            /// </summary>
            [EnumMember(Value = "NATIVE")] NATIVE = 1
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NativeToken" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected NativeToken()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NativeToken" /> class.
        /// </summary>
        /// <param name="type">Token type user is offering, which in this case is the native IMX token (required).</param>
        /// <param name="symbol">The symbol of token (required).</param>
        public NativeToken(TypeEnum type = default, string symbol = default)
        {
            Type = type;
            // to ensure "symbol" is required (not null)
            if (symbol == null)
                throw new ArgumentNullException("symbol is a required property for NativeToken and cannot be null");
            Symbol = symbol;
        }


        /// <summary>
        ///     Token type user is offering, which in this case is the native IMX token
        /// </summary>
        /// <value>Token type user is offering, which in this case is the native IMX token</value>
        /// <example>NATIVE</example>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        public TypeEnum Type { get; set; }

        /// <summary>
        ///     The symbol of token
        /// </summary>
        /// <value>The symbol of token</value>
        /// <example>IMX</example>
        [DataMember(Name = "symbol", IsRequired = true, EmitDefaultValue = true)]
        public string Symbol { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NativeToken {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Symbol: ").Append(Symbol).Append("\n");
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