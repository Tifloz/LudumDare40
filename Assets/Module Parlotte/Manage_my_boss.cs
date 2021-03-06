﻿using System;
using System.Collections;
using UnityEngine;

public class Manage_my_boss : MonoBehaviour
{
    #region "Variables"

    public SpriteRenderer Boss;
    public int Temps_Enrage_min;
    public int Temps_Enrage_max;
    public float Multiplicateur;
    public float Diviseur;
    private GameObject mm;
    public AudioClip[] Audios = new AudioClip[2];
    private int T_Angry;
    private float Spam = 10;
    private float uni_timer;
    private AudioSource A_source;
    private AudioSource A_source2;
    private int Counter = 0;
    private float tmp;
    private System.Random random = new System.Random();
    private System.Random R_Angry = new System.Random();
    private bool Angry = false;

    private Vector3 base_position;
    private bool animUp;
    #endregion

    private int RandomNumber(int min, int max)
    {
        return R_Angry.Next(min, max);
    }
    // Use this for initialization
    void Start()
    {
        mm = GameObject.Find("ModuleManager");
        T_Angry = RandomNumber(Temps_Enrage_min, Temps_Enrage_max);
        tmp = T_Angry;
        Boss = GetComponentInChildren<SpriteRenderer>();
        base_position = Boss.transform.position;
        A_source = GetComponentInChildren<AudioSource>();
        A_source2 = GetComponentInChildren<AudioSource>();
        Boss.enabled = false;
        SetRespawnTimer();
    }

    private void SetRespawnTimer()
    {
        uni_timer = random.Next(6, 8);
    }

    IEnumerator WaitBossDeath(int min, int max)
    {
        int i = 0;

        while (Boss.enabled)
        {
            yield return new WaitForSeconds(0.2f);
            i++;
            BossAlive();
            if (i >= tmp)
            {
                Angry = true;
                if (!A_source.isPlaying)
                    A_source.Play();
            }
        }
        A_source.Stop();
        Counter = 0;
        SetRespawnTimer();
    }

    void SpawnBoss()
    {
        Spam *= Multiplicateur;
        Spam = (int)Spam;
        tmp /= Diviseur;
        Boss.enabled = true;
        StartCoroutine(LiftUpBoss());
        StartCoroutine(WaitBossDeath(Temps_Enrage_min, Temps_Enrage_max));

    }

    IEnumerator LiftUpBoss()
    {
        while (Boss.transform.localPosition.y < -2.10)
        {
            Boss.transform.Translate(new Vector3(0, 0.1f, 0));
            yield return new WaitForSeconds(0.01f);
        }
        while (Boss.enabled)
        {
            Debug.Log("Boss");
            while (Boss.transform.localPosition.y > -2.80 && Boss.enabled)
            {
                Boss.transform.Translate(new Vector3(0, -0.2f, 0));
                yield return new WaitForSeconds(0.01f);
            }
            while (Boss.transform.localPosition.y < -2.10 && Boss.enabled)
            {
                Boss.transform.Translate(new Vector3(0, 0.2f, 0));
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    void BossAlive()
    {
        if (Counter >= Spam)
        {
            Boss.transform.position = base_position;
            Boss.enabled = false;
            Angry = false;
            mm.SendMessage("ReceiveValidation", "BOSS SUCCED");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (uni_timer <= 0 && !Boss.enabled)
            SpawnBoss();
        if (Input.GetKeyUp(KeyCode.S) && Boss.enabled)
        {
            if (A_source2.isPlaying == false)
            {
                A_source2.PlayOneShot(Audios[RandomNumber(0, Audios.Length)]);
            }
            Counter++;
        }
        else
            uni_timer -= Time.deltaTime;
    }
}
