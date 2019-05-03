using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject home, instruction;

    public void ShowInstruction()
    {
        instruction.SetActive(true);
        home.SetActive(false);
    }

    public void ShowHome()
    {
        home.SetActive(true);
        instruction.SetActive(false);
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("game");
    }

    void Start()
    {
        instruction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
