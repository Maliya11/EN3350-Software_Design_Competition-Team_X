using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        // Initialize the list of highscore entries
        highscoreEntryList = new List<HighscoreEntry>{
            new HighscoreEntry{ score = 5134, name = "Kumudu"},
            new HighscoreEntry{ score = 9830, name = "Malith"},
            new HighscoreEntry{ score = 5430, name = "Thisari"},
            new HighscoreEntry{ score = 4370, name = "Sandeepa"},
            new HighscoreEntry{ score = 7777, name = "Sanuli"},
            new HighscoreEntry{ score = 10000, name = "Minudi"},
            new HighscoreEntry{ score = 1255, name = "Pinidi"},
            new HighscoreEntry{ score = 120, name = "Kamal"},
            new HighscoreEntry{ score = 6782, name = "Kusum"},
            new HighscoreEntry{ score = 9967, name = "Amara"},
        };

        // Sort the highscore entries in descending order based on score
        for (int i=0; i<highscoreEntryList.Count; i++){
            for (int j= i+1; j<highscoreEntryList.Count; j++){
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
