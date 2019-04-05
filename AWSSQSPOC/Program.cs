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
    class Program
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private AmazonSQSClient _client;
        private readonly RegionEndpoint _regionEndpoint;
        public Program(string accessKey, string secretkey, RegionEndpoint regionEndpoint)
        {
            this._accessKey = accessKey;
            this._secretKey = secretkey;
            this._regionEndpoint = regionEndpoint;
        }

        private AmazonSQSClient Client
        {
            get
            {
                if (_client == null)
                    _client = new AmazonSQSClient(this._accessKey, this._secretKey, this._regionEndpoint);
                return _client;
            }
        }
        public static string Url { get; set; }

        public void Main(string[] args)
        {
            CreateQueue();
            SendMessage(Url, "Sample message to be sent");

        }

        public void SendMessage(string url, string message)
        {
            try
            {
                Console.WriteLine("Send Message");
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = url,
                    MessageBody = message

                };
                var sqsSendMessage = Client.SendMessageAsync(sendMessageRequest);
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
        private void CreateQueue()
        {
            try
            {
                Console.Write("Create a queue.\n");
                var sqsRequest = new CreateQueueRequest
                {
                    QueueName = "Queue1"

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

        private void GetQueues()
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
