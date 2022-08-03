using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Squid : MonoBehaviour
{
    private const float jump_amount = 100f;

    private static Squid instance;
    public static Squid GetInstance()
    {
        return instance;
    }

    public event EventHandler onDied;
    public event EventHandler onStartPlaying;

    private Rigidbody2D rb2d;

    public Animator SquidAnimator;
    private State state;


    private enum State
    {
        WaitingToStart,
        Playing,
        Dead
    }

    private void Awake()
    {
        instance = this;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Jump();
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown("space") || Input.GetMouseButton(0))
                {
                    state = State.Playing;
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStartPlaying != null)
                    {
                        onStartPlaying(this, EventArgs.Empty);
                    }
                }
                break;
            case State.Playing:
                if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown("space") || Input.GetMouseButton(0))
                {
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }

    }


    public void Jump()
    {
        rb2d.velocity = Vector2.up * jump_amount;
        SquidAnimator.SetTrigger("Jump");
        SoundManager.PlaySound(SoundManager.Sound.squidJump);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        rb2d.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.Lose);
        onDied(this, EventArgs.Empty);
    }
}
