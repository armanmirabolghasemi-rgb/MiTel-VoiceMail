using System;

namespace Genration.Models
{
    public class VoiceMessage
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CallDate { get; set; }

        public string VoicePath { get; set; }

        public string RefrenceCode { get; set; }

        public string ReplyVoicePath { get; set; }
    }
}