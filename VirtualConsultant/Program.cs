using System;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Reflection;
using VirtualConsultant.Models;

namespace VirtualConsultant
{
	class Program
	{
		static ITelegramBotClient botClient;
		static AnswerGenerator answerGenerator;
		public static void Main(string[] args)
		{
			botClient = new TelegramBotClient("secret") { Timeout = TimeSpan.FromSeconds(10) };

			var me = botClient.GetMeAsync().Result;
			Console.WriteLine(
			  $"Bot {me.FirstName} with number {me.Id} is running."
			);

			answerGenerator = new AnswerGenerator(@"C:\Users\Fox\source\repos\VirtualConsultant\VirtualConsultant\ClientInfoDialog.ont");

			botClient.OnMessage += Bot_OnMessage;
			botClient.StartReceiving();

			Console.WriteLine("Press any key to exit ");
			Console.ReadKey();

			botClient.StopReceiving(); 
		}
		async static void Bot_OnMessage(object sender, MessageEventArgs e)
		{
			if (e.Message.Text != null)
			{
				Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

				var answer = answerGenerator.GetNextAnswer(e.Message.Chat.Id, e.Message.Text);

				await botClient.SendTextMessageAsync(
				  chatId: e.Message.Chat,
				  text: answer
				);
			}
		} 
	}
}
