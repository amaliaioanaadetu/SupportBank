using Newtonsoft.Json;

public class Transaction
{
    public DateTime Date { get; set; }
    public string FromAccount { get; set; }
    public string ToAccount { get; set; }
    public string Narrative { get; set; }
    public decimal Amount { get; set; }
}

class SupportBank
{
    static void Main()
    {
        Console.WriteLine("File to import:\n");
        string file = Console.ReadLine();
        List<Transaction> transactions = new List<Transaction>();

        if (file.EndsWith("json"))
        {
            string input = File.ReadAllText("Transactions2013.json");
            transactions = JsonConvert.DeserializeObject<List<Transaction>>(input);
        }
        else if (file.EndsWith("csv"))
        {
            var lines = File.ReadAllLines(file);
            bool isFirstLine = true;
            foreach (var line in lines)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                
                var fields = line.Split(',');
                transactions.Add(new Transaction
                {
                    Date = DateTime.Parse(fields[0]),
                    FromAccount = fields[1],
                    ToAccount = fields[2],
                    Narrative = fields[3],
                    Amount = decimal.Parse(fields[4])
                });
            }
        }
        
        
        Dictionary<string, Double> accounts = new Dictionary<string, Double>();
        
        foreach (Transaction transaction in transactions)
        {
            if (!accounts.ContainsKey(transaction.FromAccount))
            {
                accounts[transaction.FromAccount] = 0;
            }
            accounts[transaction.FromAccount] -= Convert.ToDouble(transaction.Amount);
            
            if (!accounts.ContainsKey(transaction.ToAccount))
            {
                accounts[transaction.ToAccount] = 0;
            }
            accounts[transaction.ToAccount] += Convert.ToDouble(transaction.Amount);
        }
        
        foreach (var kvp in accounts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}