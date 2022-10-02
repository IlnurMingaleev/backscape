using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet;
using FishNet.Object;


[RequireComponent(typeof(PlayerInput))]
public class CubeManager : NetworkBehaviour
{

    //[SerializeField] private List<Material> matList = new List<Material>();
    [SerializeField] private GameObject cubePrefab;

    private GameObject sceneCube;
    private bool cubeOnScene;
    
    /*
        #region Types.

        public struct MaterialData 
        {
            public Material material;
        }

        public struct ReconcileMatData
        {
            public Material Material;

            public ReconcileMatData( Material newMaterial )
            {
                Material = newMaterial;
            }
        }
        #endregion*/

    private void Awake()
    {
        CursorController.OnMouseClick += CursorController_OnMouseClick;
    }

    private void CursorController_OnMouseClick(GameObject obj)
    {
        SetActiveCube(obj);
    }

    private void Start()
    {
        cubeOnScene = false;
        //player.OnCollisionDestroy += DespawnCube;
    }


    #region UnityEventCallbacks
    public void OnMKeyPress(InputAction.CallbackContext context) 
    {
        if (context.performed && (sceneCube != null)) 
        {
            ApplyRandomColor(sceneCube);

        }
    }

    public void OnSevenKeyPress(InputAction.CallbackContext context) 
    {
        if ( context.performed)
        {
            if (!cubeOnScene)
            {
                SpawnCube();
                
            }
            else 
            {
                DespawnCube(sceneCube);
            }

        }

    }
    #endregion

    #region RandomMaterialMethod

    private void ApplyRandomColor(GameObject cubeObj) 
    {
        var randomIndex = Random.Range(0, 3);
        cubeObj.GetComponent<CubeProperties>().g_cubeColor = SingletonColors.Instance.colors[randomIndex];
    }
    #endregion

    #region Spawn/Despawn

    private void SpawnCube()  
    {
        GameObject cube = Instantiate(cubePrefab);
        InstanceFinder.ServerManager.Spawn(cube);
        cubeOnScene = true;
    }

    public void DespawnCube(GameObject gameObject) 
    {
        InstanceFinder.ServerManager.Despawn(gameObject);
        cubeOnScene = false;
    
    }
    #endregion

    private void SetActiveCube(GameObject cubeObj) 
    {
        sceneCube = cubeObj;
    }
}
