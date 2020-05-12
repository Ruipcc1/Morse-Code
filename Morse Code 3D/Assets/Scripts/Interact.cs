using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    public GameObject Buttons;
    public GameObject InteractUI;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InteractUI.SetActive(true);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetKeyDown(KeyCode.E)) 
        {
       
                Buttons.SetActive(true);
                InteractUI.SetActive(false);
           
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Buttons.SetActive(false);
            InteractUI.SetActive(false);
        }
    }
}
