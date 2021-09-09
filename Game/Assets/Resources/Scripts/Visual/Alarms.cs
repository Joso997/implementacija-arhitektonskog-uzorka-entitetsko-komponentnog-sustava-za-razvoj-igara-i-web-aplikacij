using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager.Audio;

public class Alarms : AudioManager
{

    // public Animator alarmAnim;
    // public GameObject[] alarmLightAnims;

    public float speed;
    public float t;
    public Color startColor;
    public Color endColor;
    public Color alarmColorTransparent;
    public Color alarmColorOpaque;
    
    public bool start = true;

    public bool firstFloorAlarmFinished = true;
    public bool secondFloorAlarmFinished = true;
    public bool thirdFloorAlarmFinished = true;

    public Renderer firstFloorAlarm;
    public Renderer secondFloorAlarm;
    public Renderer thirdFloorAlarm;

    private List<bool> floors;

    public GameObject firstFloor;
    public GameObject secondFloor;
    public GameObject thirdFloor;

    public GameObject firstFloorDark;
    public GameObject secondFloorDark;
    public GameObject thirdFloorDark;

    public bool add = true;

    public bool canStartFirst = false;
    public bool canStartSecond = false;
    public bool canStartThird = false;

    public void ActivateAlarm(int floor,bool activate = true)
    {
        if (activate)
        {
            floors[floor - 1] = true;
            PlaySound(SoundEnum.Alarm);
        }
        else
        {
            floors[floor - 1] = false;
            StopSound(SoundEnum.Alarm);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        floors = new List<bool>() { false, false, false };
        firstFloorAlarm = GameObject.Find("First-Floor-Alarm-Light#").GetComponent<Renderer>();
        secondFloorAlarm = GameObject.Find("Second-Floor-Alarm-Light#").GetComponent<Renderer>();
        thirdFloorAlarm = GameObject.Find("Third-Floor-Alarm-Light#").GetComponent<Renderer>();

        firstFloor = GameObject.Find("First-Floor");
        secondFloor = GameObject.Find("Second-Floor");
        thirdFloor = GameObject.Find("Third-Floor");

        firstFloorDark = GameObject.Find("First-Floor-Dark");
        secondFloorDark = GameObject.Find("Second-Floor-Dark");
        thirdFloorDark = GameObject.Find("Third-Floor-Dark");

        firstFloorAlarmFinished = true;
        secondFloorAlarmFinished = true;
        thirdFloorAlarmFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        //First Floor
        if(floors[0] == true && t <= 0.05)
        {
            canStartFirst = true;
        }

        if (floors[0] == true)
        {
            if (canStartFirst == true)
            {
                FirstFloorAlarm();
            }
        }
        else
        {
            canStartFirst = false;

            if (firstFloorAlarmFinished == false)
            {
                FirstFloorAlarm();
            }
        }

        //Second Floor
        if (floors[1] == true && t <= 0.05)
        {
            canStartSecond= true;
        }

        if (floors[1] == true)
        {
            if (canStartSecond == true)
            {
                SecondFloorAlarm();
            }
        }
        else
        {
            canStartSecond = false;

            if (secondFloorAlarmFinished == false)
            {
                SecondFloorAlarm();
            }
        }

        //Third Floor
        if (floors[2] == true && t <= 0.05)
        {
            canStartThird = true;
        }

        if (floors[2] == true)
        {
            if (canStartThird == true)
            {
                ThirdFloorAlarm();
            }
        }
        else
        {
            canStartThird = false;

            if (thirdFloorAlarmFinished == false)
            {
                ThirdFloorAlarm();
            }
        }



        if (floors[0] == true || floors[1] == true || floors[2] == true || firstFloorAlarmFinished == false || secondFloorAlarmFinished == false  || thirdFloorAlarmFinished == false)
        {
            if (add == false)
            {
                t -= Time.deltaTime * speed;
                if (t <= 0)
                {
                    add = true;
                    start = true;
                    firstFloorAlarmFinished = true;
                    secondFloorAlarmFinished = true;
                    thirdFloorAlarmFinished = true;
                }
            }

            if (add == true)
            {
                t += Time.deltaTime * speed;
                if (t >= 1)
                {
                    add = false;
                    start = false;
                }
            }
        }    
    }

    void FirstFloorAlarm()

    {
        if (start == true) 
        {

            firstFloorAlarmFinished = false;

            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));

            Component[] renderers = firstFloor.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers)
            {
                    childRenderer.SetPropertyBlock(block);
            }

