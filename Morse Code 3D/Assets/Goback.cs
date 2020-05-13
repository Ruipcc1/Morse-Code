using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Goback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.LoadLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
