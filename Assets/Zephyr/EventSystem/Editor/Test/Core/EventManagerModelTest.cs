using NUnit.Framework;
using Zephyr.EventSystem.Core;

namespace Zephyr.EventSystem.Editor.Test.Core
{
  [TestFixture]
  [Category("EventManagerCore")]
  public class EventManagerModelTest
  {
    private EventManagerModel _manager;

    [SetUp]
    public void Init()
    {
      _manager = new EventManagerModel();
    }

    [Test]
    public void IsQueueInstantiatedEmpty()
    {
      Assert.AreEqual(0, _manager.QueueSize());
    }

    [Test]
    public void IsDelegatesEmptyOnStart()
    {
      Assert.IsTrue(_manager.IsDelegatesEmpty());
    }

    public void OnCall(GameEvent evt)
    {
    }

    [Test]
    public void DoesAddListenerAttachListener()
    {
      _manager.AddListener<GameEvent>(OnCall);
      Assert.IsTrue(_manager.HasListener<GameEvent>(OnCall));
    }

    [Test]
    public void DoesAddListenerAddToDelegates()
    {
      _manager.AddListener<GameEvent>(OnCall);
      Assert.IsFalse(_manager.IsDelegatesEmpty());
    }

    [Test]
    public void DoesAddListenerOnceAttachListener()
    {
      _manager.AddListenerOnce<GameEvent>(OnCall);
      Assert.IsTrue(_manager.HasListener<GameEvent>(OnCall));
    }

    [Test]
    public void DoessAddListenerOnceAddToDelegates()
    {
      _manager.AddListenerOnce<GameEvent>(OnCall);
      Assert.IsFalse(_manager.IsDelegatesEmpty());
    }

    [Test]
    public void DoesRemoveListenerRemoveDelegate()
    {
      _manager.AddListener<GameEvent>(OnCall);
      _manager.RemoveListener<GameEvent>(OnCall);
      Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
    }

    [Test]
    public void DoesRemoveListenerClearDelegates()
    {
      _manager.AddListener<GameEvent>(OnCall);
      _manager.RemoveListener<GameEvent>(OnCall);
      Assert.IsTrue(_manager.IsDelegatesEmpty());
    }

    [Test]
    public void DoesRemoveListenerOnOnceRemoveDelegate()
    {
      _manager.AddListenerOnce<GameEvent>(OnCall);
      _manager.RemoveListener<GameEvent>(OnCall);
      Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
    }

    [Test]
    public void DoesRemoveListenerOnOnceClearDelegates()
    {
      _manager.AddListenerOnce<GameEvent>(OnCall);
      _manager.RemoveListener<GameEvent>(OnCall);
      Assert.IsTrue(_manager.IsDelegatesEmpty());
    }

    [Test]
    public void DoesRemoveAllClearListeners()
    {
      _manager.AddListener<GameEvent>(OnCall);
      _manager.RemoveAll();
      Assert.IsFalse(_manager.HasListener<GameEvent>(OnCall));
    }

    [Test]
    public void DoesRemoveAllClearDelegates()
    {
      _manager.AddListener<GameEvent>(OnCall);
      _manager.RemoveAll();
      Assert.IsTrue(_manager.IsDelegatesEmpty());
    }

    [Test]
    public void DoesRemoveClearAllEventsQueued()
    {
      _manager.QueueEvent(new GameEvent());
      _manager.RemoveAll();
      Assert.IsTrue(_manager.IsQueueEmpty());
    }

    [Test]
    public void DoesQueueEventAddToQueue()
    {
      _manager.QueueEvent(new GameEvent());
      Assert.AreEqual(1, _manager.QueueSize());
    }

    [Test]
    public void DoesQueueEventTriggerEventOnUpdate()
    {
      _manager.QueueEvent(new GameEvent());
      _manager.OnUpdate();
    }
  }
}
