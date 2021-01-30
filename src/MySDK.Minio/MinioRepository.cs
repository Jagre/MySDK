using Microsoft.Extensions.Configuration;
using Minio;
using Minio.Exceptions;
using MySDK.Minio.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MySDK.Minio
{
    public class MinioRepository
    {
        private readonly MinioClient _client;
        private readonly MinioConfiguration _config;

        public MinioRepository(IConfigurationRoot config)
        {
            _config = config.ParseConfiguration();
            _client = new MinioClient(_config.Endpoint, _config.AccessKey, _config.SecretKey);
            if (!string.IsNullOrEmpty(_config.Scheme) && _config.Scheme.ToLower() == "https")
            {
                _client.WithSSL();
            }
        }

        public async Task<string> UploadAsync(Stream data, string bucketName, string fileName)
        {
            if (string.IsNullOrEmpty(bucketName))
                throw new MinioException("haven't specified bucket name");
            if (string.IsNullOrEmpty(fileName))
                fileName = Guid.NewGuid().ToString();

            if (!await _client.BucketExistsAsync(bucketName))
            {
                await _client.MakeBucketAsync(bucketName);
                await _client.SetPolicyAsync(bucketName, _config.GetPolicyJsonString(bucketName));
            }

            using (data)
            {
                await _client.PutObjectAsync(bucketName, fileName, data, data.Length);
                return $"{bucketName}/{fileName}";
            }
        }

        public async Task<bool> RemoveAsync(string urlSuffix)
        {
            var bucketName = urlSuffix.GetBucketName();
            var fileName = urlSuffix.GEtFileName();
            await _client.RemoveObjectAsync(bucketName, fileName);
            return true;
        }

    }
}
