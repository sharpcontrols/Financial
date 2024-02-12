namespace SharpControls.Financial.Models.Financial
{
    public class FinancialModel
    {
        public uint Id { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Description { get; set; }
        public Dictionary<DateTime, uint> Parcels { get; set; } = [];
        public uint TotalValue => GetTotalValue();
        public DateTime ReferenceDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PayDay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private uint GetTotalValue()
        {
            uint value = 0;
            foreach (KeyValuePair<DateTime, uint> parcelPair in Parcels)
            {
                value += parcelPair.Value;
            }
            return value;
        }

        public void AddParcel(DateTime dateTime, uint value)
        {
            Parcels.Add(dateTime, value);
        }

        public void RemoveParcel(DateTime dateTime)
        {
            Parcels.Remove(dateTime);
        }
    }
}
