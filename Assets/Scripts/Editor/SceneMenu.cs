using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public class SceneMenu
{
    [MenuItem("Scenes/StarLobby")]
    static void OpenLobby()
    {
        OpenScene(SceneUtils.Names.StarLobby);
    }

    [MenuItem("Scenes/MoonScene")]
    static void OpenMaze()
    {
        OpenScene(SceneUtils.Names.MoonScene);
    }

    [MenuItem("Scenes/ComplexInteractions")]
    static void OpenComplexInteractions()
    {
        OpenScene(SceneUtils.Names.ComplexInteractions);
    }

    static void OpenScene(string name)
    {
        Scene persistentScene = EditorSceneManager.OpenScene("Assets/Scenes/" + SceneUtils.Names.XRPersistent + ".unity", OpenSceneMode.Single);
        Scene currentScene = EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity", OpenSceneMode.Additive);
        SceneUtils.AlignXRRig(persistentScene, currentScene);
    }



}
