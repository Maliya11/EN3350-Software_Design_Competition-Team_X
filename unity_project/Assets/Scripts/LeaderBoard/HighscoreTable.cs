using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake(){
        entryContainer = transform.Find("Canvas/HighscoreTable/highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

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

        /*Add players to the leaderboard using AddHighScoreEntry()*/
        AddHighScoreEntry("Player");


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
        return playerHighScore;
    }

    private void AddHighScoreEntry(string Name){
        int scr;
        if(Name == "Player"){
            scr = calculatePlayerHighScore();
        }
        else{
            scr = Random.Range(0,1200);
        }
        HighscoreEntry highscoreEntry = new HighscoreEntry {score = scr, name = Name};
        
        highscoreEntryList.Add(highscoreEntry);
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList){
        float templateHeight = 70f;
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
