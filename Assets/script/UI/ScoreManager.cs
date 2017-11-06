using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager {

    private static ScoreManager _instance;

    private List<float> highscores;
    private List<string> scoreNames;

    private const string HIGHSCORE_PREF = "highscore_";
    private const string NAME_PREF = "highscorer_";

    private const string DEFAULT_NAME = "NO1";
    private const int DEFAULT_SCORE = 999;

    protected ScoreManager() {
        highscores = loadHighscores();
        scoreNames = loadScoreNames();

        highscores.Clear();
        scoreNames.Clear();

        while(highscores.Count < Constants.NUM_HIGHSCORES) {
            highscores.Add(DEFAULT_SCORE);
        }
        while(scoreNames.Count < Constants.NUM_HIGHSCORES) {
            scoreNames.Add(DEFAULT_NAME);
        }
    }

    public static ScoreManager instance() {
        if (_instance == null) {
            _instance = new ScoreManager();
        }
        return _instance;
    }

    public bool tryInsertHighscore(float score, string name) {
        score = (float) System.Math.Round(score, 2);

        int ranking = getRanking(score);
        if(ranking != -1) {
            float prev = highscores[ranking];
            string prevName = scoreNames[ranking];

            highscores[ranking] = score;
            scoreNames[ranking] = name;

            // Shift lower scores down
            for (int j = ranking; j < highscores.Count - 1; j++) {
                float tmp = highscores[j + 1];
                highscores[j + 1] = prev;
                prev = tmp;

                // shift name too
                string tmpStr = scoreNames[j + 1];
                scoreNames[j + 1] = prevName;
                prevName = tmpStr;
            }

            saveHighscores();
            saveScoreNames();
            return true;
        }
        return false;
    }

    public int getRanking(float score) {
        score = (float) System.Math.Round(score, 2);
        for (int i = 0; i < highscores.Count; i++) {
            // scores are sorted low-to-high since we want to minimize time
            if (highscores[i] > score) {
                return i;
            }
        }
        return -1;
    }

    public List<float> getHighscores() {
        return highscores;
    }

    public List<string> getScoreNames() {
        return scoreNames;
    }

    private void saveHighscores() {
        for (int i = 0; i < Constants.NUM_HIGHSCORES; i++) {
            PlayerPrefs.SetFloat(HIGHSCORE_PREF + i, highscores[i]);
        }
    }

    private void saveScoreNames() {
        for (int i = 0; i < Constants.NUM_HIGHSCORES; i++) {
            PlayerPrefs.SetString(NAME_PREF + i, scoreNames[i]);
        }
    }

    private List<float> loadHighscores() {
        List<float> scores = new List<float>();
        for (int i = 0; i < Constants.NUM_HIGHSCORES; i++) {
            scores.Insert(i, PlayerPrefs.GetFloat(HIGHSCORE_PREF + i, DEFAULT_SCORE));
        }
        return scores;
    }

    private List<string> loadScoreNames() {
        List<string> names = new List<string>();
        for(int i = 0; i < Constants.NUM_HIGHSCORES; i++) {
            names.Insert(i, PlayerPrefs.GetString(NAME_PREF + i, DEFAULT_NAME));
        }
        return names;
    }
}
