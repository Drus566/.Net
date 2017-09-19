using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {
        private static string hostname = "www.wrox.com";
        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                hostname = args[0];
            }
            IAsyncResult ar = Dns.BeginGetHostByName(hostname, null, null);
            while (!ar.IsCompleted)
            {
                Console.WriteLine("Can do something else...");
                System.Threading.Thread.Sleep(100);
            }
            DnsLookupCompleted(ar);
            Console.ReadLine();
            //Dns.BeginGetHostByName(hostname, new AsyncCallback(DnsLookupCompleted), null);
            //Console.WriteLine("Waiting for the results");
            //Console.ReadLine();
        }
        private static void DnsLookupCompleted(IAsyncResult ar)
        {
            IPHostEntry entry = Dns.EndGetHostByName(ar);
            Console.WriteLine("IP Addresses for 0: ", hostname);
            foreach(IPAddress address in entry.AddressList)
            {
                Console.WriteLine(address.ToString());
            }
            Console.WriteLine("\nAlias names:");
            foreach( string aliasName in entry.Aliases)
            {
                Console.WriteLine(aliasName);
            }
            Console.WriteLine("\nAnd the real hostname:");
            Console.WriteLine(entry.HostName);
        }
    }
}
