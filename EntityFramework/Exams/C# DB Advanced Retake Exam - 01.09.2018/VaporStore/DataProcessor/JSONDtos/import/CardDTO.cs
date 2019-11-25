namespace VaporStore.DataProcessor.JSONDtos.import
{
    using VaporStore.Data.enums;

    public class CardDTO
    {
        public string Number { get; set; }

        public string CVC { get; set; }

        public CardType Type { get; set; }
    }
}
