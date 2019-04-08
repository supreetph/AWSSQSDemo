using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQSConsumer
{
    public class Message
    {
        public string MessageBody { get; set; }
        public string MessageHandle { get; set; }
    }
}
