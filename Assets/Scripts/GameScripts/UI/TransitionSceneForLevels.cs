using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionSceneForLevels : MonoBehaviour
{
    public void SceneLoad(int numLevel)
    {
        SceneManager.LoadScene(3);
        PlayerPrefs.SetInt("NumLevel", numLevel);
    }
}
