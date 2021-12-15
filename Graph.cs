using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace FluentApi.Graph
{
    public class Graph
    {
        private readonly List<GraphEdge> edges = new List<GraphEdge>();
        private readonly Dictionary<string, GraphNode> nodes = new Dictionary<string, GraphNode>();
        private readonly Stack<GraphNode> nodesStack = new Stack<GraphNode>();
        
        private readonly List<Action> actions = new List<Action>();
        
        public Graph(string graphName, bool directed, bool strict)
        {
            GraphName = graphName;
            Directed = directed;
            Strict = strict;
        }

        public string GraphName { get; }
        public bool Directed { get; }
        public bool Strict { get; }

        public IEnumerable<GraphEdge> Edges => edges;
        public IEnumerable<GraphNode> Nodes => nodes.Values;

        public Graph AddNode(string name)
        {
            actions.Add(() =>
            {
                if (!nodes.TryGetValue(name, out _))
                {
                    var node = new GraphNode(name);
                    nodes.Add(name, node);
                    nodesStack.Push(node);
                }
            });

            return this;
        }

        public GraphEdge AddEdge(string sourceNode, string destinationNode)
        {
            var result = new GraphEdge(sourceNode, destinationNode, Directed);
            edges.Add(result);
            return result;
        }

        public string Build()
        {
            foreach (var action in actions) action();

            return this.ToDotFormat();
        }

        public Graph With(Action<GraphNode> actionOnGraphNode)
        {
            actions.Add(() =>
            {
                var lastNode = nodesStack.Peek();
                if (lastNode != null) 
                    actionOnGraphNode(lastNode);
            });
            
            return this;
        }
    }
}