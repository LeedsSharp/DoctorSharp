using System.Collections.Generic;
using System.Linq;
using DrSharp.Web.ViewModels;
using Nancy;
using Raven.Client;
using TweetSharp;
using SearchOptions = TweetSharp.SearchOptions;

namespace DrSharp.Web.Modules
{
    public class QuestionModule : NancyModule
    {
        private const string TwitterConsumerKey = "cCUfLlJdwNanXcdTFPaw";
        private const string TwitterConsumerSecret = "LeLyBeYm655HdtdAJnBr74NDZ7DXWKgxmVQomhN1Y";
        private const string TwitterAccessToken = "576199065-QkMWPvETEvstaTkC2ksQ8Y3tvrJip0XDjTVgEZu7";
        private const string TwitterAccessTokenSecret = "ydDC3XFV98i8GHihwbMkhD5VNGrbtRE0B0iQM6Mtj0nAj";

        private const string MeetupApiKey = "3fd2f5cd29c3e17615754642263b";
        private const string Christmas2015EventId = "225585634";

        // get rspv's from meetup.com
        // https://api.meetup.com/2/rsvps?key=YOUR_KEY&event_id=789&order=name

        public QuestionModule(IDocumentSession documentSession)
        {
            Get["/"] = _ =>
            {
                // Twitter
                var twitterService = new TwitterService(TwitterConsumerKey, TwitterConsumerSecret);
                twitterService.AuthenticateWith(TwitterAccessToken, TwitterAccessTokenSecret);
                //var tweets = twitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions());
                var tweets = twitterService.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions {SinceId = 5});
                //var tweets2 = twitterService.Search(new SearchOptions{Q = "#drsharp"});
                
                var questions = new List<QuestionViewModel>();
                foreach (var tweet in tweets.Where(t => !string.IsNullOrEmpty(t.Text) && t.Text.Contains("#drsharp")))
                {
                    var question = new QuestionViewModel(tweet) { Channel = MessageChannel.Twitter };

                    // Note: tweet working, but not in reply to sender. Also need to add some hashtag to the answer.
                    //twitterService.SendTweet(new SendTweetOptions
                    //{
                    //    DisplayCoordinates = true,
                    //    InReplyToStatusId = tweet.Id,
                    //    Status = question.Answer
                    //});
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