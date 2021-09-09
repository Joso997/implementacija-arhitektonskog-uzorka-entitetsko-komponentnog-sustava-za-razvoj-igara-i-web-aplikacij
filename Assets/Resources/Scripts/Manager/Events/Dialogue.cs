using Manager.Events.Type;
using Manager.Tracking;
using Manager.Tracking.Type;

namespace Manager.Events
{
    public struct Dialogue
    {
        //Database Variables
        public int Code { get; set; }
        public int CodeEventPhase { get; set; }
        public string Content { get; set; }
    }

}