using System;

namespace DrSharp.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string ToPhoneNumber { get; set; }
        public string From { get; set; }
        public DateTime DateAsked { get; set; }
        public string Content { get; set; }
        public long MessageId { get; set; }
        public string Keyword { get; set; }
        public string Answer { get; set; }
    }
}
