namespace LancuchyDostaw
{
    public class ProviderRecipientConnector
    {
        public int providerId { get; set; }
        public int recipientId { get; set; }
        public int unitCost { get; set; }
        public int currentExchange { get; set; }

        public ProviderRecipientConnector(int providerId, int recipientId, int unitCost)
        {
            this.providerId = providerId;
            this.recipientId = recipientId;
            this.unitCost = unitCost;
            this.currentExchange = 0;
        }
    }
}