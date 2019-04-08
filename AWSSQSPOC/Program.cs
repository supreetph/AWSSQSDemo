using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using Amazon.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSSQSPOC
{
    public class Program
    {
        private static string _accessKey = "AKIA2YKAGIBYZNTZMRWU";
        private static string _secretKey = "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";
        private static AmazonSQSClient _client;
        private static RegionEndpoint _regionEndpoint = RegionEndpoint.APSouth1;
        public Program()
        {
            //this._accessKey = accessKey;
            //this._secretKey = secretkey;
            //this._regionEndpoint = regionEndpoint;
        }

        private static AmazonSQSClient Client
        {
            get
            {
                if (_client == null)
                    _client = new AmazonSQSClient(_accessKey, _secretKey, _regionEndpoint);
                return _client;
            }


        }
        public static string Url { get; set; }

        public static void Main(string[] args)
        {
            // CreateQueue("Queue2");
            SendMessage("https://sqs.ap-south-1.amazonaws.com/739405873265/Queue2", "Sample message to be sent");

        }

        public static void SendMessage(string url, string message)
        {
            try
            {
                Console.WriteLine("Send Message");
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = url,
                    MessageBody = message

                };
                var sqsSendMessage = Client.SendMessageAsync(sendMessageRequest).Result;
                Console.WriteLine("Message sent");
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Method to create Queue
        /// </summary>
        private static void CreateQueue(string queueName)
        {
            try
            {
                Console.Write("Create a queue.\n");
                var sqsRequest = new CreateQueueRequest
                {
                    QueueName = queueName

                };

                // ProfileManager.RegisterProfile({ profileName}, { accessKey}, { secretKey});
                var sqsResponse = Client.CreateQueueAsync(sqsRequest).Result;
                Url = sqsResponse.QueueUrl;
                GetQueues();
                Console.ReadLine();
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void GetQueues()
        {
            var listQueueRequest = new ListQueuesRequest();
            var listqueueResponse = Client.ListQueuesAsync(listQueueRequest);
            foreach (var item in listqueueResponse.Result.QueueUrls)
            {
                Console.Write(item);
            }
        }

    }
}
