using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinNight : MonoBehaviour
{
    public Text Score;  // UI Text for the score
    public Image YouDidIt;  // UI Image for "You Did It!"
    public CanvasGroup MainScore;  // CanvasGroup for the main score display
    public int Points;  // Final score points
	public GameObject HighScore;
    public AudioSource MainAS;  // AudioSource for playing sound clips
    public AudioClip LessOr0, Less1000, Less2000, Less4000, Less6000, Less8000, Less10000, MoreOr10000;  // Audio clips for different score ranges
    public Animator TextAnimator;  // Animator for playing score-related animations
    public float[] TimeForTextIncrease;  // Time duration for text increase based on the selected clip

    private int currentScore = 0;  // Tracks the current score during the score increment
    private int scoreStep = 10;  // How much the score increases per frame
    private float timeForIncrease;  // Holds the time for the current text increase duration

    void Start()
    {
        Resources.UnloadUnusedAssets();
		System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
        Points = DataManager.GetValue<int>("LastWonScore", "data:/");
        StartCoroutine(ScoreSequence());
        int challenges = DataManager.GetValue<int>("Challenge", "data:/");
        if (challenges != -1)
        {
            int[] cchallenges = DataManager.GetValue<int[]>("CompletedChallenges", "data:/");
            if (!DataManager.ValueExists("CompletedChallenges", "data:/"))
            {
                cchallenges = new int[16];
            }
            cchallenges[challenges] = 1;
            DataManager.SaveValue<int[]>("CompletedChallenges", cchallenges, "data:/");
        }
    }

    private IEnumerator ScoreSequence()
    {
        // Show "You Did It!" for 1 second
        YouDidIt.canvasRenderer.SetAlpha(1f);

        // Determine which audio clip to play and set the corresponding time for text increase
        if (Points == 0)
        {
            MainAS.clip = LessOr0;
            timeForIncrease = TimeForTextIncrease[0];
        }
        else if (Points < 1000)
        {
            MainAS.clip = Less1000;
            timeForIncrease = TimeForTextIncrease[1];
        }
        else if (Points < 2000)
        {
            MainAS.clip = Less2000;
            timeForIncrease = TimeForTextIncrease[2];
        }
        else if (Points < 4000)
        {
            MainAS.clip = Less4000;
            timeForIncrease = TimeForTextIncrease[3];
        }
        else if (Points < 6000)
        {
            MainAS.clip = Less6000;
            timeForIncrease = TimeForTextIncrease[4];
        }
        else if (Points < 8000)
        {
            MainAS.clip = Less8000;
            timeForIncrease = TimeForTextIncrease[5];
        }
        else if (Points < 10000)
        {
            MainAS.clip = Less10000;
            timeForIncrease = TimeForTextIncrease[6];
        }
        else
        {
            MainAS.clip = MoreOr10000;
            timeForIncrease = TimeForTextIncrease[7];
        }

        // Play the audio clip
        MainAS.Play();

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Fade out "You Did It!" over 1 second
        StartCoroutine(FadeCanvasGroup(YouDidIt.canvasRenderer, 1f, 0f, 1f));
        yield return new WaitForSeconds(1f);

        // Wait 1 second before fading in the MainScore CanvasGroup
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeCanvasGroup(MainScore, 0f, 1f, 1f));

        // Start incrementing the score 1 second earlier
		if (Points > DataManager.GetValue<int>("Highscore", "data:/"))
		{
			DataManager.SaveValue<int>("Highscore",Points ,"data:/");
			HighScore.SetActive(true);
        }
        StartCoroutine(IncrementScore());

        // Wait for the audio to finish before loading the next scene
        yield return new WaitForSeconds(MainAS.clip.length);

        if (Points >= 9800 && DataManager.GetValue<bool>("HasWatchedGF", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 9100 && DataManager.GetValue<bool>("HasWatchedTCFinal", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 8400 && DataManager.GetValue<bool>("HasWatchedTC5", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 7700 && DataManager.GetValue<bool>("HasWatchedSamuraiFinal", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 7000 && DataManager.GetValue<bool>("HasWatchedTC4", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 6300 && DataManager.GetValue<bool>("HasWatchedSamurai4", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 5600 && DataManager.GetValue<bool>("HasWatchedTC3", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 4900 && DataManager.GetValue<bool>("HasWatchedSamurai3", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 4200 && DataManager.GetValue<bool>("HasWatchedTC2", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 3500 && DataManager.GetValue<bool>("HasWatchedSamurai2", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 2800 && DataManager.GetValue<bool>("HasWatchedTC1", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 2100 && DataManager.GetValue<bool>("HasWatchedSamurai1", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 1400 && DataManager.GetValue<bool>("HasWatchedTC0", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else if (Points >= 700 && DataManager.GetValue<bool>("HasWatchedSamurai0", "data:/") == false)
        {
            SceneManager.LoadScene("Intermission");
        }
        else
        {
            SceneManager.LoadScene("MainMenuLoader");
        }
        
    }

    // Coroutine to increment the score over the specified duration
    private IEnumerator IncrementScore()
    {
        float elapsedTime = 0f;

        while (currentScore < Points)
        {
            elapsedTime += Time.deltaTime;
            currentScore = (int)Mathf.Lerp(0, Points, elapsedTime / timeForIncrease);
            Score.text = Mathf.FloorToInt(currentScore).ToString();

            // Play the corresponding animation based on the score
            if (currentScore <= 2000 && currentScore != 0)
            {
                TextAnimator.Play("WinNightGreatJob");
            }
            else if (currentScore <= 4000)
            {
                TextAnimator.Play("WinNightFantastic");
            }
            else if (currentScore <= 6000)
            {
                TextAnimator.Play("WinNightAmazing");
            }
            else if (currentScore <= 8000)
            {
                TextAnimator.Play("WinNightStupendous");
            }
            else if (currentScore < 10000)
            {
                TextAnimator.Play("WinNightPerfect");
            }
            else if (currentScore >= 10000)
            {
                TextAnimator.Play("WinNightUnbeatable");
            }

            yield return null;  // Wait for the next frame
        }
    }

    // Coroutine to fade a CanvasGroup (e.g., Image)
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        cg.alpha = startAlpha;

        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cg.alpha = endAlpha;
    }

    // Overload method to fade Image's CanvasRenderer
    private IEnumerator FadeCanvasGroup(CanvasRenderer renderer, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        renderer.SetAlpha(startAlpha);

        while (elapsed < duration)
        {
            renderer.SetAlpha(Mathf.Lerp(startAlpha, endAlpha, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        renderer.SetAlpha(endAlpha);
    }
}
