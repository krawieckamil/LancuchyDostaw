using System;
using System.Collections.Generic;

namespace LancuchyDostaw
{
    // odbiorca
    class Recipient
    {
        public int AmountOfDemand { get;}
        public int ActualAmountOfDemand { get; set; }
        public int Id { get; }
        public List<ProviderRecipientConnector> ProviderIdUnitCosts { get; set; }
        public int Beta { get; set; }

        public Recipient(int id, int demand)
        {
            Id = id;
            AmountOfDemand = demand;
            ActualAmountOfDemand = AmountOfDemand;
            ProviderIdUnitCosts = new List<ProviderRecipientConnector>();
            Beta = -9999;
        }

        public override string ToString()
        {
            String unitCostsStr = "";
            
            foreach (var cost in ProviderIdUnitCosts)
            {
                unitCostsStr += "Dostawca: " + cost.providerId + "Koszt: " + cost.unitCost + ", ";
            }
            return "Id odbiorcy: " + Id +
                "\nAktualny popyt:" + AmountOfDemand +
                "\nKoszty jednostkowe: " + unitCostsStr +
                "\n-------------";
        }
    }
}
