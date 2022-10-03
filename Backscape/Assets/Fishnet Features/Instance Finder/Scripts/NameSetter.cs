using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameSetter : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _inputUsername;

    void Awake()
    {
        _inputUsername.onSubmit.AddListener(InputOnsubmit);
        
    }


    private void InputOnsubmit(string text) 
    {
        PlayerNameTracker.SetName(text);
    }
}
