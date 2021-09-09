using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager.Audio;

public class IntroInside : AudioManager
{
    public GameObject Node;
    public float t;

    bool time;

    GameObject player;

    Animator playerAnim;

    GameObject firstFloor;
    GameObject secondFloor;
    GameObject thirdFloor;

    GameObject firstFloorAlarm;
    GameObject secondFloorAlarm;
    GameObject thirdFloorAlarm;

    GameObject firstFloorDark;
    GameObject secondFloorDark;
    GameObject thirdFloorDark;

    //  Renderer firstFloorGlass;
    //   Renderer secondFloorGlass;
    // Renderer thirdFloorGlass;

    GameObject thirdFloorGlass;

    GameObject chamberLights;
    GameObject nuclearLights;

    bool startIntro = false;

    public Color glass;
    public Color dark;
    public Color white;

    public Material firstFloorDarkMaterial;
    public Material secondFloorDarkMaterial;
    public Material thirdFloorDarkMaterial;

    public Material firstFloorMaterial;
    public Material secondFloorMaterial;
    public Material thirdFloorMaterial;

    GameObject scrollingText;

    public GameObject blackout;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerAnim = player.GetComponent<Animator>();

        firstFloor = GameObject.Find("First-Floor");
        secondFloor = GameObject.Find("Second-Floor");
        thirdFloor = GameObject.Find("Third-Floor");

        firstFloorAlarm = GameObject.Find("First-Floor-Alarm");
        secondFloorAlarm = GameObject.Find("Second-Floor-Alarm");
        thirdFloorAlarm = GameObject.Find("Third-Floor-Alarm");

        firstFloorDark = GameObject.Find("First-Floor-Dark");
        secondFloorDark = GameObject.Find("Second-Floor-Dark");
        thirdFloorDark = GameObject.Find("Third-Floor-Dark");

        chamberLights = GameObject.Find("___Chamber Lights___");
        nuclearLights = GameObject.Find("___Nuclear Lights___");

        thirdFloorGlass = GameObject.Find("Third-Floor-Glass");

        scrollingText = GameObject.Find("Header-Scrolling-Text");
        scrollingText.SetActive(false);

        dark = new Color32(70, 70, 70, 255);
        white = new Color32(255, 255, 255, 255);
        glass = new Color32(221, 221, 221, 255);

        time = true;

        startIntro = true;
        PlaySound(SoundEnum.Theme1);

    }

    // Update is called once per frame
    void Update()
    {

       if(startIntro == true)
        {
            AllDark();

            t += Time.deltaTime;

            if (t > 5)
            {
                playerAnim.SetBool("playIntro", true);
                startIntro = false;
            }
        }
    }


    void EndOfIntro()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager.GameController>().ShowEventData();
        playerAnim.SetBool("playIntro", false);
        scrollingText.SetActive(true);
        this.enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().Initialize(Node);    
    }

    void AllDark()
    {
        FirstFloorDark();
        SecondFloorDark();
        ThirdFloorDark();
    }

    void FirstFloorDark()
    {
        /*
        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", dark);

        Component[] renderers = firstFloor.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.SetPropertyBlock(block);
        }
        */

        firstFloor.SetActive(false);
        firstFloorAlarm.SetActive(false);

        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", dark);

        Component[] renderers = firstFloorDark.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = firstFloorDarkMaterial;
            childRenderer.SetPropertyBlock(block);
        }

    }

    void SecondFloorDark()
    {

        /*
        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", dark);

        Component[] renderers = secondFloor.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.SetPropertyBlock(block);
        }
    
          
        blackout.SetActive(true);
        */
        chamberLights.SetActive(false);
       secondFloor.SetActive(false);
       secondFloorAlarm.SetActive(false);
        blackout.SetActive(true);

        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", dark);

        Component[] renderers = secondFloorDark.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = secondFloorDarkMaterial;
            childRenderer.SetPropertyBlock(block);
        }

        Component[] renderers2 = blackout.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = secondFloorDarkMaterial;
            childRenderer.SetPropertyBlock(block);
        }




    }

    void ThirdFloorDark()
    {
        /*
        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", dark);

        Component[] renderers = thirdFloor.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.SetPropertyBlock(block);
        }
        */
        nuclearLights.SetActive(false);
        thirdFloor.SetActive(false);
        thirdFloorGlass.SetActive(false);

        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", dark);

        Component[] renderers = thirdFloorDark.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = thirdFloorDarkMaterial;
            childRenderer.SetPropertyBlock(block);
        }

    }

    void FirstFloorLight()
    {
        /*
        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.white);

        Component[] renderers = firstFloor.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.SetPropertyBlock(block);
        }

        
        MaterialPropertyBlock block2;
        block2 = new MaterialPropertyBlock();
        block2.SetColor("_BaseColor", glass);

        firstFloorGlass.SetPropertyBlock(block2);
        */

        firstFloor.SetActive(true);
        firstFloorAlarm.SetActive(true);

        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", white);

        Component[] renderers = firstFloorDark.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = firstFloorMaterial;
            childRenderer.SetPropertyBlock(block);
        }
    }

    void SecondFloorLight()
    {
        /*
        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.white);

        Component[] renderers = secondFloor.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.SetPropertyBlock(block);
        }

        
        MaterialPropertyBlock block2;
        block2 = new MaterialPropertyBlock();
        block2.SetColor("_BaseColor", glass);

        Component[] renderers2 = secondFloorGlass.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers2)
        {
            childRenderer.SetPropertyBlock(block2);
        }

        Component[] renderers3 = secondFloorChamberGlass.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers3)
        {
            childRenderer.SetPropertyBlock(block2);
        }
        
        blackout.SetActive(false);

        */

        chamberLights.SetActive(true);
        secondFloor.SetActive(true);
        secondFloorAlarm.SetActive(true);
        blackout.SetActive(false);

        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", white);

        Component[] renderers = secondFloorDark.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = secondFloorMaterial;
            childRenderer.SetPropertyBlock(block);
        }
    }

    void ThirdFloorLight()
    {
        /*
        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.white);

        Component[] renderers = thirdFloor.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.SetPropertyBlock(block);
        }

        MaterialPropertyBlock block2;
        block2 = new MaterialPropertyBlock();
        block2.SetColor("_BaseColor", glass);

        Component[] renderers2 = thirdFloorGlass.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers2)
        {
            childRenderer.SetPropertyBlock(block2);
        }

    */

        nuclearLights.SetActive(true);
        thirdFloor.SetActive(true);
        thirdFloorGlass.SetActive(true);

        MaterialPropertyBlock block;
        block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", white);

        Component[] renderers = thirdFloorDark.GetComponentsInChildren<Renderer>();
        foreach (Renderer childRenderer in renderers)
        {
            childRenderer.material = thirdFloorMaterial;
            childRenderer.SetPropertyBlock(block);
        }

    }




}
