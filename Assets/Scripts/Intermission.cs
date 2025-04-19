using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intermission : MonoBehaviour {
	public CanvasGroup[] Groups;
	private int Points;

	// Use this for initialization
	void Start () {
		Points = DataManager.GetValue<int>("LastWonScore", "data:/");
		/*if (Points >= 700 && DataManager.GetValue<bool>("HasWatchedSamurai0", "data:/") == false)
		{
			DataManager.SaveValue<bool>("HasWatchedSamurai0", true, "data:/");
			StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Samurai0"));
		}*/

        if (Points >= 700 && DataManager.GetValue<bool>("HasWatchedSamurai0", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedSamurai0", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Samurai0"));
        }
        else if (Points >= 1400 && DataManager.GetValue<bool>("HasWatchedTC0", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTC0", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Chica0"));
        }
        else if (Points >= 2100 && DataManager.GetValue<bool>("HasWatchedSamurai1", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedSamurai1", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Samurai1"));
        }
        else if (Points >= 2800 && DataManager.GetValue<bool>("HasWatchedTC1", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTC1", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Chica1"));
        }
        else if (Points >= 3500 && DataManager.GetValue<bool>("HasWatchedSamurai2", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedSamurai2", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Samurai2"));
        }
        else if (Points >= 4200 && DataManager.GetValue<bool>("HasWatchedTC2", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTC2", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Chica2"));
        }
        else if (Points >= 4900 && DataManager.GetValue<bool>("HasWatchedSamurai3", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedSamurai3", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Samurai3"));
        }
        else if (Points >= 5600 && DataManager.GetValue<bool>("HasWatchedTC3", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTC3", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Chica3"));
        }
        else if (Points >= 6300 && DataManager.GetValue<bool>("HasWatchedSamurai4", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedSamurai4", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Samurai4"));
        }
        else if (Points >= 7000 && DataManager.GetValue<bool>("HasWatchedTC4", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTC4", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Chica4"));
        }
        else if (Points >= 7700 && DataManager.GetValue<bool>("HasWatchedSamuraiFinal", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedSamuraiFinal", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_SamuraiFinal"));
        }
        else if (Points >= 8400 && DataManager.GetValue<bool>("HasWatchedTC5", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTC5", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_Chica5"));
        }
        else if (Points >= 9100 && DataManager.GetValue<bool>("HasWatchedTCFinal", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedTCFinal", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_ChicaFinal"));
        }
        else if (Points >= 9800 && DataManager.GetValue<bool>("HasWatchedGF", "data:/") == false)
        {
            DataManager.SaveValue<bool>("HasWatchedGF", true, "data:/");
            StartCoroutine(FadeOutGroupsAndLoadScene("Cutscene_GoldenFreddy"));
        }
	}

	private IEnumerator FadeOutGroupsAndLoadScene(string sceneName)
    {
		yield return new WaitForSeconds(2f);
        float duration = 2.0f; 
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1.0f - Mathf.Clamp01(elapsed / duration);

            foreach (CanvasGroup group in Groups)
            {
                group.alpha = alpha;
            }

            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // Load the scene after fade-out
    }
}
