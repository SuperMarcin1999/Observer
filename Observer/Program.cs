var reader = new BitcoinPriceReader();

var emailNotifier = new EmailPriceChangeNotifier(5000);
var pushNotifier = new PushPriceChangeNotifier(25000); 

reader.AddObserver(emailNotifier);
reader.AddObserver(pushNotifier);

Enumerable.Repeat(0,10).ToList().ForEach(x => reader.ReadCurrentPrice());


class BitcoinPriceReader() : IObserver<decimal>
{
    private decimal _currentBitcoinPrice;
    private readonly ICollection<IObservable<decimal>> _observers = new List<IObservable<decimal>>();
    
    public void ReadCurrentPrice()
    {
        _currentBitcoinPrice = new Random().Next(0, 50000);
        NotifyObservers(_currentBitcoinPrice);
    }
    public void NotifyObservers(decimal data)
    {
        foreach (var observer in _observers)
            observer.Update(data);
    }
    
    public void AddObserver(IObservable<decimal> observer) =>
        _observers.Add(observer);
    
    public void RemoveObserver(IObservable<decimal> observer) =>
        _observers.Remove(observer);
}

class EmailPriceChangeNotifier(decimal notificationThreshold) : IObservable<decimal>
{
    public void Update(decimal currentBitcoinPrice)
    {
        if (currentBitcoinPrice > notificationThreshold)
        {
            Console.WriteLine($"Sending mail, threshold = {notificationThreshold}, price = {currentBitcoinPrice} ");
        }
    }
}
class PushPriceChangeNotifier(decimal notificationThreshold) : IObservable<decimal>
{
    public void Update(decimal currentBitcoinPrice)
    {
        if (currentBitcoinPrice > notificationThreshold)
        {
            Console.WriteLine($"Sending a push notification, threshold = {notificationThreshold}, price = {currentBitcoinPrice} ");
        }
    }
}

interface IObserver<TData>
{
    public void NotifyObservers(TData data);
    public void AddObserver(IObservable<TData> observer);
    public void RemoveObserver(IObservable<TData> observer);
}

interface IObservable<TData>
{
    public void Update(TData data);
}