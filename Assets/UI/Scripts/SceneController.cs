
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject transtionImg;
    public GameObject rotRef;

	public void StartNewGame()
    {
        Debug.Log("ad");
        //rotRef.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,180,0);
        LeanTween.rotateLocal(rotRef,new Vector3(0,180f,0),1f);
       // rotRef.GetComponent<RectTransform>().DORotate(new Vector3(0, 180f, 0), 1f);
        //SceneManager.LoadScene(1);
    }
    public void ContinueGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            pauseMenu.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void ShowPause()
    {
        pauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
    }
    public void ShowMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void StartLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
    void StartTransition()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(1);
    }

}
