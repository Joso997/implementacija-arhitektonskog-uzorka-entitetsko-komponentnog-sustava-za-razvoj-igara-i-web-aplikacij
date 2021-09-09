using Manager.Events;
using Manager.Events.Type;
using Manager.Tracking.Type;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;

namespace Manager.Tracking
{
    public delegate SeverityEnum GlobalStatsDel(SeverityEnum severity);
    public sealed partial class Decider
    {
        public static DebugLog.WriteInLogDelegate WriteToFile { get; internal set; }//TODO change to non static maybe

        public static event GlobalStatsDel GetGlobalStatsEvent;//TODO change to non static

        //[BurstCompile]
        struct ConsequencesJob : IJobParallelFor
        {
            public NativeArray<Consequence> Consequences;
            public int RandomNumber;
            public NativeArray<int> DamagePercentage;
            void IJobParallelFor.Execute(int index)
            {
                var consequence = Consequences[index];
                if (RandomNumber < (int)consequence.Probability)
                {
                    DamagePercentage[index] = (int)consequence.Severity;
                }
            }
        }

        public static void ProcessConsequences([ReadOnly] ref IEnumerable<Consequence> _Consequences)
        {
            Consequence[] consequencesArray = new Consequence[20];
            int i = 0;
            foreach (var item in _Consequences)
            {
                consequencesArray[i].Code = item.Code;
                consequencesArray[i].CodeEventPhase = item.CodeEventPhase;
                consequencesArray[i].DamageType = item.DamageType;
                consequencesArray[i].Region = item.Region;
                consequencesArray[i].ObjectType = item.ObjectType;
                consequencesArray[i].Probability = item.Probability;
                consequencesArray[i].Severity = (SeverityEnum)((int)GetGlobalStatsEvent.Invoke(item.Severity)*-1);//TODO need to make it flexible
#if UNITY_EDITOR
                WriteToFile.Invoke(new List<string>()
                    {
                        "System Consequence",
                        System.Enum.GetName(typeof(RegionEnum), consequencesArray[i].Region),
                        System.Enum.GetName(typeof(InteractableObjectTypeEnum), consequencesArray[i].ObjectType),
                        System.Enum.GetName(typeof(DamageTypeEnum), consequencesArray[i].DamageType),
                        System.Enum.GetName(typeof(ProbabilityEnum), consequencesArray[i].Probability),
                        System.Enum.GetName(typeof(SeverityEnum), consequencesArray[i].Severity)
                    }
                );
#endif
                i++;
            }
            GenerateJob(ref consequencesArray);
        }

        public static void ProcessConsequences([ReadOnly] ref Consequence[] consequencesArray)
        {
            GenerateJob(ref consequencesArray);
        }

        private static void GenerateJob([ReadOnly] ref Consequence[] consequencesArray)
        {
            var tempNativeArray = new NativeArray<Consequence>(consequencesArray, Allocator.TempJob);
            var damagePercentage = new NativeArray<int>(consequencesArray.Length, Allocator.TempJob);
            var job = new ConsequencesJob()
            {
                Consequences = tempNativeArray,
                RandomNumber = new System.Random().Next(100),
                DamagePercentage = damagePercentage
            };
            var jobHandle = job.Schedule(consequencesArray.Length, 10);
            jobHandle.Complete();
            for (int index = 0; index < job.Consequences.Length; index++)
            {
                Damage.DamageTypes[job.Consequences[index].DamageType].ChangeObjectStat(ObjectTypes.RegionTypes[job.Consequences[index].Region].Objects[job.Consequences[index].ObjectType].InvokeStatChange, job.DamagePercentage[index]);
            }
            tempNativeArray.Dispose();
            damagePercentage.Dispose();
        }
    }
}

