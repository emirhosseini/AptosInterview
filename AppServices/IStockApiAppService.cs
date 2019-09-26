namespace AppServices
{
    using System.Threading.Tasks;
    using Models;
    public interface IStockApiAppService
    {
        Task<StockResponse> GetStockPrice(string stockSymbol);
    }
}