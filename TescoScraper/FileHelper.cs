using System.Text;

namespace TescoScraper;

public class FileHelper
{
    private static int numberOfColumns;

    public static void CreateCsv(string fileName, Category root, string header)
    {
        numberOfColumns = header.Split(';').Length;
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
            if (values.Count < numberOfColumns)
            {
                if (int.TryParse(values.Last(),out _))
                {
                    var diff = numberOfColumns - values.Count - 1;
                    for (int i = 0; i <= diff; i++)
                    {
                        values.Insert(values.Count - 1, string.Empty);
                    }
                }
                else
                {
                    for (int i = values.Count; i < numberOfColumns; i++)
                    {
                        values.Add(string.Empty);
                    }
                }
            }
            
            result.Add($"{string.Join(";", values.ToArray())};");
            
        }

        return result;
    }

    public static string TransformString(string input, int count)
    {
        string[] values = input.Split(';');
        int diff = count - values.Length;
        if (diff <= 0)
        {
            return input;
        }
        else
        {
            int result;
            if (int.TryParse(values.Last(), out result))
            {
                for (int i = 0; i <= diff; i++)
                {
                    input = input.Insert(input.LastIndexOf(";"), ";");
                }
            }
            else
            {
                for (int i = 0; i < diff; i++)
                {
                    input = input + ";";
                }
            }
        }
        return input;
    }
}