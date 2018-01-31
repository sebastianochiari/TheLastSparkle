﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveNew : MonoBehaviour
{

	[Header("Speed values")] 
	public float MaxThreshold = (float)Math.Log(262144, 2);
	public float MaxSpeed = 26.24f;
	
	[Header("Jump values")]
	public float JumpForce;
	
	Rigidbody2D _playerRb;
	SpriteRenderer _playerSprite;
	Animator _playerAnim;

	private float _incrSpeed;
	private double _multi=1024;
	private bool _onGround = false;
	private bool _doubleJump = true;
	private int _count = 0;
	
	void Start () {
		_playerSprite = GetComponent<SpriteRenderer> ();
		_playerRb = GetComponent<Rigidbody2D> ();
		_playerAnim = GetComponent<Animator> ();
	}
	


	private void FixedUpdate()
	{
		if (_playerRb.velocity.y < 0.0f) {
			_playerAnim.SetBool("isJumping", false);
			_playerAnim.SetBool("isFalling", true);
		}

		_playerAnim.SetBool ("isWalking", false);
		//Movement section
		if (_playerRb.velocity.magnitude > 0f)
		{
			_playerAnim.SetBool ("isWalking", true);
			_multi+=300;
			_multi = _multi % 262144;
		}
		else if (_playerRb.velocity.magnitude == 0f)
			_multi = 1024;
		_incrSpeed = (float) Math.Log(_multi, 2);
		float x = Input.GetAxis("Horizontal");
		if (x < 0)
			_playerSprite.flipX = true;
		else if (x>0)
			_playerSprite.flipX = false;
		_playerRb.AddForce(new Vector2(x, 0)*_incrSpeed);
		
		
		//Jump section
		if (Input.GetKeyDown(KeyCode.Space)&&(_onGround||_doubleJump))
		{
			_count++;
			if (_count == 2)
			{
				_count = 0;
				_doubleJump = false;
			}

			_onGround = false;
			_playerRb.AddForce(Vector2.up * JumpForce);
			_playerAnim.SetBool("isJumping", true);
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.CompareTag("Ground"))
		{
			_playerAnim.SetBool("isFalling", false);
			_playerAnim.SetBool("isJumping", false);
			_onGround = true;
			_doubleJump = true;
			_count = 0;
		}
	}
}