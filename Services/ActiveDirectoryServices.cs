using System.Text.Json.Serialization;

namespace ETA_API.Services
{
    public class ActiveDirectoryServices
    {
        [JsonPropertyName("accountEnabled")]
        public bool? AccountEnabled { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("mailNickname")]
        public string MailNickname { get; set; }

        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        public partial class PasswordProfile
        {
            /// <summary>
            /// Gets or sets forceChangePasswordNextSignIn.
            /// true if the user must change her password on the next login; otherwise false. If not set, default is false. NOTE:  For Azure B2C tenants, set to false and instead use custom policies and user flows to force password reset at first sign in. See Force password reset at first logon.
            /// </summary>
            [JsonPropertyName("forceChangePasswordNextSignIn")]
            public bool? ForceChangePasswordNextSignIn { get; set; }

            /// <summary>
            /// Gets or sets password.
            /// The password for the user. This property is required when a user is created. It can be updated, but the user will be required to change the password on the next login. The password must satisfy minimum requirements as specified by the user’s passwordPolicies property. By default, a strong password is required.
            /// </summary>
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }
    }
}