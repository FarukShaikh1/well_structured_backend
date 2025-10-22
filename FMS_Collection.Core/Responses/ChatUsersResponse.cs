using System;

namespace FMS_Collection.Core.Response
{
    public class ChatUsersResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UnreadMessageCount { get; set; }
        public string LastMessage { get; set; }
        public Guid LastMessageTypeId { get; set; }
        public Guid LastMessageSender { get; set; }
        public bool LastMessageStatus { get; set; }
        public DateTime LastMessageTime { get; set; }

    }
}
