using OpenQA.Selenium;

namespace TescoScraper;

public class Auchan
{
    private SeleniumHelper _helper;
    private const string baseUrl = "https://online.auchan.hu/en/shop";

    public Auchan()
    {
        _helper = new SeleniumHelper(baseUrl);
    }

    public void Create()
    {
        var parent = _helper.GetElementByCssSelector("button._3ozx._-5qq.Tulx");
        var root = new Category(parent.Text,null);
        var links = _helper.GetElementsByClassName(parent, "Rb2x").Select(x => x.GetAttribute("Id")).ToList();
        foreach (var id in links)
        {
            var allCategory = _helper.GetElementByCssSelector(baseUrl, "button._3ozx._-5qq.Tulx");
            var element = _helper.GetElementsByClassName(allCategory, "Rb2x")
                .FirstOrDefault(x => x.GetAttribute("Id") == id);

            root.AddSubCategory(GetCategoryByElement(element,root));
        }
        
        FileHelper.CreateCsv("auchan", root,"Root;Level1;Level2;Level3;SKU");
    }

    private Category GetCategoryByElement(IWebElement parent, Category parentCategory)
    {
        var pCategory = CreateCategoryFromElement(parent,parentCategory);
        var elements = _helper.GetByCssSelector(parent, "a._2xbO._PA2");
        var categories = elements.Select(x => new Category(x.Text, pCategory,x.GetAttribute("href"))).ToList();
        foreach (var category in categories)
        {
            var items = _helper.GetElementsByCssSelector(category.Link, "a._2xbO.z2m7").Select(x=> new Category(x.Text,category,x.GetAttribute("href"))).ToList();
            
            foreach (var item in items)
            {
                var group =_helper.GetElementByCssSelector(item.Link.ToString(), "div.progress-bar");
                var skuString =  group.GetAttribute("aria-valuemax");
                if(int.TryParse(skuString,out var sku))
                    item.SKU = sku.ToString();
                category.AddSubCategory(item);
            }
            pCategory.AddSubCategory(category);
        }

        return pCategory;
    }

    private Category CreateCategoryFromElement(IWebElement element, Category parent)
    {
        return new Category(element.Text,parent);
    }
}

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