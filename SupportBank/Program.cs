using System.Globalization;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;


class SupportBank
{
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
    static void Main()
    {
        var config = new LoggingConfiguration();
        var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
        config.AddTarget("File Logger", target);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
        LogManager.Configuration = config;
        
        logger.Info("Program started");
        
        Dictionary<string, Double> accounts = new Dictionary<string, Double>();
        
        using var reader = new StreamReader("DodgyTransactions2015.csv");
        logger.Info("File read");
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

            if (double.TryParse(fields[4], out double amount))
            {
                logger.Info("Amount field is a correct number");
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
            else
            {
                {
                    logger.Error($"Amount field is not a number: {fields[4]}");
                    return;
                }
            }
        }

        foreach (var kvp in accounts)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}