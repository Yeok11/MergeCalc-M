using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScore, score;

    private void Start()
    {
        GameFinish();
    }

    public void GameFinish()
    {
        highScore.SetText($"High Score : {GameData.GetHScore()}");
        score.SetText($"Score : {GameData.currentScore}");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Mode-" + GameData.currentMode);
    }

    public void Home()
    {
        SceneManager.LoadScene("Home");

    }
}