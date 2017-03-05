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
                    string[] recipientsArr = recipientsRaw.Split(' ');
                    int i = 0;
                    foreach (var recipient in recipientsArr)
                    {
                        recipients.Add(new Recipient(i, Int32.Parse(recipient)));
                        i++;
                    }

                    string providersRaw = reader.ReadLine();
                    if (providersRaw != null)
                    {
                        string[] providersArr = providersRaw.Split(' ');
                        i = 0;
                        foreach (var provider in providersArr)
                        {
                            providers.Add(new Provider(i, Int32.Parse(provider)));
                            i++;
                        }
                    }
                }

                for (int i = 0; i < providers.Count; i++)
                {
                    string unitCostsRaw = reader.ReadLine();
                    if (unitCostsRaw != null)
                    {
                        string[] unitCosts = unitCostsRaw.Split(' ');
                        int j = 0;
                        foreach (var recipient in recipients)
                        {
                            recipient.UnitCosts.Add(Int32.Parse(unitCosts[j]));
                            j++;
                        }
                    }
                }
            }

            //return controlPoints;
        }
    }
}
