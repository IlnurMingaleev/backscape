using UnityEngine;
using FishNet;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine.SceneManagement;
using FishNet.Connection;
using System.Collections;
using System.Collections.Generic;


public class SceneLoader : NetworkBehaviour
{
    public const string SCENE_NAME_ADDITIVE = "New scene";
    public const string SCENE_NAME_MAIN = "3D scene";
    public bool sceneStack = false;
    private int stackedSceneHandle = 0;
    Scene mainScene;


    // Subscribing for an event
    private void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
        
    }
    
    //Unsubscribing from an event.
    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager != null) 
        {
            InstanceFinder.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
        }
    }

    // Initializing stackedSceneHandle variable
    private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (!obj.QueueData.AsServer)
            return;
        if (!sceneStack)
            return;
        if (stackedSceneHandle != 0)
            return;

        if (obj.LoadedScenes.Length > 0)
            stackedSceneHandle = obj.LoadedScenes[0].handle;
    }

    //On collision with a box our new scene loads and I want the player collided with that box move to new scene.
    //While other player will remain on main scene

    [Server(Logging = LoggingType.Off)]
    private void OnTriggerEnter(Collider other)
    {
        NetworkObject networkObject = other.GetComponent<NetworkObject>();
        if (networkObject != null) 
        {
            LoadScene(networkObject);
            //Debug.Log("Collision happened");
        }
    }

    //Main Logic of Loading scene

    private void LoadScene(NetworkObject networkObj) 
    {
        if (!networkObj.Owner.IsActive)
        {
            return;
        }

        SceneLookupData lookupData = new SceneLookupData(stackedSceneHandle, SCENE_NAME_ADDITIVE);
        
        SceneLoadData sceneLoadData = new SceneLoadData(lookupData);
        sceneLoadData.Options.LocalPhysics = LocalPhysicsMode.Physics3D;
        sceneLoadData.Options.AllowStacking = true;

        sceneLoadData.MovedNetworkObjects = new NetworkObject[] { networkObj };
        sceneLoadData.ReplaceScenes = ReplaceOption.OnlineOnly;
        InstanceFinder.SceneManager.LoadConnectionScenes(networkObj.Owner, sceneLoadData);

        SceneUnloadData sceneUnloadData = new SceneUnloadData(SCENE_NAME_MAIN);
        InstanceFinder.SceneManager.UnloadConnectionScenes(networkObj.Owner, sceneUnloadData);


    }


}

// Commented







//lookupData = new SceneLookupData(stackedSceneHandle, SCENE_NAME_ADDITIVE);




//sceneLoadData.Options.LocalPhysics = ;
//SceneLookupData unloadLookupData = new SceneLookupData(stackedSceneHandle, SCENE_NAME_MAIN);
//sceneUnloadData.Options.Mode = UnloadOptions.ServerUnloadMode.UnloadUnused;




//HashSet<NetworkConnection> networkConnectionsSet = InstanceFinder.SceneManager.SceneConnections[mainScene];
//NetworkConnection[] networkConnections = new NetworkConnection[networkConnectionsSet.Count];
//networkConnectionsSet.CopyTo(networkConnections);
//InstanceFinder.SceneManager.LoadConnectionScenes(networkConnections, sceneLoadData);

/*Debug.Log("Loading by handle? " + (stackedSceneHandle != 0));
if (stackedSceneHandle != 0)
{
    lookupData = new SceneLookupData(stackedSceneHandle);
}
else 
{
    lookupData = new SceneLookupData(SCENE_NAME_ADDITIVE);
}*/