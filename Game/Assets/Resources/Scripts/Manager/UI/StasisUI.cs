using System.Collections.Generic;
using Manager.Events.Type;
using Manager.Mechanics;
using UnityEngine;

namespace Manager.UI
{
    public class StasisUI : UIContoller<Dictionary<ConditionEnum, Transform>>
    {
        //readonly int time = 5;
        bool noPower = false;//TODO noPower and Boost are not in sync
        bool extraPower = true;
        readonly int damageTaken = -10;
        readonly int healthRestored = 5;
        public Dictionary<int, StasisData.ChamberData> StasisChambers;
        public System.Action DisableEject;
        public System.Func<string> GetChamberDataAsString;

        public StasisUI(Dictionary<ConditionEnum, Transform> conditionScreens) : base(conditionScreens) { }
        public override void SetUI(Transform uIScreens, int id, int wait)
        {
            foreach (StasisData.ChamberData chamber in StasisChambers.Values)
            {
                if (chamber.InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].Points == 0 && chamber.InteractableObject.Stats[InteractableObjectStatEnum.EnergyConsumption].maxPoints != 0)
                {
                    noPower = true;
                    break;
                }                 
                else if (chamber.InteractableObject.Stats[InteractableObjectStatEnum.Boost].Points > 0)
                {
                    extraPower = true;
                    break;
                }
                                          
            }
            if (noPower)
            {
                ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Health, damageTaken);
                noPower = false;
            }else
            if (extraPower)
            {
                ObjectTypes.InteractableObjectTypes[InteractableObjectTypeEnum.Stasis].InvokeStatChange(InteractableObjectStatEnum.Health, healthRestored);
                extraPower = false;
            }     
            if (id > 0 && id < uIScreens.childCount)
            {
                uIScreens.GetChild(id).gameObject.SetActive(true);
                int temp = id;
                switch (id)
                {        
                    case 1:
                        wait = 2;
                        temp++;
                        break;
                    case 2:
                        wait = 10;
                        temp++;
                        break;
                    case 3:
                        wait = 1;
                        temp += 2;
                        DisableEject.Invoke();
                        break;
                    case 4:
                        wait = 2;
                        temp++;
                        break;
                }
                UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, temp, wait);

            }
            else if(id >= uIScreens.childCount)
            {
                DisableEject.Invoke();
                id = 0;
                wait = 5;
                UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, id, wait);
            }
            else
            {
                id = 0;
                wait = 5;
                UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, id, wait);
            }
            if (uIScreens.childCount == 2)
            {
                UnityEngine.Debug.Log(uIScreens.GetChild(1));
                uIScreens.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = GetChamberDataAsString.Invoke();
            }
        }

        public override void StartUI(Transform uIScreens, ExtraData extraData, int id = 0, int wait = 5)
        {
            StasisChambers = extraData.StasisChambers;
            DisableEject = extraData.GetAction;
            GetChamberDataAsString = extraData.GetFunc;
            UIThread = StartTheThread(uIScreens.GetChild(id).gameObject, id, wait);
        }
    }
}