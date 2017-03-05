using System.Collections.Generic;
using System.Linq;

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

            //Utils.PrintMatrix(Providers, Recipients);
            //Utils.PrintDetailedData(Providers, Recipients);
        }

        public List<Row> CreateRows()
        {
            List<Row> listOfRows = new List<Row>();
            Row row = new Row();
            row.ValueList.Add("");
            foreach (var recipient in Recipients)
            {
                row.ValueList.Add("Odbiorca: " + recipient.Id + "\nAktualny popyt: " + recipient.ActualAmountOfDemand + " (" + recipient.AmountOfDemand + ")");
            }
            row.ValueList.Add("");
            listOfRows.Add(row);
            int i = 0;
            foreach (var provider in Providers)
            {
                row = new Row();
                row.ValueList.Add("Dostawca: " + provider.Id + "\nPodaż: " + provider.ActualAmountOfSupply + " (" + provider.AmountOfSupply + ")");
                foreach (var recipient in Recipients)
                {
                    string isIncluded = recipient.ProviderIdUnitCosts[i].currentExchange == 0 ? "[ X ]" : "[  ]";
                    row.ValueList.Add("Koszt: " + recipient.ProviderIdUnitCosts[i].unitCost + " (" + recipient.ProviderIdUnitCosts[i].currentExchange + ") " + isIncluded);
                }
                row.ValueList.Add("Alpha: " + provider.Alpha);
                listOfRows.Add(row);
                i++;
            }
            row = new Row();
            row.ValueList.Add("");
            foreach (var recipient in Recipients)
            {
                row.ValueList.Add("Beta: " + recipient.Beta);
            }
            row.ValueList.Add("");
            listOfRows.Add(row);
            return listOfRows;
        }

        public void Calculate()
        {
            List<ProviderRecipientConnector> allRecipientUnitCost = new List<ProviderRecipientConnector>();
            foreach (var recipient in Recipients)
            {
                for (var index = 0; index < recipient.ProviderIdUnitCosts.Count; index++)
                {
                    allRecipientUnitCost.Add(recipient.ProviderIdUnitCosts[index]);
                }
            }
            List<ProviderRecipientConnector> allRecipientUnitCostsSorted = allRecipientUnitCost.OrderBy(x => x.unitCost).ToList();

            foreach (var entry in allRecipientUnitCostsSorted)
            {
                Provider cheapestProvider = Providers.Find(x => x.Id == entry.providerId);
                Recipient recipient = Recipients.Find(x => x.Id == entry.recipientId);
                if (cheapestProvider.ActualAmountOfSupply > 0 && recipient.ActualAmountOfDemand > 0)
                {
                    if (cheapestProvider.ActualAmountOfSupply < recipient.ActualAmountOfDemand)
                    {
                        recipient.ActualAmountOfDemand = recipient.ActualAmountOfDemand - cheapestProvider.ActualAmountOfSupply;
                        recipient.ProviderIdUnitCosts.Find(x => x.providerId == cheapestProvider.Id).currentExchange = cheapestProvider.ActualAmountOfSupply;
                        cheapestProvider.ActualAmountOfSupply = 0;
                    }
                    else
                    {
                        cheapestProvider.ActualAmountOfSupply = cheapestProvider.ActualAmountOfSupply - recipient.ActualAmountOfDemand;
                        recipient.ProviderIdUnitCosts.Find(x => x.providerId == cheapestProvider.Id).currentExchange = recipient.ActualAmountOfDemand;
                        recipient.ActualAmountOfDemand = 0;
                    }
                }
            }
            CalculateAlphaAndBeta();
        }

        private void CalculateAlphaAndBeta()
        {
            for (var index = 0; index < Providers.Count; index++)
            {
                var provider = Providers[index];
                List<ProviderRecipientConnector> foundRecipientsConnectors = new List<ProviderRecipientConnector>();
                foreach (var recipient in Recipients)
                {
                    foundRecipientsConnectors.Add(recipient.ProviderIdUnitCosts.Find(x => x.providerId == index && x.currentExchange > 0));
                }

                foreach (var bound in foundRecipientsConnectors)
                {
                    if (bound != null)
                    {
                        Recipient recipient = Recipients.Find(x => x.Id == bound.recipientId);
                        int alpha = provider.Alpha;
                        int unitCost = bound.unitCost;
                        if (alpha == 0)
                        {
                            recipient.Beta = unitCost - alpha;
                        }
                    }
                }
                //TODO podpiąc solver
            }
        }
    }
}
