using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatches : MonoBehaviour
{

    public Animator hatchOne;
    public Animator hatchTwo;
    public Animator HatchThree;

    public bool hatchOneON = false;
    public bool hatchTwoON = false;
    public bool hatchThreeON = false;


    // Start is called before the first frame update
    void Start()
    {
        hatchOne = GameObject.Find("Circle.001").GetComponent<Animator>();
        hatchTwo = GameObject.Find("Circle").GetComponent<Animator>();
        HatchThree = GameObject.Find("Circle.002").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hatchOneON == true)
        {
            hatchOne.SetBool("First-Floor-Hatch-ON", true);
        }
        else
        {
            hatchOne.SetBool("First-Floor-Hatch-ON", false);
        }

        if (hatchTwoON == true)
        {
            hatchTwo.SetBool("Second-Floor-Hatch-ON", true);
        }
        else
        {
            hatchTwo.SetBool("Second-Floor-Hatch-ON", false);
        }

        if (hatchThreeON == true)
        {
            HatchThree.SetBool("Third-Floor-Hatch-ON", true);
        }
        else
        {
            HatchThree.SetBool("Third-Floor-Hatch-ON", false);
        }
    }
}
