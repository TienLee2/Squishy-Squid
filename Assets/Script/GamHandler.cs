using UnityEngine;

public class GamHandler : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = -1;
        Screen.SetResolution(1920, 1080, true);

        GameObject gameObject = new GameObject("Pipe" , typeof(SpriteRenderer));
        Score.Start();

#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }

}
