using System.Collections.Generic;
using System.Linq;
using DrSharp.Domain.Models;
using DrSharp.Web.ViewModels;
using Nancy;
using Raven.Client;
using Raven.Client.Linq;
using TweetSharp;

namespace DrSharp.Web.Modules
{
    public class QuestionModule : NancyModule
    {
        private const string ConsumerKey = "cCUfLlJdwNanXcdTFPaw";
        private const string ConsumerSecret = "LeLyBeYm655HdtdAJnBr74NDZ7DXWKgxmVQomhN1Y";
        private const string AccessToken = "576199065-QkMWPvETEvstaTkC2ksQ8Y3tvrJip0XDjTVgEZu7";
        private const string AccessTokenSecret = "ydDC3XFV98i8GHihwbMkhD5VNGrbtRE0B0iQM6Mtj0nAj";

        public QuestionModule(IDocumentSession documentSession)
        {
            Get["/"] = _ =>
            {
                // RavenDB
                //var questions = documentSession.Query<Question>().OrderByDescending(x => x.DateAsked).ToList();

                //var viewModel = new IndexViewModel
                //    {
                //        Questions = questions.Select(x => new QuestionViewModel(x)).ToList(),
                //        Count = documentSession.Query<Question>().Count(),
                //    };

                // Twitter
                var twitterService = new TwitterService(ConsumerKey, ConsumerSecret);
                twitterService.AuthenticateWith(AccessToken, AccessTokenSecret);
                var tweets = twitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions());

                var questions = new List<QuestionViewModel>();
                foreach (var tweet in tweets)
                {
                    var question = new QuestionViewModel(tweet) { Channel = MessageChannel.Twitter };
                    questions.Add(question);
                }

                var viewModel = new IndexViewModel
                {
                    Questions = questions,
                    Count = tweets.Count()
                };

                return View["Index", viewModel];
            };
        }
    }
}