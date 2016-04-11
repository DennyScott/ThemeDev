using System.Collections.Generic;
using UnityEngine;

namespace GameTime
{
    public abstract class Event : IEvent
    {
        protected Event(int unit, bool manualDate = false)
        {
            unit = CalculateUnit(unit, manualDate); 
            _scheduledUnit = new SingleUnitSchedule(unit);
        }

        protected Event(List<int> units, bool manualDate = false)
        {
            for (var i = 0; i < units.Count; i++)
                units[i] = CalculateUnit(units[i], manualDate);

            _scheduledUnit = new MultiUnitSchedule(units);
        }

        private ISchedule _scheduledUnit;

        public virtual void TriggerEvent()
        {
        }

        public bool IsScheduled(int unit)
        {
            return _scheduledUnit.HasUnit(unit);
        }

        public bool IsFlushedUnit(int unit)
        {
            return _scheduledUnit.IsFlushDate(unit);
        }

        private int CalculateUnit(int unit, bool manualDate)
        {
            return manualDate ? unit : GameTime.CurrentUnit + unit;
        }
    }
}
