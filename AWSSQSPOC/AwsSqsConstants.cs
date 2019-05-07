using Amazon;
using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSSQSPOC
{
    public static class AwsSqsConstants
    {
        public static string AccessKey = "";
        public static string SecretKey = "";
        public static AmazonSQSClient Client;
        public static RegionEndpoint RegionEndpoint = RegionEndpoint.APSouth1;
        public static string FilePath = @"D:\Swan docs\Functionality.docx";
    }
}
