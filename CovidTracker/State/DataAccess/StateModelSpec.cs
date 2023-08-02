using System.Runtime.CompilerServices;
using CovidTracker.State.Models;

namespace CovidTracker.State.DataAccess
{
    /***
     * In a larger application this would implement an ISpecification interface. 
     */
    public class StateModelSpec
    {
        // Normally these would be private, but in this case, since the collection of all dates for all states takes
        // so long to load, the repo needs access to them to make individual calls.
        public readonly ICollection<string> States;
        public readonly string? Date;

        public StateModelSpec(ICollection<string> states, string? date = null) 
        {
            this.States = states;
            this.Date = date;
        }

        public bool HasDate()
        {
            return !string.IsNullOrEmpty(this.Date);
        }

        public bool IsSatisfiedBy(StateModel state)
        {
            if(HasDate())
            {
                return (!States.Any() || States.Contains(state.state)) && Date.Equals(state.DateString);
            }
            return !States.Any() || States.Contains(state.state);
        }
    }
}
