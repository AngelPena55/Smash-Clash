using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject start;
    public GameObject menu;

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && start.activeInHierarchy)
        {
            start.SetActive(false);
            menu.SetActive(true);
    }

    }
}
