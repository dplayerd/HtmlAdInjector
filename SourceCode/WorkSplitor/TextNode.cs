namespace WorkSplitor
{
    public class TextNode : IWordElement
    {
        public string Context { get; set; }

        public override string ToString()
        {
            return $"{Context}(Text)";
        }
    }
}
