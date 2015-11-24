using AIMLbot;

namespace DrSharp.Domain.Logic
{
    public class DoctorClockwork
    {
        private readonly string aimlPath;
        private const string SettingsPath = @"D:\Code\Leeds Sharp\DoctorSharp\DrSharp.Web\config\Settings.xml";

        public DoctorClockwork(string aimlPath)
        {
            this.aimlPath = aimlPath;
        }

        public string AskMeAnything(string name, string question)
        {
            var sharpBot = new Bot();
            sharpBot.loadSettings(SettingsPath);
            var loader = new AIMLbot.Utils.AIMLLoader(sharpBot);
            loader.loadAIML(aimlPath);
            sharpBot.isAcceptingUserInput = false;
            sharpBot.isAcceptingUserInput = true;

            var patient = new User(name, sharpBot);
            var request = new Request(question, patient, sharpBot);
            var answer = sharpBot.Chat(request);
            return answer.Output;
        }
    }
}
