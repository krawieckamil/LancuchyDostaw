using System;
using System.Collections.Generic;
using System.IO;

namespace LancuchyDostaw
{
    class FileHandler
    {
        public static void LoadData(string path, List<Provider> providers, List<Recipient> recipients)
        {
            using (TextReader reader = File.OpenText(path))
            {
                string recipientsRaw = reader.ReadLine();
                if (recipientsRaw != null)
                {
                    int i = 0;
                    string providersRaw = reader.ReadLine();
                    if (providersRaw != null)
                    {
                        string[] providersArr = providersRaw.Split(' ');
                        i = 0;
                        foreach (var provider in providersArr)
                        {
                            if(i == 0 ) providers.Add(new Provider(i, Int32.Parse(provider), 0));
                            else providers.Add(new Provider(i, Int32.Parse(provider)));
                            i++;
                        }
                    }
                    i = 0;
                    string[] recipientsArr = recipientsRaw.Split(' ');
                    foreach (var recipient in recipientsArr)
                    {
                        recipients.Add(new Recipient(i, Int32.Parse(recipient)));
                        i++;
                    }
                }

                for (int i = 0; i < providers.Count; i++)
                {
                    string unitCostsRaw = reader.ReadLine();
                    if (unitCostsRaw != null)
                    {
                        string[] unitCosts = unitCostsRaw.Split(' ');
                        for (var j = 0; j < recipients.Count; j++)
                        {
                            var recipient = recipients[j];
                            recipient.ProviderIdUnitCosts.Add(new ProviderRecipientConnector(i, j, Int32.Parse(unitCosts[j])));
                        }
                    }
                }
            }

            //return controlPoints;
        }
    }
}
