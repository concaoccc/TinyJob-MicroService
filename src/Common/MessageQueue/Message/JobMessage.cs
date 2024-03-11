using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MessageQueue.Message
{
    public record JobMessage 
    {
        public long JobId { get; set; }
    }
}
