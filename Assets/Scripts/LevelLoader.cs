using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadLevel("Zone1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadLevel("Zone2");
        }
    }
}