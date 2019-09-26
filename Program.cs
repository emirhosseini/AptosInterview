using System;
using Models;
using AppServices;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AptosInterview
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create service collection and configure our services
            var services = ConfigureServices();
            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();

            // Kick off our actual code
            await serviceProvider.GetService<ConsoleApplication>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            //This would come from .net core options
            var stockApiUri = "https://www.alphavantage.co/";

            IServiceCollection services = new ServiceCollection();
            services.AddTransient<IStockApiAppService, StockApiAppService>();
            services.AddHttpClient<IStockApiAppService, StockApiAppService>(client =>
            {
                client.BaseAddress = new Uri(stockApiUri);
            });
            services.AddTransient<ConsoleApplication>();
            return services;
        }
    }

    public class ConsoleApplication
    {
        private readonly IStockApiAppService _stockApiService;
        public ConsoleApplication(IStockApiAppService stockApiService)
        {
            _stockApiService = stockApiService;
        }

        // Application starting point
        public async Task Run()
        {
            string todaysStockDate = DateTime.Now.ToString("yyyy-MM-dd");
            var currentPortfolio = new Portfolio();

            var applePrice = (await _stockApiService.GetStockPrice("AAPL")).TimeSeriesDaily[todaysStockDate].The4Close;
            var googlePrice = (await _stockApiService.GetStockPrice("GOOG")).TimeSeriesDaily[todaysStockDate].The4Close;
            var cybrPrice = (await _stockApiService.GetStockPrice("CYBR")).TimeSeriesDaily[todaysStockDate].The4Close;
            var abbPrice = (await _stockApiService.GetStockPrice("ABB")).TimeSeriesDaily[todaysStockDate].The4Close;

            var gfnPrice = (await _stockApiService.GetStockPrice("GFN")).TimeSeriesDaily[todaysStockDate].The4Close;
            
            //Sleeping here because apparently only 5 calls are allowed per minute
            System.Threading.Thread.Sleep(61000);            
            var acadPrice = (await _stockApiService.GetStockPrice("ACAD")).TimeSeriesDaily[todaysStockDate].The4Close;

            currentPortfolio.AddStockToPortfolio("AAPL", applePrice, 50);
            currentPortfolio.AddStockToPortfolio("GOOG", googlePrice, 200);
            currentPortfolio.AddStockToPortfolio("CYBR", cybrPrice, 150);
            currentPortfolio.AddStockToPortfolio("ABB", abbPrice, 900);

            var desiredAppleStockTotal = (int) currentPortfolio.TotalPortfolioValue * (decimal)0.22;
            var desiredGoogleStockTotal = (int) currentPortfolio.TotalPortfolioValue * (decimal)0.38;
            var desiredGfnStockTotal = (int) currentPortfolio.TotalPortfolioValue * (decimal)0.25;
            var desiredAcadStockTotal = (int) currentPortfolio.TotalPortfolioValue * (decimal)0.15;

            var numberOfSharesToBuyApple = (int)(desiredAppleStockTotal / applePrice);
            var numberOfSharesToBuyGoogle = (int)(desiredGoogleStockTotal / googlePrice);
            var numberOfSharesToBuyGfn = (int)(desiredGfnStockTotal / gfnPrice);
            var numberOfSharesToBuyAcad = (int)(desiredAcadStockTotal / acadPrice);

            var newAllocationApple = (int)((desiredAppleStockTotal / currentPortfolio.TotalPortfolioValue) * 100);
            var newAllocationGoogle = (int)((desiredGoogleStockTotal / currentPortfolio.TotalPortfolioValue) * 100);
            var newAllocationGfn = (int)((desiredGfnStockTotal / currentPortfolio.TotalPortfolioValue) * 100);
            var newAllocationAcad = (int)((desiredAcadStockTotal / currentPortfolio.TotalPortfolioValue) * 100);

            string output = $@"
            Today's Prices:
            AAPL: {applePrice}
            GOOG: {googlePrice}
            CYBR: {cybrPrice}
            ABB: {abbPrice}
            GFN: {gfnPrice}
            ACAD: {acadPrice}

            Current Total Portfolio Value: {currentPortfolio.TotalPortfolioValue}

            Desired Dollar amounts per stock:
            Apple: {desiredAppleStockTotal}
            Google: {desiredGoogleStockTotal}
            GFN: {desiredGfnStockTotal}
            ACAD: {desiredAcadStockTotal}

            Number of shares to buy for each stock:
            Apple: {numberOfSharesToBuyApple}
            Google: {numberOfSharesToBuyGoogle}
            GFN: {numberOfSharesToBuyGfn}
            ACAD: {numberOfSharesToBuyAcad}

            Portfolio allocation per stock:
            Apple: {newAllocationApple}
            Google: {newAllocationGoogle}
            GFN: {newAllocationGfn}
            ACAD: {newAllocationAcad}
            ";


            Console.WriteLine(output);
        }
    }
}
