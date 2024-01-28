using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameStates { Playing, Paused}
    enum GoalStates { None, Win, Lose}
    
    [SerializeField]GameStates currentState;
    [SerializeField] GoalStates currentGoalState;
    [SerializeField] GameObject pauseUi;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip loseAudio;

    private BaDumTsss _player;
    
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        currentState = GameStates.Playing;
        _player = FindObjectOfType<BaDumTsss>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUi.SetActive(!pauseUi.activeSelf);
            Time.timeScale= pauseUi.activeSelf? 0f : 1.0f;
            currentState = pauseUi.activeSelf? GameStates.Paused:GameStates.Playing;
        }        
    }
    
    
    public void Restart()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    [ContextMenu("Win")]    
    public void Win()
    {
        currentGoalState = GoalStates.Win;
        _player.PlayWinDialogue(winAudio);
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    [ContextMenu("Lose")]
    public void Lose()
    {
        currentGoalState = GoalStates.Lose;
        _player.PlayLoseDialogue(loseAudio);
        loseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
