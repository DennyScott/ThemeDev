using NUnit.Framework;
using Zephyr.EventSystem.Core;

namespace Zephyr.EventSystem.Editor.Test.Core
{
    [TestFixture]
    [Category("Game Event")]
    public class GameEventTest
    {
        [SetUp]
        public void Init()
        {
            _gameEvent = new GameEvent();
        }

        private GameEvent _gameEvent;


        [Test]
        public void IsNameEqualsTheTypeName()
        {
            Assert.AreEqual(_gameEvent.Name, _gameEvent.GetType().Name);
        }
    }
}