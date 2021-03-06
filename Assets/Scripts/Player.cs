﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum PlayerStatus
    {
        IDLE,
        MOVE,
        FIRE
    }

    public Bullets bullet;
    public GameObject rotObj;
    public GameObject rotObj2;
    public Player player1;
    public Player player2;
    public GameObject player1Canvas;
    public GameObject player2Canvas;
    public float moveSpeed = 1.0f;
    public float power;
    public Image powerGauge;
    public Image turn1Image;
    public Image turn2Image;

    private SpriteRenderer sprite;
    private float angle;
    private Rigidbody2D rigid;
    private bool flipX = true;
    public bool isRotObjOn;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
         if (Input.GetKey(KeyCode.Space))
         {
             if (power < 1.0f)
             {
                 powerGauge.fillAmount = power;
                 power += (Time.deltaTime / 2);
             }
             else
             {
                 power = 1.0f;
                 powerGauge.fillAmount = power;
             }
         }

         if (Input.GetKeyUp(KeyCode.Space))
         {
             FireBullet(power);
             power = 0;
             powerGauge.fillAmount = power;
         }

         if(isRotObjOn == true)
         {
             rotObj.SetActive(true);
             isRotObjOn = false;
         }
    }

    void FixedUpdate()
    {   
        float h = Input.GetAxisRaw("Horizontal");
        
        Vector2 pos = transform.position;
        pos.x += h * Time.deltaTime * moveSpeed;
        transform.position = pos;
        
        FlipImage(h);
        
        if (Input.GetKey(KeyCode.W))
        {
            if (GameManager.Instance.IsPlayer1Turn)
            {
                angle += Time.deltaTime;
                rotObj.transform.Rotate(new Vector3(0, 0, angle));
            }
            else
            {
                angle -= Time.deltaTime;
                rotObj2.transform.Rotate(new Vector3(0, 0, angle));
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (GameManager.Instance.IsPlayer1Turn)
            {
                angle -= Time.deltaTime;
                rotObj.transform.Rotate(new Vector3(0, 0, angle));
            }
            else
            {
                angle += Time.deltaTime;
                rotObj2.transform.Rotate(new Vector3(0, 0, angle));
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            angle = 0;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            angle = 0;
        }
    }

    private void FlipImage(float h)
    {
        if (h < 0)
        {
            flipX = false;
        }
        else if (h > 0)
        {
            flipX = true;
        }
        else
        {
            if (GameManager.Instance.IsPlayer1Turn)
                flipX = true;
            else
                flipX = false;
        }

        if (flipX)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    public void FireBullet(float _power)
    {
        Vector3 pos = transform.position;
        pos += transform.forward * 10;
        Bullets mBullet;
        if (GameManager.Instance.IsPlayer1Turn)
        {
            mBullet = Instantiate(bullet, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            mBullet.Fire(rotObj.transform.right, power * 5 * _power);
        }
        else
        {
            mBullet = Instantiate(bullet, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
            mBullet.Fire(rotObj2.transform.right, power * 5 * _power);
        }
        GameManager.Instance.timeLimit = 10.0f;

        ChangeTurn();
    }

    public void ChangeTurn()
    {
        if (GameManager.Instance.IsPlayer1Turn)
        {
            GameManager.Instance.IsPlayer1Turn = false;
            player1.enabled = false;
            player2.enabled = true;
            rotObj.SetActive(false);
            rotObj2.SetActive(true);
            player1Canvas.SetActive(false);
            player2Canvas.SetActive(true);
            turn1Image.enabled = false;
            turn2Image.enabled = true;
        }
        else
        {
            GameManager.Instance.IsPlayer1Turn = true;
            player1.enabled = true;
            player2.enabled = false;
            rotObj.SetActive(true);
            rotObj2.SetActive(false);
            player1Canvas.SetActive(true);
            player2Canvas.SetActive(false);
            turn1Image.enabled = true;
            turn2Image.enabled = false;
        }
    }
}
