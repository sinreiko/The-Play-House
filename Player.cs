using UnityEngine;
using System.Collections;
using Vuforia;

public class Player : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;

        public Joystick joystick;

    public DefaultTrackableEventHandler trackableEventHandler;

		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

void Update ()
{
        //if (Input.GetKey ("w")) 

        //if (trackableEventHandler.myTrackableBool == true)
        //{
            if (joystick.Vertical > 0)
            {
                anim.SetInteger("AnimationPar", 1);
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }

            if (controller.isGrounded)
            {
                //moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
                moveDirection = transform.forward * joystick.Vertical * speed;
            }

            //float turn = Input.GetAxis("Horizontal");
            float turn = joystick.Horizontal;
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;
        }

//}

    public void respawnCharacter()
    {
        transform.position = Vector3.zero;
    }
}
