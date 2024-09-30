using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    IEnumerator LoadSceneAfterDelay(float waitTime, string name)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(name);
        Debug.Log("Fin de la coroutine.");
    }

    public void Change(string name)
    {
        name = name.Trim();
        Debug.Log("Démarrage de la coroutine:");
        StartCoroutine(LoadSceneAfterDelay(2f, name));
    }


}

