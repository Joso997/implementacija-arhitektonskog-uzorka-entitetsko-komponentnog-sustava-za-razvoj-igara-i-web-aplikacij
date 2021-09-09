using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DefaultObject))]
public class DefaultObjectEditor : Editor
{
    //Dictionary<InteractableObjectStatEnum, InteractableObjectStatAbstract> tempStats = new Dictionary<InteractableObjectStatEnum, InteractableObjectStatAbstract>();
    object[] temp_array_subObject;
    object[] temp_array_Action;
    DefaultObject myScript;
    int countSubObjectType;
    int countActionType;

    override public void OnInspectorGUI()
    {
        myScript = target as DefaultObject;
        //Choosing RegionType
        myScript.Region_index = EditorGUILayout.Popup("Region: ", myScript.Region_index, System.Enum.GetNames(typeof(RegionEnum)));
        //Choosing ObjectType
        myScript.InteractableObjectType_index = EditorGUILayout.Popup("Component Type: ", myScript.InteractableObjectType_index, System.Enum.GetNames(typeof(InteractableObjectTypeEnum)));
        //Get ComponentParts of Choosen ObjectType
        var temp = ObjectTypes.InteractableObjectTypes[(InteractableObjectTypeEnum)myScript.InteractableObjectType_index].GetComponentParts();
        //Debug.Log(temp);
        //Check if Collection size has changed
        if (temp[0].Keys.Count < countSubObjectType)
            myScript.subObjectType_index = 0;
        if (temp[1].Keys.Count < countActionType)
            myScript.action_index = 0;
        countSubObjectType = temp[0].Keys.Count;
        countActionType = temp[1].Keys.Count;
        //Setting Temp Arrays
        temp_array_subObject = temp[0].Keys.ToArray();
        temp_array_Action = temp[1].Keys.ToArray();
        //Choosing SubObjectType
        myScript.subObjectType_index = EditorGUILayout.Popup("Component Part: ", myScript.subObjectType_index, System.Array.ConvertAll(temp_array_subObject, x => x.ToString()));
        //Choosing Action
        myScript.action_index = EditorGUILayout.Popup("Action: ", myScript.action_index, System.Array.ConvertAll(temp_array_Action, x => x.ToString()));
        //Setting Types into this GameObject
        myScript.ObjectType = (InteractableObjectTypeEnum)myScript.InteractableObjectType_index;
        myScript.SubObjectType = (SubObjectTypeEnum)temp[0][temp_array_subObject[myScript.subObjectType_index]];
        myScript.ActionType = (ConditionEnum)temp[1][temp_array_Action[myScript.action_index]];
        myScript.SubObjectType_index = (int)myScript.SubObjectType;
        myScript.Action_index = (int)myScript.ActionType;
    }

}
#endif
