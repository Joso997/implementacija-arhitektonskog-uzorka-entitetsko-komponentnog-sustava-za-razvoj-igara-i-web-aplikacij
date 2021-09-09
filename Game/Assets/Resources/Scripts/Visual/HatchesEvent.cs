using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchesEvent: MonoBehaviour
{

   public Hatches hatches;
    
   private void Start()
    {
        if(GameObject.FindGameObjectWithTag("GameController") != null)
            hatches = GameObject.FindGameObjectWithTag("GameController").GetComponent<Hatches>();
    }

    void FirstDisableBool()
    {
        hatches.hatchOneON = false;
    }

    void SecondDisableBool()
    {
        hatches.hatchTwoON = false;
    }

    void ThirdDisableBool()
    {
        hatches.hatchThreeON = false;
    }

    void FirstEnableBool()
    {
        hatches.hatchOneON = true;
    }

    void SecondEnableBool()
    {
        hatches.hatchTwoON = true;
    }

    void ThirdEnableBool()
    {
        hatches.hatchThreeON = true;
    }
}
