using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VirtualConsultant.Models;

namespace VirtualConsultant
{
    class AnswerGenerator
    {
		JObject ontology;
		JEnumerable<JToken> nodes;
		JEnumerable<JToken> relation;
		JToken startNode;
		JToken endNode;
		static int k = 0;
		public AnswerGenerator(string ontologyPath)
		{
			ontology = JObject.Parse(File.ReadAllText(ontologyPath));
			var nodes = ((JArray)ontology["nodes"]).Children();
			var relations = ((JArray)ontology["relations"]).Children();

			startNode = nodes.Where(n => n["name"].ToString().ToUpper() == "Start".ToUpper()).FirstOrDefault();
			endNode = nodes.Where(n => n["name"].ToString().ToUpper() == "End".ToUpper()).FirstOrDefault();
		}
		public string GetNextAnswer(long telegramClientId, string clientPhrase)
		{
			JToken clientCurrentNode;
			Клиент client;
			using (ApplicationContext db = new ApplicationContext())
			{
				client = db.Клиент.Where(_client => _client.TelegramGuid == telegramClientId).FirstOrDefault();
				if (client == null)
				{
					client = new Клиент();
					client.TelegramGuid = telegramClientId;
					client.CurrentNodeId = startNode["id"].ToString();
					clientCurrentNode = startNode;

					db.Клиент.Add(client);
					db.SaveChanges();
				}
				else
				{
					clientCurrentNode = ontology.GetNodeById(client.CurrentNodeId);
				}



				if (clientPhrase == "/info")
				{
					string infoAnswer = "";
					if (client.Имя != null)
					{
						infoAnswer += $"Вас зовут: {client.Имя}.";
					}
					if (client.Пол != null)
					{
						infoAnswer += $"\nВаш пол: { client.Пол}.";
					}
					if (client.ДатаРождения != null)
					{
						infoAnswer += $"\nВаш возраст: { client.ДатаРождения}.";
					}
					if (infoAnswer == "")
					{
						infoAnswer = "Вы еще не рассказали ничего о себе!";
					}
					return infoAnswer;
				}

				var relation = ontology.GetRelationsByForwardRelationName(clientCurrentNode, "next").FirstOrDefault();
				var dbFieldToFill = ontology.GetAttributeByRelation(relation, "dbFieldToFill");
				if (dbFieldToFill != null)
				{
					Type clientType = typeof(Клиент);
					PropertyInfo clientPropertyInfo = clientType.GetProperty(dbFieldToFill.ToString());
					clientPropertyInfo.SetValue(client, clientPhrase);
					db.SaveChanges();
				}
				
				clientCurrentNode = ontology.GetNodesByForwardRelationName(clientCurrentNode, "next").FirstOrDefault();
				if (clientCurrentNode.Equals(endNode))
				{
					clientCurrentNode = ontology.GetNodesByForwardRelationName(startNode, "next").FirstOrDefault();
				}

				client.CurrentNodeId = clientCurrentNode["id"].ToString();
				db.SaveChanges();

				var answer = clientCurrentNode["name"].ToString();
			
				return answer;
			}
		}
    }
}
