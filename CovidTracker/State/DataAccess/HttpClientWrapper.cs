using CovidTracker.State.Models;
using System.Net.Http.Json;


namespace CovidTracker.State.DataAccess
{
    /**
     * Wrapper class to be able to mock access to HttpClient for testing.
     */
    public class HttpClientWrapper
    {
        private static readonly HttpClient Client = new HttpClient();

        public virtual async Task<StateModel[]> LoadData(string url)
        {
            return await Client.GetFromJsonAsync<StateModel[]>(url);
        }

        public virtual async Task<StateModel> LoadSingle(string url)
        {
            return await Client.GetFromJsonAsync<StateModel>(url);
        }
    }
}
