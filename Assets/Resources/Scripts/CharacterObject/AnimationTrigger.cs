using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{

    public Animator anim;
    public bool ladderEndBoolean = false;
    public bool ladderEndBooleanTEST = false;

    private void Awake()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            GameObject[] gameObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(1).GetRootGameObjects();
            MainMenu.OnMenu += OnMenu;
        }
    }
    private void OnMenu(bool _enabled)
    {
        gameObject.GetComponent<Animator>().enabled = _enabled;
        gameObject.GetComponent<Movement>().enabled = _enabled;
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("playWalking", false);
    }

    public void WalkingAnim(bool start)
    {
        anim.SetBool("playWalking", start);
    }

    public void LadderAnimation(string path, bool start)
    {

        switch (path)
        {
            case "2-0":
                anim.SetBool("playLadder_Climb_3_1", start);
                break;
            case "2-1":
                anim.SetBool("playLadder_Climb_3_2", start);
                break;
            case "1-0":
                anim.SetBool("playLadder_Climb_2_1", start);
                break;
            case "1-2":
                anim.SetBool("playLadder_Climb_2_3", start);
                break;
            case "0-1":
                anim.SetBool("playLadder_Climb_1_2", start);
                break;
            case "0-2":
                anim.SetBool("playLadder_Climb_1_3", start);
                break;
        }
    }
    public void LadderAnimationEndTEST()
    {
        ladderEndBooleanTEST = true;   
    }
}
