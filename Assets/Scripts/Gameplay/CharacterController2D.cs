﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private const float _MOVESPEED = 10f;

    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField]
    private AudioSource effectAudioSource;
    [SerializeField]
    private AudioClip moveAudioClip;


    private Rigidbody2D playerRB2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector3 moveDirection;

    private bool isDashing;
    float dashCoefficent;

    float moveX = 0f, moveY = 0f;

    private void Awake()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        isDashing = false;
        dashCoefficent = 5f;
    }

    // Update is called once per frame
    private void Update()
    {
        // float moveX = Input.GetAxisRaw("Horizontal");
        // float moveY = Input.GetAxisRaw("Vertical");

        moveX = moveY = 0; 

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }

        moveDirection = new Vector3(moveX, moveY).normalized;

        if (moveDirection != Vector3.zero)
        {
            if(effectAudioSource.clip != moveAudioClip)
            {
                effectAudioSource.clip = moveAudioClip;
                effectAudioSource.Play();
            }
            animator.SetBool("isRunning", true);
        }
        else
        {
            if (effectAudioSource.clip == moveAudioClip)
            {
                effectAudioSource.clip = null;
            }
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDashing = true;
        }
    }

    private void FixedUpdate()
    {
        playerRB2D.velocity = moveDirection * _MOVESPEED;

        if (isDashing)
        {
            Vector3 dashPosition = transform.position + moveDirection * dashCoefficent;

            RaycastHit2D rayCastCollide = Physics2D.Raycast(transform.position, moveDirection, dashCoefficent, dashLayerMask);

            if (rayCastCollide.collider != null)
            {
                dashPosition = rayCastCollide.point;
            }

            playerRB2D.MovePosition(dashPosition);
            isDashing = false;
        }
    }
}
