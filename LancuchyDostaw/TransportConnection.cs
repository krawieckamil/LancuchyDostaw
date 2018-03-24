namespace LancuchyDostaw
{
    public class TransportConnection
    {
        public Supplier Supplier;
        public Customer Customer;
        public double TransportCost;
        public bool Blocked = false;
        public double Amount = 0.0;
        public int X;
        public int Y;

        public TransportConnection(Supplier supplier, Customer customer, double cost, int x, int y)
        {
            Customer = customer;
            Supplier = supplier;
            TransportCost = cost;
            X = x;
            Y = y;
        }

        public double TotalProfit()
        {
            return Amount * UnitProfit();
        }

        public double UnitProfit()
        {
            return Customer.SellingPrice - Supplier.PurchasePrice - TransportCost;
        }
    }
}