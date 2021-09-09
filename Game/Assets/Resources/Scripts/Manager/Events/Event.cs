using Manager.Streaming;
using System;
using System.Collections.Generic;

namespace Manager.Events
{
    public class Event
    {
        public delegate void NextEventMethod(int code);
        public NextEventMethod NextEventMethodDelegate;
        private readonly Streamer<EventPhase> PhaseStreamer;
        private DebugLog.WriteInLogDelegate WriteToFile;

        //Database Variables
        public int Code { get; set; }
        public string StartingCondition { get; set; }
        public int NextEventCode { get; set; }
        public int StartEventPhaseCode { get; set; }

        public Event()
        {
            PhaseStreamer = new Streamer<EventPhase>(StartEvent, EndEvent);
        }

        internal void AddDebugLog(DebugLog.WriteInLogDelegate _writeToFile)
        {
            WriteToFile = _writeToFile;
        }

        public void AfterInitialization(NextEventMethod nextEvent)
        {
            NextEventMethodDelegate = nextEvent;
            PhaseStreamer.SetQueue(new DataLoad().GetEventPhases(StartEventPhaseCode, WriteToFile));
            PhaseStreamer.StreamNext();
        }

        public void StartEvent(EventPhase eventPhase)
        {
            eventPhase.StartPhase(PhaseStreamer.StreamNext);
        }

        public void EndEvent()
        {
            UnityEngine.Debug.Log("Test");
            NextEventMethodDelegate(NextEventCode);
        }
    }
}