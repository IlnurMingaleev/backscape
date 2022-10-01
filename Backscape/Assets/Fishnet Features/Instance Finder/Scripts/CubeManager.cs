using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FishNet;


[RequireComponent(typeof(PlayerInput))]
public class CubeManager : MonoBehaviour
{

    [SerializeField] private List<Material> matList = new List<Material>();
    [SerializeField] private GameObject cubePrefab;

    private GameObject sceneCube;
    private bool isCubeOnScene;

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
        isCubeOnScene = false;
        //player.OnCollisionDestroy += DespawnCube;
    }


    #region UnityEventCallbacks
    public void OnMKeyPress(InputAction.CallbackContext context) 
    {
        if (context.performed && (sceneCube != null)) 
        {
            ApplyRandomMaterial(sceneCube);

        }
    }

    public void OnSevenKeyPress(InputAction.CallbackContext context) 
    {
        if ( context.performed)
        {
            if (!isCubeOnScene)
            {
                SpawnCube();
                isCubeOnScene = true;
            }
            else 
            {
                DespawnCube(sceneCube);
            }

        }

    }
    #endregion

    #region RandomMaterialMethod

    private void ApplyRandomMaterial(GameObject cubeObj) 
    {
        var randomIndex = Random.Range(0, 3);
        MeshRenderer meshRenderer = cubeObj.GetComponent<MeshRenderer>();
        meshRenderer.material = matList[randomIndex];
    }
    #endregion

    #region Spawn/Despawn

    private void SpawnCube()  
    {
        GameObject cube = Instantiate(cubePrefab);
        InstanceFinder.ServerManager.Spawn(cube);
    }

    public void DespawnCube(GameObject gameObject) 
    {
        InstanceFinder.ServerManager.Despawn(gameObject);
        isCubeOnScene = false;
    
    }
    #endregion

    private void SetActiveCube(GameObject cubeObj) 
    {
        sceneCube = cubeObj;
    }
}
