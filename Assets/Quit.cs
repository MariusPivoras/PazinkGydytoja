using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public void exit()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        Application.Quit();
    }
}