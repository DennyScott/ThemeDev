using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace GameTime
{
    public interface ISchedule
    {
        bool HasUnit(int unit);
        bool IsFlushDate(int unit);
    }
}
