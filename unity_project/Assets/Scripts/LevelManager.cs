using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
     
    void Awake() 
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(gameObject);
        }

    }
    
    public async void Loadscene(string sceneName) {

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

    }

}
