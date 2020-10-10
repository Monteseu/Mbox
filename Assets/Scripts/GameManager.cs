using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager _singleton;
    public static GameManager get
    {
        get
        {
            if (_singleton == null)
                _singleton = FindObjectOfType<GameManager>();
            return _singleton;
        }
    }

    [HideInInspector]
    public int currentLevel = 1;

    [Header("Level Settings")]
    [SerializeField]
    LevelView[] levelPrefabs;
    [SerializeField]
    Transform mainCharacter;

    LevelView currentLevelView;

    // References
    public Camera gameCamera;
    public FxManager fxManager;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        currentLevelView = Instantiate(levelPrefabs[currentLevel % levelPrefabs.Length]);

        Debug.Log("Current Level: " + currentLevel);
        SetupCameras();
    }

    void SetupCameras()
    {
        foreach(CinemachineVirtualCamera virtualCam in currentLevelView.GetComponentsInChildren<CinemachineVirtualCamera>(true))
        {
            virtualCam.Follow = mainCharacter;
            virtualCam.LookAt = mainCharacter;
        }
    }

    public void OnFinishLevel(bool win)
    {
        if (win)
            ChangeLevel(true);
        else
            ReloadLevel();
    }

    void ChangeLevel(bool forward)
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + (forward ? 1 : -1));
        ReloadLevel();
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    #region Debug
    private void Update()
    {
        if (!Application.isEditor)
            return;

        if (Input.GetKey(KeyCode.N))
            ChangeLevel(true);
        if (Input.GetKey(KeyCode.B))
            ChangeLevel(false);
        if (Input.GetKey(KeyCode.R))
            ReloadLevel();
        if (Input.GetKey(KeyCode.T))
            PlayerPrefs.DeleteKey("CurrentLevel");
    }
    #endregion
}
