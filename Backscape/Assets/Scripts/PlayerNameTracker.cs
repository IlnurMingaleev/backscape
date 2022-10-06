using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using System;
using FishNet.Transporting;
public class PlayerNameTracker : NetworkBehaviour
{

    public static event Action<NetworkConnection, string> OnNameChange;


    [SyncObject]

    private readonly SyncDictionary<NetworkConnection, string> _playerNames = new SyncDictionary<NetworkConnection, string>();

    private static PlayerNameTracker _instance;

    public static string GetPlayerName( NetworkConnection connection) 
    {
        string result = null;
        if (_instance._playerNames.ContainsKey(connection)) 
        {
            result = _instance._playerNames[connection];
        }
        return result;
        
    }
    void Awake()
    {
       if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        _playerNames.OnChange += _playerNames_OnChange;

    }

    private void _playerNames_OnChange(SyncDictionaryOperation op, NetworkConnection key, string value, bool asServer)
    {
        if (op == SyncDictionaryOperation.Add || op == SyncDictionaryOperation.Set)
            OnNameChange?.Invoke(key, value);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        base.NetworkManager.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
    }
    private void ServerManager_OnRemoteConnectionState(NetworkConnection arg1, FishNet.Transporting.RemoteConnectionStateArgs arg2)
    {
        if (arg2.ConnectionState != RemoteConnectionState.Started)
            _playerNames.Remove(arg1);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        base.NetworkManager.ServerManager.OnRemoteConnectionState -= ServerManager_OnRemoteConnectionState;
    }

    [Client]
    public static void SetName(string name) 
    {
        _instance.ServerSetName(name);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerSetName(string name, NetworkConnection sender = null) 
    {
        _playerNames[sender] = name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
