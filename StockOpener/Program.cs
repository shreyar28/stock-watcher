using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace StockOpener
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConfigParse.ParseConfig("StockOpener.config");

                Console.Write("Hi there. Enter the stock to be searched for: ");
                string stock = Console.ReadLine();
                Console.WriteLine(); 
                Console.WriteLine("Cool. Searching for " + stock + "...");
                SearchStock(stock);
                Console.WriteLine("My work here is done. I will self destruct momentarily.");
                Thread.Sleep(Convert.ToInt32(ConfigParse.ConfigKeys["self_destruct_time"])*1000);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception occured. Weird.");
                Console.WriteLine("Exception: " + e);
                Console.WriteLine("Please report to your local friendly software developer.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private static void SearchStock(string stock)
        {
            try
            {
                string chromepath = ConfigParse.ConfigKeys["browser_path"];
                string[] websites = ConfigParse.ConfigKeys["websites"].Split(',');
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = chromepath;

                foreach (string s in websites)
                {
                    string website = s + stock; 
                    if (IsValidURI(website))
                    {
                        startInfo.Arguments = website;
                        Process.Start(startInfo);
                        Thread.Sleep(100);
                    }
                    else
                    {
                        Console.WriteLine("Check your link. Doesn't seem right: " + website);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception occured. Weird.");
                Console.WriteLine("Exception: " + e);
                Console.WriteLine("Please report to your local friendly software developer.");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public static bool IsValidURI(string uri)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                    return false;
                Uri tmp;
                if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                    return false;
                return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception occured. Weird.");
                Console.WriteLine("Exception: " + e);
                Console.WriteLine("Please report to your local friendly software developer.");
                Console.ReadKey();
                Environment.Exit(1);
                return false;
            }
        }
    }
}
