using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace CurrencyConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest req;
            HttpWebResponse resp;
            StreamReader sr;

            char[] separator = { ',' };

            string result;
            string fullPath;
            string currencyFrom = "USD"; //доллар США
            string currencyTo = "RUB"; //Индийская рупия
            double amount = 1d;

            Console.WriteLine("Currency Converter");
            Console.WriteLine("Currency From : {0}",currencyFrom);
            Console.WriteLine("Currency To : {0}", currencyTo);
            Console.WriteLine("Amount : {0}",amount);

            //URL, возвращающий котировку
            fullPath = "http://finance.yahoo.com/d/quotes.csv?s=" + currencyFrom + currencyTo + "=X&f=sl1d1t1c1ohgv&e=.csv";

            try
            {
                req = (HttpWebRequest)WebRequest.Create(fullPath);
                resp = (HttpWebResponse)req.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), Encoding.ASCII);
                result = sr.ReadLine();
                resp.Close();
                sr.Close();
                string[] temp = result.Split(separator);

                if(temp.Length > 1)
                {
                    //Показываем только значимые части
                    double rate = Convert.ToDouble(temp[1].Replace(".",","));
                    double convert = amount * rate;
                    Console.WriteLine("{0} {1}(s) = {2} {3}(s)",amount,currencyFrom,convert,currencyTo);
                }
                else
                {
                    Console.WriteLine("Error in getting currency rates " + "from website");
                }
            }
            catch
            {
                Console.WriteLine("Exception occurred");
            }
            Console.ReadLine();
        }
    }
}
