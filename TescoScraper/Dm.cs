using System.Net.Mime;

namespace TescoScraper;

public class Dm
{
    private readonly SeleniumHelper _helper;
    private const string BASE_URL = "https://www.dm.hu/";

    public Dm()
    {
        _helper = new SeleniumHelper(BASE_URL);
    }

    public void Create()
    {
        var root = new Category("Minden kategória", null);
        var firstLevelNodes = _helper.GetElementsByXpath(@"//*[@id=""categoryNavigationContainer""]/ul/li[*]/a").Select(x=> new Node(x.Text,x.GetAttribute("href"))).ToList();
        foreach (var firstLevelNode in firstLevelNodes)
        {
            var firstLeveCategory = new Category(firstLevelNode.Name, root);
            var secondLevelNodes = _helper.GetElementsByXpath(firstLevelNode.Link, @"//*[@id=""dm-view""]/div/div/a")
                .Select(x => new Node(x.Text, x.GetAttribute("href"))).ToList()
                .Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

            foreach (var secondLevelNode in secondLevelNodes)
            {
                var secondLeveCategory = new Category(secondLevelNode.Name, firstLeveCategory);
                var thirdLevelNodes = _helper
                    .GetElementsByXpath(secondLevelNode.Link, @"//*[@id=""dm-view""]/div/div/a").Select(x => new Node(x.Text, x.GetAttribute("href"))).ToList()
                    .Where(x => !string.IsNullOrEmpty(x.Name)).ToList();

                foreach (var thirdLevelNode in thirdLevelNodes)
                {
                    var thirdLeveCategory = new Category(thirdLevelNode.Name, secondLeveCategory);

                    try
                    {
                        if (int.TryParse(_helper.GetElementByXpath(thirdLevelNode.Link,
                                    @"//*[@id=""mainSectionContainer""]/div[*]/div/div/div[*]/div[*]/span/b").Text,
                                out var sku))
                            thirdLeveCategory.Sku = sku.ToString();

                        secondLeveCategory.AddSubCategory(thirdLeveCategory);    
                    }
                    catch
                    {
                        thirdLeveCategory.Sku = "?";
                        secondLeveCategory.AddSubCategory(thirdLeveCategory);
                    }
                    
                    
                }
                
                firstLeveCategory.AddSubCategory(secondLeveCategory);
            }
            root.AddSubCategory(firstLeveCategory);
        }
        
        FileHelper.CreateCsv("dm",root,"Root;Level1;Level2;Level3;SKU");
    }
}