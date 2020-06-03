using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{


    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneCo(sceneIndex));
    }

    private IEnumerator LoadSceneCo(int sceneIndex)
    {
        // Load scene asynchronously.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
    }
}
