using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtils
{

    public enum SceneId
    {
        StarLobby,
        MoonScene,
        ComplexInteractions
        
    }

    public static readonly string[] scenes = { Names.StarLobby, Names.MoonScene, Names.ComplexInteractions };
    
    public static class Names
    {
        public static readonly string XRPersistent = "XR Persistent";
        public static readonly string StarLobby = "Star Lobby";
        public static readonly string MoonScene = "Moon Scene";
        public static readonly string ComplexInteractions = "Complex Interactions";

    }

    public static void AlignXRRig(Scene persistentScene, Scene currentScene)
    {
        GameObject[] currentObjects = currentScene.GetRootGameObjects();
        GameObject[] persistentObjects = persistentScene.GetRootGameObjects();
        
        foreach (var origin in currentObjects)
        {
            if (origin.CompareTag("XRRigOrigin"))
            {
                foreach(var rig in persistentObjects)
                {
                    if (rig.CompareTag("XRRig"))
                    {
                        rig.transform.position = origin.transform.position;
                        rig.transform.rotation = origin.transform.rotation;
                        return;
                    }
                }
            }
        }
    }
  
}
