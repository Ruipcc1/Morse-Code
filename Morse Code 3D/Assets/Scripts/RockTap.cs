using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RockTap : MonoBehaviour
{
    public float tapTime;
    public string Code;
    public float waitTime;
    public Text text;
    Dictionary<string, char> _codes;
    bool waiting;
    public string Word;
    public Text wordtext;

    // Start is called before the first frame update
    void Start()
    {
        MorseConverter();
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForInput();
        text.text = Code;
        wordtext.text = Word;
        CheckTime();
        if (waiting) {
            if (waitTime >= 3)
            {
                {
                    char result;

                    if (_codes.TryGetValue(Code, out result))
                    {
                        text.text = "" + result;
                        Word = Word + result;
                        Code = "";
                    }
                    else
                        Code = "";
                    waiting = false;
                }
            }
        }
    }
              

    public void MorseConverter()
    {
        _codes = new Dictionary<string, char>();
        _codes.Add(".-", 'A');
        _codes.Add("-...", 'B');
        _codes.Add("-.-.", 'C');
        _codes.Add("-..", 'D');
        _codes.Add(".",'E');
        _codes.Add("..-.", 'F');
        _codes.Add("--.", 'G');
        _codes.Add("....", 'H');
        _codes.Add("..", 'I');
        _codes.Add(".---", 'J');
        _codes.Add("-.-", 'K');
        _codes.Add(".-..", 'L');
        _codes.Add("--", 'M');
        _codes.Add("-.", 'N');
        _codes.Add("---", 'O');
        _codes.Add(".--.", 'P');
        _codes.Add("--.-", 'Q');
        _codes.Add(".-.", 'R');
        _codes.Add("...", 'S');
        _codes.Add("-", 'T');
        _codes.Add("..-", 'U');
        _codes.Add("...-", 'V');
        _codes.Add(".--", 'W');
        _codes.Add("-..-", 'X');
        _codes.Add("-.--", 'Y');
        _codes.Add("--..", 'Z');
        _codes.Add(".----", '1');
        _codes.Add("..---", '2');
        _codes.Add("...--", '3');
        _codes.Add("....-", '4');
        _codes.Add(".....", '5');
        _codes.Add("-....", '6');
        _codes.Add("--...", '7');
        _codes.Add("---..", '8');
        _codes.Add("----.", '9');
        _codes.Add("-----", '0');
    }
    void WaitingForInput()
    {
        if (Input.GetMouseButton(0))
        {
            tapTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            MorseDotDash();
        }

    }

    void MorseDotDash()
    {
        if (tapTime < 0.3f)
        {
            Code = Code + ".";
            waitTime = 0;
            tapTime = 0;
            waiting = true;
        }
        else if (tapTime >= 0.3f)
        {
            Code = Code + "-";
            waitTime = 0;
            tapTime = 0;
            waiting = true;
        }
    }

    void CheckTime()
    {
        waitTime = waitTime += Time.deltaTime;
    }

    
}
