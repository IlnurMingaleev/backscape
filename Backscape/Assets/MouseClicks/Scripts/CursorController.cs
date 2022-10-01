using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FishNet.Object;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;
    [SerializeField] private Texture2D cursorClicked;

    [SerializeField] private GameObject playerPrefab;
    
    private CameraController cameraController;
    private CursorControls controls;
    private Camera mainCamera;

    public static event Action<GameObject> OnMouseClick;



    private void Awake()
    {
        controls = new CursorControls();
        ChangeCursor(cursor);
        //Cursor.lockState = CursorLockMode.Confined;
        FirstObjectNotifier.OnFirstObjectSpawned += FirstObjectNotifier_OnFirstObjectSpawned;
        
        

    }

    private void FirstObjectNotifier_OnFirstObjectSpawned(Camera obj)
    {
        mainCamera = obj;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    private void ChangeCursor(Texture2D cursorType) 
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }
    // Start is called before the first frame update
    private void Start()
    {
        controls.Mouse.Click.started += _ => StartedLeftClick();
        controls.Mouse.Click.performed += _ => EndedLeftClick();
        
    }

    private void StartedLeftClick() 
    {
        ChangeCursor(cursorClicked);
    }

    private void EndedLeftClick() 
    {
        ChangeCursor(cursor);
        DetectObject();
    }

    private void DetectObject()
    {

        if (mainCamera != null)
        {
             Ray ray = mainCamera.ScreenPointToRay(controls.Mouse.Position.ReadValue<Vector2>());
             RaycastHit hit;
             if (Physics.Raycast(ray, out hit))
             {
                 //Debug.Log("We are in raycast");
                 if (hit.collider.CompareTag("Cube"))
                 {
                    //Debug.Log("3D Hit: " + hit.collider.tag);
                    OnMouseClick?.Invoke(hit.collider.gameObject);
                    //Debug.Log(hit.collider.gameObject);

                 }
             }
        }

    }

   /* public void SetCamera() 
    {
        mainCamera = Camera.main;
        Debug.Log(mainCamera);
    }*/
}
