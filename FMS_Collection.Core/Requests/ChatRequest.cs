using System;

namespace FMS_Collection.Core.Request
{
    public class ChatRequest
    {
        public Guid ReceiverId { get; set; }
        public string Message { get; set; }
        public Guid MessageTypeId { get; set; }
    }
}
