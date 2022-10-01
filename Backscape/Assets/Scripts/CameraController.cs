using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System;
using Cinemachine;

public class CameraController : NetworkBehaviour
{
    private Quaternion newRotation;
    private Vector3 newZoom;

    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private float rotationAmount;
    [SerializeField] private float movementTime;

    [SerializeField] private Transform cameraTransform;

 
   public event EventHandler<OnCameraActivateEventArgs> OnCameraActivate;

   public class OnCameraActivateEventArgs : EventArgs 
   {
        public Camera cam;

   }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner) 
        {
            gameObject.SetActive(true);
            /*Camera cam = Camera.main;
            cameraTransform = cam.transform;
            CinemachineVirtualCamera virtualCam = cam.GetComponent<CinemachineVirtualCamera>();
            virtualCam.Follow = transform;
            virtualCam.LookAt = transform;*/
            //Camera camera = gameObject.transform.GetChild(0).GetComponent<Camera>();
            //cursorController.SetCamera(camera);
            //Debug.Log(camera);
            //RaiseOnCameraActivate(camera);
            //OnCameraActivate += cursorController.CameraController_OnCameraActivate;
           
            //OnCameraActivate?.Invoke(this, new OnCameraActivateEventArgs { cam = camera });
            
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        ZoomCamera();
    }

    private void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Q)) 
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
    }

    private void ZoomCamera() 
    {
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }

        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        Vector3 clampedZoom = newZoom;

        clampedZoom.y = Mathf.Clamp(clampedZoom.y, 12, 100);
        clampedZoom.z = Mathf.Clamp(clampedZoom.z, -100, -12);

        newZoom = clampedZoom;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        
    }
}
