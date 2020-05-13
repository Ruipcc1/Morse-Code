using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Photon.Scripts {

    public class Interact : MonoBehaviourPunCallbacks
    {

        public GameObject Buttons;
        public GameObject InteractUI;
        public CameraRandomizer cams;
        public FirstPersonController1 player;
        float time = 0;
        bool go = false;

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
                photonView.RPC("RPC_StopMorse", RpcTarget.All);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                Buttons.SetActive(false);
                InteractUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                photonView.RPC("RPC_ContinueMorse", RpcTarget.All);
            }
        }
        private void Start()
        {
            time = 3f;
        }

        private void Update()
        {
            if (time > 0)
            {
                time -= Time.deltaTime;

                if (time <= 0)
                {
                    go = true;
                }
            }
            if (go)
            {
                bool done = true;
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {
                    if (done)
                    {
                        player = FindObjectOfType<FirstPersonController1>();
                        done = false;
                    }
                }
                if (cams.failState == 3)
                {
                    Buttons.SetActive(false);
                }
            }
        }

        [PunRPC]
        void RPC_StopMorse()
        {
            player.Console = false;
        }
        [PunRPC]
        void RPC_ContinueMorse()
        {
            player.Console = true;
        }
    }
}
