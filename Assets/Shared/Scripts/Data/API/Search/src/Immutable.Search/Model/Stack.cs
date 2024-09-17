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
    ///     Stack
    /// </summary>
    [DataContract(Name = "Stack")]
    public class Stack
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Stack" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected Stack()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stack" /> class.
        /// </summary>
        /// <param name="stackId">Stack ID (required).</param>
        /// <param name="chain">chain (required).</param>
        /// <param name="contractAddress">Contract address (required).</param>
        /// <param name="createdAt">When the metadata was created (required).</param>
        /// <param name="updatedAt">When the metadata was last updated (required).</param>
        /// <param name="name">The name of the NFT (required).</param>
        /// <param name="description">The description of the NFT (required).</param>
        /// <param name="image">The image url of the NFT (required).</param>
        /// <param name="externalUrl">The external website link of NFT (required).</param>
        /// <param name="animationUrl">The animation url of the NFT (required).</param>
        /// <param name="youtubeUrl">The youtube URL of NFT (required).</param>
        /// <param name="attributes">List of Metadata attributes (required).</param>
        /// <param name="totalCount">Total count of NFTs in the stack matching the filter params (required).</param>
        public Stack(Guid stackId = default, Chain chain = default, string contractAddress = default,
            DateTime createdAt = default, DateTime updatedAt = default, string name = default,
            string description = default, string image = default, string externalUrl = default,
            string animationUrl = default, string youtubeUrl = default, List<NFTMetadataAttribute> attributes = default,
            int totalCount = default)
        {
            StackId = stackId;
            // to ensure "chain" is required (not null)
            if (chain == null)
                throw new ArgumentNullException("chain is a required property for Stack and cannot be null");
            Chain = chain;
            // to ensure "contractAddress" is required (not null)
            if (contractAddress == null)
                throw new ArgumentNullException("contractAddress is a required property for Stack and cannot be null");
            ContractAddress = contractAddress;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            // to ensure "name" is required (not null)
            if (name == null)
                throw new ArgumentNullException("name is a required property for Stack and cannot be null");
            Name = name;
            // to ensure "description" is required (not null)
            if (description == null)
                throw new ArgumentNullException("description is a required property for Stack and cannot be null");
            Description = description;
            // to ensure "image" is required (not null)
            if (image == null)
                throw new ArgumentNullException("image is a required property for Stack and cannot be null");
            Image = image;
            // to ensure "externalUrl" is required (not null)
            if (externalUrl == null)
                throw new ArgumentNullException("externalUrl is a required property for Stack and cannot be null");
            ExternalUrl = externalUrl;
            // to ensure "animationUrl" is required (not null)
            if (animationUrl == null)
                throw new ArgumentNullException("animationUrl is a required property for Stack and cannot be null");
            AnimationUrl = animationUrl;
            // to ensure "youtubeUrl" is required (not null)
            if (youtubeUrl == null)
                throw new ArgumentNullException("youtubeUrl is a required property for Stack and cannot be null");
            YoutubeUrl = youtubeUrl;
            // to ensure "attributes" is required (not null)
            if (attributes == null)
                throw new ArgumentNullException("attributes is a required property for Stack and cannot be null");
            Attributes = attributes;
            TotalCount = totalCount;
        }

        /// <summary>
        ///     Stack ID
        /// </summary>
        /// <value>Stack ID</value>
        [DataMember(Name = "stack_id", IsRequired = true, EmitDefaultValue = true)]
        public Guid StackId { get; set; }

        /// <summary>
        ///     Gets or Sets Chain
        /// </summary>
        [DataMember(Name = "chain", IsRequired = true, EmitDefaultValue = true)]
        public Chain Chain { get; set; }

        /// <summary>
        ///     Contract address
        /// </summary>
        /// <value>Contract address</value>
        [DataMember(Name = "contract_address", IsRequired = true, EmitDefaultValue = true)]
        public string ContractAddress { get; set; }

        /// <summary>
        ///     When the metadata was created
        /// </summary>
        /// <value>When the metadata was created</value>
        /// <example>2022-08-16T17:43:26.991388Z</example>
        [DataMember(Name = "created_at", IsRequired = true, EmitDefaultValue = true)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     When the metadata was last updated
        /// </summary>
        /// <value>When the metadata was last updated</value>
        /// <example>2022-08-16T17:43:26.991388Z</example>
        [DataMember(Name = "updated_at", IsRequired = true, EmitDefaultValue = true)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        ///     The name of the NFT
        /// </summary>
        /// <value>The name of the NFT</value>
        /// <example>Sword</example>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
        public string Name { get; set; }

        /// <summary>
        ///     The description of the NFT
        /// </summary>
        /// <value>The description of the NFT</value>
        /// <example>2022-08-16T17:43:26.991388Z</example>
        [DataMember(Name = "description", IsRequired = true, EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        ///     The image url of the NFT
        /// </summary>
        /// <value>The image url of the NFT</value>
        /// <example>https://some-url</example>
        [DataMember(Name = "image", IsRequired = true, EmitDefaultValue = true)]
        public string Image { get; set; }

        /// <summary>
        ///     The external website link of NFT
        /// </summary>
        /// <value>The external website link of NFT</value>
        /// <example>https://some-url</example>
        [DataMember(Name = "external_url", IsRequired = true, EmitDefaultValue = true)]
        public string ExternalUrl { get; set; }

        /// <summary>
        ///     The animation url of the NFT
        /// </summary>
        /// <value>The animation url of the NFT</value>
        /// <example>https://some-url</example>
        [DataMember(Name = "animation_url", IsRequired = true, EmitDefaultValue = true)]
        public string AnimationUrl { get; set; }

        /// <summary>
        ///     The youtube URL of NFT
        /// </summary>
        /// <value>The youtube URL of NFT</value>
        /// <example>https://some-url</example>
        [DataMember(Name = "youtube_url", IsRequired = true, EmitDefaultValue = true)]
        public string YoutubeUrl { get; set; }

        /// <summary>
        ///     List of Metadata attributes
        /// </summary>
        /// <value>List of Metadata attributes</value>
        [DataMember(Name = "attributes", IsRequired = true, EmitDefaultValue = true)]
        public List<NFTMetadataAttribute> Attributes { get; set; }

        /// <summary>
        ///     Total count of NFTs in the stack matching the filter params
        /// </summary>
        /// <value>Total count of NFTs in the stack matching the filter params</value>
        /// <example>1</example>
        [DataMember(Name = "total_count", IsRequired = true, EmitDefaultValue = true)]
        public int TotalCount { get; set; }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Stack {\n");
            sb.Append("  StackId: ").Append(StackId).Append("\n");
            sb.Append("  Chain: ").Append(Chain).Append("\n");
            sb.Append("  ContractAddress: ").Append(ContractAddress).Append("\n");
            sb.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
            sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Image: ").Append(Image).Append("\n");
            sb.Append("  ExternalUrl: ").Append(ExternalUrl).Append("\n");
            sb.Append("  AnimationUrl: ").Append(AnimationUrl).Append("\n");
            sb.Append("  YoutubeUrl: ").Append(YoutubeUrl).Append("\n");
            sb.Append("  Attributes: ").Append(Attributes).Append("\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
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