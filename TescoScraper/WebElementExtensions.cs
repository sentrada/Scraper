using OpenQA.Selenium;

namespace TescoScraper;

public static class WebElementExtensions
{
    public static List<Node> GetNodesFromWebElements(this IEnumerable<IWebElement> webElements)
    {
        return webElements
            .Select(x => new Node(x.Text, x.GetAttribute("href")))
            .Where(x => !string.IsNullOrEmpty(x.Name))
            .ToList();
    }
}

public static class StringExtensions
{
    public static bool HasValue(this string? item)
    {
        return item == null;
    }   
}