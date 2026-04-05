using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int totalCubes;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int value)
    {
        score += value;
        UIManager.Instance.UpdateScore(score);

        CheckWin();
    }

    public void CheckWin()
    {
        if (score >= totalCubes)
        {

            UIManager.Instance.ShowWin();
            Time.timeScale = 0f;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; 

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}