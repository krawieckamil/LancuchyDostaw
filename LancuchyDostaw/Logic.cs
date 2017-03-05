using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LancuchyDostaw
{
    class Logic
    {
        public List<Provider> Providers { get; }
        public List<Recipient> Recipients { get; }

        public Logic()
        {
            Providers = new List<Provider>();
            Recipients = new List<Recipient>();

            FileHandler.LoadData("InitialData.txt", Providers, Recipients);
            PrintMatrix();
            //PrintDetailedData();
        }

        private void PrintDetailedData()
        {
            Console.WriteLine("-------DOSTAWCY-------");
            foreach (var provider in Providers)
            {
                Console.WriteLine(provider.ToString());
            }
            Console.WriteLine("\n-------ODBIORCY-------");
            foreach (var recipient in Recipients)
            {
                Console.WriteLine(recipient.ToString());
            }
        }

        private void PrintMatrix()
        {
            Console.WriteLine("-------MATRIX--------\n");
            Console.Write("\t");
            foreach (var recipient in Recipients)
            {
                Console.Write(recipient.AmountOfDemand + "\t");
            }
            Console.Write("\n");
            int i = 0;
            foreach (var provider in Providers)
            {
                Console.Write(provider.AmountOfSupply + "\t");
                foreach (var recipient in Recipients)
                {
                    Console.Write(recipient.UnitCosts[i] + "\t");
                }
                i++;
                Console.Write("\n");
            }
        }

        public List<Row> CreateRows()
        {
            List<Row> listOfRows = new List<Row>();
            Row row = new Row();
            row.ValueList.Add("");
            foreach (var recipient in Recipients)
            {
                row.ValueList.Add("Odbiorca: " + recipient.Id + "\nPopyt: " + recipient.AmountOfDemand.ToString());
            }
            listOfRows.Add(row);
            int i = 0;
            foreach (var provider in Providers)
            {
                row = new Row();
                row.ValueList.Add("Dostawca: " + provider.Id + "\nPodaż: " + provider.AmountOfSupply.ToString());
                foreach (var recipient in Recipients)
                {
                    row.ValueList.Add("Koszt: " + recipient.UnitCosts[i].ToString() + "   [  ]");
                }
                listOfRows.Add(row);
                i++;
            }
            foreach (var VARIABLE in listOfRows)
            {
                foreach (var value in VARIABLE.ValueList)
                {
                    Console.Write(value + " ");
                }
                Console.WriteLine("");
            }
            return listOfRows;
        }
    }
}
