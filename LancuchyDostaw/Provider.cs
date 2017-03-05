using System.ComponentModel;

namespace LancuchyDostaw
{
    //dostawca
    class Provider
    {
        [DisplayName("Amount of Supply"), Category("General"), ReadOnly(true)]
        public int AmountOfSupply { get; }

        [DisplayName("ID"), Category("General"), ReadOnly(true)]
        public int Id { get; }

        public Provider(int id, int demand)
        {
            AmountOfSupply = demand;
            Id = id;
        }

        public override string ToString()
        {
            return "Id dostawcy: " + Id +
                "\nDostępna podaż:" + AmountOfSupply +
                "\n-------------";
        }
    }
}
