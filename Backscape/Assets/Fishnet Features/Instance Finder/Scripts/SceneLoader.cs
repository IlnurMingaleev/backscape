using UnityEngine;
using FishNet;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine.SceneManagement;
using FishNet.Connection;


public class SceneLoader : NetworkBehaviour
{
    public const string SCENE_NAME_ADDITIVE = "New scene";
    public const string SCENE_NAME_MAIN = "3D scene";
    public bool sceneStack = false;
    private int stackedSceneHandle = 0;

    private void Start()
    {
        InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd;
    }

    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager != null) 
        {
            InstanceFinder.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
        }
    }
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

    private void LoadScene(NetworkObject networkObj) 
    {
        if (!networkObj.Owner.IsActive)
        {
            return;
        }

        SceneLookupData lookupData;
        //SceneLoadData sceneLoadData = new SceneLoadData(SCENE_NAME);
        /*Debug.Log("Loading by handle? " + (stackedSceneHandle != 0));
        if (stackedSceneHandle != 0)
        {
            lookupData = new SceneLookupData(stackedSceneHandle);
        }
        else 
        {
            
        }
        */
        lookupData = new SceneLookupData(stackedSceneHandle, SCENE_NAME_ADDITIVE);

        SceneLoadData sceneLoadData = new SceneLoadData(lookupData);
        sceneLoadData.Options.AllowStacking = true;
        //sceneLoadData.Options.LocalPhysics = LocalPhysicsMode.Physics3D;

        //SceneLookupData unloadLookupData = new SceneLookupData(stackedSceneHandle, SCENE_NAME_MAIN);
        SceneUnloadData sceneUnloadData = new SceneUnloadData(SCENE_NAME_MAIN);
        sceneUnloadData.Options.Mode = UnloadOptions.ServerUnloadMode.UnloadUnused;

        sceneLoadData.MovedNetworkObjects = new NetworkObject[] { networkObj };
        sceneLoadData.ReplaceScenes = ReplaceOption.All;
        //base.NetworkManager.SceneManager.LoadConnectionScenes(sceneLoadData);
        InstanceFinder.SceneManager.LoadConnectionScenes(networkObj.Owner, sceneLoadData);
        InstanceFinder.SceneManager.UnloadConnectionScenes(networkObj.Owner, sceneUnloadData);

        //Scene mainScene = base.NetworkManager.SceneManager.GetScene(SCENE_NAME_MAIN);
        //InstanceFinder.SceneManager.SceneConnections[]
        
        
    }

 
}
