namespace Models
{
    public class Stock
    {
        public string Symbol { get; private set; }
        public decimal Price { get; private set; }

        public void SetStock(string stockSymbol, decimal price)
        {
            Symbol = stockSymbol;
            Price = price;
        }
    }
}