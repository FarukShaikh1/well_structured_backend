using System;

namespace FMS_Collection.Core.Response
{
    public class ChatResponse
    {
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Message { get; set; }
        public Guid MessageTypeId { get; set; }
        public bool IsMessageRead { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
