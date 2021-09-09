using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image blackFade;

    // Start is called before the first frame update
    void Start()
    {
        blackFade.canvasRenderer.SetAlpha(1.0f);

        blackFade.CrossFadeAlpha(0, 2, false);
    }

    
}
