namespace WorkSplitor.Models
{
    public interface IWordNode
    {
        string Context { get; set; }

        TextNodeType NodeType { get; set; }
    }
}
