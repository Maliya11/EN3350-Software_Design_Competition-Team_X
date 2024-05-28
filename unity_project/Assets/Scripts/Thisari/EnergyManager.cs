using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    // Reference to the EnergyDataFetch
    private EnergyDataFetch energyDataFetch;

    // Start is called before the first frame update
    void Start()
    {
        energyDataFetch = new EnergyDataFetch();
        energyDataFetch.GetCurrentPowerConsumption();
    }
}
