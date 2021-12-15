using System;
using System.Collections.Generic;

namespace FluentApi.Graph
{
    public class GraphNode
    {
        public readonly Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public string Name { get; }

        public GraphNode(string name)
        {
            Name = name;
        }

        public GraphNode Color(string color)
        {
            if (!Attributes.TryGetValue("color", out _)) 
                Attributes.Add("color", color);

            return this;
        }

        public GraphNode Shape(NodeShape nodeShape)
        {
            if (!Attributes.TryGetValue("shape", out _))
                Attributes.Add("shape", GetShapeName(nodeShape));
            
            return this;
        }

        private string GetShapeName(NodeShape shape)
        {
            switch (shape)
            {
                case NodeShape.Box:
                    return "box";
                case NodeShape.Ellipse:
                    return "ellipse";
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
            }
        }
    }
}