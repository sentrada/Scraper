namespace TescoScraper;

public class Node
{
    public string Name { get; set; }
    
    public string Link { get; set; }

    public Node(string name, string link)
    {
        Name = name;
        Link = link;
    }
}