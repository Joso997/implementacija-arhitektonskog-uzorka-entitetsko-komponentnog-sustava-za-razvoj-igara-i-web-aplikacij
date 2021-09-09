using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using Manager.Mechanics;
using System.Collections.Generic;
using UnityEngine;

namespace Manager.UI
{
    public sealed class AssingmentUI : UIContoller<Dictionary<ConditionEnum, Transform>>
    {
        readonly int waitTimeIntroduction = 5;
        readonly int waitTimeAssignment = 20;
        public AssingmentUI(Dictionary<ConditionEnum, Transform> conditionScreens) : base(conditionScreens) { }

        public override void SetUI(Transform uIScreens, int id, int wait = 20)
        {
            id = RefreshChildCount(id);
            if (uIScreens.childCount > id)
            {
                if (id < 2)
                    wait = waitTimeIntroduction;
                else
                    wait = waitTimeAssignment;
                uIScreens.GetChild(id).gameObject.SetActive(true);
                GetConditionDefinition.Invoke(uIScreens.GetChild(id));
                UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, id, wait);
            }
            else
            {
                StopUI(uIScreens, id - 1);
                CheckAnswers.Invoke(false, new EndActionData());
                Debug.Log("Assingment Passed");
            }
        }

        public override void StartUI(Transform uIScreens, ExtraData extraData, int id = 0, int wait = 20)
        {
            CheckAnswers = extraData.CheckAnswers;
            GetConditionDefinition = extraData.GetConditionDefinition;
            RefreshChildCount = extraData.RefreshChildCount;
            uIScreens.GetChild(id).gameObject.SetActive(true);
            GetConditionDefinition.Invoke(uIScreens.GetChild(id));
            UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, id, wait);
        }
    }
}
