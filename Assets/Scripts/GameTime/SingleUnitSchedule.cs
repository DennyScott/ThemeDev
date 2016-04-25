namespace GameTime
{
    /// <summary>
    /// A single date scheduled unit.  Only contains one date, and then will be flushed on the same date.
    /// </summary>
    public class SingleUnitSchedule : ISchedule
    {
        #region Private Fields
        private int _scheduledUnit;
        #endregion

        #region Constructors
        /// <summary>
        /// Passed unit will be the _scheduledUnit of this instance
        /// </summary>
        /// <param name="unit">The unit that this will be scheduled for</param>
        public SingleUnitSchedule(int unit)
        {
            _scheduledUnit = unit;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Is true if the flush date is the passed unit
        /// </summary>
        /// <param name="unit">The unit to check is the flush date</param>
        /// <returns>True if the flush date is the passed unit, else, returns false.</returns>
        public bool IsFlushDate(int unit)
        {
            //Returns the scheduled date, as a singleUnitSchedule only has a single date.
            return IsScheduledDate(unit);
        }

        /// <summary>
        /// Is true if the scheduled unit of this is the passed unit
        /// </summary>
        /// <param name="unit">The unit to check if this is the current unit</param>
        /// <returns>True if the passed unit is the scheduled unit of this instance</returns>
        public bool IsScheduledDate(int unit)
        {
            return _scheduledUnit == unit;
        }

        #endregion
    }
}
