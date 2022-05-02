using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.LoadUserData();
    }
    private void Start() {
        // 更新最高分
        TextMeshProUGUI bestScoreText = GameObject.Find("Best Score").GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance.BestGamerName != "") {
            bestScoreText.text = "Best Score: " + GameManager.Instance.BestGamerName + ": " + GameManager.Instance.BestGamerScore;
        }
    }
    // scene 管理
    public void StartGame() {
        SceneManager.LoadScene(1);
    }
    public void ExitGame() {
        GameManager.Instance.SaveUserData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // 目前玩家信息
    public string PlayerName;
    public int PlayerScore;
    // 最高分信息
    public string BestGamerName;
    public int BestGamerScore;
    // 信息更新
    public void ChangePlayerName(string newName) {
        GameManager.Instance.PlayerName = newName;
    }

    // data 管理
    class SavaData {
        public string Name;
        public int Score;
    }

    public void SaveUserData() {
        SavaData data = new SavaData();
        data.Name = GameManager.Instance.BestGamerName;
        data.Score = GameManager.Instance.BestGamerScore;
        string jsonString = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/saveData.json", jsonString);
    }
    public void LoadUserData() {
        string filePath = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(filePath)) {
            string jsonString = File.ReadAllText(filePath);
            SavaData data = JsonUtility.FromJson<SavaData>(jsonString);
            GameManager.Instance.BestGamerName = data.Name;
            GameManager.Instance.BestGamerScore = data.Score;
        }
    }
}
