using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AWSSQSPOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQSAPI
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;


        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task CreateBucket(string bucketName)
        {
            if (await AmazonS3Util.DoesS3BucketExistAsync(_client, bucketName) == false)
            {
                var createBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };
                var createBucketResponse = await _client.PutBucketAsync(createBucketRequest);


            }

        }

        public void UploadFile(string bucketName)
        {
            var resultModel = new ResultModel();
            try
            {

                var fileTransferUtility = new TransferUtility(_client);
                fileTransferUtility.UploadAsync(AwsSqsConstants.FilePath, bucketName).Wait();

            }
            catch (Exception)
            {

                throw;
            }
        }
        public GetObjectResponse GetFiles(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };
            var response = _client.GetObjectAsync(request).Result;

            response.WriteResponseStreamToFile(@"D:\Documents\" + fileName);
            return response;
        }

        public string GetFileUrl(string bucketName, string fileName)
        {
            string uploadedFileUrl = _client.GetPreSignedURL(new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.Now.AddMinutes(20)
            });
            return uploadedFileUrl;
        }
    }
}
