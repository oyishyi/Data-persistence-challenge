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
        // ������߷�
        TextMeshProUGUI bestScoreText = GameObject.Find("Best Score").GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance.BestGamerName != "") {
            bestScoreText.text = "Best Score: " + GameManager.Instance.BestGamerName + ": " + GameManager.Instance.BestGamerScore;
        }
    }
    // scene ����
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

    // Ŀǰ�����Ϣ
    public string PlayerName;
    public int PlayerScore;
    // ��߷���Ϣ
    public string BestGamerName;
    public int BestGamerScore;
    // ��Ϣ����
    public void ChangePlayerName(string newName) {
        GameManager.Instance.PlayerName = newName;
    }

    // data ����
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
