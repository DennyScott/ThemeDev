using UnityEngine;

namespace Zephyr.EventSystem.Core
{
  public class GameEvent
  {
    public string Name;
    public GameEvent()
    {
      Name = this.GetType().Name;
    }
  }
}