            Component[] renderers2 = firstFloorDark.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers2)
            {
                childRenderer.SetPropertyBlock(block);
            }

            MaterialPropertyBlock block2;
            block2 = new MaterialPropertyBlock();
            block2.SetColor("_BaseColor", Color.Lerp(alarmColorTransparent, alarmColorOpaque, t));

            firstFloorAlarm.SetPropertyBlock(block2);
        }

        if (start == false)
        {

            firstFloorAlarmFinished = false;


            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));

            Component[] renderers = firstFloor.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers)
            {
                    childRenderer.SetPropertyBlock(block);
            }

            Component[] renderers2 = firstFloorDark.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers2)
            {
                childRenderer.SetPropertyBlock(block);
            }

            MaterialPropertyBlock block2;
            block2 = new MaterialPropertyBlock();
            block2.SetColor("_BaseColor", Color.Lerp(alarmColorTransparent, alarmColorOpaque, t));

            firstFloorAlarm.SetPropertyBlock(block2);
        }
    }



    void SecondFloorAlarm()

    {
        if (start == true)
        {

            secondFloorAlarmFinished = false;


            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));

            Component[] renderers = secondFloor.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers)
            {
                childRenderer.SetPropertyBlock(block);
            }

            Component[] renderers2 = secondFloorDark.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers2)
            {
                childRenderer.SetPropertyBlock(block);
            }

            MaterialPropertyBlock block2;
            block2 = new MaterialPropertyBlock();
            block2.SetColor("_BaseColor", Color.Lerp(alarmColorTransparent, alarmColorOpaque, t));

            secondFloorAlarm.SetPropertyBlock(block2);
        }

        if (start == false)
        {

            secondFloorAlarmFinished = false;


            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));

            Component[] renderers = secondFloor.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers)
            {
                    childRenderer.SetPropertyBlock(block);
            }

            Component[] renderers2 = secondFloorDark.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers2)
            {
                childRenderer.SetPropertyBlock(block);
            }

            MaterialPropertyBlock block2;
            block2 = new MaterialPropertyBlock();
            block2.SetColor("_BaseColor", Color.Lerp(alarmColorTransparent, alarmColorOpaque, t));

            secondFloorAlarm.SetPropertyBlock(block2);
        }
    }



    void ThirdFloorAlarm()

    {
        if (start == true)
        {

            thirdFloorAlarmFinished = false;


            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));

            Component[] renderers = thirdFloor.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers)
            {
                childRenderer.SetPropertyBlock(block);
            }

            Component[] renderers2 = thirdFloorDark.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers2)
            {
                childRenderer.SetPropertyBlock(block);
            }

            MaterialPropertyBlock block2;
            block2 = new MaterialPropertyBlock();
            block2.SetColor("_BaseColor", Color.Lerp(alarmColorTransparent, alarmColorOpaque, t));

            thirdFloorAlarm.SetPropertyBlock(block2);
        }

        if (start == false)
        {

            thirdFloorAlarmFinished = false;


            MaterialPropertyBlock block;
            block = new MaterialPropertyBlock();
            block.SetColor("_BaseColor", Color.Lerp(startColor, endColor, t));

            Component[] renderers = thirdFloor.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers)
            {
                childRenderer.SetPropertyBlock(block);
            }

            Component[] renderers2 = thirdFloorDark.GetComponentsInChildren<Renderer>();
            foreach (Renderer childRenderer in renderers2)
            {
                childRenderer.SetPropertyBlock(block);
            }

            MaterialPropertyBlock block2;
            block2 = new MaterialPropertyBlock();
            block2.SetColor("_BaseColor", Color.Lerp(alarmColorTransparent, alarmColorOpaque, t));

            thirdFloorAlarm.SetPropertyBlock(block2);
        }
    }
}

