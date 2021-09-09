using CyberTale.Collections;
using Manager.Tracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Manager.Events.Type
{
    public enum InteractableObjectStatEnum { Health, EnergyConsumption, EnergyGeneration, Fuel, Pressure, EnergyDemand, Boost };
    public enum StatTypeEnum { minPoints, Points, maxPoints };
    public delegate InteractableObjectStatAbstract CreateStatDel();
    public abstract class InteractableObjectStatAbstract
    {
        /*private int index;
        public virtual int minPoints { get { return ObjectTypes.StatsArray[index]; } set { ObjectTypes.StatsArray[index] = value; } }
        public virtual int Points { get { return ObjectTypes.StatsArray[index + 1]; } set { ObjectTypes.StatsArray[index + 1] = value; } }
        public virtual int maxPoints { get { return ObjectTypes.StatsArray[index + 2]; } set { ObjectTypes.StatsArray[index + 2] = value; } }*/
        public virtual int minPoints { get; set; }
        public virtual int Points { get; set; }
        public virtual int maxPoints { get; set; }
        public abstract void CheckRequirements(InteractableObject @object);
        public InteractableObjectStatAbstract CreateStat()
        {
            //return Activator.CreateInstance(this.GetType()) as InteractableObjectStatAbstract;
            return InstanceFactory.CreateInstance(this.GetType()) as InteractableObjectStatAbstract;
        }
        public void ReverseIfBorderPassed()
        {
            if (this.Points > this.maxPoints)
                this.Points = this.maxPoints;
            else if (this.Points < this.minPoints)
                this.Points = this.minPoints;
        }
    }

    public sealed partial class ObjectTypes
    {

        //public static int[] StatsArray { get; set; }

        public static Dictionary<InteractableObjectStatEnum, CreateStatDel> InteractableObjectStats { get; } = new Dictionary<InteractableObjectStatEnum, CreateStatDel>()
        {
            {InteractableObjectStatEnum.Health, new ObjectHealth().CreateStat},
            {InteractableObjectStatEnum.EnergyConsumption, new ObjectEnergyConsumption().CreateStat},
            {InteractableObjectStatEnum.EnergyGeneration, new ObjectEnergyGeneration().CreateStat},
            {InteractableObjectStatEnum.EnergyDemand, new ObjectEnergyDemand().CreateStat},
            {InteractableObjectStatEnum.Fuel, new ObjectFuel().CreateStat},
            {InteractableObjectStatEnum.Pressure, new ObjectPressure().CreateStat},
            {InteractableObjectStatEnum.Boost, new ObjectBoost().CreateStat}
        };

        sealed class ObjectHealth : InteractableObjectStatAbstract
        {/*Health*/
            public override void CheckRequirements(InteractableObject @object)
            {
                if (this.Points <= this.minPoints)
                {
                    this.maxPoints = 0;
                    this.Points = this.minPoints;
                } 
                else if (this.Points > this.maxPoints)
                    this.Points = this.maxPoints;
                if(@object.Indicator != null)
                {
                    @object.Indicator[0].SetInteger(System.Enum.GetName(typeof(InteractableObjectStatEnum), InteractableObjectStatEnum.Health), this.Points);
                }
                if(@object.ObjectType == InteractableObjectTypeEnum.Reactor)
                {
                    InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.EnergyGeneration, 0);
                }

                    
            }
        }

        sealed class ObjectEnergyConsumption : InteractableObjectStatAbstract
        {/*Energy Consumption*/
            public override void CheckRequirements(InteractableObject @object)
            {
                ReverseIfBorderPassed();
            }
        }

        sealed class ObjectEnergyGeneration : InteractableObjectStatAbstract
        {/*Energy Generation*/
            public override void CheckRequirements(InteractableObject @object)
            {
                if (this.Points < @object.Stats[InteractableObjectStatEnum.EnergyDemand].Points)
                {
                    Damage.DamageTypes[DamageTypeEnum.Electrical].ChangeGroupStat(-100);
                }
                else
                {
                    Damage.DamageTypes[DamageTypeEnum.Electrical].ChangeGroupStat(100);
                    
                }
                
                ReverseIfBorderPassed();
                InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.Pressure,
                    (@object.Stats[InteractableObjectStatEnum.Pressure].Points - (this.Points + (100 - @object.Stats[InteractableObjectStatEnum.Health].Points))) * -1);
                
                if (this.Points >= @object.Stats[InteractableObjectStatEnum.EnergyDemand].Points + 10)//Heal
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Boost, 1);
                else if(this.Points < @object.Stats[InteractableObjectStatEnum.EnergyDemand].Points +10 && this.Points >= @object.Stats[InteractableObjectStatEnum.EnergyDemand].Points)
                {//Do nothing
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Boost, -2);
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Boost, 1);
                } 
                else//Hurt
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Boost, -1);
                if (@object.Indicator != null)
                {
                    @object.Indicator[2].transform.Rotate(0, 360 - @object.Indicator[2].transform.localEulerAngles.y, 0);
                    @object.Indicator[2].transform.Rotate(0, 360 - (this.Points * 1.8f), 0);
                }
            }
        }

        sealed class ObjectEnergyDemand : InteractableObjectStatAbstract
        {/*Energy Generation*/
            public override void CheckRequirements(InteractableObject @object)
            {
                if (this.Points < @object.Stats[InteractableObjectStatEnum.EnergyGeneration].Points+10)
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.EnergyConsumption, 1);
                else
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.EnergyConsumption, -1);
                ReverseIfBorderPassed();
            }
        }

        sealed class ObjectFuel : InteractableObjectStatAbstract
        {/*Fuel*/
            public override void CheckRequirements(InteractableObject @object)
            {
                ReverseIfBorderPassed();
            }
        }

        sealed class ObjectPressure : InteractableObjectStatAbstract
        {
            
            public override void CheckRequirements(InteractableObject @object)
            {
                if (this.Points > this.maxPoints)
                {
                    Damage.DamageTypes[DamageTypeEnum.Electrical].ChangeGroupStat(-100);
                    InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.EnergyGeneration, -100);
                    InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Boost, -2);
                    this.Points = this.minPoints;
                }else if(this.Points < this.minPoints)
                {
                    this.Points = this.minPoints;
                }
                if (@object.Indicator != null)
                {
                    @object.Indicator[1].transform.Rotate(0, 360 - @object.Indicator[1].transform.localEulerAngles.y, 0);
                    @object.Indicator[1].transform.Rotate(0, 360-(this.Points*1.8f), 0);
                }
            }
        }

        sealed class ObjectBoost : InteractableObjectStatAbstract
        {
            public override void CheckRequirements(InteractableObject @object)
            {
                if (this.Points > 0)
                    this.Points = 1;
                else if (this.Points < 0)
                    this.Points = -1;
                if (@object.Indicator != null)
                    @object.Indicator[0].SetInteger(System.Enum.GetName(typeof(InteractableObjectStatEnum), InteractableObjectStatEnum.Boost), this.Points);
            }
        }
    }
}
