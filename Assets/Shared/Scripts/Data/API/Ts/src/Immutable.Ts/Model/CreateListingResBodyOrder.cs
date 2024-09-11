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
    /// CreateListingResBodyOrder
    /// </summary>
    [DataContract(Name = "createListingResBodyOrder")]
    public partial class CreateListingResBodyOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateListingResBodyOrder" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected CreateListingResBodyOrder() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateListingResBodyOrder" /> class.
        /// </summary>
        /// <param name="accountAddress">accountAddress.</param>
        /// <param name="buy">buy.</param>
        /// <param name="chain">chain.</param>
        /// <param name="createdAt">createdAt.</param>
        /// <param name="endAt">Time after which the Order is expired.</param>
        /// <param name="fees">fees.</param>
        /// <param name="fillStatus">fillStatus.</param>
        /// <param name="id">id (required).</param>
        /// <param name="orderHash">orderHash (required).</param>
        /// <param name="protocolData">protocolData.</param>
        /// <param name="salt">salt.</param>
        /// <param name="sell">sell (required).</param>
        /// <param name="signature">signature (required).</param>
        /// <param name="startAt">Time after which the Order is considered active (required).</param>
        /// <param name="status">status (required).</param>
        /// <param name="type">type (required).</param>
        /// <param name="updatedAt">updatedAt (required).</param>
        public CreateListingResBodyOrder(string accountAddress = default(string), List<CreateListingResBodyOrderBuyInner> buy = default(List<CreateListingResBodyOrderBuyInner>), CreateListingResBodyOrderChain chain = default(CreateListingResBodyOrderChain), string createdAt = default(string), string endAt = default(string), List<CreateListingResBodyFee> fees = default(List<CreateListingResBodyFee>), CreateListingResBodyOrderFillStatus fillStatus = default(CreateListingResBodyOrderFillStatus), string id = default(string), string orderHash = default(string), CreateListingResBodyOrderProtocolData protocolData = default(CreateListingResBodyOrderProtocolData), string salt = default(string), List<CreateListingResBodyOrderSellInner> sell = default(List<CreateListingResBodyOrderSellInner>), string signature = default(string), string startAt = default(string), CreateListingResBodyOrderStatus status = default(CreateListingResBodyOrderStatus), string type = default(string), string updatedAt = default(string))
        {
            // to ensure "id" is required (not null)
            if (id == null)
            {
                throw new ArgumentNullException("id is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.Id = id;
            // to ensure "orderHash" is required (not null)
            if (orderHash == null)
            {
                throw new ArgumentNullException("orderHash is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.OrderHash = orderHash;
            // to ensure "sell" is required (not null)
            if (sell == null)
            {
                throw new ArgumentNullException("sell is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.Sell = sell;
            // to ensure "signature" is required (not null)
            if (signature == null)
            {
                throw new ArgumentNullException("signature is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.Signature = signature;
            // to ensure "startAt" is required (not null)
            if (startAt == null)
            {
                throw new ArgumentNullException("startAt is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.StartAt = startAt;
            // to ensure "status" is required (not null)
            if (status == null)
            {
                throw new ArgumentNullException("status is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.Status = status;
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new ArgumentNullException("type is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.Type = type;
            // to ensure "updatedAt" is required (not null)
            if (updatedAt == null)
            {
                throw new ArgumentNullException("updatedAt is a required property for CreateListingResBodyOrder and cannot be null");
            }
            this.UpdatedAt = updatedAt;
            this.AccountAddress = accountAddress;
            this.Buy = buy;
            this.Chain = chain;
            this.CreatedAt = createdAt;
            this.EndAt = endAt;
            this.Fees = fees;
            this.FillStatus = fillStatus;
            this.ProtocolData = protocolData;
            this.Salt = salt;
        }

        /// <summary>
        /// Gets or Sets AccountAddress
        /// </summary>
        [DataMember(Name = "accountAddress", EmitDefaultValue = false)]
        public string AccountAddress { get; set; }

        /// <summary>
        /// Gets or Sets Buy
        /// </summary>
        [DataMember(Name = "buy", EmitDefaultValue = false)]
        public List<CreateListingResBodyOrderBuyInner> Buy { get; set; }

        /// <summary>
        /// Gets or Sets Chain
        /// </summary>
        [DataMember(Name = "chain", EmitDefaultValue = false)]
        public CreateListingResBodyOrderChain Chain { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        [DataMember(Name = "createdAt", EmitDefaultValue = false)]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Time after which the Order is expired
        /// </summary>
        /// <value>Time after which the Order is expired</value>
        [DataMember(Name = "endAt", EmitDefaultValue = false)]
        public string EndAt { get; set; }

        /// <summary>
        /// Gets or Sets Fees
        /// </summary>
        [DataMember(Name = "fees", EmitDefaultValue = false)]
        public List<CreateListingResBodyFee> Fees { get; set; }

        /// <summary>
        /// Gets or Sets FillStatus
        /// </summary>
        [DataMember(Name = "fillStatus", EmitDefaultValue = false)]
        public CreateListingResBodyOrderFillStatus FillStatus { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets OrderHash
        /// </summary>
        [DataMember(Name = "orderHash", IsRequired = true, EmitDefaultValue = true)]
        public string OrderHash { get; set; }

        /// <summary>
        /// Gets or Sets ProtocolData
        /// </summary>
        [DataMember(Name = "protocolData", EmitDefaultValue = false)]
        public CreateListingResBodyOrderProtocolData ProtocolData { get; set; }

        /// <summary>
        /// Gets or Sets Salt
        /// </summary>
        [DataMember(Name = "salt", EmitDefaultValue = false)]
        public string Salt { get; set; }

        /// <summary>
        /// Gets or Sets Sell
        /// </summary>
        [DataMember(Name = "sell", IsRequired = true, EmitDefaultValue = true)]
        public List<CreateListingResBodyOrderSellInner> Sell { get; set; }

        /// <summary>
        /// Gets or Sets Signature
        /// </summary>
        [DataMember(Name = "signature", IsRequired = true, EmitDefaultValue = true)]
        public string Signature { get; set; }

        /// <summary>
        /// Time after which the Order is considered active
        /// </summary>
        /// <value>Time after which the Order is considered active</value>
        [DataMember(Name = "startAt", IsRequired = true, EmitDefaultValue = true)]
        public string StartAt { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "status", IsRequired = true, EmitDefaultValue = true)]
        public CreateListingResBodyOrderStatus Status { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets UpdatedAt
        /// </summary>
        [DataMember(Name = "updatedAt", IsRequired = true, EmitDefaultValue = true)]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class CreateListingResBodyOrder {\n");
            sb.Append("  AccountAddress: ").Append(AccountAddress).Append("\n");
            sb.Append("  Buy: ").Append(Buy).Append("\n");
            sb.Append("  Chain: ").Append(Chain).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  EndAt: ").Append(EndAt).Append("\n");
            sb.Append("  Fees: ").Append(Fees).Append("\n");
            sb.Append("  FillStatus: ").Append(FillStatus).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  OrderHash: ").Append(OrderHash).Append("\n");
            sb.Append("  ProtocolData: ").Append(ProtocolData).Append("\n");
            sb.Append("  Salt: ").Append(Salt).Append("\n");
            sb.Append("  Sell: ").Append(Sell).Append("\n");
            sb.Append("  Signature: ").Append(Signature).Append("\n");
            sb.Append("  StartAt: ").Append(StartAt).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
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
