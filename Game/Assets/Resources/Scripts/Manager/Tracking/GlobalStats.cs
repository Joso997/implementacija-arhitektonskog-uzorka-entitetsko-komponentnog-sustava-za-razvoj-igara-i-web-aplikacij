using Manager.Tracking.Type;

namespace Manager.Tracking
{
    public enum ProbabilityEnum { Unavoidable = 100, Probable = 75, Even = 50, Unlikely = 25 };
    public class GlobalStats
    {
        public Tracker Tracker { get; } = new Tracker();
        public DifficultyTypeAbstract DifficultyType { get; private set; }

        public GlobalStats(DifficultyEnum difficulty, DebugLog.WriteInLogDelegate _writeToFile)
        {
            DifficultyType = Difficulty.DifficultyTypes[difficulty].Invoke();
            Decider.GetGlobalStatsEvent += GetGlobalStats;
            Decider.WriteToFile = _writeToFile;
        }

        public void Stop()
        {
            Decider.GetGlobalStatsEvent -= GetGlobalStats;
        }

        public SeverityEnum GetGlobalStats(SeverityEnum severity)
        {
            UnityEngine.Debug.Log("Experience number: "+Tracker.CalculateExperiencedDifficulty());
            UnityEngine.Debug.Log("Difficulty number: "+DifficultyType.SeverityChanger);
            return severity + DifficultyType.SeverityChanger + Tracker.CalculateExperiencedDifficulty();
        }

    }
}

