using System;
using System.Collections;
using UnityEngine;
using SimpleJSON;

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

    // Flags to pause the energy manager
    public static bool isPausedEM;

    // Start is called before the first frame update
    void Start()
    {
        // Initiate the flags
        isPausedEM = false;

        // Debug.Log("Energy Manager Started");

        // Get the last fetched time and power consumption from the player prefs
        lastFetchTime = PlayerPrefs.GetString("LastFetchTime", "");
        lastPowerConsumption = PlayerPrefs.GetFloat("LastPowerConsumption", 0.0f);
        lastPowerConsumptionGap = PlayerPrefs.GetFloat("LastPowerConsumptionGap", 0.0f);

        Debug.Log("Last Fetch Time: " + lastFetchTime);
        Debug.Log("Last Power Consumption: " + lastPowerConsumption);
        Debug.Log("Last Power Consumption Gap: " + lastPowerConsumptionGap);

        energyDataFetch = FindObjectOfType<EnergyDataFetch>();
        treasureManager = FindObjectOfType<TreasureManager>();

        // Compare the power data when initially starting the game
        // Wait until CompareInitialWithInactivePowerConsumption is over
        StartCoroutine(InitializeAndStartFetching());
    }

    // Coroutine to initialize and start fetching power consumption
    private IEnumerator InitializeAndStartFetching()
    {
        int initialPowerChange;
        yield return StartCoroutine(CompareInitialWithInactivePowerConsumption(value => initialPowerChange = value));

        StartCoroutine(FetchCurrentPowerConsumption());
    }

    // Routine to fetch the current power consumption 
    private IEnumerator FetchCurrentPowerConsumption()
    {
        while (true)
        {
            Debug.Log("Energy Manager Paused.");
            // Wait until not paused
            yield return new WaitWhile(() => isPausedEM);
            Debug.Log("Energy Manager Resumed.");

            Debug.Log("Routinely Fetching Current Power Consumption");
            Debug.Log("Last Fetch Time: " + lastFetchTime);
            Debug.Log("Last Power Consumption: " + lastPowerConsumption);
            Debug.Log("Last Power Consumption Gap: " + lastPowerConsumptionGap);

            bool isFetchCompleted = false;

            // Fetch the current power consumption
            energyDataFetch.GetCurrentPowerConsumption((float powerConsumption) =>
            {
                currentPowerConsumption = powerConsumption;
                currentFetchTime = System.DateTime.Now.ToString("o");
                currentPowerConsumptionGap = currentPowerConsumption - lastPowerConsumption;
                isFetchCompleted = true;
            });

            // Wait until the fetch is completed
            yield return new WaitUntil(() => isFetchCompleted);

            Debug.Log("Current Fetch Time: " + currentFetchTime);
            Debug.Log("Current Power Consumption: " + currentPowerConsumption);
            Debug.Log("Current Power Consumption Gap: " + currentPowerConsumptionGap);

            CompareCurrentPowerConsumption();

            // Wait for the next fetch
            yield return new WaitForSeconds(repeatRate);
        }
    }

    // Method to compare the power consumption
    private void CompareCurrentPowerConsumption()
    {
        // Compare the current power consumption with the last power consumption
        if (currentPowerConsumptionGap > lastPowerConsumptionGap)
        {
            Debug.Log("Energy Consumption Increased");

            // Decrease the number of visible treasures
            treasureManager.DecreaseVisibleTreasures();

        }
        else if (currentPowerConsumptionGap < lastPowerConsumptionGap)
        {
            Debug.Log("Energy Consumption Decreased");
            // Increase the number of visible treasures
            treasureManager.IncreaseVisibleTreasures();
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

    private IEnumerator CompareInitialWithInactivePowerConsumption(Action<int> result)
    {   
        int initialPowerChange = 5;
        
        //string lastTime = lastFetchTime;
        string lastTime = "6/1/2024 1:06:26 PM";
        DateTime lastDateTime;
        if (DateTime.TryParse(lastTime, out lastDateTime))
        {
            //DateTime currentDateTime = DateTime.Now;
            string currentTime = "6/5/2024 1:06:26 PM";
            DateTime currentDateTime = DateTime.Parse(currentTime);
            
            // If currentTime and lastTime are on the same day
            // No change in Power Consumption considered
            if (lastDateTime.Date == currentDateTime.Date)
            { 
                Debug.Log("Same day comparison => No change");
                initialPowerChange = 0;

                bool isFetchCompleted = false;

                // Fetch the current power consumption
                energyDataFetch.GetCurrentPowerConsumption((float powerConsumption) =>
                {
                    lastPowerConsumption = powerConsumption;
                    lastFetchTime = System.DateTime.Now.ToString("o");
                    isFetchCompleted = true;
                });

                // Wait until the fetch is completed
                yield return new WaitUntil(() => isFetchCompleted);
            }

            // If currentTime and lastTime are not on the same day 
            // Get the power consumption in between
            else 
            {
                Debug.Log("Different day comparison => Change in power consumption");
                
                // Wait until the method is over
                yield return StartCoroutine(GetDifferentDayPowerComparison(lastDateTime, currentDateTime, (int result) =>
                {
                    initialPowerChange = result;
                }));
            }

            // Set the initial treasure count according to the results
            TreasureManager.isTreasureCountInitialized = true;
            Debug.Log("Treasure count initialized");
            treasureManager.SetInitialTreasureCount(initialPowerChange);

            // Wait for the first recurrent fetch
            yield return new WaitForSeconds(repeatRate);
        }

        // Return the result 
        result(initialPowerChange);
    }

    private IEnumerator GetDifferentDayPowerComparison(DateTime lastDateTime, DateTime currentDateTime, Action<int> callback)
    {
        // Variables to store the power consumption
        double inactivePeriodPowerConsumption = 0.0f;
        double currentDayPowerConsumption = 0.0f;

        // Get current day power consumption using the GetCurrentPowerConsumption method
        bool isFetchCompleted = false;
        float fetchValue = 0.0f;
        string fetchTime = "";
        int initialPowerChange = 5;

        // Fetch the current power consumption
        energyDataFetch.GetCurrentPowerConsumption((float powerConsumption) =>
        {
            fetchValue = powerConsumption;    
            fetchTime = System.DateTime.Now.ToString("o");
            
            // Set Last fetch player prefs
            lastPowerConsumption = fetchValue;
            lastFetchTime = fetchTime;
            isFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isFetchCompleted);

        // Calculate per day power consumption of current day using fetchValue and fetchTime
        DateTime fetchDateTime = DateTime.Parse(fetchTime);
        double secondsPast = (fetchDateTime - fetchDateTime.Date).TotalSeconds;
        currentDayPowerConsumption = (fetchValue / 1000.0) * (86400.0 / secondsPast);

        // Get the inactive period power consumption
        // Wait until the method is over
        yield return StartCoroutine(GetInactivePeriodPowerConsumption(lastDateTime, currentDateTime, (double result) => 
        {
            inactivePeriodPowerConsumption = result;
        }));

        Debug.Log("Per day consumption of Inactive Period: " + inactivePeriodPowerConsumption);
        Debug.Log("Current Day Power Consumption: " + currentDayPowerConsumption);

        // Compare the two values
        if (inactivePeriodPowerConsumption > currentDayPowerConsumption)
        {
            Debug.Log("Consumption decreased than the Inactive Period");
            initialPowerChange = -1;
        }
        else
        {
            Debug.Log("Consumption increased than the Inactive Period");
            initialPowerChange = 1;
        }

        // Invoke the callback with the calculated value
        callback(initialPowerChange);
    }

    private IEnumerator GetInactivePeriodPowerConsumption(DateTime lastDateTime, DateTime currentDateTime, Action<double> callback)
    {
        double inactivePeriodPowerConsumption = 0.0f;

        // If the lastDateTime is within the same month
        if (lastDateTime.Month == currentDateTime.Month)
        {
            // Get current day power consumption using the GetCurrentPowerConsumption method
            bool isFetchCompleted = false;
            JSONNode fetchValue = null;
            string fetchTime = "";
            double inactivePeriodTotalPowerConsumption = 0.0f;

            // Fetch the current power consumption
            energyDataFetch.GetCurrentMonthPowerConsumption((JSONNode value) =>
            {
                fetchValue = value;    
                fetchTime = System.DateTime.Now.ToString("o");
                isFetchCompleted = true;
            });

            // Wait until the fetch is completed
            yield return new WaitUntil(() => isFetchCompleted);

            Debug.Log("Current Month Daily Power Consumption: " + fetchValue.ToString());

            // Calculate the inactive period power consumption
            // Add the power consumption of the days from lastDateTime to currentDateTime
            DateTime tempDateTime = lastDateTime;
            Debug.Log("Last Date Time: " + tempDateTime.Date);
            Debug.Log("Current Date Time: " + currentDateTime.Date);
            while (tempDateTime.Day < currentDateTime.Day)
            {
                // Get only the day of the date
                string dayString = tempDateTime.Day.ToString();
                Debug.Log("Date String: " + dayString);
                Debug.Log("Power Consumption: " + fetchValue[dayString].AsDouble);
                inactivePeriodTotalPowerConsumption += fetchValue[dayString].AsDouble;
                tempDateTime = tempDateTime.AddDays(1);
            }
            Debug.Log("Inactive Period Power Consumption: " + inactivePeriodTotalPowerConsumption);

            // Calculate per day inactive period power consumption
            double daysPast = currentDateTime.Day - lastDateTime.Day;
            Debug.Log("Days Past: " + daysPast);
            inactivePeriodPowerConsumption = inactivePeriodTotalPowerConsumption / daysPast;

            // Invoke the callback with the calculated value
            callback(inactivePeriodPowerConsumption);
        }
        else
        {
            // Invoke the callback with a default value or handle accordingly
            callback(0.0);
        }
    }
}