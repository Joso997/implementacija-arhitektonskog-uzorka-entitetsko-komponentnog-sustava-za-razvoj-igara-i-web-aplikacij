using Manager.Tracking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager.Events.Type
{
    public enum DamageTypeEnum { Physical, Electrical, ReactorSystems };
    public delegate void DamageDelegate(InteractableObjectStatEnum objectStat, int amount);
    public abstract class DamageTypeAbstract
    {
        public virtual void ChangeGroupStat(int amount)
        {
            throw new System.NotImplementedException();
        }
        public abstract void ChangeObjectStat(DamageDelegate invokeStatChange, int amount);
    }

    public class Damage
    {
        public static Dictionary<DamageTypeEnum, DamageTypeAbstract> DamageTypes { get; private set; } = new Dictionary<DamageTypeEnum, DamageTypeAbstract>()
        {
            {DamageTypeEnum.Physical, new Physical()},
            {DamageTypeEnum.Electrical, new Electrical()},
            {DamageTypeEnum.ReactorSystems, new ReactorSystems() }
        };

        public sealed class Physical : DamageTypeAbstract
        {

            public override void ChangeObjectStat(DamageDelegate invokeStatChange, int amount)
            {
                invokeStatChange.Invoke(InteractableObjectStatEnum.Health, amount);
            }
        }

        public sealed class Electrical : DamageTypeAbstract
        {
            readonly List<InteractableObjectTypeEnum> temp = new List<InteractableObjectTypeEnum>()
            {
                InteractableObjectTypeEnum.Stasis,
                InteractableObjectTypeEnum.Gas
            };
            public override void ChangeGroupStat(int amount)
            {
                Consequence[] consequencesArray = new Consequence[temp.Count];
                for (int i = 0; i < consequencesArray.Length; i++)
                {
                    consequencesArray[i].Code = -2;
                    consequencesArray[i].CodeEventPhase = -2;
                    consequencesArray[i].DamageType = DamageTypeEnum.Electrical;
                    consequencesArray[i].ObjectType = temp[i];
                    consequencesArray[i].Probability = ProbabilityEnum.Unavoidable;
                    consequencesArray[i].Severity = (Tracking.Type.SeverityEnum)amount;
                }
                Decider.ProcessConsequences(ref consequencesArray);
            }

            public override void ChangeObjectStat(DamageDelegate invokeStatChange, int amount)
            {
                invokeStatChange.Invoke(InteractableObjectStatEnum.EnergyConsumption, amount);
            }
        }

        public sealed class ReactorSystems : DamageTypeAbstract
        {

            public override void ChangeObjectStat(DamageDelegate invokeStatChange, int amount)
            {
                UnityEngine.Debug.Log(amount);
                invokeStatChange.Invoke(InteractableObjectStatEnum.Health, amount);
                invokeStatChange.Invoke(InteractableObjectStatEnum.EnergyGeneration, 0);
            }
        }
    }
}
