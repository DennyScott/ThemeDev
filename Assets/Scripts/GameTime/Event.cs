using System.Collections.Generic;

namespace GameTime
{
    /// <summary>
    /// Abstract class event, used in the GameTime event loop.
    /// </summary>
    public abstract class Event : IEvent
    {
        #region Private Fields
        private ISchedule _scheduledUnit;
        #endregion

        #region Contructors

        /// <summary>
        /// Constructor for event. This constuctor creates a single scheduled date. Schedules the event at the date depending on the manualDate.
        /// </summary>
        /// <param name="unit">The unit to schedule this event for.</param>
        /// <param name="manualDate">if true, will schedule the event for the passed day, and if false, schedules it for the amount of units from current day</param>
        protected Event(int unit, bool manualDate = false)
        {
            unit = CalculateUnit(unit, manualDate); 
            _scheduledUnit = new SingleUnitSchedule(unit);
        }

        /// <summary>
        /// Constructor for event. This constuctor creates multiple scheduled dates. Schedules the events at the date depending on the manualDate.
        /// </summary>
        /// <param name="units">A list of scheduled dates</param>
        /// <param name="manualDate">if true, will schedule the dates for the passed day, and if false, schedules it for the amount of units from current day</param>
        protected Event(List<int> units, bool manualDate = false)
        {
            // For each unit in the units list...
            for (var i = 0; i < units.Count; i++)
                // ...Calculate the date of the unit, and place it back into the same position of the units list.
                units[i] = CalculateUnit(units[i], manualDate);

            // Creates a MultiUnitSchedule object with the updates units list.
            _scheduledUnit = new MultiUnitSchedule(units);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Abstract method, triggered when this object hits its scheduled date(s).
        /// </summary>
        public abstract void TriggerEvent();

        #endregion

        #region Public Methods
        /// <summary>
        /// Checks to see if the passed unit is a scheduled date of the event
        /// </summary>
        /// <param name="unit">The unit to check is a scheduled date of this event</param>
        /// <returns>true if the unit is a scheduled date of this event, false otherwise</returns>
        public bool IsScheduledDate(int unit)
        {
            return _scheduledUnit.IsScheduledDate(unit);
        }

        /// <summary>
        /// Checks to see if the passed unit is the flushed date of this event
        /// </summary>
        /// <param name="unit">The unit to check is the flush date</param>
        /// <returns>True is the unit is the flush date, false otherwise</returns>
        public bool IsFlushedDate(int unit)
        {
            return _scheduledUnit.IsFlushDate(unit);
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// Calculates the scheduled unit based on the manual date.
        /// </summary>
        /// <param name="unit">The unit to schedule for the scheduled date</param>
        /// <param name="manualDate">If true, the unit passed will be the scheduled date, if false, it will be the amount of units ahead of the current date</param>
        /// <returns>The unit that will be the scheduled date</returns>
        private int CalculateUnit(int unit, bool manualDate)
        {
            return manualDate ? unit : GameTime.CurrentUnit + unit;
        }

        #endregion
    }
}
