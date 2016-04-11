using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTime
{
    public class SingleUnitSchedule : ISchedule
    {
        public SingleUnitSchedule(int unit)
        {
            _scheduledUnit = unit;
        }

        private int _scheduledUnit;

        public bool HasUnit(int unit)
        {
            return _scheduledUnit == unit;
        }

        public bool IsFlushDate(int unit)
        {
            return unit == _scheduledUnit;
        }
    }
}
