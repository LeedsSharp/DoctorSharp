using System.Collections.Generic;

namespace DrSharp.Web.ViewModels
{
    public class IndexViewModel
    {
        public List<QuestionViewModel> Questions { get; set; }
        public int Count { get; set; }
    }
}