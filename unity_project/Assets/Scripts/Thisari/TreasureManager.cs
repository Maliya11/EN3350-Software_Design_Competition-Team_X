using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages the visibility of the treasures based on the energy consumption
public class TreasureManager : Singleton<TreasureManager>
{
    public List<GameObject> treasureObjects; // Manually populated in the Inspector
    public int initialVisibleTreasures = 5;
    public int maxVisibleTreasures;
    public int currentVisibleTreasures;
    public float apiCheckInterval = 5f;

    private List<int> treasureQuestionIDs;

    void Start()
    {
        // Initialize the random seed using the current time
        Random.InitState(System.DateTime.Now.Millisecond);

        // Set the number of visible treasures
        currentVisibleTreasures = initialVisibleTreasures;
        maxVisibleTreasures = treasureObjects.Count;

        InitializeTreasures();
        SetRandomTreasuresVisible(currentVisibleTreasures);
        //StartCoroutine(CheckEnergyConsumption());
    }

    void InitializeTreasures()
    {
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
            treasure.GetComponent<Treasure>().treasureID = questionID;

            // Debug.Log($"Treasure {treasure.name} assigned question ID: {questionID}");
        }
    }

    void SetRandomTreasuresVisible(int count)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < treasureObjects.Count; i++)
        {
            // Disable the trasure and add the index to the list if the treasure is not opened
            if (!treasureObjects[i].GetComponent<Treasure>().isOpened)
            {
                treasureObjects[i].SetActive(false);
                indices.Add(i);
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (indices.Count == 0)
            {
                Debug.LogWarning("No more treasures to activate.");
                break;
            }

            int randomIndex = Random.Range(0, indices.Count);
            int treasureIndex = indices[randomIndex];
            treasureObjects[treasureIndex].SetActive(true);
            indices.RemoveAt(randomIndex);

            // Debug.Log($"Treasure {treasureObjects[treasureIndex].name} activated at index: {treasureIndex}");
        }

        Debug.Log("Final active treasures count: " + count);
    }

    IEnumerator CheckEnergyConsumption()
    {
        float lastEnergyConsumption = 0;

        while (true)
        {
            yield return new WaitForSeconds(apiCheckInterval);

            float currentEnergyConsumption = Random.Range(0f, 100f); // Mock API call
            Debug.Log($"Energy consumption: {currentEnergyConsumption}");

            if (currentEnergyConsumption > lastEnergyConsumption)
            {
                Debug.Log("Energy consumption increased, hiding treasures.");
                currentVisibleTreasures = Mathf.Max(1, currentVisibleTreasures - 1);
                SetRandomTreasuresVisible(currentVisibleTreasures);
            }
            else if (currentEnergyConsumption < lastEnergyConsumption)
            {
                Debug.Log("Energy consumption decreased, showing more treasures.");
                currentVisibleTreasures = Mathf.Min(treasureObjects.Count, currentVisibleTreasures + 1);
                SetRandomTreasuresVisible(currentVisibleTreasures);
            }

            lastEnergyConsumption = currentEnergyConsumption;
        }
    }
}