using System.Collections.Generic;
using System.Collections;

namespace EventManager
{
public class EventManager() : Monobehaviour
{
	public bool LimitQueueProcesing = false;
	public float QueueProcessTime = 0.0f;

	private static EventManager _instance = null;
	private Queue _eventQueue = new Queue();

  public delegate void EvntDelegate<T> (T e) where T : GameEvent;
  private delegate void EventDelegate (GameEvent e);
  
  private var _delegates = new Dictionary<System.Type, EventDelegate>();
  private var _delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
  private var _onceLookups = new Dictionary<System.Delegate, System.Delegate>();

  public static EventManager Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = GameObject.FindObjectByType(typeof(EventManager)) as EventManager;
      }
      return _instance;
    }
  }

  private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent 
  {
    // Early out if we've already registered this delegate
    if (_delegateLookup.ContainsKey(del))
      return null;

    //Create a new non-generic delegate which calls our generic one.
    // This is the delegate we actually invoke
    EventDelegate internalDelegate = (e) => del((T) e);
    _delegateLookup[del] = internalDelegate;

    EventDelegate tempDel;
    if (_delegates.TryGetValue(typeof(T), out tempDel))
    {
      delegates[typeof(T)] = tempDel += internalDelegate;
    }
    else
    {
      delegates[typeof(T)] = internalDelegate;
    }

    return internalDelegate;
  }
}

}
