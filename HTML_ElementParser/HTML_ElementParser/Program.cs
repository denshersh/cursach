using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;

class Program
{
    private const uint NUMBER_OF_DIRECT_CONNECTIONS = 1000;
    private const string FILE_PATH = "";
    static Dictionary<string, int> StringCounts = new Dictionary<string, int>();
    static List<string> strings = new List<string>();
    static async Task Main(string[] args)
    {
        ChromeOptions options = new ChromeOptions();
        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;
        options.AddArgument("--headless");
        options.AddArgument("--silent");

        using (var driver = new ChromeDriver(service, options))
        {
            await ExecuteConnectionsAsync(driver);
        }

        foreach (var pair in StringCounts)
        {
            strings.Add(pair.Key);
        }
        File.WriteAllLines(FILE_PATH, strings);
        Console.WriteLine(strings.Count);
    }

    static async Task ExecuteConnectionsAsync(ChromeDriver driver)
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < NUMBER_OF_DIRECT_CONNECTIONS; i++)
        {
            tasks.Add(ProcessConnectionAsync(driver));
        }

        await Task.WhenAll(tasks);
    }


    static async Task ProcessConnectionAsync(ChromeDriver driver)
    {
        try
        {
            string url = "https://randomwordgenerator.com/paragraph.php";
            driver.Navigate().GoToUrl(url);

            await Task.Delay(TimeSpan.FromSeconds(2));

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            HtmlNode node = doc.DocumentNode.SelectSingleNode("//span[@class='support-paragraph']");
            if (node != null)
            {
                if (StringCounts.ContainsKey(node.InnerText))
                {
                    StringCounts[node.InnerText]++;
                }
                else
                {
                    StringCounts[node.InnerText] = 1;
                }
            }
            else
            {
                Console.WriteLine("Paragraph not found on the page.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing connection: {ex.Message}");
        }
    }
}
