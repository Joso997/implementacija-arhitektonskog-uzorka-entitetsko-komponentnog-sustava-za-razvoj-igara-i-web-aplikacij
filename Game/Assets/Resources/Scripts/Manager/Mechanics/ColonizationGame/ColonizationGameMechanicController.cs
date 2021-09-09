using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Manager.Mechanics.ColonizationGame
{
    public class ColonizationGameMechanicController : MechanicInterface.HasUIController<StasisUI>
    {
        protected Dictionary<int, StasisData.ChamberData> StasisChambers { get; set; }
        private string healthName;
        private string boostName;
        private int activeChamberId;
        public ColonizationGameMechanicController(StasisUI _uIContoller, Dictionary<int, StasisData.ChamberData> _stasisChambers) : base(_uIContoller)
        {
            StasisChambers = _stasisChambers;
            healthName = System.Enum.GetName(typeof(InteractableObjectStatEnum), InteractableObjectStatEnum.Health);
            boostName = System.Enum.GetName(typeof(InteractableObjectStatEnum), InteractableObjectStatEnum.Boost);
            UIScreens = UIContoller.Screens[ConditionEnum.Eject];
            UIContoller.StartUI(UIScreens, new ExtraData(StasisChambers, DisableEject, GetGenderAndName), 0, 5);
        }

        protected override void SubscribeConditions()
        {
            (Condition.Conditions[ConditionEnum.Eject] as ISubController<ActedDefaultDel>).SubscribeController(ActivateUI);
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].SubscribeLogic(Button);
        }
        public override void Stop()
        {
            (Condition.Conditions[ConditionEnum.Dialogue] as ISubController<ActedDefaultDel>).UnSubscribeController(ActivateUI);
            ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].UnSubscribeLogic(Button);
            base.Stop();
        }
        protected override void ActivateUI(ComputerConditionSetEventArgs e)
        {
            ConditionSet = e;
        }

        protected override void Button(SubObjectTypeEnum button_id)
        {
            EndGame();
            if (button_id == SubObjectTypeEnum.MiddleButton)
            {
                UIContoller.StopUI(UIScreens);
                UIScreens = UIContoller.Screens[ConditionEnum.Eject];
                foreach (StasisData.ChamberData chamber in StasisChambers.Values)
                {
                    if(chamber.InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].maxPoints != 0)
                        chamber.InteractableObject.ActionType = ConditionEnum.Choose;
                }
                UIContoller.RestartAll(UIScreens, 1, 2);
            }
            else
            {
                if (CheckIfConditionEnumModeIsChoose((int)button_id))
                {
                    StasisChambers[(int)button_id].InteractableObject.gameObject.SetActive(false);
                    ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Reactor].InvokeStatChange(InteractableObjectStatEnum.EnergyDemand,
                        StasisChambers[(int)button_id].InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].maxPoints * -1);
                    StasisChambers[(int)button_id].InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].maxPoints = 0;
                    StasisChambers[(int)button_id].InteractableObject.Stats[InteractableObjectStatEnum.Health].Points = 0;
                    StasisChambers[(int)button_id].InteractableObject.Stats[InteractableObjectStatEnum.Health].maxPoints = 0;
                    StasisChambers[(int)button_id].InteractableObject.Indicator[0].SetInteger(healthName, 0);
                    StasisChambers[(int)button_id].InteractableObject.Indicator[0].SetInteger(boostName, 0);
                    DisableEject();
                    UIContoller.RestartAll(UIScreens, 4, 1);
                    if (ConditionSet != null)
                        ConditionSet.CheckAnswers(true);
                }
                else
                {
                    UIContoller.StopUI(UIScreens);
                    UIScreens = UIContoller.Screens[ConditionEnum.RepairChamber];
                    activeChamberId = (int)button_id;
                    UIContoller.RestartAll(UIScreens, 1, 5);
                }
                
            }
        }

        bool CheckIfConditionEnumModeIsChoose(int button_id)
        {
            if (StasisChambers[button_id].InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].maxPoints != 0 && StasisChambers[button_id].InteractableObject.ActionType != ConditionEnum.Choose)
                return false;
            return true;
        }
        void DisableEject()
        {
            foreach (StasisData.ChamberData chamber in StasisChambers.Values)
            {
                chamber.InteractableObject.ActionType = ConditionEnum.RepairChamber;
            }
        }

        string GetGenderAndName()
        {
            return System.Enum.GetName(typeof(StasisData.GenderEnum), StasisChambers[activeChamberId].Gender) + "\n" + System.Enum.GetName(typeof(StasisData.JobTypeEnum), StasisChambers[activeChamberId].JobReq);
        }

        void EndGame()
        {
            var temp = new ReproductionScenarios();
            temp.Calculate(StasisChambers.Values);
            var test = new WayOfLifeScenarios();
            test.Calculate(StasisChambers.Values);
        }
    }
}