using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Collections.Generic;

namespace SQSConsumer
{
    public class Program
    {

        static void Main(string[] args)
        {
            var msg = new Message();
            var sqsClient = new AmazonSQSClient("", "", RegionEndpoint.APSouth1);
            var url = sqsClient.GetQueueUrlAsync("Queue2").Result.QueueUrl;
            //string messageHandle = string.Empty;




            AwsSqsConsumer consumer = new AwsSqsConsumer();
            //  msg = AwsSqsConsumer.RecieveMessages(sqsClient, "Queue2");
            // var listOfMessages = AwsSqsConsumer.GetAllMessages(sqsClient, url).Result;
            //consumer.DeleteMessage(sqsClient, url);
            //  AwsSqsConsumer.DeleteMessage(sqsClient, "https://sqs.ap-south-1.amazonaws.com/739405873265/Queue2", msg.ReceiptHandle);
            consumer.Subscribe(sqsClient, url);
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
                msg.Body = item.Body;
                msg.ReceiptHandle = item.ReceiptHandle;
            }
            return msg;

        }
        //private static List<string> GetAllMessages(AmazonSQSClient sqsClient, string queueName)
        //{
        //    List<string> messagesAll = new List<string>();
        //    var message = RecieveMessages(sqsClient, queueName);
        //    foreach (var item in message.Body)
        //    {
        //        messagesAll.Add(item.ToString());
        //    }
        //    return messagesAll;
        //}
    }
}
