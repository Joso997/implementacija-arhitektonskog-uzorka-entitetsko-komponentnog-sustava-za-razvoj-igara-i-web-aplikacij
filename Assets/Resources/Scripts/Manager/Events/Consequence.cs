using Manager.Events.Type;
using Manager.Tracking;
using Manager.Tracking.Type;

namespace Manager.Events
{
    public struct Consequence
    {
        //Database Variables
        public int Code { get; set; }
        public int CodeEventPhase { get; set; }
        public ProbabilityEnum Probability { get; set; }
        public InteractableObjectTypeEnum ObjectType { get; set; }
        public DamageTypeEnum DamageType { get; set; }
        public SeverityEnum Severity { get; set; }
        public RegionEnum Region { get; set; }
    }
    
}