using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public static class CameraSwitch
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera activeCam = null;
    // Start is called before the first frame update
    public static bool isActiveCam(CinemachineVirtualCamera camera)
    {
        return camera == activeCam;
    }
    public static void swithcam(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        activeCam = camera;
        Debug.Log(activeCam.Name);
        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != null && c != activeCam)
            {
                Debug.Log(c.Name);
                c.Priority = 0;
            }
        }
    }
    public static void register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }
    public static void unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
