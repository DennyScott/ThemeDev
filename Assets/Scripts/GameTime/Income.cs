using System.Collections.Generic;
using UnityEngine;

namespace GameTime
{
    public class Income : Event
    {
        public Income(int date):base(date)
        {

        }

        public Income(List<int> dates) : base(dates)
        {
            
        }

        public override void TriggerEvent()
        {
            Debug.Log("Make some money!");
        }
    }
}
