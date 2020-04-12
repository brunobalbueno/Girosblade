﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject personagem;
    public GameObject foice;
    public GameObject foicePivot, foicePivot2;
    public bool rotating = false;
    public float rotate = 90;
    public bool right = true;
    public int rotating90 = 0;
    public GameObject girospot;
    public bool colliding = false;
    private int speed = 10;
    bool rotatingGirospot = false;

    Vector3 initialPosition;
    Quaternion initialRotation;

    Vector3 corpoInitialPosition;
    Quaternion corpoInitialRotation;

    public Vector3 direction = new Vector3(0,0,0);
    Vector3 esquerda = new Vector3(-1, 0, 0);
    Vector3 direita = new Vector3(1, 0, 0);
    Vector3 cima = new Vector3(0, 0, 1);
    Vector3 baixo = new Vector3(0, 0, -1);

    bool paredeCol = false;
    // Start is called before the first frame update
    void Start()
    {
        rotate = 90;
        colliding = false;
        rotating = false;
        rotatingGirospot = false;
        initialPosition = foice.transform.localPosition;
        initialRotation = foice.transform.localRotation;

        corpoInitialPosition = transform.localPosition;
        corpoInitialRotation = transform.localRotation;
        
    }

    void Update()
    {
        //gameObject.GetComponent<CapsuleCollider>().tr

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = esquerda;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = direita;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = cima;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = baixo;
        }

        if (!rotating && !rotatingGirospot)
        {
            if (right) {
                //StartCoroutine(RotateAround(Vector3.up, 360, 1f, foice, personagem));
                foice.transform.RotateAround(personagem.transform.position,Vector3.up, 360 * Time.deltaTime);
            }
            else
            {
                //StartCoroutine(RotateAround(Vector3.up, -360, 1f, foice, personagem));
                //RotateAround(Vector3.up, -360, 1f, foice, personagem);
                foice.transform.RotateAround(personagem.transform.position, Vector3.up, -360 * Time.deltaTime);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && colliding)
        {
            rotatingGirospot = !rotatingGirospot;
            if (rotatingGirospot)
            {
                transform.localPosition = corpoInitialPosition;
                transform.localRotation = corpoInitialRotation;
                foice.transform.localRotation = initialRotation;
                foice.transform.localPosition = initialPosition;
                
                //transform.SetParent(girospot.transform);
                if (right)
                {
                    transform.position = girospot.GetComponent<Girospot>().getP1();
                }
                else
                {
                    foice.transform.Rotate(0, 0, 180);
                    transform.position = girospot.GetComponent<Girospot>().getP2();
                }
                girospot.GetComponent<Girospot>().PlayerConectado(right, gameObject);
            }
            else
            {
                //transform.SetParent(null);
                girospot.GetComponent<Girospot>().PlayerSolto();
                direction = gameObject.transform.localRotation * -Vector3.forward;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foice.transform.Rotate(0, 0, 180);
            right = !right;
        }
        if(!rotatingGirospot)
        Move();
    }



    IEnumerator RotateAround(Vector3 axis, float angle, float duration, GameObject p_me, GameObject p_object)
    {
        float elapsed = 0.0f;
        float rotated = 0.0f;
        while (elapsed < duration)
        {
            float step = angle / duration * Time.deltaTime;
            p_me.transform.RotateAround(p_object.transform.position, axis, step);
            elapsed += Time.deltaTime;
            rotated += step;
            rotating = true;
            yield return null;
        }
        p_me.transform.RotateAround(p_object.transform.position, axis, angle - rotated);
        rotating = false;
    }
    public void Move()
    {

        //Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.position += direction * speed * Time.deltaTime;


    }
    public void ColisaoParede(Vector3 dir)
    {
        direction = new Vector3(direction.x * dir.x, 0, direction.z * dir.z);
        foice.transform.Rotate(0, 0, 180);
        right = !right;
        paredeCol = true;
    }
    public void SaiuParede()
    {
        paredeCol = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        
        if (other.gameObject.tag == "Girospot")
        {
            girospot = other.gameObject;
            colliding = true;
        }
        if(other.gameObject.tag == "Parede" && !paredeCol)
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Girospot")
        {
            girospot = null;
            colliding = false;
        }
        if (other.gameObject.tag == "Parede")
        {
        }
    }
        //private void OnTriggerEnter(Collision collision)
        //{
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    if (collision.gameObject.tag == "Girospot")
        //    {
        //        girospot = null;
        //        colliding = false;
        //    }
        //}
    }
