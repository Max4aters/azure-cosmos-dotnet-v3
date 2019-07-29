﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos
{
    using System.Net;
    using Microsoft.Azure.Documents;

    /// <summary>
    /// The cosmos user response
    /// </summary>
    public class PermissionResponse : Response<PermissionProperties>
    {
        /// <summary>
        /// Create a <see cref="UserResponse"/> as a no-op for mock testing
        /// </summary>
        protected PermissionResponse()
            : base()
        {
        }

        /// <summary>
        /// A private constructor to ensure the factory is used to create the object.
        /// This will prevent memory leaks when handling the HttpResponseMessage
        /// </summary>
        internal PermissionResponse(
            HttpStatusCode httpStatusCode,
            Headers headers,
            PermissionProperties permissionProperties,
            Permission permission)
        {
            this.StatusCode = httpStatusCode;
            this.Headers = headers;
            this.Resource = permissionProperties;
            this.Permission = permission;
        }

        /// <summary>
        /// The reference to the cosmos user. This allows additional operations on the user
        /// or for easy access permissions
        /// </summary>
        public virtual Permission Permission { get; private set; }

        /// <inheritdoc/>
        public override Headers Headers { get; }

        /// <inheritdoc/>
        public override PermissionProperties Resource { get; }

        /// <inheritdoc/>
        public override HttpStatusCode StatusCode { get; }

        /// <inheritdoc/>
        public override double RequestCharge => this.Headers?.RequestCharge ?? 0;

        /// <inheritdoc/>
        public override string ActivityId => this.Headers?.ActivityId;

        /// <inheritdoc/>
        public override string ETag => this.Headers?.ETag;

        /// <inheritdoc/>
        internal override string MaxResourceQuota => this.Headers?.GetHeaderValue<string>(HttpConstants.HttpHeaders.MaxResourceQuota);

        /// <inheritdoc/>
        internal override string CurrentResourceQuotaUsage => this.Headers?.GetHeaderValue<string>(HttpConstants.HttpHeaders.CurrentResourceQuotaUsage);

        /// <summary>
        /// Get <see cref="Cosmos.User"/> implicitly from <see cref="UserResponse"/>
        /// </summary>
        /// <param name="response">UserResponse</param>
        public static implicit operator Permission(PermissionResponse response)
        {
            return response.Permission;
        }
    }
}