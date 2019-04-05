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
            var queueUrl = sqsClient.GetQueueUrlAsync("Queue1").Result.QueueUrl;
            var readMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl
            };
            var readMessageResponse = sqsClient.ReceiveMessageAsync(readMessageRequest).Result.Messages;
            foreach (var item in readMessageResponse)
            {
                Console.WriteLine(item.Body);
            }
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = queueUrl
            };
            var deleteMessage = sqsClient.DeleteMessageAsync(deleteMessageRequest);
            Console.ReadLine();

        }
    }
}
