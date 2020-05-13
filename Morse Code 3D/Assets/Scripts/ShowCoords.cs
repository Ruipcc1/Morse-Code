﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCoords : MonoBehaviour
{
    public GameObject coords;
    public GameObject EtoShow;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EtoShow.SetActive(true);

        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {

            coords.SetActive(true);
            EtoShow.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            coords.SetActive(false);
            EtoShow.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            
        }
    }
}
