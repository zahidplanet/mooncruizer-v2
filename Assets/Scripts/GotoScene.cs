using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoScene : MonoBehaviour
{
    public SceneUtils.SceneId nextScene = SceneUtils.SceneId.StarLobby;

    public void Go()
    {
        SceneLoader.Instance.LoadScene(SceneUtils.scenes[(int)nextScene]);
    }
    
}
