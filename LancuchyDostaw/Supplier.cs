using System.Collections.Generic;

namespace LancuchyDostaw
{
    //dostawca
    public class Supplier
    {
        public double Supply { get; }
        public List<TransportConnection> Connections { get; set; }
        public int Id { get; }
        public bool IsFake { get; set; }
        public bool IsPrivileged { get; set; }
        public double PurchasePrice { get; set; }

        public Supplier(double supply, double purchasePrice)
        {
            Supply = supply;
            PurchasePrice = purchasePrice;
        }

        public override string ToString()
        {
            return "Id dostawcy: " + Id +
                "\nDostępna podaż:" + Supply +
                "\n-------------";
        }

        public void addConnection(TransportConnection c)
        {
            if (!Connections.Contains(c))
            {
                Connections.Add(c);
            }
        }
    }
}
