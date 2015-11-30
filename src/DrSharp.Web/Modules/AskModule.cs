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
        {
            Get["/ask"] = _ =>
            {
                try
                {
                    // RavenDb
                    var storedQuestions = documentSession.Query<Question>().OrderByDescending(x => x.DateAsked).ToList();
                    
                    // Twitter
                    var twitterService = new TwitterService(TwitterConsumerKey, TwitterConsumerSecret);
                    twitterService.AuthenticateWith(TwitterAccessToken, TwitterAccessTokenSecret);

                    var twitterOptions = new ListTweetsMentioningMeOptions();
                    if (storedQuestions.Any())
                    {
                        var sinceId = storedQuestions.First().MessageId;
                        twitterOptions.SinceId = sinceId;
                    }
                    
                    var tweets = twitterService.ListTweetsMentioningMe(twitterOptions).ToList();
                    if (!tweets.Any()) return null;

                    var nextQuestion = tweets.First(t => !string.IsNullOrEmpty(t.Text) && t.Text.Contains("#drsharp"));

                    //var model = this.Bind<AskViewModel>();

                    var pathToAiml = System.Web.HttpContext.Current.Server.MapPath(@"~/aiml");
                    var drSharp = new DoctorSharp(pathToAiml);
                    var answer = drSharp.Ask(nextQuestion.Author.ScreenName, nextQuestion.Text);

                    // Note: tweet working, but not in reply to sender. Also need to add some hashtag to the answer.
                    //twitterService.SendTweet(new SendTweetOptions
                    //{
                    //    DisplayCoordinates = false,
                    //    InReplyToStatusId = nextQuestion.Id,
                    //    Status = answer
                    //});

                    var question = new Question
                    {
                        From = nextQuestion.Author.ScreenName,
                        DateAsked = nextQuestion.CreatedDate,
                        Content = nextQuestion.Text,
                        MessageId = nextQuestion.Id,
                        Answer = answer
                    };

                    documentSession.Store(question);
                    documentSession.SaveChanges();

                    // SignalR
                    hubContext.Clients.All.broadcastAnswer(question.Content, question.Answer, question.From);
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