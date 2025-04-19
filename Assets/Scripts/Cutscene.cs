using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour {

	public float CutsceneDuration;

	public AudioSource[] Audios;
	public float[] AudiosPlayAfter;
	public float[] AudiosStopAfter;
	public Text Subtitles;
	public string[] SubtitleTexts;
	public float[] ChangeSubtitlesAfter;

	void Start () {
		if (Audios.Length != 0)
		{
			AudioStopping();
			AudioPlaying();
		}
		if (Subtitles != null)
		{
			StartSubtitles();
		}
		StartCoroutine(EndCutscene());
	}

	IEnumerator EndCutscene()
	{
		int Points = DataManager.GetValue<int>("LastWonScore", "data:/");
		yield return new WaitForSeconds(CutsceneDuration);
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
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuLoader");
		}
	}

	void AudioPlaying()
	{
		for (int i = 0; i < Audios.Length; i++)
		{
			StartCoroutine(PlayAudioAfter(Audios[i], AudiosPlayAfter[i]));
		}
	}

	void AudioStopping()
	{
		for (int i = 0; i < AudiosStopAfter.Length; i++)
		{
			if (AudiosStopAfter[i] != -1)
			{
				StartCoroutine(StopAudioAfter(Audios[i], AudiosStopAfter[i]));
			}
		}
	}

	void StartSubtitles()
	{
		for (int i = 0; i < ChangeSubtitlesAfter.Length; i++)
		{
			StartCoroutine(ChangeSubtitlesAfterE(SubtitleTexts[i], ChangeSubtitlesAfter[i]));
		}
	}

	IEnumerator PlayAudioAfter(AudioSource a, float time)
	{
		yield return new WaitForSeconds(time);
		a.Play();
	}

	IEnumerator StopAudioAfter(AudioSource a, float time)
	{
		yield return new WaitForSeconds(time);
		a.Stop();
	}

	IEnumerator ChangeSubtitlesAfterE(string changeTo, float time)
	{
		yield return new WaitForSeconds(time);
		Subtitles.text = changeTo;
	}

	private IEnumerator FadeOutGroupsAndLoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		yield return null;
    }
}
