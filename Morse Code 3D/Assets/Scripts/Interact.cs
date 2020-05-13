using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Scripts {

    public class Interact : MonoBehaviour
    {

        public GameObject Buttons;
        public GameObject InteractUI;
        public CameraRandomizer cams;

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                InteractUI.SetActive(true);
            }
        }
        public void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
            {

                Buttons.SetActive(true);
                InteractUI.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                Buttons.SetActive(false);
                InteractUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Update()
        {
            if(cams.failState == 3)
            {
                Buttons.SetActive(false);
            }
        }
    }
}
