using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
// ReSharper disable InconsistentNaming

namespace FluentApi.Graph
{
	
	public class DotGraphBuilder
	{
		private readonly Graph graph;

		protected DotGraphBuilder(string graphName, bool directed)
		{
			graph = new Graph(graphName, directed, false);
		}
		
		public static DotGraphBuilder DirectedGraph(string graphName)
		{
			return new DotGraphBuilder(graphName, directed: true);
		}

		public static DotGraphBuilder UndirectedGraph(string graphName)
		{
			return new DotGraphBuilder(graphName, directed: false);
		}

		public GraphNodeBuilder AddNode(string nodeName)
		{
			var node = graph?.AddNode(nodeName);

			return new GraphNodeBuilder(node, this);
		}

		public string Build() => graph.ToDotFormat();
	}

	public class GraphNodeBuilder
	{
		private readonly GraphNode graphNode;
		private readonly DotGraphBuilder parentBuilder;
		
		public GraphNodeBuilder(GraphNode graphNode, DotGraphBuilder parent)
		{
			this.graphNode = graphNode;
			parentBuilder = parent;
		}
		
		public GraphNodeBuilder AddNode(string nodeName)
		{
			return parentBuilder.AddNode(nodeName);
		}
		
		public string Build() => parentBuilder.Build();
	}
}