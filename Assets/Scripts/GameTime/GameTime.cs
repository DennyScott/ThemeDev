using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameTime
{
    public class GameTime : MonoBehaviour
    {
        public static int CurrentUnit = 0;
        public int SecondsPerUnit = 30;
        private List<Event> _events;
        private List<int> _eventsRanThisUnit;


        public void AddEvent(Event evt)
        {
            _events.Add(evt);
        }

        public void RemoveEvent(Event evt)
        {
            _events.Remove(evt);
        }

        public int DateFromCurrent(int unit)
        {
            return CurrentUnit + unit;
        }

        private void Awake()
        {
            _events = new List<Event>();
            _eventsRanThisUnit = new List<int>();
        }

        // Use this for initialization
        private void Start()
        {
            StartCoroutine("Run");
        }

        private void RenderUnit()
        {
            for (var i = 0; i < _events.Count; i++)
            {
                if (_events[i].IsScheduled(CurrentUnit))
                    RenderEvent(i);
            }
        }

        private void RenderEvent(int i)
        {
            _events[i].TriggerEvent();
            _eventsRanThisUnit.Insert(0, i);
        }

        private void FlushEvents()
        {
            for (var i = 0; i < _eventsRanThisUnit.Count; i++)
            {
                if (_events[i].IsFlushedUnit(CurrentUnit))
                    _events.RemoveAt(_eventsRanThisUnit[i]);
            }
            _eventsRanThisUnit.Clear();
        }

        private IEnumerator Run()
        {
            while (true)
            {
                CurrentUnit++;
                RenderUnit();
                FlushEvents();
                yield return new WaitForSeconds(SecondsPerUnit);
            }
        }
    }
}