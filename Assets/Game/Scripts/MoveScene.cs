using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    private string NextScene = "Pose Tracking";

    public void MoveGameScene()
    {
        SceneManager.LoadScene(NextScene);
    }
}
