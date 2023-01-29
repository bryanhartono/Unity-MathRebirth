using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] string nextScene;

    void OnEnable()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
    public void skip()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
