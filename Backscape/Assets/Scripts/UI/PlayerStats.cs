using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using TMPro;
using FishNet.Connection;
using System;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private Transform playerName;
    [SerializeField] private Camera mainCamera;

    public static event Action<string> OnPlayerNameSet;

    private TextMeshPro _text;

    private void Awake()
    {
        _text = playerName.GetComponent<TextMeshPro>(); 
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        SetName();
        PlayerNameTracker.OnNameChange += PlayerNameTracker_OnNameChange;

    }

    private void PlayerNameTracker_OnNameChange(NetworkConnection arg1, string arg2)
    {
        if (arg1 != base.Owner) 
        {
            return;
        }
        SetName();
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        PlayerNameTracker.OnNameChange -= PlayerNameTracker_OnNameChange;
    }
    public override void OnOwnershipClient(NetworkConnection prevOwner)
    {
        base.OnOwnershipClient(prevOwner);
        SetName();
    }
    private void SetName() 
    {
        string result = null;
        if (base.Owner.IsValid)
            result = PlayerNameTracker.GetPlayerName(base.Owner);

        if (string.IsNullOrEmpty(result))
            result = "Default";

        PlayerStats.OnPlayerNameSet?.Invoke(result);
        _text.text = result;
    }
    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null)
        {
            playerName.LookAt(mainCamera.transform.position);
            playerName.Rotate(0, 180, 0);
        }
        else 
        {
            Debug.LogError("Camera is NULL");
        }
    }

}
