using AIMLbot;

namespace DrSharp.Domain.Logic
{
    public class DoctorSharp
    {
        private readonly string aimlPath;
        //private const string SettingsPath = @"D:\Code\Leeds Sharp\DoctorSharp\src\DrSharp.Web\config\";
        private const string SettingsPath = @"E:\Code\GitHub\DoctorSharp\src\DrSharp.Web\config\";

        public DoctorSharp(string aimlPath)
        {
            this.aimlPath = aimlPath;
        }

        public string Ask(string name, string question)
        {
            var sharpBot = new Bot();
            sharpBot.loadSettings(aimlPath);
            //sharpBot.loadSettings(@"D:\Code\config\settings.xml");
            var loader = new AIMLbot.Utils.AIMLLoader(sharpBot);
            loader.loadAIML(@"D:\Code\Leeds Sharp\DoctorSharp\src\DrSharp.Tests\aiml");
            sharpBot.isAcceptingUserInput = false;
            sharpBot.isAcceptingUserInput = true;

            var patient = new User(name, sharpBot);
            var request = new Request(question, patient, sharpBot);
            var answer = sharpBot.Chat(request);
            return answer.Output;
        }
    }
}
