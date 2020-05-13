using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMC : MonoBehaviour
{
    public GameObject IntMorseCode;
    public bool isEnabled = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IntMorseCode.SetActive(true);
                isEnabled = true;
            }
        }

       else if (isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IntMorseCode.SetActive(false);
                isEnabled = false;
            }
        }


    }
}
