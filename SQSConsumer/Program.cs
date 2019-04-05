using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SQSConsumer
{
    class Program
    {

        static void Main(string[] args)
        {
            var sqsClient = new AmazonSQSClient(RegionEndpoint.APSouth1);
            var url = sqsClient.GetQueueUrlAsync("Queue1").Result.QueueUrl;
            RecieveMessages(sqsClient, "Queue1");
            DeleteMessage(sqsClient, url);

        }

        private static void DeleteMessage(AmazonSQSClient sqsClient, string queueUrl)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = queueUrl
            };
            var deleteMessage = sqsClient.DeleteMessageAsync(deleteMessageRequest);
            Console.ReadLine();
        }

        private static string RecieveMessages(AmazonSQSClient sqsClient, string queueName)
        {
            string queueUrl = sqsClient.GetQueueUrlAsync(queueName).Result.QueueUrl;
            var readMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl
            };
            var readMessageResponse = sqsClient.ReceiveMessageAsync(readMessageRequest).Result.Messages;
            foreach (var item in readMessageResponse)
            {
                Console.WriteLine(item.Body);
            }

            return queueUrl;
        }
    }
}
