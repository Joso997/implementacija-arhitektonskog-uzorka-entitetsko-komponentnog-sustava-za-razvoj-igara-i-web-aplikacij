using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using Manager.Mechanics;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Manager.UI
{
    public sealed class PowerUI : UIContoller<Dictionary<ConditionEnum, Transform>>
    {
        public PowerUI(Dictionary<ConditionEnum, Transform> conditionScreens) : base(conditionScreens)
        {

        }

        public override void SetUI(Transform uIScreens, int id, int wait = 20)
        {
            if (id > 0 && id < uIScreens.childCount)
            {
                uIScreens.GetChild(0).gameObject.SetActive(true);
                UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, 0, 40);
            }
            else
            {
                StopUI(uIScreens, 0);
            }
        }

        public override void StartUI(Transform uIScreens, ExtraData extraData, int id = 0, int wait = 20)
        {
            UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, id, wait);
        }
    }
}
