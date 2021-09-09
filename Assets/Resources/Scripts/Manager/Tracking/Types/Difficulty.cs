using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager.Tracking.Type
{
    public enum DifficultyEnum { Easy, Medium, High };
    public delegate DifficultyTypeAbstract CreateDifficultyDel();
    public abstract class DifficultyTypeAbstract
    {
        public abstract int SeverityChanger { get; }
        public DifficultyTypeAbstract CreateStat()
        {
            return Activator.CreateInstance(this.GetType()) as DifficultyTypeAbstract;
        }
    }
    public class Difficulty : MonoBehaviour
    {
        public static Dictionary<DifficultyEnum, CreateDifficultyDel> DifficultyTypes { get; } = new Dictionary<DifficultyEnum, CreateDifficultyDel>()
        {
            {DifficultyEnum.Easy, new Easy().CreateStat},
            {DifficultyEnum.Medium, new Medium().CreateStat},
            {DifficultyEnum.High, new High().CreateStat}
        };

        public sealed class Easy : DifficultyTypeAbstract
        {
            public override int SeverityChanger { get => -5; }
        }

        public sealed class Medium : DifficultyTypeAbstract
        {
            public override int SeverityChanger { get => 0; }
        }

        public sealed class High : DifficultyTypeAbstract
        {
            public override int SeverityChanger { get => 5; }
        }
    }
}
