using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class myScript : MonoBehaviour
{
		public fight fdata;
		public float animSpeed = 1.5f;				
		public float lookSmoother = 3.0f;			
		public bool useCurves = true;
		public float useCurvesHeight = 0.5f;
		public CapsuleCollider capsuleCollider;
		public float forwardSpeed = 7.0f;
		public float sideSpeed = 7.0f;
		public float backwardSpeed = 2.0f;
		public float rotateSpeed = 5.0f;
		public float jumpPower = 3.0f;
		private CapsuleCollider col;
		private Rigidbody rb;
		private Vector3 velocity;
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;							
		private AnimatorStateInfo currentBaseState;
		private GameObject cameraObject;
		
		static int idleState = Animator.StringToHash ("Base Layer.Idle");
		static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash ("Base Layer.Jump");
		static int restState = Animator.StringToHash ("Base Layer.Rest");
		
		void Start ()
		{
			fdata.Init();
			anim = GetComponent<Animator> ();
			col = GetComponent<CapsuleCollider> ();
			rb = GetComponent<Rigidbody> ();
			cameraObject = GameObject.FindWithTag ("MainCamera");
			orgColHight = col.height;
			orgVectColCenter = col.center;
		}

		void Update()
		{
			if (fdata.IsEnd())
			{
				return;
			}
			anim.SetBool("attack1", false);
			anim.SetBool("attack2", false);
			capsuleCollider.enabled = false;
			if (Input.GetMouseButton(0))
			{
				anim.SetBool("attack1", true);
				capsuleCollider.enabled = true;
			}
			if (Input.GetMouseButton(1))
			{
				anim.SetBool("attack2", true);
				capsuleCollider.enabled = true;
			}
		}
		
		void FixedUpdate ()
		{
			anim.SetBool ("Rest", false);
			float h = Input.GetAxis ("Horizontal");				
			float v = Input.GetAxis ("Vertical");			
			anim.SetFloat ("Speed", v + Math.Abs(h));			
			anim.SetFloat ("Direction", h); 					
			anim.speed = animSpeed;								
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	
			rb.useGravity = true;
			transform.Rotate(0, h * rotateSpeed, 0);
			velocity = new Vector3 (h, 0, v);
			velocity = transform.TransformDirection (velocity);
			if (v > 0.1 || h > 0.1 || h < -0.1) {
				velocity *= forwardSpeed;		
			} else if (v < -0.1) {
				velocity *= backwardSpeed;	
			}

			if (Input.GetButtonDown ("Jump")) {
				if (currentBaseState.nameHash == locoState) {
					if (!anim.IsInTransition (0)) {
						rb.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool ("Jump", true);	
					}
				}
			}
			
			transform.localPosition += velocity * Time.fixedDeltaTime;
			
			if (currentBaseState.nameHash == locoState) {
				if (useCurves) {
					resetCollider ();
				}
			}
			else if (currentBaseState.nameHash == jumpState) {
				if (!anim.IsInTransition (0)) {
					if (useCurves) {
						float jumpHeight = anim.GetFloat ("JumpHeight");
						float gravityControl = anim.GetFloat ("GravityControl"); 
						if (gravityControl > 0)
							rb.useGravity = false;
						Ray ray = new Ray (transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit ();
						if (Physics.Raycast (ray, out hitInfo)) {
							if (hitInfo.distance > useCurvesHeight) {
								col.height = orgColHight - jumpHeight;			
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3 (0, adjCenterY, 0);
							} else {
								resetCollider ();
							}
						}
					}
					anim.SetBool ("Jump", false);
				}
			}
			else if (currentBaseState.nameHash == idleState) {
				if (useCurves) {
					resetCollider ();
				}
				if (Input.GetButtonDown ("Jump")) {
					anim.SetBool ("Rest", true);
					Behaviour halo = (Behaviour)GetComponent("Halo");
					halo.enabled = true;
				}
			}
			else if (currentBaseState.nameHash == restState) {
				if (!anim.IsInTransition (0)) {
					anim.SetBool ("Rest", false);
				}
			}
		}

		void OnTriggerEnter(Collider other)
		{
			
			if(fdata.IsEnd()){return;}
			if (other.tag == "Weapon")
			{
				var swordHalo = (Behaviour) GameObject.Find("katana").GetComponent("Halo");
				swordHalo.enabled = true;
				fdata.p1Attack();
				print("aaaaaaaaaa");
				//キャラ２体目をここに書く
			}
		}

		void OnTriggerExit(Collider other)
		{
			AllOffHalo();
			if (fdata.IsEnd())
			{
				Finish();
			}
		}

		void AllOffHalo()
		{
			var swordHalo = (Behaviour) GameObject.Find("katana").GetComponent("Halo");
			swordHalo.enabled = false;
		}

		void Finish()
		{
			Debug.Log(fdata.Win());
		}

		void OnGUI ()
		{
			GUI.Box (new Rect (Screen.width - 260, 10, 250, 150), "Interaction");
			GUI.Label (new Rect (Screen.width - 245, 30, 250, 30), "Up/Down Arrow : Go Forwald/Go Back");
			GUI.Label (new Rect (Screen.width - 245, 50, 250, 30), "Left/Right Arrow : Turn Left/Turn Right");
			GUI.Label (new Rect (Screen.width - 245, 70, 250, 30), "Hit Space key while Running : Jump");
			GUI.Label (new Rect (Screen.width - 245, 90, 250, 30), "Hit Spase key while Stopping : Rest");
			GUI.Label (new Rect (Screen.width - 245, 110, 250, 30), "Left Control : Front Camera");
			GUI.Label (new Rect (Screen.width - 245, 130, 250, 30), "Alt : LookAt Camera");
		}
		
		void resetCollider ()
		{
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}
}
