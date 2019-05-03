using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneController : MonoBehaviour
{
    Scene currentScene;
    public string sceneName;
    public bool gameStart;
    public bool gameReset;
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        gameReset = false;
        gameStart = false;
    }

    private void Update()
    {
        if (sceneName == "Tutorial" )
        {
        }
    }

    public void OnGameStart()
    {
        gameStart = true;
        gameReset = false;
    }

    public void OnGameReset()
    {
        gameReset = true;
        gameStart = false;
    }


    //ToggleAR m_ToggleAR;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    m_ToggleAR = FindObjectOfType<ToggleAR>();
    //    m_ToggleAR.CloseAR();
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
