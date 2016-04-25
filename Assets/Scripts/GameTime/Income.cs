using System.Collections.Generic;
using InGameConsole;

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
            DirectorActionConsole.Instance.WriteActionToConsole("Income", "Make it Rain!");
        }
    }
}
