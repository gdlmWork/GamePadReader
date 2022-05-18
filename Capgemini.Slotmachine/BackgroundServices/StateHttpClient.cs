using Capgemini.Slotmachine.Models;

namespace Capgemini.Slotmachine.BackgroundServices
{
    public class StateHttpClient
    {
        private HttpClient _httpClient;
        public StateHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async void  PostState(ButtonState state)
        {
            await _httpClient.PostAsJsonAsync<ButtonState>("/buttonstate", state);
        }
    }
}
