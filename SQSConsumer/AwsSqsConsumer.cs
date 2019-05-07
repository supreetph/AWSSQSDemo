using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQSConsumer
{
    public class AwsSqsConsumer
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();



        public static void DeleteMessage(AmazonSQSClient sqsClient, string queueUrl, string recieptHandle)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = recieptHandle
            };
            var deleteMessageResponse = sqsClient.DeleteMessageAsync(deleteMessageRequest).Result;
        }

        public static Message RecieveMessages(AmazonSQSClient sqsClient, string queueName)
        {
            var msg = new Message();

            string queueUrl = sqsClient.GetQueueUrlAsync(queueName).Result.QueueUrl;
            var readMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20,

            };
            var readMessageResponse = sqsClient.ReceiveMessage(readMessageRequest).Messages;
            foreach (var item in readMessageResponse)
            {
                msg.Body = item.Body;
                msg.ReceiptHandle = item.ReceiptHandle;
            }
            return msg;

        }

        public void Subscribe(AmazonSQSClient sqsClient, string queueUrl)
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                var allmsgs = GetAllMessages(sqsClient, queueUrl).Result;

                if (allmsgs.Count > 0)
                {
                    foreach (var msg in allmsgs)
                    {
                        DeleteMessage(sqsClient, queueUrl, msg.ReceiptHandle);
                    }
                }


            }
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                Subscribe(sqsClient, queueUrl);
            }


        }
        private static async Task<IList<Message>> GetAllMessages(AmazonSQSClient sqsClient, string queueUrl)
        {
            IList<Message> messagesAll = new List<Message>();
            try
            {
                List<string> AttributesList = new List<string>();
                AttributesList.Add("*");
                var recieveRequest = new ReceiveMessageRequest
                {
                    MessageAttributeNames = AttributesList,
                    QueueUrl = queueUrl
                };

                ReceiveMessageResponse message = await sqsClient.ReceiveMessageAsync(recieveRequest);
                foreach (Message item in message.Messages)
                {

                    messagesAll.Add(item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return messagesAll;
        }

        public static void DeleteAllQueues(AmazonSQSClient sqsClient)
        {
            var deletQueueResponse = sqsClient.ListQueues(new ListQueuesRequest());
            foreach (var queueUrl in deletQueueResponse.QueueUrls)
            {
                sqsClient.DeleteQueue(queueUrl);
            }
        }

        public static void PurgeQueueByQueueUrl(AmazonSQSClient sqsClient, string queueUrl)
        {

            var purgeRequest = new PurgeQueueRequest()
            {
                QueueUrl = queueUrl
            };

            sqsClient.PurgeQueue(purgeRequest);

        }
        //public static bool QueueHasItemsByQueueUrl(AmazonSQSClient sqsClient, string queueUrl)
        //{
        //    return QueueLengthByQueueUrl(sqsClient, queueUrl) > 0;
        //}
        //public static int QueueLength(AmazonSQSClient sqsClient, string queueName)
        //{
        //    return QueueLengthByQueueUrl(sqsClient, FindQueueUrlByQueueName(sqsClient, queueName));
        //}
        public static string FindQueueUrlByQueueName(AmazonSQSClient sqsClient, string name)
        {

            ListQueuesResponse response = sqsClient.ListQueues(new ListQueuesRequest(name));

            foreach (string queueUrl in response.QueueUrls)
            {
                return queueUrl;
            }


            return null;
        }
        //public static int QueueLengthByQueueUrl(AmazonSQSClient sqsClient, string queueUrl)
        //{

        //    var request = new GetQueueAttributesRequest
        //    {
        //        QueueUrl = queueUrl,
        //        AttributeNames = new List<string> { "ApproximateNumberOfMessages" }
        //    };

        //    return sqsClient.GetQueueAttributes(request).ApproximateNumberOfMessages;

        //}
    }
}

