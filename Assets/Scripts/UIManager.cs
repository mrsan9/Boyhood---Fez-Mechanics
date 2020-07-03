using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseMenu;
    public GameObject levelDone;
    public static int redBoxes = 0;
    public int currBoxed = 8;

    private void Awake()
    {
        instance = this;
        redBoxes = 0;
    }

    public void ShowPause()
    {
        pauseMenu.SetActive(true);
        LeanTween.scale(pauseMenu.transform.GetChild(0).GetComponent<RectTransform>(),Vector3.one,0.5f).setEaseOutQuart();        
    }

    public void ResumeGame()
    {
        
        LeanTween.scale(pauseMenu.transform.GetChild(0).GetComponent<RectTransform>(), Vector3.zero, 0.5f).setEaseInBack().setOnComplete(()=> { pauseMenu.SetActive(false); });

    }

    public void ShowLevelDone()
    {
        levelDone.SetActive(true);
        LeanTween.scale(levelDone.transform.GetChild(0).GetComponent<RectTransform>(), Vector3.one, 0.5f).setEaseOutQuart();
    }

    public void NextLevel()
    {

        LeanTween.scale(levelDone.transform.GetChild(0).GetComponent<RectTransform>(), Vector3.zero, 0.5f).setEaseInBack().setOnComplete(() => { GameController.THIS.index++;RestartGame(); });

    }

    public void ExitGame()
    {
        GameController.THIS.Restart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowPause();

       
    }

    public bool LevelDoneCheck()
    {
        if (redBoxes >= currBoxed)
        {
            StartCoroutine(ShowDelayedLevelDone());
            return true;
        }
        return false;
    }

    IEnumerator ShowDelayedLevelDone()
    {
        yield return new WaitForSeconds(2f);
        ShowLevelDone();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(GameController.THIS.index);
    }
}
