using System;
using System.Collections;
using UnityEngine;
using SimpleJSON;

public class EnergyManager : MonoBehaviour
{
    /*
    This script is used to manage the energy consumption of the player and update the treasure count according to the power consumption
    */
    // public static int ScoreChange;
    // Reference to the EnergyDataFetch
    private EnergyDataFetch energyDataFetch;
    // Reference to the TreasureManager
    private TreasureManager treasureManager;

    // private LeaderboardManager highscoreTable;

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

        // Debug.Log("Last Fetch Time: " + lastFetchTime);
        // Debug.Log("Last Power Consumption: " + lastPowerConsumption);
        // Debug.Log("Last Power Consumption Gap: " + lastPowerConsumptionGap);

        energyDataFetch = FindObjectOfType<EnergyDataFetch>();
        treasureManager = FindObjectOfType<TreasureManager>();

        // Compare the power data when initially starting the game
        // Wait until CompareInitialWithInactivePowerConsumption is over
        StartCoroutine(InitializeAndStartFetching());
    }

    // Coroutine to initialize and start fetching power consumption
    private IEnumerator InitializeAndStartFetching()
    {
        // Wait until the comparison of power consumption with the inactive period is over
        yield return StartCoroutine(CompareInitialWithInactivePowerConsumption());

        // Start routinely fetching the current power consumption
        StartCoroutine(FetchCurrentPowerConsumption());
    }

    // Routine to fetch the current power consumption repeatedly by the repeat rate
    private IEnumerator FetchCurrentPowerConsumption()
    {
        while (true)
        {
            // Debug.Log("Energy Manager Paused.");
            // Wait until not paused
            yield return new WaitWhile(() => isPausedEM);
            // Debug.Log("Energy Manager Resumed.");

            // Debug.Log("Routinely Fetching Current Power Consumption");
            Debug.Log("Last Fetch Time: " + lastFetchTime);
            // Debug.Log("Last Power Consumption: " + lastPowerConsumption);
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
            // Debug.Log("Current Power Consumption: " + currentPowerConsumption);
            Debug.Log("Current Power Consumption Gap: " + currentPowerConsumptionGap);

            CompareCurrentPowerConsumption();

            // Wait for the next fetch
            yield return new WaitForSeconds(repeatRate);
        }
    }

    // Method to compare the routinely power consumption
    private void CompareCurrentPowerConsumption()
    {
        // Compare the current power consumption with the last power consumption
        if (currentPowerConsumptionGap > lastPowerConsumptionGap)
        {
            // Debug.Log("Energy Consumption Increased");

            // Decrease the number of visible treasures
            if (treasureManager != null){

            
            treasureManager.DecreaseVisibleTreasures();
            }

        }
        else if (currentPowerConsumptionGap < lastPowerConsumptionGap)
        {
            // Debug.Log("Energy Consumption Decreased");
            // Increase the number of visible treasures
            if (treasureManager != null){
            treasureManager.IncreaseVisibleTreasures();
            }
        }

        // Update the last power consumption and fetch time
        lastPowerConsumption = currentPowerConsumption;
        lastFetchTime = currentFetchTime;
        lastPowerConsumptionGap = currentPowerConsumptionGap;

        // Save the last power consumption and fetch time to the player prefs
        PlayerPrefs.SetFloat("LastPowerConsumption", lastPowerConsumption);
        PlayerPrefs.SetString("LastFetchTime", lastFetchTime);
        PlayerPrefs.SetFloat("LastPowerConsumptionGap", lastPowerConsumptionGap);
        PlayerPrefs.Save();
    }

    // Method to compare the initial power consumption with the inactive period power consumption
    private IEnumerator CompareInitialWithInactivePowerConsumption()
    {   
        int initialPowerChange = 5; // Random value to ensure the correct value is assigned
        
        // Get Last Fetched Time
        string lastTime = lastFetchTime;
        //string lastTime = "4/3/2024 9:43:21 AM";
        DateTime lastDateTime;

        // Get Current Time
        DateTime currentDateTime = DateTime.Now;
        //string currentTime = "4/21/2024 1:06:26 PM";
        //DateTime currentDateTime = DateTime.Parse(currentTime);

        if (DateTime.TryParse(lastTime, out lastDateTime))
        {    
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
            Debug.Log("Initial Power Change: " + initialPowerChange);
            if (treasureManager != null){

                treasureManager.SetInitialTreasureCount(initialPowerChange);

            }
            
            PlayerPrefs.SetInt("ScoreChange", initialPowerChange);
            PlayerPrefs.Save();
            // LeaderboardManager.SetpowerConsumptionScore(initialPowerChange);
            

            // Wait for the first recurrent fetch
            yield return new WaitForSeconds(repeatRate);
        }
        else
        {
            Debug.Log("Invalid DateTime Format");
        }
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
        int initialPowerChange = 5; // Random value to ensure the correct value is assigned

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
            // Debug.Log("Consumption decreased than the Inactive Period");
            initialPowerChange = -1;
        }
        else
        {
            // Debug.Log("Consumption increased than the Inactive Period");
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
            // Wait until the method is over
            yield return StartCoroutine(SameMonthScenario(lastDateTime, currentDateTime, (double result) =>
            {
                inactivePeriodPowerConsumption = result;
            }));

            // Invoke the callback with the calculated value
            callback(inactivePeriodPowerConsumption);
        }
        // If the lastDateTime is not within the same month but the same year
        else if (lastDateTime.Year == currentDateTime.Year)
        {
            // Wait until the method is over
            yield return StartCoroutine(DifferentMonthSameYearScenario(lastDateTime, currentDateTime, (double result) =>
            {
                inactivePeriodPowerConsumption = result;
            }));

            // Invoke the callback with the calculated value
            callback(inactivePeriodPowerConsumption);
            
        }
        // If the lastDateTime is in the previous year
        else if ((currentDateTime.Year - lastDateTime.Year) == 1)
        {
            // Wait until the method is over
            yield return StartCoroutine(PreviousYearScenario(lastDateTime, currentDateTime, (double result) =>
            {
                inactivePeriodPowerConsumption = result;
            }));

            // Invoke the callback with the calculated value
            callback(inactivePeriodPowerConsumption);
        }
        // If the lastDateTime is not in the previous year
        else
        {
            // Debug.Log("Inactive Period Power Consumption is disregarded. Returning 0.0f");
            callback(inactivePeriodPowerConsumption);
        }
    }

    private IEnumerator SameMonthScenario(DateTime lastDateTime, DateTime currentDateTime, Action<double> callback)
    {
        double inactivePeriodPowerConsumption = 0.0f;
        double inactivePeriodTotalPowerConsumption = 0.0f;
        bool isFetchCompleted = false;
        JSONNode fetchValue = null;
        
        // Fetch the current power consumption
        energyDataFetch.GetCurrentMonthPowerConsumption((JSONNode value) =>
        {
            fetchValue = value;    
            isFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isFetchCompleted);
        // Debug.Log("Current Month Daily Power Consumption: " + fetchValue.ToString());

        // Calculate the inactive period power consumption
        // Add the power consumption of the days from lastDateTime to currentDateTime
        DateTime tempDateTime = lastDateTime;
        // Debug.Log("Last Date Time: " + tempDateTime.Date);
        // Debug.Log("Current Date Time: " + currentDateTime.Date);
        while (tempDateTime.Date < currentDateTime.Date)
        {
            // Get only the day of the date
            string dayString = tempDateTime.Day.ToString();
            // Debug.Log("Date String: " + dayString);
            // Debug.Log("Power Consumption: " + fetchValue[dayString].AsDouble);
            inactivePeriodTotalPowerConsumption += fetchValue[dayString].AsDouble;
            tempDateTime = tempDateTime.AddDays(1);
        }
        // Debug.Log("Inactive Period Power Consumption: " + inactivePeriodTotalPowerConsumption);

        // Calculate per day inactive period power consumption
        double daysPast = (currentDateTime.Date - lastDateTime.Date).TotalDays;
        // Debug.Log("Days Past: " + daysPast);
        inactivePeriodPowerConsumption = inactivePeriodTotalPowerConsumption / daysPast;

        // Invoke the callback with the calculated value
        callback(inactivePeriodPowerConsumption);
    }

    private IEnumerator DifferentMonthSameYearScenario(DateTime lastDateTime, DateTime currentDateTime, Action<double> callback)
    {
        double inactivePeriodPowerConsumption = 0.0f;
        double inactivePeriodTotalPowerConsumption = 0.0f;
        bool isLastFetchedMonthFetchCompleted = false;
        bool isInBetweenMonthFetchCompleted = false;
        bool isCurrentMonthFetchCompleted = false;
        JSONNode lastFetchedMonthValue = null;
        JSONNode inBetweenMonthValue = null;
        JSONNode currentMonthValue = null;
        string currentYear = currentDateTime.Year.ToString();
        string lastFetchedMonth = lastDateTime.ToString("MMMM").ToUpper();
        string currentMonth = currentDateTime.ToString("MMMM").ToUpper();
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the daily power consumption of the last fetched month 
        // Debug.Log("Last Fetched Year: " + currentYear);
        // Debug.Log("Last Fetched Month: " + lastFetchedMonth);
        // Fetch the specific month power consumption
        energyDataFetch.GetSpecificMonthPowerConsumption(currentYear, lastFetchedMonth, (JSONNode value) =>
        {
            lastFetchedMonthValue = value;    
            isLastFetchedMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isLastFetchedMonthFetchCompleted);
        // Debug.Log("Specific Month Daily Power Consumption: " + lastFetchedMonthValue.ToString());

        // Add the power consumption of the days from lastDateTime to the end of the lastFetchedMonth
        DateTime tempDateTime = lastDateTime;
        DateTime lastDayOfMonthDateTime = new DateTime(lastDateTime.Year, lastDateTime.Month, DateTime.DaysInMonth(lastDateTime.Year, lastDateTime.Month));
        // Debug.Log("Last Date Time: " + tempDateTime.Date);
        // Debug.Log("Last Day of Month Date Time: " + lastDayOfMonthDateTime.Date);
        while (tempDateTime.Date <= lastDayOfMonthDateTime.Date)
        {
            // Get only the day of the date
            string dayString = tempDateTime.Day.ToString();
            // Debug.Log("Date String: " + dayString);
            // Debug.Log("Power Consumption: " + lastFetchedMonthValue[dayString].AsDouble);
            inactivePeriodTotalPowerConsumption += lastFetchedMonthValue[dayString].AsDouble;
            tempDateTime = tempDateTime.AddDays(1);
        }
        // Debug.Log("Inactive Period Power Consumption Upto Last Fetched Month: " + inactivePeriodTotalPowerConsumption);
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the monthly power consumption of the in between months
        energyDataFetch.GetYearlyPowerConsumption(currentYear, (JSONNode value) =>
        {
            inBetweenMonthValue = value;    
            isInBetweenMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isInBetweenMonthFetchCompleted);
        // Debug.Log("Yearly Power Consumption: " + inBetweenMonthValue.ToString());

        // Add the power consumption of the months between the last fetched month and the current month
        string[] monthNames = { "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER" };
        bool isBetween = false;

        foreach (string month in monthNames)
        {
            if (month == lastFetchedMonth)
            {
                isBetween = true;
                continue;
            }

            if (month == currentMonth)
            {
                break;
            }

            if (isBetween)
            {
                inactivePeriodTotalPowerConsumption += inBetweenMonthValue[month]["units"].AsDouble;
                // Debug.Log("Adding power consumption for month: " + month + ", Units: " + inBetweenMonthValue[month]["units"].AsDouble);
            }
        }
        // Debug.Log("Inactive Period Power Consumption Upto Start of Current Month: " + inactivePeriodTotalPowerConsumption);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the daily power consumption of the current month
        energyDataFetch.GetCurrentMonthPowerConsumption((JSONNode value) =>
        {
            currentMonthValue = value;    
            isCurrentMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isCurrentMonthFetchCompleted);
        // Debug.Log("Current Month Daily Power Consumption: " + currentMonthValue.ToString());

        // Add the power consumption of the days from the start of the current month to currentDateTime
        tempDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
        // Debug.Log("Current Date Time: " + currentDateTime.Date);
        // Debug.Log("Start of Month Date Time: " + tempDateTime.Date);
        while (tempDateTime.Date < currentDateTime.Date)
        {
            // Get only the day of the date
            string dayString = tempDateTime.Day.ToString();
            // Debug.Log("Date String: " + dayString);
            // Debug.Log("Power Consumption: " + currentMonthValue[dayString].AsDouble);
            inactivePeriodTotalPowerConsumption += currentMonthValue[dayString].AsDouble;
            tempDateTime = tempDateTime.AddDays(1);
        }
        // Debug.Log("Inactive Period Power Consumption Upto Current Date: " + inactivePeriodTotalPowerConsumption);

        // Calculate per day inactive period power consumption
        double daysPast = (currentDateTime.Date - lastDateTime.Date).TotalDays;
        // Debug.Log("Days Past: " + daysPast);
        inactivePeriodPowerConsumption = inactivePeriodTotalPowerConsumption / daysPast;

        // Invoke the callback with the calculated value
        callback(inactivePeriodPowerConsumption);
    }

    private IEnumerator PreviousYearScenario(DateTime lastDateTime, DateTime currentDateTime, Action<double> callback)
    {
        double inactivePeriodPowerConsumption = 0.0f;
        double inactivePeriodTotalPowerConsumption = 0.0f;
        bool isLastFetchedMonthFetchCompleted = false;
        bool isLastYearMonthFetchCompleted = false;
        bool isCurrentYearMonthFetchCompleted = false;
        bool isCurrentMonthFetchCompleted = false;
        JSONNode lastFetchedMonthValue = null;
        JSONNode lastFetchedYearValue = null;
        JSONNode currentYearValue = null;
        JSONNode currentMonthValue = null;
        string lastFetchedYear = lastDateTime.Year.ToString();
        string currentYear = currentDateTime.Year.ToString();
        string lastFetchedMonth = lastDateTime.ToString("MMMM").ToUpper();
        string currentMonth = currentDateTime.ToString("MMMM").ToUpper();

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the daily power consumption of the last fetched month
        // Debug.Log("Last Fetched Year: " + lastFetchedYear);
        // Debug.Log("Last Fetched Month: " + lastFetchedMonth);
        // Fetch the specific month power consumption
        energyDataFetch.GetSpecificMonthPowerConsumption(lastFetchedYear, lastFetchedMonth, (JSONNode value) =>
        {
            lastFetchedMonthValue = value;    
            isLastFetchedMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isLastFetchedMonthFetchCompleted);
        // Debug.Log("Specific Month Daily Power Consumption: " + lastFetchedMonthValue.ToString());

        // Add the power consumption of the days from lastDateTime to the end of the lastFetchedMonth
        DateTime tempDateTime = lastDateTime;
        DateTime lastDayOfMonthDateTime = new DateTime(lastDateTime.Year, lastDateTime.Month, DateTime.DaysInMonth(lastDateTime.Year, lastDateTime.Month));
        // Debug.Log("Last Date Time: " + tempDateTime.Date);
        // Debug.Log("Last Day of Month Date Time: " + lastDayOfMonthDateTime.Date);
        while (tempDateTime.Date <= lastDayOfMonthDateTime.Date)
        {
            // Get only the day of the date
            string dayString = tempDateTime.Day.ToString();
            // Debug.Log("Date String: " + dayString);
            // Debug.Log("Power Consumption: " + lastFetchedMonthValue[dayString].AsDouble);
            inactivePeriodTotalPowerConsumption += lastFetchedMonthValue[dayString].AsDouble;
            tempDateTime = tempDateTime.AddDays(1);
        }
        // Debug.Log("Inactive Period Power Consumption Upto Last Fetched Month: " + inactivePeriodTotalPowerConsumption);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the monthly power consumption of the last year
        energyDataFetch.GetYearlyPowerConsumption(lastFetchedYear, (JSONNode value) =>
        {
            lastFetchedYearValue = value;    
            isLastYearMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isLastYearMonthFetchCompleted);
        // Debug.Log("Yearly Power Consumption: " + lastFetchedYearValue.ToString());

        // Add the power consumption of the months from the last fetched month to the end of the year 
        string[] monthNames = { "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER" };
        bool isBetween = false;

        foreach (string month in monthNames)
        {
            if (month == lastFetchedMonth)
            {
                isBetween = true;
                continue;
            }

            if (isBetween)
            {
                inactivePeriodTotalPowerConsumption += lastFetchedYearValue[month]["units"].AsDouble;
                // Debug.Log("Adding power consumption for month: " + month + ", Units: " + lastFetchedYearValue[month]["units"].AsDouble);
            }
        }
        // Debug.Log("Inactive Period Power Consumption Upto End of Last Year: " + inactivePeriodTotalPowerConsumption); 

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the yearly power consumption of the current year
        energyDataFetch.GetYearlyPowerConsumption(currentYear, (JSONNode value) =>
        {
            currentYearValue = value;    
            isCurrentYearMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isCurrentYearMonthFetchCompleted);
        // Debug.Log("Yearly Power Consumption: " + currentYearValue.ToString());

        // Add the power consumption of the months from the start of the year to the current month
        foreach (string month in monthNames)
        {
            if (month == currentMonth)
            {
                break;
            }

            inactivePeriodTotalPowerConsumption += currentYearValue[month]["units"].AsDouble;
            // Debug.Log("Adding power consumption for month: " + month + ", Units: " + currentYearValue[month]["units"].AsDouble);
        }
        // Debug.Log("Inactive Period Power Consumption Upto Start of Current Month: " + inactivePeriodTotalPowerConsumption);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Fetch the daily power consumption of the current month
        energyDataFetch.GetCurrentMonthPowerConsumption((JSONNode value) =>
        {
            currentMonthValue = value;    
            isCurrentMonthFetchCompleted = true;
        });

        // Wait until the fetch is completed
        yield return new WaitUntil(() => isCurrentMonthFetchCompleted);
        // Debug.Log("Current Month Daily Power Consumption: " + currentMonthValue.ToString());

        // Add the power consumption of the days from the start of the current month to currentDateTime
        tempDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
        // Debug.Log("Current Date Time: " + currentDateTime.Date);
        // Debug.Log("Start of Month Date Time: " + tempDateTime.Date);
        while (tempDateTime.Date < currentDateTime.Date)
        {
            // Get only the day of the date
            string dayString = tempDateTime.Day.ToString();
            // Debug.Log("Date String: " + dayString);
            // Debug.Log("Power Consumption: " + currentMonthValue[dayString].AsDouble);
            inactivePeriodTotalPowerConsumption += currentMonthValue[dayString].AsDouble;
            tempDateTime = tempDateTime.AddDays(1);
        }
        // Debug.Log("Inactive Period Power Consumption Upto Current Date: " + inactivePeriodTotalPowerConsumption);

        // Calculate per day inactive period power consumption
        double daysPast = (currentDateTime.Date - lastDateTime.Date).TotalDays;
        // Debug.Log("Days Past: " + daysPast);
        inactivePeriodPowerConsumption = inactivePeriodTotalPowerConsumption / daysPast;

        // Invoke the callback with the calculated value
        callback(inactivePeriodPowerConsumption);
    }
}