using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Collections.Generic;

namespace SQSConsumer
{
    class Program
    {

        static void Main(string[] args)
        {
            var msg = new Message();
            var sqsClient = new AmazonSQSClient("AKIA2YKAGIBYZNTZMRWU", "cVN+JEls3PoOwgN7KV6IbM+14iQFPcDUhK2iBaWc", RegionEndpoint.APSouth1);
            var url = sqsClient.GetQueueUrlAsync("Queue2").Result.QueueUrl;
            string messageHandle = string.Empty;

            msg = RecieveMessages(sqsClient, "Queue2");

            var listOfMessages = GetAllMessages(sqsClient, "Queue2");

            DeleteMessage(sqsClient, url);

        }

        private static void DeleteMessage(AmazonSQSClient sqsClient, string queueUrl)
        {
            var message = RecieveMessages(sqsClient, "Queue2");
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = message.MessageHandle
            };
            var deleteMessage = sqsClient.DeleteMessageAsync(deleteMessageRequest).Result;
            Console.ReadLine();
        }

        private static Message RecieveMessages(AmazonSQSClient sqsClient, string queueName)
        {
            var msg = new Message();

            string queueUrl = sqsClient.GetQueueUrlAsync(queueName).Result.QueueUrl;
            var readMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl
            };
            var readMessageResponse = sqsClient.ReceiveMessageAsync(readMessageRequest).Result.Messages;
            foreach (var item in readMessageResponse)
            {
                msg.MessageBody = item.Body;
                msg.MessageHandle = item.ReceiptHandle;
            }
            return msg;

        }
        private static List<string> GetAllMessages(AmazonSQSClient sqsClient, string queueName)
        {
            List<string> messagesAll = new List<string>();
            var message = RecieveMessages(sqsClient, queueName);
            foreach (var item in message.MessageBody)
            {
                messagesAll.Add(item.ToString());
            }
            return messagesAll;
        }
    }
}
