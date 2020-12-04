using System;
using System.Collections.Generic;
using System.Text;

namespace PrvaVjezba
{
    class Event
    {
        public Event(string name, int eventId, EventType typeOfEvent, DateTime startTime, DateTime endTime)
        {
            Name = name;
            EventId = eventId;
            TypeOfEvent = typeOfEvent;
            StartTime = startTime;
            EndTime = endTime;
        }

        public string Name { get; set; }

        public int EventId { get; set; }

        public enum EventType { Coffee = 1, Lecture = 2, Concert = 3, StudySession = 4 }

        public EventType TypeOfEvent { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public void PrintingEvent()
        {
            var duration = (EndTime - StartTime).TotalDays;
            Console.Write(Name + " - " + TypeOfEvent + " - " + StartTime + " - " + EndTime + " - " + duration);
        }

    }
}