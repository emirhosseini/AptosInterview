using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Portfolio
    {
        public List<OwnedStock> OwnedStocks { get; set; }
        public decimal TotalPortfolioValue => OwnedStocks.Sum(s => s.TotalDollarAmountOwned);
        public Portfolio()
        {
            OwnedStocks = new List<OwnedStock>();
        }

        public void AddStockToPortfolio(string stockSymbol, decimal price, int numberOwned)
        {
            var ownedStock = new OwnedStock();
            ownedStock.GainStockOwnership(stockSymbol, price, numberOwned);

            OwnedStocks.Add(ownedStock);
        }
    }
}