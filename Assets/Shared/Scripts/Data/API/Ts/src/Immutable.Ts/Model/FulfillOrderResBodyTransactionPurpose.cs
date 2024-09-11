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
using OpenAPIDateConverter = Immutable.Ts.Client.OpenAPIDateConverter;

namespace Immutable.Ts.Model
{
    /// <summary>
    /// Defines fulfillOrderResBodyTransactionPurpose
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FulfillOrderResBodyTransactionPurpose
    {
        /// <summary>
        /// Enum APPROVAL for value: APPROVAL
        /// </summary>
        [EnumMember(Value = "APPROVAL")]
        APPROVAL = 1,

        /// <summary>
        /// Enum FULFILLORDER for value: FULFILL_ORDER
        /// </summary>
        [EnumMember(Value = "FULFILL_ORDER")]
        FULFILLORDER = 2,

        /// <summary>
        /// Enum CANCEL for value: CANCEL
        /// </summary>
        [EnumMember(Value = "CANCEL")]
        CANCEL = 3
    }

}
