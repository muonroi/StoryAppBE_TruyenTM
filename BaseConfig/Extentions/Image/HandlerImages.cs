using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using BaseConfig.Setting.AppSettings.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MuonRoiSocialNetwork.Common.Settings.Appsettings;
namespace BaseConfig.Extentions.Image
{
    public class HandlerImages
    {
        public async static Task<Dictionary<string, string>> UploadImageToAwsAsync(IConfiguration configuration, IFormFile imgData)
        {
            if (imgData == null || imgData.Length <= 0) return null;
            string accessKey = configuration.GetSection(ConstAppSettings.Instance.ENV_ACCESSKEY).Value;
            string secretKey = configuration.GetSection(ConstAppSettings.Instance.ENV_SERECT).Value;
            AmazonS3Client client = new(accessKey, secretKey, RegionEndpoint.APNortheast1);
            using MemoryStream memmoryStream = new();
            await imgData.CopyToAsync(memmoryStream).ConfigureAwait(false);
            string bucketName = FolderSettingBase.BUCKET_USER;
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = FolderSettingBase.IMG_STORY + $"{FolderSettingBase.DefaultNameImages}" + "_" + $"{imgData.FileName}",
                InputStream = memmoryStream
            };
            PutObjectResponse result = await client.PutObjectAsync(request).ConfigureAwait(false);
            Dictionary<string, string> temp = new()
            {
                { request.Key, result.HttpStatusCode.ToString() }
            };
            return temp;
        }
        public static string TakeLinkImage(IConfiguration configuration, string nameImg)
        {
            if (nameImg == null || string.IsNullOrEmpty(nameImg)) return null;
            string accessKey = configuration.GetSection(ConstAppSettings.Instance.ENV_ACCESSKEY).Value;
            string secretKey = configuration.GetSection(ConstAppSettings.Instance.ENV_SERECT).Value;
            string bucket = FolderSettingBase.BUCKET_USER;
            BasicAWSCredentials credentials = new(accessKey, secretKey);
            AmazonS3Config config = new()
            {
                RegionEndpoint = RegionEndpoint.APNortheast1
            };
            AmazonS3Client client = new(credentials, config);
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = nameImg,
                Expires = DateTime.UtcNow.AddDays(30)
            };
            string url = client.GetPreSignedURL(request);
            return url;
        }
    }
}
