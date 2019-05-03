using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class PlayerScript : MonoBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }

    private Animator anim;
    public CharacterController controller;
    private Rigidbody rb;

    [SerializeField] private float speed = 0.50f;
    [SerializeField] private float turnSpeed = 150.0f;
    public float gravity = 10.0f;
    public float climbingSpeed = 0.5f;
    [SerializeField] private float m_jumpForce = 0.10f;

    public Joystick joystick;

    [SerializeField] private CustomTrackableBehaviour trackableEventHandler;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;
    public GameObject respawnPt;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_runScale = 6.0f;
    private readonly float m_walkScale = 4.0f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;
    private Vector3 m_oldDirection;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    public bool m_isGrounded;
    public bool hasJumped;
    private float stayCount = 0.0f;
    float lastStep, timeBetweenSteps = 0.5f;


    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        trackableEventHandler = FindObjectOfType<CustomTrackableBehaviour>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        anim = gameObject.GetComponentInChildren<Animator>();
        hasJumped = false;
        //m_isGrounded = true;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -20.0f)
        {
            transform.position = respawnPt.transform.position;
        }
    }

    void Update()
    {
        Debug.Log("isGrounded: " + m_isGrounded);

        anim.SetBool("Grounded", m_isGrounded);
        m_wasGrounded = m_isGrounded;

        float v = joystick.Vertical;
        float h = joystick.Horizontal;

        if (v > 0)
        {
            anim.SetFloat("MoveSpeed", v * m_runScale);
        }
        else if (v < 0)
        {
            anim.SetFloat("MoveSpeed", v * m_backwardRunScale);
        }
        else
        {
            anim.SetFloat("MoveSpeed", m_currentV);

        }

        if (controller.isGrounded)
        {
            //moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
            //moveDirection = transform.forward * joystick.Vertical * speed;
            m_currentDirection = new Vector3(h, 0.0f, v);
            //m_currentDirection = transform.TransformDirection(m_currentDirection);
            //m_currentDirection = m_currentDirection * speed;
            //transform.rotation = Quaternion.Slerp(transform.rotation, m_currentDirection, turnSpeed * Time.deltaTime);
            if (h != 0 || v != 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(m_currentDirection, Vector3.up);
                float step = turnSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, step);
                m_oldDirection = m_currentDirection;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(m_oldDirection);
            }
        }

        //float turn = Input.GetAxis("Horizontal");
        float turn = joystick.Horizontal;
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        //controller.Move(m_currentDirection * Time.deltaTime, Space.World);
        controller.transform.TransformDirection(Vector3.forward);
        //m_currentDirection.y -= gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - lastStep > timeBetweenSteps)
            {
                lastStep = Time.time;
                Jump();
            }
        }

        JumpingAndLanding();

    }

    public void Jump()
    {
        hasJumped = true;
    }

    public void JumpingAndLanding() {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;
        if (jumpCooldownOver && m_isGrounded)
        {
            if (hasJumped)
            {
                m_currentDirection.y = m_jumpForce;
                hasJumped = false;
            }
            //Debug.Log("jumped");
            //rb.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);

        }

        if (!m_wasGrounded && m_isGrounded)
        {
            anim.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            anim.SetTrigger("Jump");
        }

        m_currentDirection.y = m_currentDirection.y - (gravity * Time.deltaTime);
        controller.Move(m_currentDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Ground" || other.gameObject.transform.tag == "Obstacle")
        {
            Debug.Log("Enter:" + other.gameObject.tag);
            Debug.Log("Trigger Entered");
            m_isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.transform.tag == "Ground" || other.gameObject.transform.tag == "Obstacle")
        {
            Debug.Log("Stay:" + other.gameObject.tag);
            if (stayCount > 0.25f)
            {
                stayCount = stayCount - 0.25f;
                m_isGrounded = true;
            }
            else
            {
                stayCount = stayCount + Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Ground" || other.gameObject.transform.tag == "Obstacle")
        {
            Debug.Log("Exit:" + other.gameObject.tag);
            m_isGrounded = false;
        }
    }
}
