using Manager.Events.Type;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(InteractableObject))]
public class InteractableObjectEditor : Editor
{
    Dictionary<InteractableObjectStatEnum, InteractableObjectStatAbstract> tempStats = new Dictionary<InteractableObjectStatEnum, InteractableObjectStatAbstract>();
    object[] temp_array_subObject;
    object[] temp_array_Action;
    InteractableObject myScript;
    int countSubObjectType;
    int countActionType;
    readonly InteractableObject parentObject;
    List<Dictionary<object, object>> temp;

    override public void OnInspectorGUI()
    {
        myScript = target as InteractableObject;
        myScript.Node = (GameObject)EditorGUILayout.ObjectField("Node", myScript.Node, typeof(UnityEngine.GameObject), true);
        // myScript.Indicator = (Animator)EditorGUILayout.ObjectField("Indicator", myScript.Indicator[0], typeof(UnityEngine.Animator), true);
        int size = Mathf.Max(0, EditorGUILayout.IntField("Size", myScript.Indicator.Count));
        while (size > myScript.Indicator.Count)
        {
            myScript.Indicator.Add(null);
        }
        while (size < myScript.Indicator.Count)
        {
            myScript.Indicator.RemoveAt(myScript.Indicator.Count - 1);
        }
        for(int i = 0; i< myScript.Indicator.Count; i++)
        {
            myScript.Indicator[i] = (Animator)EditorGUILayout.ObjectField("Animator " + i, myScript.Indicator[i], typeof(Animator), true);
        }
        var tempObjectType = (InteractableObjectTypeEnum)myScript.InteractableObjectType_index;
        //Choosing RegionType
        myScript.Region_index = EditorGUILayout.Popup("Region: ", myScript.Region_index, System.Enum.GetNames(typeof(RegionEnum)));
        //Choosing ObjectType
        myScript.InteractableObjectType_index = EditorGUILayout.Popup("Component Type: ", myScript.InteractableObjectType_index, System.Enum.GetNames(typeof(InteractableObjectTypeEnum)));
        //Initialzie ObjectTypes
        if (Application.isEditor && !Application.isPlaying)
        {
            ObjectTypes.InitializeObjectTypes();
        }
        //Get ComponentParts of Choosen ObjectType
        temp = ObjectTypes.InteractableObjectTypes[(InteractableObjectTypeEnum)myScript.InteractableObjectType_index].GetComponentParts();
        if (myScript.SubObjectType != SubObjectTypeEnum.ParentObject)
        {
            EditorGUILayout.BeginHorizontal();
            myScript.ParentObject = (InteractableObject)EditorGUILayout.ObjectField("ParentObject", myScript.ParentObject, typeof(InteractableObject), true);
            if (GUILayout.Button("Set"))
            {
                myScript.GiveParent();
            }
            EditorGUILayout.EndHorizontal();
        }
        
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
        //Setting Stats to Temp
        //if(tempStats.Keys.Count < temp[2].Keys.Count)
        tempStats = temp[2].ToDictionary(x => (InteractableObjectStatEnum)x.Key, x => x.Value as InteractableObjectStatAbstract);

        if (tempObjectType != (InteractableObjectTypeEnum)myScript.InteractableObjectType_index)
            myScript.StatsEnums = null;
        //Generate Int Fields for Stats
        if(myScript.SubObjectType == SubObjectTypeEnum.ParentObject || myScript.SubObjectType == SubObjectTypeEnum.DummyParentObject)
        {
            if (myScript.Stats != null)
            {
                //Generated from this GameObject Dictionary on Play
                foreach (var stat in myScript.Stats)
                {
                    EditorGUILayout.LabelField(System.Enum.GetName(typeof(InteractableObjectStatEnum), stat.Key));
                    EditorGUILayout.IntField("___" + System.Enum.GetName(typeof(StatTypeEnum), StatTypeEnum.minPoints), myScript.Stats[stat.Key].minPoints);
                    EditorGUILayout.IntField("___" + System.Enum.GetName(typeof(StatTypeEnum), StatTypeEnum.Points), myScript.Stats[stat.Key].Points);
                    EditorGUILayout.IntField("___" + System.Enum.GetName(typeof(StatTypeEnum), StatTypeEnum.maxPoints), myScript.Stats[stat.Key].maxPoints);
                }
            }
            else if (myScript.StatsEnums != null)
            {
                //Generated from this GameObject Dictionary on Editor
                foreach (var listEnum in myScript.StatsEnums.ToList().Select((value, index) => new { Value = value, Index = index }))
                {
                    EditorGUILayout.LabelField(System.Enum.GetName(typeof(InteractableObjectStatEnum), listEnum.Value));
                    int i_temp = 3 * listEnum.Index;
                    myScript.StatsValues[i_temp] = EditorGUILayout.IntField("___" + System.Enum.GetName(typeof(StatTypeEnum), StatTypeEnum.minPoints), myScript.StatsValues[i_temp]);
                    myScript.StatsValues[i_temp + 1] = EditorGUILayout.IntField("___" + System.Enum.GetName(typeof(StatTypeEnum), StatTypeEnum.Points), myScript.StatsValues[i_temp + 1]);
                    myScript.StatsValues[i_temp + 2] = EditorGUILayout.IntField("___" + System.Enum.GetName(typeof(StatTypeEnum), StatTypeEnum.maxPoints), myScript.StatsValues[i_temp + 2]);
                }
            }
            //Convert TempStats to ListStats(for serialization, located in this GameObject)
            else
            {
                myScript.StatsEnums = new List<InteractableObjectStatEnum>();
                myScript.StatsValues = new List<int>();
                myScript.StatsEnums.AddRange(tempStats.Keys);
                foreach (var list in tempStats.Values)
                {
                    myScript.StatsValues.Add(list.minPoints);
                    myScript.StatsValues.Add(list.Points);
                    myScript.StatsValues.Add(list.maxPoints);
                }
            }
        }
        

    }

}
#endif
