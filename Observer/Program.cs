var reader = new BitcoinPriceReader();

var emailNotifier = new EmailPriceChangeNotifier(5000);
var pushNotifier = new PushPriceChangeNotifier(25000); 

reader.SubscribersEvent += emailNotifier.Update;
reader.SubscribersEvent += pushNotifier.Update;

Enumerable.Repeat(0,10).ToList().ForEach(x => reader.ReadCurrentPrice());

void SomeMethod(decimal number)
{
    Console.WriteLine(number);
}

class BitcoinPriceReader()
{
    private decimal _currentBitcoinPrice;
    public event EventHandler<PriceReadEventArgs>? SubscribersEvent;
    
    public void ReadCurrentPrice()
    {
        _currentBitcoinPrice = new Random().Next(0, 50000);
        NotifySubscribes(_currentBitcoinPrice);
    }
    public void NotifySubscribes(decimal data)
    {
        SubscribersEvent?.Invoke(this, new PriceReadEventArgs(_currentBitcoinPrice));
    }
}

class PriceReadEventArgs : EventArgs
{
    public decimal Price { get; }

    public PriceReadEventArgs(decimal price) => Price = price;
}

class EmailPriceChangeNotifier(decimal notificationThreshold)
{
    public void Update(object? sender, PriceReadEventArgs eventArgs)
    {
        if (eventArgs.Price > notificationThreshold)
        {
            Console.WriteLine($"Sending mail, threshold = {notificationThreshold}," +
                              $" price = {eventArgs.Price} ");
        }
    }
}
class PushPriceChangeNotifier(decimal notificationThreshold)
{
    public void Update(object? sender, PriceReadEventArgs eventArgs)
    {
        if (eventArgs.Price > notificationThreshold)
        {
            Console.WriteLine($"Sending a push notification, threshold = {notificationThreshold}" +
                              $", price = {eventArgs.Price} ");
        }
    }
}