using UnityEngine;
using System.Collections;

namespace GameTime
{
    public interface IEvent
    {
        void TriggerEvent();
        bool IsScheduled(int unit);
        bool IsFlushedUnit(int unit);
    }
}