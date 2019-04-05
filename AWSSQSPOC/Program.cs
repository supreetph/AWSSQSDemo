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

        public static IAmazonSQS sqs = new AmazonSQSClient(RegionEndpoint.APSouth1);
        public static string Url { get; set; }

        public static void Main(string[] args)
        {
            CreateQueue();
            SendMessage(sqs);

        }

        private static void SendMessage(IAmazonSQS sqs)
        {
            Console.WriteLine("Send Message");
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = Url,
                MessageBody = "Sample message to be sent"

            };
            var sqsSendMessage = sqs.SendMessageAsync(sendMessageRequest);
            Console.WriteLine("Message sent");
        }

        /// <summary>
        /// Method to create Queue
        /// </summary>
        private static void CreateQueue()
        {
            Console.Write("Create a queue.\n");
            var sqsRequest = new CreateQueueRequest
            {
                QueueName = "Queue1"

            };

            // ProfileManager.RegisterProfile({ profileName}, { accessKey}, { secretKey});
            var sqsResponse = sqs.CreateQueueAsync(sqsRequest).Result;
            Url = sqsResponse.QueueUrl;
            var listQueueRequest = new ListQueuesRequest();
            var listqueueResponse = sqs.ListQueuesAsync(listQueueRequest);
            foreach (var item in listqueueResponse.Result.QueueUrls)
            {
                Console.Write(item);
            }
            Console.ReadLine();
        }
    }
}
