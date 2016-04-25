using System.Collections.Generic;

namespace GameTime
{
    /// <summary>
    /// Contains multuple scheduled dates for an event.  Will be flushed on the latest date.
    /// </summary>
    public class MultiUnitSchedule : ISchedule
    {

        #region Private Fields
        // List of scheduled units
        private List<int> _scheduledUnits;

        // The flush date of this object
        private int _flushDate;

        #endregion

        #region Constructor
        public MultiUnitSchedule(List<int> units)
        {
            // If units is passed as null...
            if (units == null)
                // ...then throw a new argument null exception.
                throw new System.ArgumentNullException("units", "Passed into a MultiUnitSchedule constructor as null");

            // Gets the latest date in the units list as the flush date
            _flushDate = GetLatestDate(units);

            // scheduledunits is the passed unit list
            _scheduledUnits = units;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Check to see if the passed unit is a scheduled date in the list of dates for this object
        /// </summary>
        /// <param name="unit">The unit to check is a unit in the scheduled units</param>
        /// <returns>True if the unit is a date in the scheduled units list</returns>
        public bool IsScheduledDate(int unit)
        {
            return _scheduledUnits.Contains(unit);
        }

        /// <summary>
        /// Checks to see if the passed unit is the flush date of this object
        /// </summary>
        /// <param name="unit">The unit to check is the flush date unit</param>
        /// <returns>True if the flush date matches the passed unit</returns>
        public bool IsFlushDate(int unit)
        {
            return unit == _flushDate;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the latest day from the list of units
        /// </summary>
        /// <param name="units">A list of units</param>
        /// <returns>The latest unit in the list of int units</returns>
        private int GetLatestDate(List<int> units) {
            // If units is passed as null...
            if (units == null)
                // ...then throw a new argument null exception.
                throw new System.ArgumentNullException("units", "Passed into a MultiUnitSchedule constructor as null");

            // Gets the 0th list item as a base case.
            var latestDate = units[0];

            // For each item in the units list...
            for (var i = 0; i < units.Count; i++) {
                // ...If the unit at the ith position is a larger value then the latest date variable...
                if (units[i] > latestDate) {
                    // ...Then set the latest date variable to that value.
                    latestDate = units[i];
                }
            }

            //Return the latest date
            return latestDate;
        }

        #endregion
    }
}
