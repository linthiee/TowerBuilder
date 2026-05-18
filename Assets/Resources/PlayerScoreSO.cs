using UnityEngine;

public enum GameDifficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "PlayerScoreSO", menuName = "TowerBuilder/PlayerScoreSO")]
public class PlayerScoreSO : ScriptableObject
{
   public int easyScore;
   public int mediumScore;
   public int hardScore;

    public void LoadSettings()
    {
        easyScore = PlayerPrefs.GetInt("HighScore " + GameDifficulty.Easy.ToString(), 0);
        mediumScore = PlayerPrefs.GetInt("HighScore " + GameDifficulty.Medium.ToString(), 0);
        hardScore = PlayerPrefs.GetInt("HighScore " + GameDifficulty.Hard.ToString(), 0);
    }

    public void CheckAndSaveHighScore(GameDifficulty difficulty, int score)
    {
        string key = "HighScore " + difficulty.ToString();
        int savedHighScore = PlayerPrefs.GetInt(key, 0);

        if (score > savedHighScore)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();

            UpdateSOVariable(difficulty, score);
        }
    }

    public int GetHighScore(GameDifficulty difficulty)
    {
        switch (difficulty)
        {
            case GameDifficulty.Easy: 
                return easyScore;
            case GameDifficulty.Medium: 
                return mediumScore;
            case GameDifficulty.Hard: 
                return hardScore;
            default: 
                return 0;
        }
    }

    private void UpdateSOVariable(GameDifficulty difficulty, int score)
    {
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                easyScore = score;
                break;
            case GameDifficulty.Medium: 
                mediumScore = score; 
                break;
            case GameDifficulty.Hard:
                hardScore = score; 
                break;
        }
    }
}
