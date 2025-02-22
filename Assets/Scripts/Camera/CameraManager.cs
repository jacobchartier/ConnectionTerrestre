using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private CinemachineVirtualCamera playerVCam;
    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public List<IMenuHandler> menus = new List<IMenuHandler>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        EnableInteractiveMode(true);
        FreezeCamera(false);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        playerVCam = GameObject.Find("vCam (1st Person View)").GetComponent<CinemachineVirtualCamera>();

        foreach (var cam in cameras)
            if (cam == null) 
                cameras.Remove(cam);
    }

    private void Update()
    {
        foreach (var cam in cameras)
            if (cam == null)
                cameras.Remove(cam);
    }

    public static void EnableInteractiveMode(bool isEnable)
    {
        Instance.FreezeCamera(isEnable);

        if (isEnable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    internal void FreezeCamera(bool isFrozen)
    {
        if (isFrozen)
            playerVCam.GetComponent<CinemachineInputProvider>().enabled = false;
        else
            playerVCam.GetComponent<CinemachineInputProvider>().enabled = true;
    }

    public void SetCamera(CinemachineVirtualCamera vCam)
    {
        foreach (var camera in cameras)
            camera.Priority = 0;

        playerVCam.Priority = 0;

        vCam.Priority = 1;
    }

    public void ResetCameras()
    {
        playerVCam.Priority = 1;

        foreach (var camera in cameras)
            if (camera == null)
                cameras.Remove(camera);
            else
                camera.Priority = 0;
    }
}
