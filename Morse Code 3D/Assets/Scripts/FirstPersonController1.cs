using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;

namespace Photon.Scripts
{
    public class FirstPersonController1 : MonoBehaviourPun
    {
        public float tapTime;
        public string Code;
        public float waitTime;
        public Text text;
        public Text wordtext;
        Dictionary<string, char> _codes;
        bool waiting;
        bool waiting2;
        public string Word;
        public float endMessage;
        public Vector3 StartingPosition;
        private CameraRandomizer cams;
        public FirstPersonController Helper;
        public bool Console = true;




        [SerializeField] private float mouseSensitivity = 1f;

        private CharacterController characterController;
        private float cameraVerticalAngle;
        private float characterVelocityY;
        private Camera playerCamera;
        public Camera SecondCamera;


        private void Start()
        {
            if (!photonView.IsMine && GetComponent<CharacterController>() != null)
            {
                SecondCamera.enabled = false;
            }
            Cursor.lockState = CursorLockMode.Locked;
            StartingPosition = transform.position;
        }
        private void Awake()
        {
            cams = GameObject.Find("GameManager").GetComponent<CameraRandomizer>();
            MorseConverter();
            characterController = GetComponent<CharacterController>();
            playerCamera = transform.Find("Camera").GetComponent<Camera>();
            if (!photonView.IsMine && GetComponent<CharacterController>() != null)
            {
                Destroy(GetComponent<CharacterController>());
            }
        }

        private void Update()
        {
            bool done = false;
            if (cams.failState == 3)
            {
                if (GetComponent<PhotonView>().InstantiationId == 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (GetComponent<PhotonView>().IsMine)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                wordtext.text = Word;
                if (Console)
                {
                    MorseCode();
                }

                if (!done)
                {
                    Helper = FindObjectOfType<FirstPersonController>();
                    done = true;
                }
               
            }

            
            if (!photonView.IsMine) { return; }
            if (Console)
            {
                WaitingForInput();
            }
            
            HandleCharacterLook();
            HandleCharacterMovement();
        }

        private void HandleCharacterLook()
        {
            float lookX = Input.GetAxisRaw("Mouse X");
            float LookY = Input.GetAxisRaw("Mouse Y");

            // Rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

            // Add vertical inputs to the camera's vertical angle
            cameraVerticalAngle -= LookY * mouseSensitivity;

            // Limit the camera's vertical angle to min/max
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

            // Apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
        }

        private void HandleCharacterMovement()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            float moveSpeed = 15f;

            Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;

            if (characterController.isGrounded)
            {
                characterVelocityY = 0f;
                //Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    float jumpSpeed = 30f;
                    characterVelocityY = jumpSpeed;
                }
            }

            //Apply Gravitiy to the velocity
            float gravityDownForce = -60f;
            characterVelocityY += gravityDownForce * Time.deltaTime;

            // Apply Y velocity to move vector
            characterVelocity.y = characterVelocityY;

            // Move Character Controller
            characterController.Move(characterVelocity * Time.deltaTime);
        }

        private void MorseCode()
        {
            

            text.text = Code;

            CheckTime();
            CheckTime2();
            if (waiting)
            {
                if (waitTime >= 3)
                {
                    {
                        char result;

                        if (_codes.TryGetValue(Code, out result))
                        {
                            string newResult = "" + result;
                            Code = "";
                            photonView.RPC("RPC_SendMessage", RpcTarget.Others, newResult);
                            endMessage = 0;
                            waiting2 = true;
                        }
                        else
                            Code = "";
                        waiting = false;
                    }
                }
            }
            if (waiting2)
            {
                if (endMessage >= 10)
                {
                    photonView.RPC("RPC_NoMessage", RpcTarget.Others);
                    waiting2 = false;
                }
            }
        }

        [PunRPC]
        public void RPC_NoMessage()
        {
            Helper.Word = "";
        }

        [PunRPC]
        public void RPC_SendMessage(string message)
        {
            Helper.Word = Helper.Word + message;
        }

        public void MorseConverter()
        {
            _codes = new Dictionary<string, char>();
            _codes.Add(".-", 'A');
            _codes.Add("-...", 'B');
            _codes.Add("-.-.", 'C');
            _codes.Add("-..", 'D');
            _codes.Add(".", 'E');
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

        void CheckTime2()
        {
            endMessage = endMessage += Time.deltaTime;
        }
    }
}