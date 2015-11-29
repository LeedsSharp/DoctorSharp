using System.Linq;
using DrSharp.Domain.Models;
using DrSharp.Web.ViewModels;
using Nancy;
using Raven.Client;

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
                // RavenDB
                var questions = documentSession.Query<Question>().OrderByDescending(x => x.DateAsked).ToList();

                var viewModel = new IndexViewModel
                {
                    Questions = questions.Select(x => new QuestionViewModel(x)).ToList(),
                    Count = documentSession.Query<Question>().Count(),
                };
                
                return View["Index", viewModel];
            };
        }
    }
}