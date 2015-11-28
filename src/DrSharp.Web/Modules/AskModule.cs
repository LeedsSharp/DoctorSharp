using System;
using System.Collections.Generic;
using System.Linq;
using DrSharp.Domain.Logic;
using DrSharp.Domain.Models;
using DrSharp.Web.ViewModels;
using Microsoft.AspNet.SignalR;
using Nancy;
using Nancy.ModelBinding;
using Raven.Client;
using TweetSharp;

namespace DrSharp.Web.Modules
{
    public class AskModule : NancyModule
    {
        private const string TwitterConsumerKey = "cCUfLlJdwNanXcdTFPaw";
        private const string TwitterConsumerSecret = "LeLyBeYm655HdtdAJnBr74NDZ7DXWKgxmVQomhN1Y";
        private const string TwitterAccessToken = "576199065-QkMWPvETEvstaTkC2ksQ8Y3tvrJip0XDjTVgEZu7";
        private const string TwitterAccessTokenSecret = "ydDC3XFV98i8GHihwbMkhD5VNGrbtRE0B0iQM6Mtj0nAj";

        private const string MeetupApiKey = "3fd2f5cd29c3e17615754642263b";
        private const string Christmas2015EventId = "225585634";

        public AskModule(IDocumentSession documentSession, IHubContext hubContext)
            : base("Ask")
        {
            Post["/"] = _ =>
            {
                try
                {
                    //// Twitter
                    var twitterService = new TwitterService(TwitterConsumerKey, TwitterConsumerSecret);
                    twitterService.AuthenticateWith(TwitterAccessToken, TwitterAccessTokenSecret);
                    ////var tweets = twitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions());
                    //var tweets = twitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions { SinceId = 5 });
                    ////var tweets2 = twitterService.Search(new SearchOptions{Q = "#drsharp"});

                    //var questions = new List<QuestionViewModel>();
                    //foreach (var tweet in tweets.Where(t => !string.IsNullOrEmpty(t.Text) && t.Text.Contains("#drsharp")))
                    //{
                    //    var question = new QuestionViewModel(tweet) { Channel = MessageChannel.Twitter };

                    //    // Note: tweet working, but not in reply to sender. Also need to add some hashtag to the answer.
                    //    //twitterService.SendTweet(new SendTweetOptions
                    //    //{
                    //    //    DisplayCoordinates = true,
                    //    //    InReplyToStatusId = tweet.Id,
                    //    //    Status = question.Answer
                    //    //});
                    //    questions.Add(question);
                    //}

                    var model = this.Bind<AskViewModel>();

                    var pathToAiml = System.Web.HttpContext.Current.Server.MapPath(@"~/aiml");

                    var drSharp = new DoctorSharp(pathToAiml);

                    var answer = drSharp.Ask(model.From, model.Content);

                    // Note: tweet working, but not in reply to sender. Also need to add some hashtag to the answer.
                    twitterService.SendTweet(new SendTweetOptions
                    {
                        DisplayCoordinates = false,
                        InReplyToStatusId = long.Parse(model.Msg_Id),
                        Status = answer
                    });

                    var question = new Question
                    {
                        ToPhoneNumber = model.To,
                        FromPhoneNumber = model.From,
                        DateAsked = DateTime.Now,
                        Content = model.Content,
                        MessageId = model.Msg_Id,
                        Keyword = model.Keyword,
                        Answer = answer
                    };

                    documentSession.Store(question);
                    documentSession.SaveChanges();

                    // SignalR
                    hubContext.Clients.All.broadcastAnswer(model.Content, answer, model.From);
                    return null;
                }
                catch (Exception ex)
                {
                    return string.Format("Message: {0}\r\nDetail {1}", ex.Message, ex.StackTrace);
                }
                
            };
        }
    }
}