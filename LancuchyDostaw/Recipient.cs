using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancuchyDostaw
{
    // odbiorca
    class Recipient
    {
        public int AmountOfDemand { get; }
        public int Id { get; }
        public List<int> UnitCosts { get; set; }
        public Dictionary<Provider, int> SupplyFromProviderDictionary;

        public Recipient(int id, int demand)
        {
            Id = id;
            AmountOfDemand = demand;
            UnitCosts = new List<int>();
        }

        public override string ToString()
        {
            String UnitCostsStr = "";
            foreach (var cost in UnitCosts)
            {
                UnitCostsStr += cost + ", ";
            }
            return "Id odbiorcy: " + Id +
                "\nAktualny popyt:" + AmountOfDemand +
                "\nKoszty jednostkowe: " + UnitCostsStr +
                "\n-------------";
        }
    }
}
