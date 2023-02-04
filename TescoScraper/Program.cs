using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TescoScraper;

GetAll();

static void GetAll()
{
    //var auchan = new Auchan();
    //auchan.Create();

    // var dm = new Dm();
    // dm.Create();

    var pepita = new Pepita();
    pepita.Create();
    //DoDo();
}

static void Do()
{
    var options = new ChromeOptions();
    options.AddArguments("--headless");
    options.AddArguments("--window-size=1920,1080");
    options.AddArguments("--start-maximized");
    var driver = new ChromeDriver(options);
    driver.Url = "https://pepita.hu/epitkezes-felujitas-c3957";
    var elements = driver.FindElement(By.CssSelector("ul#category-filters")).FindElements(By.TagName("a"))
        .Select(x => x.Text).ToList();
}

static void DoDo()
{
    var baseUrl = "https://pepita.hu/epitkezes-felujitas-c3957";
    var _helper = new SeleniumHelper(baseUrl);
    var root = new Category("Összes kategória", null);
    var secondLevelCategory = new Category("Építkezés & Felújítás", root);
    var thirdLevelNodes = new List<Node>();
    try
    {
        thirdLevelNodes = _helper
            .GetElementByCssSelector(baseUrl, @"ul#category-filters")
            .FindElements(By.TagName("a")).GetNodesFromWebElements();
    }
    catch (Exception e)
    {
        Console.WriteLine($"3.\t{baseUrl}");
    }

    foreach (var thirdLevelNode in thirdLevelNodes)
    {
        var thirdLevelParsed = Pepita.ParseString(thirdLevelNode.Name);
        var thirdLevelCategory =
            new Category(thirdLevelParsed.Name, secondLevelCategory, thirdLevelNode.Link);
        var fourthLevelNodes = GetFourthLevelNodes(thirdLevelNode, _helper);

        if (fourthLevelNodes.Any())
        {
            foreach (var fourthLevelNode in fourthLevelNodes)
            {
                var parsed = Pepita.ParseString(fourthLevelNode.Name);
                var fourthLevelCategory =
                    new Category(parsed.Name, thirdLevelCategory, fourthLevelNode.Link)
                    {
                        SKU = parsed.Sku
                    };

                thirdLevelCategory.AddSubCategory(fourthLevelCategory);
            }
        }
        else
        {
            thirdLevelCategory.SKU = thirdLevelParsed.Sku;
        }

        secondLevelCategory.AddSubCategory(thirdLevelCategory);

        root.AddSubCategory(secondLevelCategory);
    }

    FileHelper.CreateCsv("pepita", root, "Root;Level1;Level2;Level3;SKU");
}

static List<Node> GetFourthLevelNodes(Node node, SeleniumHelper _helper)
{
    try
    {
        var parent = _helper
            .GetElementByCssSelector(node.Link, @"ul#category-filters");

        var elements = parent?.FindElements(By.TagName("a"));

        if (elements != null)
        {
            return elements.GetNodesFromWebElements();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"4.\t{node.Link}");
    }

    return new List<Node>();
}