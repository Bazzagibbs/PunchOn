using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHelper : MonoBehaviour
{


    public void StartGame()
    {
        //Scene scene = SceneManager.GetSceneByName("Game");
        SceneManager.LoadScene("Game");
    }
}
