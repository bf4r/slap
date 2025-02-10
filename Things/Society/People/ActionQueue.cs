namespace slap.Things.Society.People;

public partial class Person : Thing
{
    private List<(Action action, TimeSpan duration)> _queuedActions = new();
    private (DateTime time, TimeSpan duration) _lastAction;
    private void DoQueuedActions()
    {
        if (_queuedActions.Count == 0) return;
        if (_lastAction.time + _lastAction.duration <= Sim.Now)
        {
            var nextAction = _queuedActions[0];
            try
            {
                nextAction.action();
                _lastAction = (Sim.Now, nextAction.duration);
            }
            finally
            {
                _queuedActions.RemoveAt(0);
            }
        }
    }
    public void Do(Action action, TimeSpan? takes = null)
    {
        takes ??= TimeSpan.Zero;

        // Check if the person is currently free:
        if (_lastAction.time + _lastAction.duration <= Sim.Now)
        {
            // If the person is free, execute the action immediately.
            action();
            _lastAction = (Sim.Now, takes.Value);
        }
        else
        {
            // If the person is busy, add the action to the queue and it will be executed when free.
            _queuedActions.Add((action, takes.Value));
        }
    }
}
