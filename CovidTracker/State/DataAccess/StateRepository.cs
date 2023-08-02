using CovidTracker.State.Models;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using System.Linq;
using System.Collections.Immutable;

namespace CovidTracker.State.DataAccess
{
    /**
     * Http backed implementation of COVID data access.
     * 
     * One of the challenges for implementing this is that the API to fetch daily data does not filter by date
     * without also filtering for a specific state.  It seemed to be a choice 
     * between loading the entire huge document into memory for all states and dates and making 50+ separate calls for 
     * individual states any time you filter only by date.  After testing I found making the individual calls was
     * much faster.
     * 
     * The HttpClient is wrapped in a class so the methods can be mocked.
     */
    public class StateRepository : IStateRepository
    {
        private List<StateModel> States = new List<StateModel>();
        private HttpClientWrapper Client = new HttpClientWrapper();

        public async Task Initialize()
        {
            StateModel[]? stateArray = await Client.LoadData("https://api.covidtracking.com/v1/states/current.json");
            if (stateArray != null)
            {
                States.AddRange(stateArray);
            }

        }

        /**
         * Method to inject a custom HTTPClient for testing purposes.
         */
        public void InjectClient(HttpClientWrapper client)
        {
            this.Client = client;
        }

        /**
         * Load data according to a given spec.  It's not optimal that this method has to look at
         * the spec data, but given performance of the available APIs, different endpoints need to be
         * called depending on whether there's a date specified.  Because there is no endpoint that
         * returns a single date for all states, we are making a call for each selected state, because
         * it is much faster than fetching the whole historical data by states document.
         */
        public async Task <IEnumerable<StateModel>> Get(StateModelSpec stateModelSpec)
        {
            if(!States.Any())
            {
                throw new NotInitializedException("Repository data has not been initialized");
            }
            if (stateModelSpec.HasDate())
            {
                List<StateModel> statesByDate = new List<StateModel>();
                ICollection<string> stateCodes = stateModelSpec.States.Any() ? stateModelSpec.States :
                    StateCode.States.Keys.ToList();
                try
                { 
                    foreach(var state in stateCodes)
                    {
                        StateModel? stateModel = await Client.LoadSingle($"https://api.covidtracking.com/v1/states/{state.ToLower()}/{stateModelSpec.Date}.json");
                        if (stateModel != null)
                        {
                            statesByDate.Add(stateModel);
                        }

                    }
                }
                catch (Exception ex)
                {
                    // Logging library out of scope so just writing to the console.
                    Console.WriteLine(ex.Message);
                    throw;
                }
                return statesByDate;

            }
            else
            {
                return States.Where((state) => stateModelSpec.IsSatisfiedBy(state));
            }
        }

        public IEnumerable<StateModel> GetAll()
        {
            if(!States.Any())
            {
                throw new NotInitializedException("Repository data has not been initialized");
            }
            return States;
        }
    }
}
