using System.Collections;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // Reference to the EnergyDataFetch
    private EnergyDataFetch energyDataFetch;
    // Reference to the TreasureManager
    private TreasureManager treasureManager;

    // Variables
    private float repeatRate = 15.0f;
    private string lastFetchTime = "";
    private string currentFetchTime = "";
    private float lastPowerConsumption = 0.0f;
    private float currentPowerConsumption = 0.0f;
    private float lastPowerConsumptionGap = 0.0f;
    private float currentPowerConsumptionGap = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Energy Manager Started");

        // Get the last fetched time and power consumption from the player prefs
        lastFetchTime = PlayerPrefs.GetString("LastFetchTime", "");
        lastPowerConsumption = PlayerPrefs.GetFloat("LastPowerConsumption", 0.0f);
        lastPowerConsumptionGap = PlayerPrefs.GetFloat("LastPowerConsumptionGap", 0.0f);

        energyDataFetch = FindObjectOfType<EnergyDataFetch>();
        treasureManager = FindObjectOfType<TreasureManager>();

        // StartCoroutine(FetchCurrentPowerConsumption());
    }

    // Routine to fetch the current power consumption 
    private IEnumerator FetchCurrentPowerConsumption()
    {
        while (true)
        {
            Debug.Log("Fetching Current Power Consumption");
            Debug.Log("Last Fetch Time: " + lastFetchTime);
            Debug.Log("Last Power Consumption: " + lastPowerConsumption);
            Debug.Log("Last Power Consumption Gap: " + lastPowerConsumptionGap);

            bool isFetchCompleted = false;

            // Fetch the current power consumption
            energyDataFetch.GetCurrentPowerConsumption((float powerConsumption) =>
            {
                currentPowerConsumption = powerConsumption;
                currentFetchTime = System.DateTime.Now.ToString();
                currentPowerConsumptionGap = currentPowerConsumption - lastPowerConsumption;
                isFetchCompleted = true;
            });

            // Wait until the fetch is completed
            yield return new WaitUntil(() => isFetchCompleted);

            Debug.Log("Current Fetch Time: " + currentFetchTime);
            Debug.Log("Current Power Consumption: " + currentPowerConsumption);
            Debug.Log("Current Power Consumption Gap: " + currentPowerConsumptionGap);

            ComparePowerConsumption();

            // Wait for the next fetch
            yield return new WaitForSeconds(repeatRate);
        }
    }

    // Method to compare the power consumption
    private void ComparePowerConsumption()
    {
        // Compare the current power consumption with the last power consumption
        if (currentPowerConsumptionGap > lastPowerConsumptionGap)
        {
            Debug.Log("Energy Consumption Increased");

            // Decrease the number of visible treasures
            // treasureManager.DecreaseVisibleTreasures();

        }
        else if (currentPowerConsumptionGap < lastPowerConsumptionGap)
        {
            Debug.Log("Energy Consumption Decreased");
            // Increase the number of visible treasures
            // treasureManager.IncreaseVisibleTreasures();
        }

        // Update the last power consumption and fetch time
        lastPowerConsumption = currentPowerConsumption;
        lastFetchTime = currentFetchTime;
        lastPowerConsumptionGap = currentPowerConsumptionGap;

        // Save the last power consumption and fetch time to the player prefs
        PlayerPrefs.SetFloat("LastPowerConsumption", lastPowerConsumption);
        PlayerPrefs.SetString("LastFetchTime", lastFetchTime);
        PlayerPrefs.SetFloat("LastPowerConsumptionGap", lastPowerConsumptionGap);
    }
}
