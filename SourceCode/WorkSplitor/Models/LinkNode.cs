using System;

namespace WorkSplitor.Models
{
    public class LinkNode : IWordNode
    {
        public string Context { get; set; }
        public Uri LinkUrl { get; set; }
        public TextNodeType NodeType { get; set; } = TextNodeType.Link;

        public override string ToString()
        {
            return $"{Context}({LinkUrl})";
        }

    }
}
