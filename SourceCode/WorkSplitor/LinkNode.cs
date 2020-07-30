using System;

namespace WorkSplitor
{
    public class LinkNode : IWordElement
    {
        public string Context { get; set; }
        public Uri LinkUrl { get; set; }

        public override string ToString()
        {
            return $"{Context}({LinkUrl})";
        }

    }
}
