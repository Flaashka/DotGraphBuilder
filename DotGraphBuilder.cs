using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FluentApi.Graph
{
	
	public class DotGraphBuilder
	{
		public static Graph DirectedGraph(string graphName)
		{
			var isDirected = true;
			var isStrict = true;
			
			return new Graph(graphName, isDirected, isStrict);
		}
		
		public static Graph UndirectedGraph(string graphName)
		{
			var isDirected = false;
			var isStrict = true;
			
			return new Graph(graphName, isDirected, isStrict);
		}		
	}
}