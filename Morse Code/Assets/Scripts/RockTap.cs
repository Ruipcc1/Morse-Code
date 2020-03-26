using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTap : MonoBehaviour
{
    public float tapTime;
    public bool tap;
    public bool dash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) {
            tapTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            tapTime = 0f;
            MorseDotDash();
        }
    }

    void MorseDotDash()
    {
        if(tapTime < 0.5f)
        {

        }
    }
}
