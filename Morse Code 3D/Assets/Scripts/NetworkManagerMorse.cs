using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[AddComponentMenu("")]
    public class NetworkManagerMorse : MonoBehaviourPunCallbacks
    {

    public Button ConnectMaster;
    public Button ConnectRoom;

    public bool TriesToConnectToMaster;
    public bool TriesToConnectToRoom;
    public bool Connected;

    bool roomCreated;

    // instance
    public static NetworkManagerMorse instance;

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // set the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
    }

    private void Update()
    {
        if (ConnectMaster != null)
        {
            ConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
        }
        if (ConnectRoom != null)
        {
            ConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
        }
    }

    public void OnClickConnectToMaster()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "PlayerName";
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "V1";

        TriesToConnectToMaster = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
        Debug.Log(cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TriesToConnectToMaster = false;
        Debug.Log("Connected To Master!");
    }

    public void OnClickConnectToRoom()
    {
        TriesToConnectToRoom = false;
        SceneManager.LoadScene("MainScene");
        if (!PhotonNetwork.IsConnected)

            return;

        TriesToConnectToRoom = true;
        PhotonNetwork.JoinRandomRoom();
    }
    

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("CreatedRoom:" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        base.OnCreateRoomFailed(returnCode, message);
        TriesToConnectToRoom = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }
}
