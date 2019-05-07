using Amazon.Runtime.Internal.Util;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSSQSPOC
{
    public class AwsSqsCreator
    {
        private static AmazonSQSClient Client
        {
            get
            {
                if (AwsSqsConstants.Client == null)
                    AwsSqsConstants.Client = new AmazonSQSClient(AwsSqsConstants.AccessKey, AwsSqsConstants.SecretKey, AwsSqsConstants.RegionEndpoint);
                return AwsSqsConstants.Client;
            }
        }
        public static string Url { get; set; }
        //public string MessageBody { get; set; }

        //public Logger Log
        //{
        //    get
        //    {
        //        return log;
        //    }

        //    set
        //    {
        //        log = value;
        //    }
        //}

        public void SendMessage(string url, Message sqsMessage, Dictionary<string, MessageAttributeModel> attributes)
        {
            try
            {
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = url,
                    MessageBody = JsonConvert.SerializeObject(sqsMessage.Body),
                    MessageAttributes = attributes.ToDictionary(item => item.Key, item => (MessageAttributeValue)item.Value),


                };
                var sqsSendMessage = Client.SendMessageAsync(sendMessageRequest).Result;
            }

            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);


            }
        }

        /// <summary>
        /// Method to create Queue
        /// </summary>
        public static string CreateQueue(string queueName)
        {
            try
            {
                var createQueueRequest = new CreateQueueRequest
                {
                    QueueName = queueName,
                };
                var createQueueResponse = Client.CreateQueueAsync(createQueueRequest).Result;
                Url = createQueueResponse.QueueUrl;
                return Url;
                //GetQueues();
            }
            catch (AmazonSQSException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void GetQueues()
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
