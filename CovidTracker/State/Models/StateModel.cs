namespace CovidTracker.State.Models
{
    /**
     * Data model to contain data about COVID rates in a given state.
     */
    public class StateModel : IComparable<StateModel>
    {

        public string state { get; set; } = string.Empty;

        public string Name => StateCode.StateNameFromCode(state);
        public int? positive { get; set; }

        public int? negative { get; set; }
        public int? totalTestResults { get; set; }

        public double? PositiveRate => positive * 100.0 / totalTestResults;

        public int? hospitalizedCurrently { get; set; }

        public string dateChecked { get; set; } = string.Empty;

        public string DateString => string.IsNullOrEmpty(dateChecked) ? "Unavailable" : 
            DateTime.Parse(dateChecked).ToShortDateString();

        /**
         * Default ordering by positivity rate descending.
         */
        public int CompareTo(StateModel? other)
        {
            if (other == null) return -1;
            if (other.PositiveRate > this.PositiveRate) return 1;
            if (other.PositiveRate < this.PositiveRate) return -1;
            return 0;
        }
    }
}
