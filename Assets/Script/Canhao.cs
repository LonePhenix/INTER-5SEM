﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canhao : MonoBehaviour
{
    public GameObject posicao_inicial, posicao_final, canhao, agua, player;
    public float Angulo, TempoDePausa;
    float vel, velY, velZ;
    float posFinal, pos;
    Vector3 tragetoriaInicial, tragetoriaFinal, aguaInicial;
    Quaternion rotationOring, rotationOff;
    public bool DestinoInicial, rotation, rotationR, DestinoFinal;
    public bool RotacaoEmX, RotacaoEmY, RotacaoEmZ, movimentoOn, disparo;
    RaycastHit hit;
    Vector3 RayOring;
    float angulo;
    public LayerMask portal;
    // Start is called before the first frame update
    void Start()
    {
        rotationOring = canhao.transform.localRotation;
        rotationOff = canhao.transform.localRotation;
        aguaInicial = agua.transform.position;
        player = FindObjectOfType<player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotation)
        {
            vel = Angulo;
            rotacao();
        }
        if (rotationR)
        {
            vel = -Angulo;
            rotacaoR();
        }
        if (rotation == false && rotationR == false)
        {
            if (canhao.transform.localRotation != rotationOff)
                canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, rotationOff, .2f);
            angulo = 0;
        }
        if (disparo == true)
        {
            agua.SetActive(true);
            agua.transform.Translate(-.8f, 0, 0);
            if (Physics.Raycast(transform.GetChild(0).transform.position, transform.GetChild(0).transform.up, out hit))
            {
                if (hit.collider.tag == "Canhao")
                {
                    hit.collider.transform.parent.GetComponent<Canhao>().disparo = true;
                }
                if (hit.transform.tag == "Vaso")
                {
                    hit.transform.gameObject.GetComponent<Vaso>().animator.SetBool("WaterBool", true);
                }
                if (hit.transform.tag == "Gramofone")
                {
                    hit.transform.gameObject.GetComponent<Gramofone>().animator.SetBool("WaterBool", true);
                }
                if (hit.transform.tag == "VasoFixo")
                {
                    hit.transform.gameObject.GetComponent<VasoFixo>().gameObject.GetComponent<Animator>().SetBool("Water", true);
                    hit.transform.gameObject.GetComponent<VasoFixo>().NotWater = false;
                }
                if (hit.transform.tag == "Portal")
                {
                    GameObject outherPortal = hit.collider.gameObject.GetComponent<teleport>().reciever;
                    outherPortal.gameObject.transform.GetChild(1).GetComponent<teleport>().disparo = true;
                }
                if (hit.transform.tag == "ParedePortal")
                {
                    Debug.Log("portal");
                    player.GetComponent<player>().portalIndex++;
                    switch (player.GetComponent<player>().portalIndex)
                    {
                        case 1:
                            player.GetComponent<player>().portais[0].SetActive(true);
                            break;
                        case 2:
                            player.GetComponent<player>().portais[1].SetActive(true);
                            break;
                        case 3:
                            player.GetComponent<player>().portais[0].SetActive(true);
                            player.GetComponent<player>().portalIndex = 1;
                            break;
                    }
                    if (player.GetComponent<player>().portalIndex == 1)
                    {
                        player.GetComponent<player>().portais[0].transform.position = new Vector3(hit.rigidbody.position.x, hit.rigidbody.position.y, hit.rigidbody.position.z + .1f);
                        player.GetComponent<player>().portais[0].transform.rotation = hit.rigidbody.rotation;
                    }
                    if (player.GetComponent<player>().portalIndex == 2)
                    {
                        player.GetComponent<player>().portais[1].transform.position = new Vector3(hit.rigidbody.position.x, hit.rigidbody.position.y, hit.rigidbody.position.z + .1f);
                        player.GetComponent<player>().portais[1].transform.rotation = hit.rigidbody.rotation;
                    }
                }
                if (hit.transform.tag == "ZonaDeFrio")
                {
                    Debug.Log("gelo");
                    hit.collider.GetComponent<IceArea>().gelo.SetActive(true);
                }
            }
            Invoke("disparoR", .1f);
        }

        if (movimentoOn)
            movimento();
        if (Physics.Raycast(transform.GetChild(0).transform.position, transform.GetChild(0).transform.up, out hit))
        {
            if (hit.collider.tag == "Portal")
            {

                if (hit.collider.gameObject.GetComponent<teleport>().reciever != null)
                {

                    if (hit.collider.gameObject.GetComponent<teleport>().reciever.gameObject.transform.eulerAngles.y == 90 || hit.collider.gameObject.GetComponent<teleport>().reciever.gameObject.transform.eulerAngles.y == -90)
                    {
                        GameObject outherPortal = hit.collider.gameObject.GetComponent<teleport>().reciever;
                        outherPortal.gameObject.transform.GetChild(1).GetComponent<teleport>().rayDirecion.origin = new Vector3(outherPortal.gameObject.transform.GetChild(1).GetComponent<teleport>().rayDirecion.origin.x, hit.point.y - .45f, hit.point.z);

                    }
                    else
                    {
                        GameObject outherPortal = hit.collider.gameObject.GetComponent<teleport>().reciever;
                        outherPortal.gameObject.transform.GetChild(1).GetComponent<teleport>().rayDirecion.origin = new Vector3(outherPortal.gameObject.transform.GetChild(1).GetComponent<teleport>().rayDirecion.origin.x, hit.point.y - .45f, outherPortal.gameObject.transform.GetChild(1).GetComponent<teleport>().rayDirecion.origin.z);

                    }
                }

            }
        }
        Debug.DrawRay(transform.GetChild(0).transform.position, transform.GetChild(0).transform.up, Color.blue, 5);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(posicao_inicial.transform.position, posicao_final.transform.position);

    }
    void movimento()
    {

        if (DestinoFinal == true)
        {
            tragetoriaInicial = Vector3.Lerp(canhao.transform.position, posicao_final.transform.position, .5f);
            canhao.transform.position = tragetoriaInicial;

        }
        if (DestinoInicial == true)
        {
            tragetoriaFinal = Vector3.Lerp(canhao.transform.position, posicao_inicial.transform.position, .5f);
            canhao.transform.position = tragetoriaFinal;

        }

    }
    void rotacao()
    {

        if (RotacaoEmX == true)
        {
            canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, Quaternion.Euler(rotationOff.x + vel, rotationOff.y, rotationOff.z), .2f);
        }
        if (RotacaoEmY == true)
        {
            canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, Quaternion.Euler(rotationOff.x, rotationOff.y, rotationOff.z + vel), .2f);
        }
        if (RotacaoEmZ == true)
        {
            canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, Quaternion.Euler(rotationOff.x + vel, rotationOff.y, rotationOff.z), .2f);
        }
    }
    void rotacaoR()
    {
        if (RotacaoEmX == true)
        {
            canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, Quaternion.Euler(rotationOff.x, rotationOff.y + vel, rotationOff.z), .2f);
        }
        if (RotacaoEmY == true)
        {
            canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, Quaternion.Euler(rotationOff.x, rotationOff.y, rotationOff.z + vel), .2f);
        }
        if (RotacaoEmZ == true)
        {
            canhao.transform.localRotation = Quaternion.Lerp(canhao.transform.localRotation, Quaternion.Euler(rotationOff.x + vel, rotationOff.y, rotationOff.z), .2f);
        }
    }
    void disparoR()
    {
        disparo = false;
        agua.transform.position = canhao.transform.position;
        agua.SetActive(false);
    }
    void movimentoOff()
    {
        movimentoOn = false;
    }





}
