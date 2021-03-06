﻿     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manage_ventilo : MonoBehaviour
{
    public float pressure_speed;
    private float unpress_speed;
    public GameObject NeedlePivot;
    public SpriteRenderer VaporRender;
    public SpriteRenderer HandleRender;
    public Sprite pressureSprite;
    public Sprite unpressSprite;
    public Vector3 unpressSpritePos;
    public Vector3 pressSpritePos;

    private AudioSource windNoise;

    private GameObject mm;   // Module Manager
    private bool releasing = false;

    // Use this for initialization
    void Start()
    {
        mm = GameObject.Find("ModuleManager");
        unpress_speed = pressure_speed * 2;
        windNoise = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ReleasePressure();
        }
        else
        {
            AccumulatePressure();
        }
    }

    void ReleasePressure()
    {
        if (!releasing)
        {
            windNoise.Play();
            releasing = true;
            HandleRender.sprite = unpressSprite;
            HandleRender.transform.position = unpressSpritePos;
            VaporRender.enabled = true;
        }
        if (NeedlePivot.transform.localEulerAngles.z < 122 || NeedlePivot.transform.localEulerAngles.z > 240)
        {
            NeedlePivot.transform.localEulerAngles =
                new Vector3(NeedlePivot.transform.localEulerAngles.x, NeedlePivot.transform.localEulerAngles.y,
                    NeedlePivot.transform.localEulerAngles.z + unpress_speed);
        }
    }

    void AccumulatePressure()
    {
        if (releasing)
        {
            windNoise.Stop();
            releasing = false;
            HandleRender.sprite = pressureSprite;
            HandleRender.transform.position = pressSpritePos;
            VaporRender.enabled = false;
        }
        if (NeedlePivot.transform.localEulerAngles.z < 240 || NeedlePivot.transform.localEulerAngles.z > 250)
        {
            NeedlePivot.transform.localEulerAngles =
                new Vector3(NeedlePivot.transform.localEulerAngles.x, NeedlePivot.transform.localEulerAngles.y,
                    NeedlePivot.transform.localEulerAngles.z - pressure_speed);
        }
        else
        {
            mm.SendMessage("ReceiveValidation", "VANNE FAILED");
        }
    }
}
