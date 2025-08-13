using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    private void Start()
    {
        GameData.Init();
    }

    public void Play()
    {
        SceneManager.LoadScene("Mode-Live");
    }
}
