using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using Manager.Mechanics;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Manager.UI
{
    public sealed class DialogueUI : UIContoller<Dictionary<ConditionEnum, Transform>>
    {
        public DialogueUI(Dictionary<ConditionEnum, Transform> conditionScreens) : base(conditionScreens)
        {

        }

        public override void SetUI(Transform uIScreens, int id, int wait = 10)
        {
            if (uIScreens.childCount > RefreshChildCount(0))
            {
                uIScreens.GetChild(0).gameObject.SetActive(true);
                GetConditionDefinition.Invoke(uIScreens.GetChild(0));
                UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, 0, wait);
            }
            else
            {
                StopUI(uIScreens, 0);
                CheckAnswers.Invoke(true, new EndActionData());
                Debug.Log("Dialogue auto end.");
            }
        }

        public override void StartUI(Transform uIScreens, ExtraData extraData, int id = 0,  int wait = 10)
        {
            CheckAnswers = extraData.CheckAnswers;
            GetConditionDefinition = extraData.GetConditionDefinition;
            RefreshChildCount = extraData.RefreshChildCount;
            GetConditionDefinition.Invoke(uIScreens.GetChild(0));
            UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, 0, wait);
            uIScreens.GetChild(0).gameObject.SetActive(true);
        }
    }
}
