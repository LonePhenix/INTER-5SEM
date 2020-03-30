﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterShooter : MonoBehaviour
{

    [HideInInspector]
    public RaycastHit hit;
    public GameObject[] ShotWater;
    public Camera fpsCam;
    LayerMask Grade;



    void Update()
    {

        //direcao do disparo 
        foreach (GameObject shot in ShotWater)
        {
            shot.transform.position += Camera.main.transform.forward;

        }
        //ativacoe dos eventos  
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            if (Input.GetMouseButtonDown(0) && hit.transform.tag == "Vaso" && FindObjectOfType<Orbit>().InHand == true)
            {
                hit.transform.gameObject.GetComponent<WaterMoviment>().anim.SetBool("WaterBool", true);
                if (hit.transform.gameObject.GetComponent<WaterMoviment>().anim.speed == 0)
                    hit.transform.gameObject.GetComponent<WaterMoviment>().anim.speed = 1;

            }
            if (Input.GetMouseButtonDown(0) && hit.transform.tag == "Gramofone" && FindObjectOfType<Orbit>().InHand == true)
            {
                hit.transform.gameObject.GetComponent<WaterMoviment>().anim.SetBool("WaterBool", true);

                if (hit.transform.gameObject.GetComponent<WaterMoviment>().anim.speed == 0)
                    hit.transform.gameObject.GetComponent<WaterMoviment>().anim.speed = 1;

            }
        }

    }
}


















































