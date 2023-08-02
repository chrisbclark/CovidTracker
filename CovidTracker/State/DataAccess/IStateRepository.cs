using CovidTracker.State.Models;

namespace CovidTracker.State.DataAccess
{
    /**
     * Interface to access state COVID data.
     */
    public interface IStateRepository
    {
        public Task<IEnumerable<StateModel>> GetAll();
        public Task<IEnumerable<StateModel>> Get(StateModelSpec stateModelSpec);
    }
}
