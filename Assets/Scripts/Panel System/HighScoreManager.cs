using System.Collections.Generic;
using UnityEngine;

public static class HighScoreManager
{
    private const int MaxScores = 5;
    private const string ScoreCountKey = "ScoreCount";
    private const string ScoreKeyPrefix = "Score_";

    public static void SaveScore(float time)
    {
        List<float> scores = GetScores();

        scores.Add(time);

        scores.Sort();

        if (scores.Count > MaxScores)
        {
            scores.RemoveRange(
                MaxScores,
                scores.Count - MaxScores
            );
        }

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetFloat(
                ScoreKeyPrefix + i,
                scores[i]
            );
        }

        PlayerPrefs.SetInt(
            ScoreCountKey,
            scores.Count
        );

        PlayerPrefs.Save();
    }

    public static List<float> GetScores()
    {
        List<float> scores = new List<float>();

        int count =
            PlayerPrefs.GetInt(
                ScoreCountKey,
                0
            );

        for (int i = 0; i < count; i++)
        {
            scores.Add(
                PlayerPrefs.GetFloat(
                    ScoreKeyPrefix + i
                )
            );
        }

        return scores;
    }

    public static bool IsHighScore(float time)
    {
        List<float> scores = GetScores();

        if (scores.Count < MaxScores)
            return true;

        return time < scores[scores.Count - 1];
    }

    public static void ClearScores()
    {
        int count =
            PlayerPrefs.GetInt(
                ScoreCountKey,
                0
            );

        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey(
                ScoreKeyPrefix + i
            );
        }

        PlayerPrefs.DeleteKey(
            ScoreCountKey
        );

        PlayerPrefs.Save();
    }
}