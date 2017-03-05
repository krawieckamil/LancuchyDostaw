using System;
using System.Collections.Generic;

namespace LancuchyDostaw
{
    class Utils
    {
        public static void PrintDetailedData(List<Provider> providers, List<Recipient> recipients)
        {
            Console.WriteLine("-------DOSTAWCY-------");
            foreach (var provider in providers)
            {
                Console.WriteLine(provider.ToString());
            }
            Console.WriteLine("\n-------ODBIORCY-------");
            foreach (var recipient in recipients)
            {
                Console.WriteLine(recipient.ToString());
            }
        }

        public static void PrintMatrix(List<Provider> providers, List<Recipient> recipients)
        {
            Console.WriteLine("-------MATRIX--------\n");
            Console.Write("\t");
            foreach (var recipient in recipients)
            {
                Console.Write(recipient.AmountOfDemand + "\t");
            }
            Console.Write("\n");
            int i = 0;
            foreach (var provider in providers)
            {
                Console.Write(provider.AmountOfSupply + "\t");
                foreach (var recipient in recipients)
                {
                    Console.Write(recipient.ProviderIdUnitCosts[i].unitCost + "\t");
                }
                i++;
                Console.Write("\n");
            }
        }
    }
}