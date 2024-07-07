using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    // Reference to the AllUserLoader
    private AllUserLoader allUserLoader;

    // UI Elements
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<string> usernames;

    // Player's username 
    private string playerUserName;

    // List to score highscore entries
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    public int ScoreChange;
    public float powerScore = 1.0f;

    private void Awake()
    {
        // Initialize the AllUserLoader
        allUserLoader = FindObjectOfType<AllUserLoader>();

        entryContainer = transform.Find("Scroll area/Scroll/Container/highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Initialize the list of highscore entries
        highscoreEntryList = new List<HighscoreEntry>();

        // Get the player's username from PlayerPrefs
        playerUserName = PlayerPrefs.GetString("username");
        Debug.Log("Player username: " + playerUserName);

        StartCoroutine(FetchUserScores());
    }

    private IEnumerator FetchUserScores()
    {
        // Wait until all the users' information is fetched
        yield return StartCoroutine(allUserLoader.fetchAllUsers());

        // Get the usernames of the users 
        usernames = allUserLoader.usernames;

        /*// Initialize the list of highscore entries
        highscoreEntryList = new List<HighscoreEntry>{
            new HighscoreEntry{ score = calculatePlayerHighScore(), name = "Player"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Kumudu"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Thisari"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Sandeepa"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Sanuli"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Minudi"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Pinidi"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Kamal"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Kusum"},
            new HighscoreEntry{ score = Random.Range(0,1200), name = "Amara"},
        };*/

        // Add the scores of the users to the highscore list
        foreach (string username in usernames){
            Debug.Log(username);
            AddHighScoreEntry(username);
        }

        // Sort the highscore entries in descending order based on score
        for (int i=0; i < highscoreEntryList.Count; i++){
            for (int j = i+1; j < highscoreEntryList.Count; j++){
                if (highscoreEntryList[j].score > highscoreEntryList[i].score){
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }   
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList){
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    //Calculate the total score of the player that is logged into the game
    private int calculatePlayerHighScore(){
        int playerHighScore = 0;
        for(int currentLevelIndex = 3; currentLevelIndex <= 8; currentLevelIndex++){
            string highScoreKey = "HighScore_Level_" + currentLevelIndex;
            playerHighScore += PlayerPrefs.GetInt(highScoreKey, 0);
        }
        
        ScoreChange = PlayerPrefs.GetInt("ScoreChange", 0);
        SetpowerConsumptionScore(ScoreChange);
        playerHighScore = (int)(playerHighScore * powerScore);
        
        return playerHighScore;
    }

    public void SetpowerConsumptionScore(int energyChange)
    {
        Debug.Log("Energy consumption code: " + energyChange);

        if (energyChange == -1) 
        {
            powerScore = 1.1f;
        }
        if (energyChange == 0) 
        {
            powerScore = 1.0f;
        }
        if (energyChange == 1)
        {
            powerScore = 0.9f;
        }

    }

    private void AddHighScoreEntry(string Name){
        int scr;
        if(Name == playerUserName){
            scr = calculatePlayerHighScore();
        }
        else{
            scr = Random.Range(0,1200);
        }
        HighscoreEntry highscoreEntry = new HighscoreEntry {score = scr, name = Name};
        
        highscoreEntryList.Add(highscoreEntry);
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList){
        float templateHeight = 150f;
        Transform entryTransform = Instantiate(entryTemplate,container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight*transformList.Count);
        entryTransform.gameObject.SetActive(true);

        // Determine the rank of the entry
        int rank = transformList.Count + 1;
        string rankString;
        switch(rank){
            default:
            rankString = rank + "th"; break;

            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3rd"; break;
        }

        // Set the rank, score, and name text for the entry
        entryTransform.Find("posText").GetComponent<Text>().text=rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text=score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text=name;

        transformList.Add(entryTransform);

    }

    // Class to hold highscore entry data
    private class HighscoreEntry{
        public int score;
        public string name;
    }
}
