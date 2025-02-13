using System;

public class CounterState
{
    private int currentCount = 0;

    public int CurrentCount => currentCount;

    public void IncrementCount()
    {
        currentCount++;
        NotifyStateChanged();
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
} 