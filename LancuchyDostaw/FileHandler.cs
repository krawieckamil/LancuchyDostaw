using System;
using System.Collections.Generic;
using System.IO;

namespace LancuchyDostaw
{
    class FileHandler
    {
        public static void LoadData(string path, List<Supplier> providers, List<Customer> customers)
        {
            using (TextReader reader = File.OpenText(path))
            {
                string customersRaw = reader.ReadLine();
                if (customersRaw != null)
                {
                    int i = 0;
                    string providersRaw = reader.ReadLine();
                    if (providersRaw != null)
                    {
                        string[] providersArr = providersRaw.Split(' ');
                        i = 0;
                        foreach (var provider in providersArr)
                        {
                            if (i == 0) providers.Add(new Supplier(Int32.Parse(provider), 0));
                            else providers.Add(new Supplier(i, Int32.Parse(provider)));
                            i++;
                        }
                    }
                    i = 0;
                    string[] customersArr = customersRaw.Split(' ');
                    foreach (var customer in customersArr)
                    {
                        customers.Add(new Customer(i, Int32.Parse(customer)));
                        i++;
                    }
                }

                for (int i = 0; i < providers.Count; i++)
                {
                    string unitCostsRaw = reader.ReadLine();
                    if (unitCostsRaw != null)
                    {
                        string[] unitCosts = unitCostsRaw.Split(' ');
                        for (var j = 0; j < customers.Count; j++)
                        {
                            var customer = customers[j];
                            customer.Connections.Add(new TransportConnection(providers[i], customers[j], Int32.Parse(unitCosts[j]), i, j));
                        }
                    }
                }
            }

            //return controlPoints;
        }
    }
}
