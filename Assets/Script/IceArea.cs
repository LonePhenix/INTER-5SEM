﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArea : MonoBehaviour
{
    public GameObject gelo;
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && other.gameObject.GetComponent<player>().power.GetComponent<Orbit>().InHand == true)
        {
            gameObject.layer = 2;
            Instantiate(FindObjectOfType<player>().GetComponent<IceTranform>().Gelo).SetActive(true);
        }
        if (other.tag == "Vaso" && other.gameObject.GetComponent<Vaso>().animator.GetBool("WaterBool") == true)
        {
            other.gameObject.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.GetComponent<Vaso>().animator.SetBool("WaterBool", false);
            other.gameObject.GetComponent<Vaso>().congelado = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.layer = 0;
        }


    }
}
