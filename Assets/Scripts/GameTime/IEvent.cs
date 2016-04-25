namespace GameTime
{
    /// <summary>
    /// Interface for the events.
    /// </summary>
    public interface IEvent
    {
        void TriggerEvent();
        bool IsScheduledDate(int unit);
        bool IsFlushedDate(int unit);
    }
}