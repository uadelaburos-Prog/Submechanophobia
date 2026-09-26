using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int targetFps = 30;
    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFps;
    }

}
