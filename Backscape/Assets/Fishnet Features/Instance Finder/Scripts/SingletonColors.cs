using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonColors : MonoBehaviour
{
    public static SingletonColors Instance { get; private set; }

    public List<Color> colors = new List<Color>() { Color.red, Color.blue, Color.green };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else 
        {
            Instance = this;
        }
    }
}
