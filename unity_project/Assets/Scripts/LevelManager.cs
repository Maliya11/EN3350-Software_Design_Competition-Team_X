using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    /* public static LevelManager instance;

    public GameObject loadingScreen;
    public GameObject slider;

    private float adjustLoadingTime;
    private float target;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        target = 0;
        slider.value = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        while (scene.progress < 0.9f)
        {
            await Task.Delay(100);
            target = scene.progress;
        }

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        slider.value = Mathf.MoveTowards(slider.value, target, Time.deltaTime * adjustLoadingTime);
    } */
}
