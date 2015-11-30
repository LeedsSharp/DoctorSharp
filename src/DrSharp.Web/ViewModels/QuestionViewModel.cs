using System.Web;
using DrSharp.Domain.Logic;
using DrSharp.Domain.Models;
using TweetSharp;

namespace DrSharp.Web.ViewModels
{
    public class QuestionViewModel
    {
        #region Fields
        private string _from; 
        #endregion

        #region Constructors
        public QuestionViewModel(Question question)
        {
            To = question.ToPhoneNumber;
            From = question.From;
            Content = question.Content;
            Msg_Id = question.MessageId;
            DateAsked = question.DateAsked.ToString("g");
            Keyword = question.Keyword;
            Answer = question.Answer;
        }

        public QuestionViewModel(TwitterStatus twitterStatus)
        {
            Channel = MessageChannel.Twitter;
            To = "";
            From = twitterStatus.User != null ? twitterStatus.User.Name : "LS#er";
            Content = twitterStatus.Text;
            Msg_Id = twitterStatus.Id;
            DateAsked = twitterStatus.CreatedDate.ToShortDateString();
            Keyword = "";
            if (!string.IsNullOrEmpty(Content))
            {
                var aimlPath = HttpContext.Current.Server.MapPath("~/aiml/");
                var chatbot = new DoctorSharp(aimlPath);
                var answer = chatbot.Ask(From, Content);
                Answer = answer; 
            }
            else
            {
                Answer = "There was no question.";
            }
        }
        #endregion

        #region Properties
        public string To { get; set; }
        public string From
        {
            get
            {
                return Channel == MessageChannel.Twitter 
                    ? _from 
                    : string.Format("{0}*****{1}", _from.Substring(0, 2), _from.Substring(7, _from.Length - 7));
            }
            set
            {
                _from = value;
            }
        }
        public string Content { get; set; }
        public long Msg_Id { get; set; }
        public string DateAsked { get; set; }
        public string Keyword { get; set; }
        public string Answer { get; set; }
        public MessageChannel Channel { get; set; } 
        #endregion
    }
}