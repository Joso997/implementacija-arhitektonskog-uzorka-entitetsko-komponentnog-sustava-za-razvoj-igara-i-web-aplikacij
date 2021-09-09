using CyberTale.Collections;
using Manager.Events;
using Manager.Events.Type;
using Manager.Mechanics;
using Manager.Mechanics.ColonizationGame;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Manager.UI
{
    public sealed class EndingUI : UIContoller<Dictionary<ConditionEnum, Transform>>
    {
        public EndingUI(Dictionary<ConditionEnum, Transform> conditionScreens) : base(conditionScreens)
        {

        }

        public override void SetUI(Transform uIScreens, int id, int wait = 10)
        {
            uIScreens.GetChild(0).gameObject.SetActive(true);
            UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, 0, wait);
        }

        public override void StartUI(Transform uIScreens, ExtraData extraData, int id = 0, int wait = 10)
        {
            uIScreens.GetChild(0).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = new ReproductionScenarios().Calculate(extraData.StasisChambers.Values);
            uIScreens.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = new WayOfLifeScenarios().Calculate(extraData.StasisChambers.Values);
            //UIThread = StartTheThread(uIScreens.GetChild(0).gameObject, 0, wait);
            uIScreens.GetChild(0).gameObject.SetActive(true);
        }
    }
}