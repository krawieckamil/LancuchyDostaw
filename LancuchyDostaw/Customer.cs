using System;
using System.Collections.Generic;

namespace LancuchyDostaw
{
    // odbiorca
    public class Customer
    {
        public double Demand { get;}
        public bool IsFake { get; set; }
        public bool IsPrivileged { get; set; }
        public double SellingPrice { get; set; }
        public List<TransportConnection> Connections { get; set; }

        public Customer(double demand, double sellingPrice)
        {
            SellingPrice = sellingPrice;
            Demand = demand;
            Connections = new List<TransportConnection>();
        }

        public void addConnection(TransportConnection c)
        {
            if(!Connections.Contains(c))
                Connections.Add(c);
        }
    }
}
