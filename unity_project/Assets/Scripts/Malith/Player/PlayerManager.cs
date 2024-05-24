using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static bool isGameOver;
    public GameObject gameOverScreen;
    public static int numberOfPoints;
    public TextMeshProUGUI pointsText;
    
    private void Awake()
    {
        numberOfPoints = PlayerPrefs.GetInt("NumberOfPoints", 0); //default value is 0
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = numberOfPoints.ToString();
        if(isGameOver)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
