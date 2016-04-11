using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime
{
    public class MultiUnitSchedule : ISchedule
    {
        private List<int> _scheduledUnits;
        private int _flushDate;

        public MultiUnitSchedule(List<int> units)
        {
            _flushDate = units[0];
            for (var i = 0; i < units.Count; i++)
            {
                if (units[i] > _flushDate)
                {
                    _flushDate = units[i];
                }
            }

            _scheduledUnits = units;
        }

        public bool HasUnit(int unit)
        {
            return _scheduledUnits.Exists(e => e == unit);
        }

        public bool IsFlushDate(int unit)
        {
            return unit == _flushDate;
        }
    }
}
