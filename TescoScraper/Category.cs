namespace TescoScraper;

public class Category
{
    public string Name { get; set; }
    public List<Category> SubCategories { get; set; }

    public Uri Link { get; set; }

    public string? SKU { get; set; }

    private string _path;
    
    public Category(string name, Category parent, string? url = null)
    {
        Name = name;
        SubCategories = new List<Category>();
        if (url != null)
        {
            Link = new Uri(url);
        }

        _path = parent != null ? $"{parent.GetPath()};{name}" : name;
    }

    public string GetPath()
    {
        return string.IsNullOrEmpty(SKU) ? _path : $"{_path};{SKU}";
    }
    
    public void AddSubCategory(Category subCategory)
    {
        SubCategories.Add(subCategory);
    }

    public void BFS(Action<Category> action)
    {
        Queue<Category> queue = new Queue<Category>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            Category current = queue.Dequeue();
            action(current);
            foreach (Category subCategory in current.SubCategories)
            {
                queue.Enqueue(subCategory);
            }
        }
    }

    public Category Search(string name)
    {
        Queue<Category> queue = new Queue<Category>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            Category current = queue.Dequeue();
            if (current.Name == name)
            {
                return current;
            }

            foreach (Category subCategory in current.SubCategories)
            {
                queue.Enqueue(subCategory);
            }
        }

        return null;
    }
}