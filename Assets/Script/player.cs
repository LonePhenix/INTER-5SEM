﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class player : MonoBehaviour
{
    Vector3 input, velocity;
    LayerMask water, Puzzle, portal;
    [HideInInspector]
    public bool jumpBool, vision, take;
    public float vel, Sensitivity;
    [HideInInspector]
    public float x, y, maxX, Jump, maxJump, gravidade;
    [HideInInspector]
    public Rigidbody rigidbodyPlayer;
    [HideInInspector]
    public Transform hand, cameraTransform, checkGround;
    Animator setAnimacao;
    public LayerMask JumpLayerMask;
    public GameObject power, handTrue;
    [HideInInspector]
    public GameObject Ice;
    [HideInInspector]
    public Vector3 CameraController;
    [HideInInspector]
    public int index, portalIndex, ShotIndex, animationIndex;
    public GameObject[] portais;
    public GameObject[] shotWater;
    CharacterController characterController;
    public Ray eyes;
    public RaycastHit hit;
    [HideInInspector]
    public bool inHand;
    Rigidbody hitPortal;
    public Image[] cursor;
    GameObject objectsMove;
    public CanvasRenderer Menu, load;
    public static player Instance;
    public bool CutsceneMode;


    private void Awake()
    {
        Instance = this;
        rigidbodyPlayer = GetComponent<Rigidbody>();
        setAnimacao = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        inHand = power.GetComponent<Orbit>().InHand;
        power = GameObject.FindGameObjectWithTag("Power");
        Ice = GameObject.FindGameObjectWithTag("Ice");
        checkGround = GameObject.FindGameObjectWithTag("CheckGraound").transform;
        water = LayerMask.GetMask("Water");
        Puzzle = LayerMask.GetMask("Puzzle");
        portal = LayerMask.GetMask("Portal");
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        handTrue = GameObject.FindGameObjectWithTag("HandTrue");
        cameraTransform = Camera.main.transform;
        Jump = 3f;
        index = -1;
        animationIndex = -1;
        gravidade = -9.81f;
        ShotIndex = -1;
        characterController.detectCollisions = false;

    }
    void Update()
    {
        transform.GetChild(2).transform.gameObject.SetActive(!CutsceneMode);
        if (CutsceneMode) return;
        if (Menu.gameObject.activeInHierarchy == false && load.gameObject.activeInHierarchy == false)
        {
            MouseConfi();
            inputs();
            Config();
            interacao();

        }





    }
    private void LateUpdate()
    {

    }
    private void FixedUpdate()
    {
        //if (Input.GetButtonDown("Jump"))
        //    rigidbodyPlayer.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
    }
    //configuracao do mose
    void MouseConfi()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cursor[0].enabled = true;
        cursor[1].enabled = false;
    }
    //interacao com objetos 
    void interacao()
    {
        //direcao da ray de visao
        eyes = Camera.main.ScreenPointToRay(Input.mousePosition);
        //atira 
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(cameraTransform.position, cameraTransform.transform.forward, 10, water) == false && power.GetComponent<Orbit>().InHand == true)
        {
            ShotIndex = 4;
            if (ShotIndex < 5)
            {
                shotWater[ShotIndex].transform.position = hand.transform.position;
                shotWater[ShotIndex].SetActive(true);
            }
            index = -1;
        }
        //interacao com a agua
        if (Physics.Raycast(cameraTransform.position, cameraTransform.transform.forward, 1.5f, water))
        {
            cursor[0].enabled = false;
            cursor[1].enabled = true;
            if (Input.GetMouseButtonDown(1) && take == false)
            {
                ShotIndex = 0;
                power.transform.position = transform.position * 2;
                index = 4;
            }
        }
        //Ray para indentificar objetos que podem ser movidos
        if (Physics.Raycast(eyes, out hit, 2) && hit.transform.GetComponent<TakeObject>() != null)
        {
            cursor[0].enabled = false;
            cursor[1].enabled = true;
            if (take == false)
                objectsMove = hit.transform.gameObject;
        }
        //objetos que podem ser movidos
        if (Input.GetKeyDown(KeyCode.E) && Vector3.Distance(objectsMove.transform.position, transform.position) < 2.5f)
        {
            objectsMove.GetComponent<TakeObject>().take = !objectsMove.GetComponent<TakeObject>().take;
            take = !take;

        }
        //intercao com os portais
        if (Physics.Raycast(hand.position, hand.transform.forward, 20, portal))
        {
            if (Physics.Raycast(eyes, out hit) && Input.GetMouseButtonDown(0) && FindObjectOfType<Orbit>().InHand == true)
            {
                portalIndex++;
                switch (portalIndex)
                {
                    case 1:
                        portais[0].SetActive(true);
                        break;
                    case 2:
                        portais[1].SetActive(true);
                        break;
                    case 3:
                        portais[0].SetActive(true);
                        portalIndex = 1;
                        break;
                }
                if (portalIndex == 1)
                {
                    portais[0].transform.position = hit.rigidbody.position;
                    portais[0].transform.rotation = hit.rigidbody.rotation;
                }
                if (portalIndex == 2)
                {
                    portais[1].transform.position = hit.rigidbody.position;
                    portais[1].transform.rotation = hit.rigidbody.rotation;
                }
            }

        }
        // Ativacao da orbis 
        switch (index)
        {
            case -1:
                power.SetActive(true);
                power.GetComponent<Orbit>().Orbis[0].SetActive(false);
                power.GetComponent<Orbit>().Orbis[1].SetActive(false);
                power.GetComponent<Orbit>().Orbis[2].SetActive(false);
                power.GetComponent<Orbit>().Orbis[3].SetActive(false);
                power.GetComponent<Orbit>().Orbis[4].SetActive(false);
                StartCoroutine(ShotInHand());
                break;
            case 0:
                power.GetComponent<Orbit>().InHand = true;
                power.GetComponent<Orbit>().Orbis[0].SetActive(true);
                power.GetComponent<Orbit>().Orbis[1].SetActive(false);
                power.GetComponent<Orbit>().Orbis[2].SetActive(false);
                power.GetComponent<Orbit>().Orbis[3].SetActive(false);
                power.GetComponent<Orbit>().Orbis[4].SetActive(false);
                break;
            case 1:
                power.GetComponent<Orbit>().InHand = true;
                power.GetComponent<Orbit>().Orbis[0].SetActive(true);
                power.GetComponent<Orbit>().Orbis[1].SetActive(true);
                power.GetComponent<Orbit>().Orbis[2].SetActive(false);
                power.GetComponent<Orbit>().Orbis[3].SetActive(false);
                power.GetComponent<Orbit>().Orbis[4].SetActive(false);
                break;
            case 2:
                power.GetComponent<Orbit>().InHand = true;
                power.GetComponent<Orbit>().Orbis[0].SetActive(true);
                power.GetComponent<Orbit>().Orbis[1].SetActive(true);
                power.GetComponent<Orbit>().Orbis[2].SetActive(true);
                power.GetComponent<Orbit>().Orbis[3].SetActive(false);
                power.GetComponent<Orbit>().Orbis[4].SetActive(false);
                break;
            case 3:
                power.GetComponent<Orbit>().InHand = true;
                power.GetComponent<Orbit>().Orbis[0].SetActive(true);
                power.GetComponent<Orbit>().Orbis[1].SetActive(true);
                power.GetComponent<Orbit>().Orbis[2].SetActive(true);
                power.GetComponent<Orbit>().Orbis[3].SetActive(true);
                power.GetComponent<Orbit>().Orbis[4].SetActive(false);
                break;
            case 4:
                power.GetComponent<Orbit>().InHand = true;
                power.GetComponent<Orbit>().Orbis[0].SetActive(true);
                power.GetComponent<Orbit>().Orbis[1].SetActive(true);
                power.GetComponent<Orbit>().Orbis[2].SetActive(true);
                power.GetComponent<Orbit>().Orbis[3].SetActive(true);
                power.GetComponent<Orbit>().Orbis[4].SetActive(true);
                StopCoroutine(ShotInHand());
                break;
        }
    }
    // configuracao para Gameplayer
    void Config()
    {
        // numero maximo de bolhas d'agua
        if (index >= 4)
        {
            index = 4;
        }
        // numero minimo de disparos
        if (index <= -1)
        {
            index = -1;
        }
        if (ShotIndex >= 5)
        {
            ShotIndex = 5;
        }
        // numero minimo de dissparos
        if (ShotIndex <= -1)
        {
            ShotIndex = -1;
        }
        if (animationIndex > 5)
        {
            animationIndex = -1;
        }
        // numero minimo de dissparos
        if (animationIndex < -1)
        {
            animationIndex = 5;
        }
        if (CameraController.x > 45)
        {
            CameraController.x = 45;
        }
        if (CameraController.x < -60)
        {
            CameraController.x = -60;
        }

    }
    //Inputs de comandos da Gameplay
    void inputs()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");


        if (isGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }





        maxX = Mathf.Clamp(CameraController.x, -60, 45);
        CameraController += new Vector3(Input.GetAxis("Mouse Y") * Sensitivity, Input.GetAxis("Mouse X") * Sensitivity, 0);
        input = (x * cameraTransform.right + y * cameraTransform.forward) * vel * Time.deltaTime;
        characterController.Move(input * vel * Time.deltaTime);
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(Jump * -2 * gravidade);
        }
        velocity.y += gravidade * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        cameraTransform.transform.localRotation = Quaternion.Euler(-maxX, CameraController.y, 0);
        transform.rotation = Quaternion.Euler(0, CameraController.y, 0);
    }
    // verifica se o personagem está no ar; 
    bool isGrounded()
    {
        return Physics.CheckSphere(checkGround.position, .2f, JumpLayerMask);
    }
    IEnumerator ShotInHand()
    {
        yield return new WaitForSeconds(0.5f);
        power.GetComponent<Orbit>().InHand = false;
    }


}