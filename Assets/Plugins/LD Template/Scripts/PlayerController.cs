using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharkUtils;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Camera MainCamera;
    public Rigidbody2D PlayerRB;
    public Animator PlayerAnimator;
    public AudioClip EndSound;
    public AudioSource ASrc;
    public Animator WalkAnimator;

    [Header("Cache")]
    private float _changeY;
    private float _changeX;
    private float _tCx, _tCy;
    private bool _facingRight;

    [Header("Movement")]
    public float YAccel;
    public float XAccel;
    public float XCap;
    public float YCap;
    public float Friction;
    public float ZeroThreshold;

    // Update is called once per frame
    void Update()
    {
        HandleMovment();
        _tCx = _changeX * 150; 
        _tCx *= Time.deltaTime;
        _tCy = _changeY * 150; 
        _tCy *= Time.deltaTime;

        ASrc.volume = (_changeX != 0 || _changeY != 0) ? 1.0f : 0.0f;
        WalkAnimator.SetBool("Walking", _changeX != 0 || _changeY != 0);

        _facingRight = (_tCx != 0) ? _tCx > 0 : PlayerAnimator.GetBool("FacingRight");

        PlayerAnimator.SetBool("FacingRight", _facingRight);
        Move();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("ViewZone"))
            return;

        GetComponent<AudioSource>().PlayOneShot(EndSound);
        LevelManager.Instance.ResetLevel();
    }

    #region Movement

    public void HandleMovment()
    {
        HandleXAxisInput();
        HandleYAxisInput();
        ZeroIn();
    }

    public void Move()
    {
        PlayerRB.AddForce(new Vector2(_tCx, _tCy), ForceMode2D.Force);
    }

    public void HandleYAxisInput()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && _changeY < YCap)
        {
            _changeY += YAccel;
        }
        else if (_changeY > 0)
        {
            _changeY -= Friction;
        }

        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && _changeY > YCap * -1)
        {
            _changeY -= YAccel;
        }
        else if (_changeY < 0)
        {
            _changeY += Friction;
        }
    }

    public void HandleXAxisInput()
    {
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && _changeX < XCap)
        {
            _changeX += XAccel;
        }
        else if (_changeX > 0)
        {
            _changeX -= Friction;
        }

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && _changeX > XCap * -1)
        {
            _changeX -= XAccel;
        }
        else if (_changeX < 0)
        {
            _changeX += Friction;
        }
    }

    public void ZeroIn()
    {
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            if (_changeX > 0 && _changeX < ZeroThreshold)
                _changeX = 0;

            if (_changeX < 0 && _changeX > ZeroThreshold * -1)
                _changeX = 0;

            if (_changeY > 0 && _changeY < ZeroThreshold)
                _changeY = 0;

            if (_changeY < 0 && _changeY > ZeroThreshold * -1)
                _changeY = 0;
        }
    }

    #endregion
}
