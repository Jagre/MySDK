using Microsoft.Extensions.Configuration;
using Minio.Exceptions;
using MySDK.Configuration;
using MySDK.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace MySDK.Minio.Configuration
{
    internal static class MinioConifigurationExtension
    {
        internal static MinioConfiguration ParseConfiguration(this IConfigurationRoot config)
        {
            var sectionName = typeof(MinioConfiguration).Name;
            return config.GetConfiguration<MinioConfiguration>(sectionName);
        }

        internal static string GetBucketName(this string urlSuffix)
        {
            var items = urlSuffix.Split("/");
            if (items.Length > 0)
                return items[0];
            return string.Empty;
        }

        internal static string GEtFileName(this string urlSuffix)
        {
            var items = urlSuffix.Split("/");
            if (items.Length > 1)
                return items[1];
            return string.Empty;
        }


        internal static string GetPolicyJsonString(this MinioConfiguration config, string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new MinioException("haven't specified bucket name");

            MinioBucketPolicyConfiguration policyConfig = null;
            if (config.BucketPolicies != null && config.BucketPolicies.Any())
                policyConfig = config.BucketPolicies.FirstOrDefault(i => i.Name.ToLower() == bucketName.ToLower());
            // setting "read" is default policy
            if (policyConfig == null)
                policyConfig = new MinioBucketPolicyConfiguration { Name = bucketName, Policy = "read" };

            var bucketStatement = new StatementDto { Principal = new PrincipalDto(), Action = new List<string>() };
            var objectStatement = new StatementDto { Principal = new PrincipalDto(), Action = new List<string>() };
            if (policyConfig.Policy.ToLower().Contains("read"))
            {
                bucketStatement.Action.Add("s3:GetBucketLocation");
                bucketStatement.Action.Add("s3:ListBucket");
                objectStatement.Action.Add("s3:GetObject");
            }
            if (policyConfig.Policy.ToLower().Contains("write"))
            {
                bucketStatement.Action.Add("s3:ListBucketMultipartUploads");
                objectStatement.Action.Add("s3:PutObject");
                objectStatement.Action.Add("s3:AbortMultipartUpload");
                objectStatement.Action.Add("s3:DeleteObject");
                objectStatement.Action.Add("s3:ListMultipartUploadParts");
            }
            bucketStatement.Resource = new List<string> { $"arn:aws:s3:::{policyConfig.Name}" };
            objectStatement.Resource = new List<string> { $"arn:aws:s3:::{policyConfig.Name}/*" };

            var policy = new PolicyDto
            {
                Statement = new List<StatementDto> { bucketStatement, objectStatement }
            };

            return policy.ToJson();
        }
    }
}
