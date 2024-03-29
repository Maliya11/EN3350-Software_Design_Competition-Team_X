using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    private RequestManager requestManager; // Reference to the RequestManager

    public void PlayGame(){
        Debug.Log("Play game");
    }

    public void PlayerProfile(){
        Debug.Log("Player profile");

        requestManager = ScriptableObject.CreateInstance<RequestManager>();

        string url = "http://20.15.114.131:8080/api/user/profile/view";
        string method = "GET";

        requestManager.SendRequest(url, method, null, this);
    }

    public void Leaderboard(){
        Debug.Log("Leaderboard");
    }

    public void ExitGame(){
        Debug.Log("Exit Game");
    }
}
