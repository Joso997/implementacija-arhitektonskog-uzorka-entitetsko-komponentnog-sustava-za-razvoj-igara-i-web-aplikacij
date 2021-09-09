using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorSpeedModifier : StateMachineBehaviour
{
    private string healthName;

    IndicatorSpeedModifier()
    {
        healthName = System.Enum.GetName(typeof(InteractableObjectStatEnum), InteractableObjectStatEnum.Health);
    }
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f / (animator.GetInteger(healthName) / 50f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1;
    }

}
