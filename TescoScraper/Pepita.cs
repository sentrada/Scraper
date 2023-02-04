using OpenQA.Selenium;

namespace TescoScraper;

public class Pepita
{
    private readonly SeleniumHelper _helper;
    private const string BASE_URL = "https://pepita.hu/osszes-kategoria";

    public Pepita()
    {
        _helper = new SeleniumHelper(BASE_URL);
    }

    public void Create()
    {
        var root = new Category("Összes kategória", null);
        var firstLevelNodes =
            _helper.GetElementsByCssSelector(@"a.compact-cat__element.scrollable-filter__link")
                .GetNodesFromWebElements();

        foreach (var firstLevelNode in firstLevelNodes)
        {
            var firstLeveCategory = new Category(firstLevelNode.Name, root);
            var secondLevelNodes =
                _helper.GetElementsByCssSelector(new Uri(firstLevelNode.Link),
                    @"a.compact-cat__element.scrollable-filter__link").GetNodesFromWebElements();

            foreach (var secondLevelNode in secondLevelNodes)
            {
                var secondLevelCategory = new Category(secondLevelNode.Name, firstLeveCategory);

                var thirdLevelNodes = GetWhatWereYouThinkingNodes(secondLevelNode);

                foreach (var thirdLevelNode in thirdLevelNodes)
                {
                    var thirdLevelParsed = ParseString(thirdLevelNode.Name);
                    var thirdLevelCategory =
                        new Category(thirdLevelParsed.Name, secondLevelCategory, thirdLevelNode.Link);
                    var fourthLevelNodes = GetWhatWereYouThinkingNodes(thirdLevelNode);

                    if (fourthLevelNodes.Any())
                    {
                        foreach (var fourthLevelNode in fourthLevelNodes)
                        {
                            var parsed = ParseString(fourthLevelNode.Name);
                            var fourthLevelCategory =
                                new Category(parsed.Name, thirdLevelCategory, fourthLevelNode.Link);
                            var fifthLevelNodes = GetWhatWereYouThinkingNodes(fourthLevelNode);
                            if (fifthLevelNodes.Any())
                            {
                                foreach (var fifthLevelNode in fifthLevelNodes)
                                {
                                    var fitthLevelparsed = ParseString(fifthLevelNode.Name);
                                    var fifthLevelCategory = new Category(parsed.Name, fourthLevelCategory,
                                        fifthLevelNode.Link)
                                    {
                                        Sku = fitthLevelparsed.Sku
                                    };

                                    fourthLevelCategory.AddSubCategory(fifthLevelCategory);
                                }
                            }
                            else
                            {
                                thirdLevelCategory.Sku = thirdLevelParsed.Sku;
                            }

                            thirdLevelCategory.AddSubCategory(fourthLevelCategory);
                        }
                    }
                    else
                    {
                        thirdLevelCategory.Sku = thirdLevelParsed.Sku;
                    }

                    secondLevelCategory.AddSubCategory(thirdLevelCategory);
                }

                firstLeveCategory.AddSubCategory(secondLevelCategory);
            }

            root.AddSubCategory(firstLeveCategory);
        }

        FileHelper.CreateCsv("pepita", root, "Root;Level1;Level2;Level3;Level4;SKU");
    }

    private List<Node> GetWhatWereYouThinkingNodes(Node node)
    {
        var parent = _helper
            .GetElementByCssSelector(node.Link, @"ul#category-filters");

        var elements = parent?.FindElements(By.TagName("a"));

        return elements != null ? elements.GetNodesFromWebElements() : new List<Node>();
    }


    public static (string Name, string Sku) ParseString(string input)
    {
        var lastIndexOfBracket = input.LastIndexOf('(');
        var firstPart = input.Substring(0, lastIndexOfBracket - 1).Trim();
        var secondPart = input.Substring(lastIndexOfBracket + 1, input.Length - lastIndexOfBracket - 2).Trim();


        return (firstPart, secondPart);
    }
}