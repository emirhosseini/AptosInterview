//DI is not wired up for this class due to time constraints
//This class would be in a separate project
using System.Net.Http;
using Newtonsoft.Json;
using Models;
using System.Threading.Tasks;

namespace AppServices
{
    public class StockApiAppService : IStockApiAppService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;

        public StockApiAppService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StockResponse> GetStockPrice(string stockSymbol)
        {
            var responseString = await _httpClient.GetStringAsync("query?apikey=BPYXEC00NPEI4YSY&function=TIME_SERIES_DAILY_ADJUSTED&symbol=" + stockSymbol);

            var stockResponse = JsonConvert.DeserializeObject<StockResponse>(responseString);
            return stockResponse;
        }
    }
}