using Manager.Events.Type;
using Manager.Mechanics.Interface;
using Manager.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager.Mechanics.MemoryGame
{
    public class ThrustersMechanicController : MechanicInterface.DefaultControler
    {
        public ThrustersMechanicController() : base(){}

        protected override void SubscribeConditions()
        {
            //InteractableObjectType.InteractableObjectTypes[InteractableObjectTypeEnum.Gas].SubscribeLogic(Button);
        }

        protected override void Button(SubObjectTypeEnum button_id)
        {
            switch (button_id)
            {
                case SubObjectTypeEnum.MiddleButton:
                    break;
                default:
                    break;
            }
        }

        
    }
}