using AIMLbot;

namespace DrSharp.Domain.Logic
{
    public class DoctorSharp
    {
        private readonly string aimlPath;
        //private const string SettingsPath = @"D:\Code\Leeds Sharp\DoctorSharp\src\DrSharp.Web\config\settings.xml";
        private const string SettingsPath = @"E:\Code\GitHub\DoctorSharp\src\DrSharp.Web\config\settings.xml";

        public DoctorSharp(string aimlPath)
        {
            this.aimlPath = aimlPath;
        }

        public string Ask(string name, string question)
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
