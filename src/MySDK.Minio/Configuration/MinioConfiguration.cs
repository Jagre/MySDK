using System.Collections.Generic;

namespace MySDK.Minio.Configuration
{
    /// <summary>
    /// Minio server configuration
    /// </summary>
    public class MinioConfiguration
    {
        /// <summary>
        /// Url scheme http | https
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Minio server endpoint (no scheme http[s]://)
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string AccessKey{ get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Bucket policy
        /// </summary>
        public List<MinioBucketPolicyConfiguration> BucketPolicies { get; set; }
    }

    /// <summary>
    /// Minio bucket policy setting
    /// </summary>
    public class MinioBucketPolicyConfiguration
    {
        /// <summary>
        /// bucket name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// bucket policy read | write | read & write
        /// </summary>
        public string Policy { get; set; }
    }
}
