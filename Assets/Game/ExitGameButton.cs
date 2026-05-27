using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    // This function will be called from the button's OnClick event
    public void ExitGame()
    {
        Debug.Log("Exit button pressed — quitting game...");

        // Quit the application (works in build)
        Application.Quit();

        // Works only in Unity Editor (for testing)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
