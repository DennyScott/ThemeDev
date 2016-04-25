using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameTime
{
    public class GameTime : MonoBehaviour
    {

        #region Public Static Variables
        public static int CurrentUnit = 0;
        #endregion

        #region Serialized Fields
        [SerializeField, Tooltip("The amount of seconds between each unity"), Range(0, 30)]
        private int _secondsPerUnit = 30;

        #endregion

        #region Private Fields
        private List<Event> _events;
        private List<int> _eventsRanThisUnit;
        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an event to the list of events
        /// </summary>
        /// <param name="evt">The event to add to the list of events</param>
        public void AddEvent(Event evt)
        {
            _events.Add(evt);
        }

        /// <summary>
        /// Removes and event from the list of events.
        /// </summary>
        /// <param name="evt">The event to remove from the list of events</param>
        public void RemoveEvent(Event evt)
        {
            _events.Remove(evt);
        }

        /// <summary>
        /// Gets a date ahead of the current date unit by the amount passed.
        /// </summary>
        /// <param name="unit">The amount of days ahead wanted</param>
        /// <returns>The unit date of the future date</returns>
        public int GetFutureUnitFromCurrentUnity(int unit)
        {
            return CurrentUnit + unit;
        }

        #endregion

        #region Standard Methods

        /// <summary>
        /// Runs on awake
        /// </summary>
        private void Awake()
        {
            _events = new List<Event>();
            _eventsRanThisUnit = new List<int>();
        }

        /// <summary>
        /// Runs on Start
        /// </summary>
        private void Start()
        {
            StartCoroutine("Run");
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Render the events that are scheduled for this unit.
        /// </summary>
        private void RenderUnitsEvents()
        {
            // For each event in the events list...
            for (var i = 0; i < _events.Count; i++)
            {
                // ...Store the currentEvent...
                var currentEvent = _events[i];

                // ...If the event is scheduled for the current unit...
                if (currentEvent.IsScheduledDate(CurrentUnit))
                    // ...Then Render the event
                    RenderEvent(currentEvent, i);
            }
        }

        /// <summary>
        /// Renders the passed event, and uses it's index for later flushing.
        /// </summary>
        /// <param name="renderedEvent">The event to render.</param>
        /// <param name="eventIndex">The index to store for flushing speed later.</param>
        private void RenderEvent(Event renderedEvent, int eventIndex)
        {
            // Triggers the event
            renderedEvent.TriggerEvent();

            // If the renderedEvent's flush date is the current date.
            if (renderedEvent.IsFlushedDate(CurrentUnit))
                // ...then add it to the flush list.
                _eventsRanThisUnit.Insert(0, eventIndex);
        }

        /// <summary>
        /// Flushes any event that has a flush date of the current unit
        /// </summary>
        private void FlushEvents()
        {
            // For each event index that ran this unit...
            for (var i = 0; i < _eventsRanThisUnit.Count; i++)
                // ...Remove the event at the given index.
                _events.RemoveAt(_eventsRanThisUnit[i]);

            // Clears the flushed event list.
            _eventsRanThisUnit.Clear();
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// The Run loop for the Game World Time.
        /// </summary>
        /// <returns>Coroutine</returns>
        private IEnumerator Run()
        {
            // While the game is running...
            while (true)
            {
                // Update the Current Units...
                CurrentUnit++;

                // ...Then Redner the UnitEvents...
                RenderUnitsEvents();

                // ...Then Flush any completed Events...
                FlushEvents();

                // ...Then wait the amount set until the next unit change.
                yield return new WaitForSeconds(_secondsPerUnit);
            }
        }

        #endregion
    }
}