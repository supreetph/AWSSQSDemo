using Amazon.S3;
using AWSSQSPOC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQSAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new AmazonS3Client(AwsSqsConstants.AccessKey, AwsSqsConstants.SecretKey, AwsSqsConstants.RegionEndpoint);
            S3Service service = new S3Service(client);
            // service.UploadFile("scraper.swan.com");
            service.GetFiles("scraper.swan.com", "Functionality.docx");


        }
    }
}
