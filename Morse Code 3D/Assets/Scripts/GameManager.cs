﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Photon.Scripts
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Header("GameManager")]
        public GameObject playerPrefab;
        public GameObject playerPrefab2;
        public Transform Helper;
        public Transform Prisoner;
        public Player localPlayer;
        float SpawnPoint;
        public float RespawnTimer;
        public bool spawn1 = true;
        public CameraRandomizer cams;
        private FirstPersonController Controller1;
        private FirstPersonController1 Controller2;

        public GameObject player;
        public GameObject player2;

        [HideInInspector]
        public Player LocalPlayer;
        // Start is called before the first frame update

        float time;
        bool go = false;

        private void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("MainMenu");
                return;
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Master:" + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | Room Name:" + PhotonNetwork.CurrentRoom.Name);
            SpawnPlayer();
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                cams.GetCoordinates();

            }
        }

        private void Update()
        {

            if (time > 0)
            {
                time -= Time.deltaTime;

                if (RespawnTimer <= 0)
                {
                    go = true;
                }
            }

            if (RespawnTimer > 0)
            {
                RespawnTimer -= Time.deltaTime;

                if (RespawnTimer <= 0)
                {
                    cams.failState = 0;
                    SpawnPlayer1();
                    SpawnPlayer2();
                }
            }
        }

        void SpawnPlayer()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Helper.position, Helper.rotation);
                photonView.RPC("RPC_First", RpcTarget.AllBufferedViaServer, player.GetPhotonView().ViewID);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                player2 = PhotonNetwork.Instantiate(playerPrefab2.gameObject.name, Prisoner.position, Prisoner.rotation);
                time = 1;
                if (go)
                {
                    Controller1 = player.GetComponent<FirstPersonController>();
                    Controller2 = player2.GetComponent<FirstPersonController1>();
                    photonView.RPC("RPC_Spawning", RpcTarget.All, Controller1, Controller2, player2.GetPhotonView().ViewID);
                }
            }
        }

        void SpawnPlayer1()
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, Helper.position, Helper.rotation);
        }
        void SpawnPlayer2()
        {
            GameObject player2 = PhotonNetwork.Instantiate(playerPrefab2.gameObject.name, Prisoner.position, Prisoner.rotation);
        }

        [PunRPC]
        void RPC_Spawning(FirstPersonController Con1, FirstPersonController1 Con2, int Player2)
        {
            player2 = PhotonView.Find(Player2).gameObject;

        }
        [PunRPC]
        void RPC_First(int Player1)
        {
            player = PhotonView.Find(Player1).gameObject;
        }
    }
}