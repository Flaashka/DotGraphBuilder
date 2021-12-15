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

		private DotGraphBuilder(string graphName, bool directed)
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

		public DotGraphBuilder With(Action<NodeCommonAttributesConfig> applyAttributes)
		{
			applyAttributes(new NodeCommonAttributesConfig(graphNode));
			
			return parentBuilder;
		}
		
		public string Build() => parentBuilder.Build();
	}

	public class CommonAttributesConfig<TConfig>
		where TConfig : CommonAttributesConfig<TConfig>
	{
		private readonly IDictionary<string, string> attributes;

		protected CommonAttributesConfig(IDictionary<string, string> attributes)
		{
			this.attributes = attributes;
		}
		
		public TConfig Label(string label)
		{
			attributes["label"] = label;
			
			return (TConfig)this;
		}
		
		public TConfig Color(string color)
		{
			attributes["color"] = color;
			
			return (TConfig)this;
		}
		
		public TConfig FontSize(float sizeInPt)
		{
			attributes["fontsize"] = sizeInPt.ToString(CultureInfo.InvariantCulture);
			
			return (TConfig)this;
		}
	}
	
	public class NodeCommonAttributesConfig : CommonAttributesConfig<NodeCommonAttributesConfig>
	{
		private readonly GraphNode node;

		public NodeCommonAttributesConfig(GraphNode node) : base(node.Attributes)
		{
			this.node = node;
		}
		
		public NodeCommonAttributesConfig Shape(NodeShape shape)
		{
			node.Attributes["shape"] = shape.ToString().ToLowerInvariant();
			
			return this;
		}
	}
	
	public class EdgeCommonAttributesConfig : CommonAttributesConfig<EdgeCommonAttributesConfig>
	{
		private readonly GraphEdge edge;

		public EdgeCommonAttributesConfig(GraphEdge edge) : base(edge.Attributes) => this.edge = edge;
		
		public EdgeCommonAttributesConfig Weight(double weight)
		{
			edge.Attributes["weight"] = weight.ToString(CultureInfo.InvariantCulture);
			
			return this;
		}
	}
	
	public enum NodeShape
	{
		Box,
		Ellipse
	}
}