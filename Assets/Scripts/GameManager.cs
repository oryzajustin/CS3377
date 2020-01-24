using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameManager _instance;

    public GameManager instance 
    { 
        get
        {
            return _instance;
        } 
    }

    private void Awake()
    {
        if (this._instance != null && this._instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this._instance = this;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
