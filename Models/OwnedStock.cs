namespace Models
{
    public class OwnedStock
    {
        public Stock StockOwned { get; private set; }
        public int NumberOwned { get; private set; }

        public decimal TotalDollarAmountOwned => StockOwned.Price * NumberOwned;

        public void GainStockOwnership(string stockSymbol, decimal price, int numberOwned)
        {
            StockOwned = new Stock();
            StockOwned.SetStock(stockSymbol, price);
            NumberOwned = numberOwned;
        }
    }
}