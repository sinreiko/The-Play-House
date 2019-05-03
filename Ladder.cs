using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Ladder : MonoBehaviour {
	private float climbingSpeed;

	private bool isClimbingLadder = false;
	private bool isCollidingWithLadder = false;
	//private CharacterController characterController;
	private GameObject playerObject;
	private Transform ladderTransform;
    PlayerScript m_PlayerScript;

	private void Start()
	{
        //characterController = GetComponent<CharacterController>();
        m_PlayerScript = FindObjectOfType<PlayerScript>();
		playerObject = GameObject.FindWithTag("Player");
        climbingSpeed = m_PlayerScript.climbingSpeed;
    }

    private void Update()
	{
		if (isClimbingLadder)
		{
			if (!isCollidingWithLadder || m_PlayerScript.hasJumped)
			{
				ToggleLadderClimbing(false);
			}
		}
		else if (isCollidingWithLadder)
		{
			if (m_PlayerScript.joystick.Vertical > 0 || !m_PlayerScript.m_isGrounded) 
            {
                Debug.Log("Ladder climbing is true");
				ToggleLadderClimbing(true);
			}
		}
	}

	private void FixedUpdate()
	{
        if (isClimbingLadder)
        {
            if (m_PlayerScript.joystick.Vertical > 0)
            {
                Debug.Log("isClimbingLadder" +  m_PlayerScript.joystick.Vertical * climbingSpeed * Time.deltaTime);
                playerObject.GetComponent<Rigidbody>().useGravity = false;
                m_PlayerScript.gravity = 0.0f;
                playerObject.transform.position += new Vector3(0, (m_PlayerScript.joystick.Vertical * climbingSpeed * Time.deltaTime), 0);
                ////playerObject.transform.Translate(Vector3.up * m_PlayerScript.joystick.Vertical * climbingSpeed * Time.deltaTime);
                //playerObject.transform.position += Vector3.up * m_PlayerScript.joystick.Vertical * climbingSpeed * Time.deltaTime;

            }
            if (m_PlayerScript.joystick.Vertical < 0)
            {
                playerObject.transform.position += new Vector3(0,m_PlayerScript.joystick.Vertical * climbingSpeed, 0);
                //playerObject.transform.Translate(new Vector3(0, -5, 0) * Time.deltaTime * climbingSpeed);
                playerObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            m_PlayerScript.gravity = 20.0f;

        }
    }

	private void ToggleLadderClimbing(bool isEnabled)
	{
		isClimbingLadder = isEnabled;
		//playerObject.ToggleMovement(!isEnabled);
	}

	//private void OnTriggerEnter(Collider other)
	//{
	//	if (other.gameObject.tag == "Player")
	//	{
 //           Debug.Log("Enter:" + other.gameObject.tag);
 //           isCollidingWithLadder = true;
	//		ladderTransform = other.transform;
	//	}
	//}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
            Debug.Log("Exit:" + other.gameObject.tag);
            isCollidingWithLadder = false;
            playerObject.GetComponent<CharacterController>().enabled = true;
        }
    }
    private void OnTriggerStay(Collider other){
      if(other.gameObject.tag == "Player"){
            isCollidingWithLadder = true;
            ladderTransform = other.transform;
            playerObject.GetComponent<CharacterController>().enabled = false;
        }
    }
}
//public GameObject playerObject;
//public bool canClimb = false;
//public float speed = 1f;
//   PlayerScript m_PlayerScript;

//// Use this for initialization
//void Start () {
//	playerObject = GameObject.FindWithTag ("Player");
//       m_PlayerScript = GetComponent<PlayerScript>();
//}
//void OnTriggerEnter (Collider col){
//	if(col.gameObject.tag == "Player"){
//		canClimb = true;
//		Debug.Log ("is colliding");

//	}
//}
//void OnTriggerExit(Collider col){
//	if(col.gameObject.tag == "Player"){
//		canClimb = false;
//           Debug.Log("Climbing" + col.gameObject.tag);
//       }
//}

//void OnTriggerStay(Collider col){
//	if(col.gameObject.tag == "Player"){
//		canClimb = true;
//           Debug.Log("Climbing" + col.gameObject.tag);
//	}
//}

//// Update is called once per frame
//void Update () {
//	if(canClimb){
//		if(m_PlayerScript.joystick.Vertical > 0){
//			playerObject.transform.Translate(new Vector3(0,30,0) * Time.deltaTime * speed);
//			playerObject.GetComponent<Rigidbody>().useGravity = false;
//		}
//		if(m_PlayerScript.joystick.Vertical < 0)
//           {
//			playerObject.transform.Translate(new Vector3(0,-5,0) * Time.deltaTime * speed);
//			playerObject.GetComponent<Rigidbody>().useGravity = true;
//		}
//	}
//}


//}
