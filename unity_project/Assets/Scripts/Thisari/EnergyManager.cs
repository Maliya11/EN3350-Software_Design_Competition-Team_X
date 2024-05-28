using System.Collections;
using System.Collections;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // Reference to the EnergyDataFetch
    private EnergyDataFetch energyDataFetch;


    // Variables
    private float repeatRate = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Energy Manager Started");
        energyDataFetch = FindObjectOfType<EnergyDataFetch>();
        StartCoroutine(FetchCurrentPowerConsumption());
    }

    // Routine to fetch the current power consumption 
    private IEnumerator FetchCurrentPowerConsumption()
    {
        Debug.Log("Fetching Current Power Consumption");

        // Fetch the current power consumption
        Debug.Log("Power Consumption from EnergyManager: " + energyDataFetch.GetCurrentPowerConsumption());

        // Wait for the next fetch
        yield return new WaitForSeconds(repeatRate);

        // Repeat the process
        StartCoroutine(FetchCurrentPowerConsumption());
    }

}
