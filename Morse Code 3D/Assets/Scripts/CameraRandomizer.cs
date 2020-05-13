using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;

namespace Photon.Scripts
{
    public class CameraRandomizer : MonoBehaviourPun
    {
        [Header("SecurityCameras")]
        public GameObject Camera;
        public GameObject ButtonStorage;
        public Button[] buttons = new Button[16];
        public string[] ButtonCoord;
        Dictionary<float, char> location;

        [SerializeField]
        string[] coordinates = new string[4];

        public Vector3[] cameraPosition = new Vector3[4];
        public Text[] HelperCoordinates;
        int index = 0;
        int index2 = 0;
        Vector3 newPosition;

        [SerializeField]
        public List<string> HelperCode = new List<string>();

        public string checkCode;
        bool test;
        public int failState = 0;
        Vector3 RestartPosition;
        public float RespawnTimer;
        public bool clicked = true;
        public float clickTimer = 0;
        public GameManager gameManage;

        private void Awake()
        {
            LocationX();
            GetButtons();
        }

        public void LocationX()
        {
            location = new Dictionary<float, char>();
            location.Add(1, 'A');
            location.Add(2, 'B');
            location.Add(3, 'C');
            location.Add(4, 'D');
        }

        public void GetCoordinates()
        {
            Start:
            index = 0;
            while (true)
            {
                for (int i = 0; i < cameraPosition.Length; i++)
                {
                    newPosition = new Vector3(Random.Range(1, 5), 0, Random.Range(1, 5));

                    foreach (Vector3 x in cameraPosition)
                    {
                        if (x.Equals(newPosition))
                        {
                            goto Start;
                        }
                    }
                    if (i < cameraPosition.Length)
                    {
                        cameraPosition[index++] = newPosition;
                    }
                }
                goto Middle;
            }

            Middle:
            while (true)
            {
                char result;

                for (int i = 0; i < coordinates.Length; i++)
                {
                    if (location.TryGetValue(cameraPosition[i].x, out result))
                    {
                        coordinates[i] = "" + result + cameraPosition[i].z;
                        
                    }
                }
                photonView.RPC("RPC_Coords", RpcTarget.All, coordinates[0], coordinates[1], coordinates[2], coordinates[3]);
                goto Outer;
            }
            Outer:
            ;
        }

        public void Update()
        {
            if (clickTimer > 0)
            {
                clicked = false;
                clickTimer -= Time.deltaTime;

                if (clickTimer <= 0)
                {
                    if (!clicked)
                    {
                        failState++;
                        photonView.RPC("RPC_GameOver", RpcTarget.All, failState);
                        clicked = true;
                    }
                }
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                int currentint = i;
                buttons[i].onClick.AddListener(delegate { TaskOnClick(currentint); });
            }
            if (!test)
            {
                if (HelperCode.Count == 0)
                {
                    Debug.Log("Winning");
                    //Cameras Are Disabled
                    test = true;
                }
            }
            if (failState == 3)
            {
                if (gameManage.RespawnTimer <= 1)
                {
                    GetCoordinates();
                }
            }
        }
        [PunRPC]
        public void RPC_GameOver(int fails)
        {
            failState = fails;
        }
        [PunRPC]
        public void RPC_Coords(string cords1, string cords2, string cords3, string cords4)
        {
            HelperCode.Clear();
            
            coordinates[0] = cords1;
            coordinates[1] = cords2;
            coordinates[2] = cords3;
            coordinates[3] = cords4;

            HelperCode.Add(coordinates[0]);
            HelperCode.Add(coordinates[1]);
            HelperCode.Add(coordinates[2]);
            HelperCode.Add(coordinates[3]);

            HelperCoordinates[0].text = coordinates[0];
            HelperCoordinates[1].text = coordinates[1];
            HelperCoordinates[2].text = coordinates[2];
            HelperCoordinates[3].text = coordinates[3];
        }

        void TaskOnClick(int i)
        {
            checkCode = buttons[i].GetComponentInChildren<Text>().text;
            foreach (string z in HelperCode)
            {
                if (z.Equals(checkCode))
                {
                    HelperCode.Remove(checkCode);
                    clicked = false;
                }
                if (!z.Equals(checkCode))
                {
                    clickTimer = .1f;
                }
            }
        }

        public void GetButtons()
        {

            ButtonCoord = new string[] { "A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4" };
            buttons = ButtonStorage.GetComponentsInChildren<Button>();
            foreach (Transform child in ButtonStorage.transform)
            {
                foreach (Transform grandChild in child)
                {
                    grandChild.GetComponent<Text>().text = ButtonCoord[index2++];
                }
            }
        }
    }
}
