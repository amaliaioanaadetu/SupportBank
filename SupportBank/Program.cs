using System.Globalization;
using System.IO;

class SupportBank
{
    static void Main()
    {
        Dictionary<string, Double> accounts = new Dictionary<string, Double>();
        
        using var reader = new StreamReader("Transactions2014.csv");
        string line;
        bool isFirstLine = true;
        while ((line = reader.ReadLine()) != null)
        {
            var fields = line.Split(',');
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }
            
            if (!accounts.ContainsKey(fields[1]))
            {
                accounts[fields[1]] = 0;
            }
            accounts[fields[1]] -= Convert.ToDouble(fields[4]);
            
            if (!accounts.ContainsKey(fields[2]))
            {
                accounts[fields[2]] = 0;
            }
            accounts[fields[2]] += Convert.ToDouble(fields[4]);

        }

        foreach (var kvp in accounts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}