//Remove on 2023/01/01
//using UnityEngine;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnitySceneManager = UnityEngine.SceneManagement;

using UnityEngine;
using FishNet;
using FishNet.Managing.Logging;
using FishNet.Managing.Scened;
using FishNet.Object;
using UnityEngine.SceneManagement;
using FishNet.Connection;
using System.Collections;
using System.Collections.Generic;
using System;


public class SceneChanger : MonoBehaviour
{


    public const string SCENE_NAME_ADDITIVE = "New scene";
    public const string SCENE_NAME_MAIN = "3D scene";
    public const string SCENE_NAME_WAREHOUSE = "Warehouse";
    public bool sceneStack = false;
    private NetworkObject playerNOB;
    private Vector3 spawnPoint = new Vector3(-26, 10, 41);
    private int stackedSceneHandle = 0;

    //Scene mainScene;
    private string[] sceneOrder = new string[] { SCENE_NAME_MAIN, SCENE_NAME_WAREHOUSE, SCENE_NAME_ADDITIVE };

    private int currentScene = 0;

    private NetworkObject currentNOB;

    public static event Action<NetworkObject> OnSceneLoadEnd;

    //private float raycastDistance = 2.0f;
    //[SerializeField] private LayerMask cubeLayerMask;
    //private bool xKeyPressed = false;




    //On collision with a box our new scene loads and I want the player collided with that box move to new scene.
    //While other player will remain on main scene
    [Server(Logging = LoggingType.Off)]
    private void OnTriggerEnter(Collider other)
    {
        NetworkObject networkObject = other.GetComponent<NetworkObject>();
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (networkObject != null)
        {
            //Debug.Log("Triggered");
            if (playerStats.CurrentScene == 0)
            {
                LoadScene(networkObject, 0);
                playerStats.CurrentScene = 1;
            }
            else if (playerStats.CurrentScene == 1)
            {
                LoadScene(networkObject, 1);
                playerStats.CurrentScene = 2;
            }

        }
    }

    private void Start()
    {
        //InstanceFinder.SceneManager.OnLoadEnd += SceneManager_OnLoadEnd; ;
        //InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
    }

    /*private void SceneManager_OnLoadEnd(SceneLoadEndEventArgs obj)
    {
        if (!obj.QueueData.AsServer)
        {
            return;
        }
        if (sceneStack)
        {
            return;
        }

        if (stackedSceneHandle != 0)
        {
            return;
        }
        if (obj.LoadedScenes.Length > 0)
        {
            stackedSceneHandle = obj.LoadedScenes[0].handle;
        }
    }
*/

   /* private void TimeManager_OnTick()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            xKeyPressed = true;

        }
        if (xKeyPressed)
        {
            PerformRaycast();
            xKeyPressed = false;
        }




    }*/

/*    private void OnDestroy()
    {
        if (InstanceFinder.SceneManager != null)
        {
            InstanceFinder.SceneManager.OnLoadEnd -= SceneManager_OnLoadEnd;
        }
    }*/
    //Main Logic of Loading scene
    
    private void LoadScene(NetworkObject networkObj, int index)
    {
        if (!networkObj.Owner.IsActive)
        {
            return;
        }

        currentNOB = networkObj;
        SceneLookupData lookupData = new SceneLookupData( sceneOrder[index + 1]);

        SceneLoadData sceneLoadData = new SceneLoadData(lookupData);
        //sceneLoadData.Options.LocalPhysics = LocalPhysicsMode.Physics3D;
        sceneLoadData.Options.AllowStacking = false;

        sceneLoadData.MovedNetworkObjects = new NetworkObject[] { networkObj };
        sceneLoadData.ReplaceScenes = ReplaceOption.None;
        InstanceFinder.SceneManager.LoadConnectionScenes(networkObj.Owner, sceneLoadData);

        if (index == 1)
        { 
            
            InstanceFinder.SceneManager.OnClientPresenceChangeEnd += SceneManager_OnClientPresenceChangeEnd1; ;
            
        }




    }

    private void SceneManager_OnClientPresenceChangeEnd1(ClientPresenceChangeEventArgs obj)
    {

        SceneUnloadData sceneUnloadData = new SceneUnloadData(SCENE_NAME_WAREHOUSE);
        sceneUnloadData.Options.Mode = UnloadOptions.ServerUnloadMode.UnloadUnused;
        if (currentNOB != null)
        {
            InstanceFinder.SceneManager.UnloadConnectionScenes(currentNOB.Owner, sceneUnloadData);
            //currentNOB.transform.position = spawnPoint;
        }
    }

    /*private void PerformRaycast()
    {

        RaycastHit hit;
        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastDistance, cubeLayerMask);
        if (hit.collider != null)
        {
            //Debug.Log("raycast performed");
            ChooseScene();
        }


        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);



    }*/
}

