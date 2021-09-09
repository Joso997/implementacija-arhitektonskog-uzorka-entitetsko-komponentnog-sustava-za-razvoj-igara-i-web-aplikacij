using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunFirst : MonoBehaviour
{
    private void Awake()
    {
        ObjectTypes.InitializeObjectTypes();
        ObjectTypes.InitializeRegionTypes();
        
        ObjectTypes.InitializeSubObjectTypes();
        Condition.Initialize();
    }
}
