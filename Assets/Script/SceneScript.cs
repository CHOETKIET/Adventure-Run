using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneScript : MonoBehaviour
{
    public static SceneScript instance;
    // Start is called before the first frame update
    public GameObject Tutorial,Setting,Upgrade,StageSelect,Main,LoginUI,RegisterUI,userInfo;
    public bool GamePause;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Awake()
    {

        /* Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;*/

        if(instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exist , destroy object!");
            Destroy(this);
        }

        Time.timeScale = 1f;
        GamePause = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Tutorial.SetActive(false);
        }*/
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePause == false)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }
    public void Play()
    {
        SceneManager.LoadScene("GamePlay");
        Time.timeScale = 1f;
    }
    public void Play2()
    {
        SceneManager.LoadScene("GamePlay2");
        Time.timeScale = 1f;
    }
    public void Play3()
    {
        SceneManager.LoadScene("GamePlay3");
        Time.timeScale = 1f;
    }
    public void Play4()
    {
        SceneManager.LoadScene("GamePlay4");
        Time.timeScale = 1f;
    }
    public void Play5()
    {
        SceneManager.LoadScene("GamePlay5");
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Tutorial_Scene()
    {
        Tutorial.SetActive(true);
        Main.SetActive(false);

    }
    public void CloseOtherWindow()
    {
        Tutorial.SetActive(false);
        Setting.SetActive(false);
        Upgrade.SetActive(false);
        StageSelect.SetActive(false);
        RegisterUI.SetActive(false);
        LoginUI.SetActive(false);
        userInfo.SetActive(false);
        Main.SetActive(true);

    }

    public void Setting_Scene()
    {
        Setting.SetActive(true);
        Main.SetActive(false);
    }

    public void Upgrade_Scene()
    {
        Upgrade.SetActive(true);
        Main.SetActive(false);
    }
    public void StageSelect_Scene()
    {
        StageSelect.SetActive(true);
        Main.SetActive(false);
    }


    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePause = true;
    }
    public void UnPause()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePause = false;
    }
    public void Login()
    {
        LoginUI.SetActive(true);
        RegisterUI.SetActive(false);
        Main.SetActive(false);
    }
    public void Register()
    {
        RegisterUI.SetActive(true);
        LoginUI.SetActive(false);
    }
    public void UserInfo()
    {
        userInfo.SetActive(true);
        Main.SetActive(false);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
