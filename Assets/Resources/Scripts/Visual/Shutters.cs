using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutters : MonoBehaviour
{
    public void shutter10ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-10", temp);
    }
    public void shutter1ON(bool temp)
    {
        shutterAnim [0].SetBool("Shutter-1", temp);
    }
    public void shutter2ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-2", temp);
    }
    public void shutter3ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-3", temp);
    }
    public void shutter4ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-4", temp);
    }
    public void shutter5ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-5", temp);
    }
    public void shutter6ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-6", temp);
    }
    public void shutter7ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-7", temp);
    }
    public void shutter8ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-8", temp);
    }
    public void shutter9ON(bool temp)
    {
        shutterAnim[0].SetBool("Shutter-9", temp);
    }

    public Animator[] shutterAnim;

    // Start is called before the first frame update
    void Start()
    {
        shutterAnim = GetComponentsInChildren<Animator>();
        //Debug.Log(this.gameObject.name);
    }

    
}
