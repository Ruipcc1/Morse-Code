using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace MorseCode3D
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Header("GameManager")]
        public GameObject playerPrefab;
        public Transform Helper;
        public Transform Prisoner;
        public Transform start;
        public Player localPlayer;
        public bool inside;

        [HideInInspector]
        public Player LocalPlayer;
        // Start is called before the first frame update

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
            inside = true;

            // add player at correct spawn position
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                start = Helper.gameObject.transform;
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                start = Prisoner.gameObject.transform;
            }
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, start.position, start.rotation);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {

            }
        }
    }
}