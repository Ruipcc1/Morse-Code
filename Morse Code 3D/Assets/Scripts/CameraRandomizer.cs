using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CameraRandomizer : MonoBehaviourPunCallbacks
{
    [Header("SecurityCameras")]
    public GameObject Camera;
    public GameObject ButtonStorage;
    public Button[] buttons = new Button[16];
    public string[] ButtonCoord;
    Dictionary<float, char> location;
    string[] coordinates = new string[4];
    public Vector3[] cameraPosition = new Vector3[4];
    public Text[] HelperCoordinates;
    int index = 0;
    int index2 = 0;
    Vector3 newPosition;
    public List<string> HelperCode = new List<string>();
    public string checkCode;
    bool test;
    public float failState = 0;



    public void Start()
    {
        GetCoordinates();
        HelperCoordinates[0].text = coordinates[0];
        HelperCoordinates[1].text = coordinates[1];
        HelperCoordinates[2].text = coordinates[2];
        HelperCoordinates[3].text = coordinates[3];

        HelperCode.Add(coordinates[0]);
        HelperCode.Add(coordinates[1]);
        HelperCode.Add(coordinates[2]);
        HelperCode.Add(coordinates[3]);


    }

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
            goto Outer;
        }
        Outer:
        ;
    }

    public void Update()
    {
        for (int i = 0; i < buttons.Length; i++) {
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
            //photonView.RPC("LoadMyScene", PhotonTargets.All, SceneManagerHelper.GetActiveScene().myEmptyScene);
            
        }
    }

    void TaskOnClick(int i)
    {
        checkCode = buttons[i].GetComponentInChildren<Text>().text;
        foreach (string z in HelperCode)
        {
            if (z.Equals(checkCode))
            { 
                if (!z.Equals(checkCode))
                {
                    failState++;
                }
            
                HelperCode.Remove(checkCode);
            }
        }
    }

    public void GetButtons()
    {

        ButtonCoord = new string[] { "A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4" };
        buttons = ButtonStorage.GetComponentsInChildren<Button>();
        foreach(Transform child in ButtonStorage.transform)
        {
            foreach(Transform grandChild in child)
            {
                grandChild.GetComponent<Text>().text = ButtonCoord[index2++];
            }
        }
    }
    
    public void LoadMyScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
