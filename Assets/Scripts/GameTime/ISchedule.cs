namespace GameTime
{
    /// <summary>
    /// Interface for schedule.
    /// </summary>
    public interface ISchedule
    {
        bool IsScheduledDate(int unit);
        bool IsFlushDate(int unit);
    }
}
