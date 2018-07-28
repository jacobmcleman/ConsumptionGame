using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    GameObject aboutPanel;
    bool aboutShown;

    void Start()
    {
        aboutPanel = GameObject.Find("Canvas").transform.Find("AboutPanel").gameObject;
        aboutShown = false;

        aboutPanel.SetActive(false);
    }

    public void PlayGame()
    {
        Application.LoadLevel("canyon1");
    }

    public void ToggleAbout()
    {
        aboutShown = !aboutShown;
        aboutPanel.SetActive(aboutShown);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
