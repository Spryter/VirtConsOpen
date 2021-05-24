﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VirtualConsultant
{
	public static class jsonExtensions
	{
		public static IEnumerable<JToken> GetNodesByForwardRelationName(this JObject ontology, JToken baseNode, string relationName)
		{
			var nodes = ((JArray)ontology["nodes"]).Children();
			var relations = ((JArray)ontology["relations"]).Children();
			var relation = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).FirstOrDefault();
			var needRelationDestinations = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).Select(r => r["destination_node_id"].ToString());
			return nodes.Where(n => needRelationDestinations.Contains(n["id"].ToString()));
		}
		public static IEnumerable<JToken> GetNodesByBackwardRelationName(this JObject ontology, JToken baseNode, string relationName)
		{
			var nodes = ((JArray)ontology["nodes"]).Children();
			var relations = ((JArray)ontology["relations"]).Children();
			var relation = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).FirstOrDefault();
			var needRelationDestinations = relations.Where(r => r["destination_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName).Select(r => r["source_node_id"].ToString());
			return nodes.Where(n => needRelationDestinations.Contains(n["id"].ToString()));
		}
		public static IEnumerable<JToken> GetRelationsByForwardRelationName(this JObject ontology, JToken baseNode, string relationName)
		{
			var nodes = ((JArray)ontology["nodes"]).Children();
			var relations = ((JArray)ontology["relations"]).Children();
			var relation = relations.Where(r => r["source_node_id"].ToString() == baseNode["id"].ToString() && r["name"].ToString() == relationName);
			return relation;
		}
		public static JToken GetAttributeByRelation(this JObject ontology, JToken relation, string attributeName)
		{
			var attribute = relation["attributes"][attributeName];
			return attribute;	
		}
		public static JToken GetNodeById(this JObject ontology, string nodeId)
		{
			var nodes = ((JArray)ontology["nodes"]).Children();
			var node = nodes.Where(n => n["id"].ToString() == nodeId).FirstOrDefault();
			return node;
		}
	}
	//public static class DbSetExtensions
	//{
	//	public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
	//	{
	//		var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
	//		return !exists ? dbSet.Add(entity) : null;
	//	}
	//}
}
