using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

// Manages the visibility of the treasures based on the energy consumption
public class TreasureManager : Singleton<TreasureManager>
{
    // UI Elements 
    public TextMeshProUGUI treasureText;

    public List<GameObject> treasureObjects; // Manually populated in the Inspector
    public int initialVisibleTreasures;
    public int maxVisibleTreasures;
    private int currentVisibleTreasures;
    private List<int> openedTreasureIndices = new List<int>(); 
    private List<int> closedTreasureIndices = new List<int>(); 
    private List<int> treasureQuestionIDs;

    // Variable to store the potions collected in the gameplay
    public int potionsCollected = 0;

    // Flags 
    public static bool isTreasureCountInitialized;
    public static bool isPausedTM;

    void Start()
    {
        // Initialize the flags
        isPausedTM = false;
        isTreasureCountInitialized = false;
        
        // Initialize the random seed using the current time
        Random.InitState(System.DateTime.Now.Millisecond);

        // Set the maximum number of visible treasures
        maxVisibleTreasures = treasureObjects.Count;

        // Wait while EnergyManager initializes the treasure count
        StartCoroutine(WaitForTreasureCountInitialization());
    }

    private IEnumerator WaitForTreasureCountInitialization()
    {
        yield return new WaitUntil(() => isTreasureCountInitialized);

        // Set the number of visible treasures
        currentVisibleTreasures = initialVisibleTreasures;

        // Initialize closed treasure indices as a list from 0 to treasureObjects.Count
        for (int i = 0; i < treasureObjects.Count; i++)
        {
            closedTreasureIndices.Add(i);
        }

        InitializeTreasures();
        SetRandomTreasuresVisible(currentVisibleTreasures);

        // Start the coroutine to change treasure visibility
        // StartCoroutine(ChangeTreasureVisibility());
    }

    void Update()
    {  
        // Pause the Update 
        if (isPausedTM)
        {
            Debug.Log("Treasure Manager Paused");
            return;
        } 

        Debug.Log("Treasure Manager not Paused");

        // Update the UI text
        treasureText.text = $"{(openedTreasureIndices.Count)} / {currentVisibleTreasures}";

        // Clear the opened and closed treasure indices
        openedTreasureIndices.Clear();
        closedTreasureIndices.Clear();

        // Check the Indices of opened treasures and closed treasures
        for (int i = 0; i < treasureObjects.Count; i++)
        {
            if (treasureObjects[i].GetComponent<Treasure>().isOpened)
            {
                // Add the index of the opened treasure to the list
                openedTreasureIndices.Add(i);
            }
            else
            {
                // Add the index of the closed treasure to the list
                closedTreasureIndices.Add(i);
            }
        }
    }

    public void SetInitialTreasureCount(bool energyIncreased)
    {
        // If Power consumption has increased
        // Set the Initial visible treasure count to 1/3 of total
        if (energyIncreased) 
        {
            initialVisibleTreasures = Mathf.CeilToInt(maxVisibleTreasures * (1f / 3f));
        }
        // Else
        // Set it to 2/3 of total
        else 
        {
            initialVisibleTreasures = Mathf.CeilToInt(maxVisibleTreasures * (2f / 3f));
        }
        Debug.Log("Treasure count initializing in Treasure Manager: " + initialVisibleTreasures);
    }

    // Method to initialize the treasures with random question IDs
    private void InitializeTreasures()
    {
        // Hide all the treasures
        foreach (var treasure in treasureObjects)
        {
            treasure.SetActive(false);
        }

        // Initialize treasure question IDs (for demonstration purposes, random values)
        treasureQuestionIDs = new List<int>();
        for (int i = 11; i <= 57; i++)
        {
            treasureQuestionIDs.Add(i);
        }

        foreach (var treasure in treasureObjects)
        {
            int randomIndex = Random.Range(0, treasureQuestionIDs.Count);
            int questionID = treasureQuestionIDs[randomIndex];
            treasureQuestionIDs.RemoveAt(randomIndex);
            treasure.GetComponent<Treasure>().treasureQID = questionID;
        }
    }

    // Method to set random treasures visible
    private void SetRandomTreasuresVisible(int count)
    {
        // Debug.Log("Setting " + count.ToString() + " random treasures visible");
        // Check if the closed treasure indices are empty
        if (closedTreasureIndices.Count == 0)
        {
            return;
        }

        // Hide all the closed treasure indices
        for (int i = 0; i < closedTreasureIndices.Count; i++)
        {
            treasureObjects[closedTreasureIndices[i]].SetActive(false);            
        }

        // Activate random closed treasure indices
        List<int> indices = new List<int>(closedTreasureIndices);
        // Print the indices of the closed treasures
        // Debug.Log("Closed Treasure Indices: " + string.Join(", ", indices));
        // Debug.Log(Math.Min(count, indices.Count) + " treasures will be visible");
        for (int i = 0; i < Math.Min(count, closedTreasureIndices.Count); i++)
        {
            if (indices.Count == 0)
            {
                Debug.LogWarning("No more treasures to activate.");
                break;
            }

            int randomIndex = Random.Range(0, indices.Count);
            int randomTreasureIndex = indices[randomIndex];
            treasureObjects[randomTreasureIndex].SetActive(true);
            // Debug.Log("Treasure " + randomTreasureIndex + " is visible");
            indices.RemoveAt(randomIndex);
        }
    }

    // Method to Incrase the number of visible treasures if the energy consumption decreases
    public void IncreaseVisibleTreasures()
    {
        if (currentVisibleTreasures < maxVisibleTreasures)
        {
            currentVisibleTreasures++;
            Debug.Log("Increasing visible treasures to " + currentVisibleTreasures);
            SetRandomTreasuresVisible(currentVisibleTreasures - openedTreasureIndices.Count);
        }
        else
        {
            Debug.Log("All treasures are visible");
        }
    }

    // Method to Decrease the number of visible treasures if the energy consumption increases
    public void DecreaseVisibleTreasures()
    {
        if (currentVisibleTreasures > openedTreasureIndices.Count)
        {
            currentVisibleTreasures--;
            Debug.Log("Decreasing visible treasures to " + currentVisibleTreasures);
            SetRandomTreasuresVisible(currentVisibleTreasures - openedTreasureIndices.Count);
        }
        else
        {
            Debug.Log("No treasures to hide");
        }
    } 

    // Method to collect the potion received from the treasures
    public void CollectPotion()
    {
        potionsCollected++;
        Debug.Log("Potions collected: " + potionsCollected);
    }

    // Method for testing the functionality of Changing treasure visibility
    private IEnumerator ChangeTreasureVisibility()
    {
        while (true)
        {
            Debug.Log("Changing treasure visibility");
            // Generate a random value
            int randomValue = Random.Range(0, 2);
            Debug.Log("Random Value: " + randomValue);

            // If the random value is 0, Decrease the number of visible treasures
            if (randomValue == 0)
            {
                DecreaseVisibleTreasures();
            }
            // If the random value is 1, Increase the number of visible treasures
            else
            {
                IncreaseVisibleTreasures();
            }

            // Wait for 5 seconds
            yield return new WaitForSeconds(15);
        }
    }
}