using System;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldValidation : MonoBehaviour
{
    [SerializeField]
    InputField[] inputFields;

    public void PrintNamesInsideInputFields()
    {
        string[] names = new string[] { "HelloSandeepa1", "HelloSandeepa2", "HelloSandeepa3", "HelloSandeepa4", "HelloSandeepa5" };
        
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (i < names.Length)
            {
                inputFields[i].text = names[i];
                Debug.Log("Text inside InputField " + (i+1) + ": " + inputFields[i].text);
            }
            else
            {
                Debug.LogWarning("Not enough names provided for all input fields.");
                break;
            }
        }
    }
}
