using System.Text;

namespace TescoScraper;

public static class FileHelper
{
    private static int _numberOfColumns;

    public static void CreateCsv(string fileName, Category root, string header)
    {
        _numberOfColumns = header.Split(';').Length;
        var allLines = new HashSet<string>();
        allLines.Add(header);
        root.BFS(category => allLines.Add(category.GetPath()));
        var modifiedLines = ModifyLines(allLines.ToList());
        File.WriteAllLines($".\\{fileName}_{DateTime.Now:yyyy-MM-dd_hh-mm-ss}.csv", modifiedLines, Encoding.UTF8);
    }

    private static List<string> ModifyLines(List<string> allLines)
    {
        var result = new List<string>();
        foreach (var line in allLines)
        {
            var values = line.Split(';').ToList();
            if (values.Count < _numberOfColumns)
            {
                if (int.TryParse(values.Last(), out _))
                {
                    var diff = _numberOfColumns - values.Count - 1;
                    for (int i = 0; i <= diff; i++)
                    {
                        values.Insert(values.Count - 1, string.Empty);
                    }
                }
                else
                {
                    for (int i = values.Count; i < _numberOfColumns; i++)
                    {
                        values.Add(string.Empty);
                    }
                }
            }

            result.Add($"{string.Join(";", values.ToArray())};");
        }

        return result;
    }
}
