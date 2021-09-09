using Manager.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroOutside : SceneTransitionManager
{
    public Animator[] introAnim;
    public GameObject node;
    public Image blackFade;
    public float t = 0;
    public GameObject player;
    public Movement movement;
    bool fadeIn = false;


    // Start is called before the first frame update
    void Start()
    {
        introAnim = GetComponentsInChildren<Animator>();

        blackFade.canvasRenderer.SetAlpha(1.0f);

        movement = player.GetComponent<Movement>();
        StartCoroutine(StartCutScene());
    }

    IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(1);
        FadeOut();
        introAnim[0].SetBool("playElevator", true);
    }

    void StartWalk()
    {
        GetComponent<Animator>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().Initialize(node);
        StartCoroutine(FadeIn());
    }

    void FadeOut ()
    {
        blackFade.CrossFadeAlpha(0, 2, false);
    }

    IEnumerator FadeIn ()
    {
        yield return new WaitUntil(() => movement.enabled == false);
        blackFade.CrossFadeAlpha(1, 2, false);
        Continue(2);
    }
}
