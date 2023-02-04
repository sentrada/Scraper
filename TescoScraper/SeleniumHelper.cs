using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TescoScraper;

public class SeleniumHelper
{
    private readonly ChromeDriver _driver;

    public SeleniumHelper(string url)
    {
        var options = new ChromeOptions();
        options.AddArguments("--headless");
        options.AddArguments("--window-size=1920,1080");
        options.AddArguments("--start-maximized");
        _driver = new ChromeDriver(options);
        _driver.Url = url;
    }

    #region By.Id
    public IWebElement GetElementById(IWebElement webElement, string id)
    {
        webElement.Click();
        var element = webElement.FindElement(By.Id(id));
        return element;
    }

    public IWebElement GetElementById(Uri uri, string id)
    {
        _driver.Url = uri.ToString();
        _driver.Navigate();
        var element = _driver.FindElement(By.Id(id));
        return element;
    }
    #endregion
    
    public ReadOnlyCollection<IWebElement> GetElementsByClassName(IWebElement webElement, string className)
    {
        webElement.Click();
        var elements = _driver.FindElements(By.ClassName(className));
        return elements;
    }

    public ReadOnlyCollection<IWebElement> GetElementsByClassName(Uri uri, string className)
    {
        _driver.Url = uri.ToString();
        _driver.Navigate();
        var elements = _driver.FindElements(By.ClassName(className));
        return elements;
    }

    public IWebElement GetElementByXpath(string url, string xpath)
    {
        _driver.Url = url.ToString();
        _driver.Navigate();
        var element = Retry.Do(()=> _driver.FindElement(By.XPath(xpath)), TimeSpan.FromMilliseconds(300), 10);
        return element;
    }
    
    public ReadOnlyCollection<IWebElement> GetElementsByXpath(string xpath)
    {
        var elements = _driver.FindElements(By.XPath(xpath));
        return elements;
    }
    
    public ReadOnlyCollection<IWebElement> GetElementsByXpath(string url, string xpath)
    {
        _driver.Url = url.ToString();
        _driver.Navigate();
        var elements = _driver.FindElements(By.XPath(xpath));
        return elements;
    }
    
    public ReadOnlyCollection<IWebElement> GetElementsByXpath(IWebElement webElement, string xpath)
    {
        webElement.Click();
        var elements = webElement.FindElements(By.XPath(xpath));
        return elements;
    }

    #region By CssSelector

    #region Single
    public IWebElement GetElementByCssSelector(string cssSelector)
    {
        return _driver.FindElement(By.CssSelector(cssSelector));
    }

    public IWebElement? GetElementByCssSelector(string url, string cssSelector)
    {
        _driver.Url = url.ToString();
        _driver.Navigate();
        var webElement = _driver.FindElements(By.CssSelector(cssSelector))?.FirstOrDefault();
        return webElement;
    }
    #endregion
    
    public ReadOnlyCollection<IWebElement> GetByCssSelector(IWebElement webElement, string cssSelector)
    {
        webElement.Click();
        var elements = _driver.FindElements(By.CssSelector(cssSelector));
        return elements;
    }

    public ReadOnlyCollection<IWebElement> GetElementsByCssSelector(string cssSelector)
    {
        var elements = _driver.FindElements(By.CssSelector(cssSelector));
        return elements;
    }
    
    public ReadOnlyCollection<IWebElement> GetElementsByCssSelector(IWebElement webElement, string cssSelector)
    {
        webElement.Click();
        var elements = _driver.FindElements(By.CssSelector(cssSelector));
        return elements;
    }

    public ReadOnlyCollection<IWebElement> GetElementsByCssSelector(Uri uri, string cssSelector)
    {
        _driver.Url = uri.ToString();
        _driver.Navigate();
        var elements = _driver.FindElements(By.CssSelector(cssSelector));
        return elements;
    }
    #endregion
}