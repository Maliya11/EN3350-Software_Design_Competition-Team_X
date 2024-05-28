using UnityEngine;

public class EnergyDataFetch : RequestBase
{
    // URL related to the energy information
    // URL to view yearly power consumption
    private string yearlyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/yearly/view";

    // URL to view power consumption by specific month
    private string monthlyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/month/view";

    // URL to view power consumption by current month
    private string currentMonthPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current-month/view";

    // URL to view daily power consumption by specific month
    private string dailyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/month/daily/view";

    // URL to view daily power consumption by current month
    private string currentMonthDailyPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current-month/daily/view";

    // URL to view all power consumption
    private string allPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/all/view";

    // URL to view current power consumption
    private string currentPowerConsumptionURL = "http://20.15.114.131:8080/api/power-consumption/current/view";

    // Method to fetch energy data
    private string viewMethod = "GET";
    


}
