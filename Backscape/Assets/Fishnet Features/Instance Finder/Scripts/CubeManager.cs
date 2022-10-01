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

    private MeshRenderer meshRenderer;
    private bool isCubeOnScene;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        isCubeOnScene = false;
        //player.OnCollisionDestroy += DespawnCube;
    }


    #region UnityEventCallbacks
    public void OnMKeyPress(InputAction.CallbackContext context) 
    {
        if (context.started || context.performed) 
        {
            ApplyRandomMaterial();

        }
    }

    public void OnSevenKeyPress(InputAction.CallbackContext context) 
    {
        if (context.started || context.performed)
        {
            if (!isCubeOnScene)
            {
                SpawnCube();
            }

        }

    }
    #endregion

    #region RandomMaterialMethod

    private void ApplyRandomMaterial() 
    {
        var randomIndex = Random.Range(0, 3);
        meshRenderer.material = matList[randomIndex];
    }
    #endregion

    #region Spawn/Despawn

    private void SpawnCube()  
    {
        GameObject sceneCube = Instantiate(cubePrefab);
        InstanceFinder.ServerManager.Spawn(sceneCube);
        isCubeOnScene = true;
    }

    public void DespawnCube(GameObject gameObject) 
    {
        InstanceFinder.ServerManager.Despawn(gameObject);
        isCubeOnScene = false;
    
    }
    #endregion

}
