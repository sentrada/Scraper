namespace TescoScraper;

public class Category
{
    private string Name { get; set; }
    private List<Category> SubCategories { get; set; }

    public Uri Link { get; set; }

    public string? Sku { get; set; }

    private readonly string _path;
    
#pragma warning disable CS8618
    public Category(string name, Category? parent, string? url = null)
#pragma warning restore CS8618
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
        return string.IsNullOrEmpty(Sku) ? _path : $"{_path};{Sku}";
    }
    
    public void AddSubCategory(Category subCategory)
    {
        SubCategories.Add(subCategory);
    }

    // ReSharper disable once InconsistentNaming
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

    public Category? Search(string name)
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