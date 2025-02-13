namespace BlazorHelloWorld.Client.Services;

public class CounterState
{
    private int _currentCount = 0;
    public int CurrentCount => _currentCount;

    public event Action? OnChange;

    public void IncrementCount()
    {
        _currentCount++;
        OnChange?.Invoke();
    }
} 