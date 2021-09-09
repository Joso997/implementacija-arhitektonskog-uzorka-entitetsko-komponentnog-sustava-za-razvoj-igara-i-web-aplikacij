using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorHeaderScroll : MonoBehaviour
{

    public float speed;
    public Vector3 direction;
    public float min;
    public float max;
    float units = -190f;
    public float transformX;
    RectTransform rs;
    public bool reset = false;
    public static string textToScroll = "Hello Player";

    public void changeTextToScroll()
    {
        if(GetComponent<TMPro.TextMeshProUGUI>().text != textToScroll)
            GetComponent<TMPro.TextMeshProUGUI>().text = textToScroll;
    }

    void Start()
    {
        rs = GetComponent<RectTransform>();

        min = rs.anchoredPosition.x;
        max = rs.anchoredPosition.x + units;

        direction = Vector3.left;

        speed = 10f;

    }

    void Update()
    {
        changeTextToScroll();


        if (reset == false)
        {
            rs.Translate(direction * speed * Time.deltaTime);
        }

        if (rs.anchoredPosition.x <= max)
        {
            reset = true;
            rs.anchoredPosition += new Vector2(195f, 0.0f);
            reset = false;
        }
    }
}