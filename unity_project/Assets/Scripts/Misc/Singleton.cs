using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Create a static instance of any class that inherits from MonoBehaviour
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        // Check if an instance of the class already exists
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

