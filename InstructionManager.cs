using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public GameObject[] ins;
    public Animator anim;

    public bool allowTap;
    public bool trackedMarker;

    public Joystick joystick;

    public GameObject playerObject;
    [SerializeField] private GameObject respawnPt2;
    [SerializeField] private GameObject respawnPt1;

    public GameObject jump;

    public GameObject obstacle1;
    public GameObject obstacle2;
    public GameObject obstacle3;
    CustomTrackableBehaviour m_CustomTrackableBehaviour;
    public GameObject characterPrefab;
    float lastStep, timeBetweenSteps = 0.5f;

    private bool resetPlayer1;
    private bool resetPlayer2;
    // Start is called before the first frame update
    void Start()
    {
        //m_FixedJoystick = realJoystick.GetComponent<FixedJoystick>();
        m_CustomTrackableBehaviour = FindObjectOfType<CustomTrackableBehaviour>();
        respawnPt2 = GameObject.Find("respawnPt2");
        respawnPt1 = GameObject.Find("respawnPt1");
        jump.SetActive(false);
        resetPlayer1 = false;
        resetPlayer2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        ins = GameObject.FindGameObjectsWithTag("UI");
        Debug.Log(allowTap);
        if (Input.GetMouseButtonDown(0) && allowTap || trackedMarker)
        {
            Next();
        }

        for (int i = 0; i < ins.Length; i++)
        {
            if (i > 0)
            {
                ins[i].GetComponent<CanvasGroup>().alpha = 0;
            }

            ins[0].GetComponent<CanvasGroup>().alpha = 1;
            Debug.Log("Story No: " + i);
            Debug.Log("Stories length: " + ins.Length);

            if (ins.Length <= 1)
            {
                //tick.SetActive(true);
                //next.SetActive(false);
            }
        }

        switch (ins.Length)
        {
            case 6: //0
                obstacle2.SetActive(false);
                obstacle3.SetActive(false);
                break;
            case 5: //1
                StartCoroutine(StepTwo());
                break;
            case 4: //2
                allowTap = true;
                break;
            case 3: //3
                if (Time.time - lastStep > timeBetweenSteps)
                {
                    lastStep = Time.time;
                    RespawnAt1();
                }
                allowTap = false;
                playerObject.SetActive(true);
                Destroy(obstacle1);
                obstacle2.SetActive(true);
                jump.SetActive(true);
                break;
            case 2: //4
                if (Time.time - lastStep > timeBetweenSteps)
                {
                    lastStep = Time.time;
                    RespawnAt2();
                }
                allowTap = false;
                playerObject.SetActive(true);
                Destroy(obstacle2);
                obstacle3.SetActive(true);
                break;
            case 1: //5
                allowTap = true;
                break;
        }
    }

    public void Next()
    {
        for (int i = 0; i < ins.Length; i++)
        {
            Destroy(ins[0]);
        }

        if (ins.Length <= 1)
        {
            Debug.Log("ins length is less than 1");
        }
    }

    IEnumerator StepTwo()
    {
        anim.SetBool("MoveJoystick", true);

        yield return new WaitForSeconds(3.0f);
        anim.enabled = false;
    }

    void RespawnAt1()
    {
        if (!resetPlayer1) Destroy(playerObject);
        if (playerObject == null) { 
            playerObject = Instantiate(characterPrefab);
            Transform respawn1 = respawnPt1.GetComponent<Transform>();
            playerObject.transform.SetParent(respawn1, false);
            playerObject.SetActive(true);
            playerObject.GetComponent<PlayerScript>().respawnPt = respawnPt1;
            playerObject.transform.localScale = new Vector3(0.04615531f, 0.167448f, 0.02099458f);
            resetPlayer1 = true;
        }
    }

    void RespawnAt2()
    {
        if (!resetPlayer2) Destroy(playerObject);
        if (playerObject == null)
        {
            playerObject = Instantiate(characterPrefab);
            Transform respawn2 = respawnPt2.GetComponent<Transform>();
            playerObject.transform.SetParent(respawn2, false);
            playerObject.SetActive(true);
            playerObject.GetComponent<PlayerScript>().respawnPt = respawnPt2;
            playerObject.transform.localScale = new Vector3(0.4627311f, 0.4627311f, 0.4627311f);
            resetPlayer2 = true;
        }
    }
}
