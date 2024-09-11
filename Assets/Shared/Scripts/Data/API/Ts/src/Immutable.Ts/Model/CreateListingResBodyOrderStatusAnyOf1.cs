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
    /// CreateListingResBodyOrderStatusAnyOf1
    /// </summary>
    [DataContract(Name = "createListingResBodyOrderStatus_anyOf_1")]
    public partial class CreateListingResBodyOrderStatusAnyOf1
    {

        /// <summary>
        /// Gets or Sets CancellationType
        /// </summary>
        [DataMember(Name = "cancellation_type", EmitDefaultValue = false)]
        public CreateListingResBodyCancellationType? CancellationType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateListingResBodyOrderStatusAnyOf1" /> class.
        /// </summary>
        /// <param name="cancellationType">cancellationType.</param>
        /// <param name="name">The order status indicating a order is has been cancelled or about to be cancelled..</param>
        /// <param name="pending">Whether the cancellation of the order is pending.</param>
        public CreateListingResBodyOrderStatusAnyOf1(CreateListingResBodyCancellationType? cancellationType = default(CreateListingResBodyCancellationType?), string name = default(string), bool pending = default(bool))
        {
            this.CancellationType = cancellationType;
            this.Name = name;
            this.Pending = pending;
        }

        /// <summary>
        /// The order status indicating a order is has been cancelled or about to be cancelled.
        /// </summary>
        /// <value>The order status indicating a order is has been cancelled or about to be cancelled.</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Whether the cancellation of the order is pending
        /// </summary>
        /// <value>Whether the cancellation of the order is pending</value>
        [DataMember(Name = "pending", EmitDefaultValue = true)]
        public bool Pending { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateListingResBodyOrderStatusAnyOf1 {\n");
            sb.Append("  CancellationType: ").Append(CancellationType).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Pending: ").Append(Pending).Append("\n");
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
