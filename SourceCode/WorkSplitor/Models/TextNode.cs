namespace WorkSplitor.Models
{
    public class TextNode : IWordNode
    {
        public string Context { get; set; }
        public TextNodeType NodeType { get; set; } = TextNodeType.Text;

        public override string ToString()
        {
            return $"{Context}(Text)";
        }
    }
}
