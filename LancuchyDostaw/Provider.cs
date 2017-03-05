namespace LancuchyDostaw
{
    //dostawca
    class Provider
    {
        private int v;

        public int AmountOfSupply { get; }
        public int ActualAmountOfSupply { get; set; }
        public int Id { get; }
        public int Alpha { get; set; }

        public Provider(int id, int demand)
        {
            AmountOfSupply = demand;
            ActualAmountOfSupply = AmountOfSupply;
            Id = id;
            Alpha = -9999;
        }

        public Provider(int id, int demand, int alpha) : this(id, demand)
        {
            this.Alpha = alpha;
        }

        public override string ToString()
        {
            return "Id dostawcy: " + Id +
                "\nDostępna podaż:" + AmountOfSupply +
                "\n-------------";
        }
    }
}
