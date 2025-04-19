using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public AudioSource MainAudioSource;
	public Text DeathTime;
	public Animator GameOverAnimator;
	public CanvasGroup Cassidy;
	[Header("Animatronic Voice Lines")]
	public string[] PuppetVoiceLineAddresses;
	public string[] MangleVoiceLineAddresses;
	public string[] FuntimeFoxyVoiceLineAddresses;
	public string[] FoxyVoiceLineAddresses;
	public string[] NMAdresses = new string[] {
		"VoiceLines/NM/0",
		"VoiceLines/NM/1",
		"VoiceLines/NM/2",
		"VoiceLines/NM/3",
		"VoiceLines/NM/4"
	};
	public string[] RCAdresses = new string[] {
		"VoiceLines/RC/0",
		"VoiceLines/RC/1",
		"VoiceLines/RC/2",
		"VoiceLines/RC/3",
		"VoiceLines/RC/4"
	};
	public string SpringtrapAddress = "VoiceLines/SPT/0";
	public string ENAddress = "VoiceLines/Ennard/0";
	public string[] NBBAdresses = new string[] {
		"VoiceLines/NBB/0",
		"VoiceLines/NBB/1",
		"VoiceLines/NBB/2",
		"VoiceLines/NBB/3",
		"VoiceLines/NBB/4"
	};
	public string[] BalloraAdresses = new string[] {
		"VoiceLines/Ballora/0",
		"VoiceLines/Ballora/1",
		"VoiceLines/Ballora/2",
		"VoiceLines/Ballora/3",
		"VoiceLines/Ballora/4"
	};
	public string[] TFAddresses = new string[] {
		"VoiceLines/TF/0",
		"VoiceLines/TF/1",
		"VoiceLines/TF/2",
		"VoiceLines/TF/3",
		"VoiceLines/TF/4"
	};
	public string RFRAddress = "RockstarFreddy/6";
	public string[] WCAddresses = new string[] {
		"VoiceLines/WC/0",
		"VoiceLines/WC/1",
		"VoiceLines/WC/2",
		"VoiceLines/WC/3",
		"VoiceLines/WC/4"
	};
	public string[] NFAddresses = new string[] {
		"VoiceLines/NF/0",
		"VoiceLines/NF/1",
		"VoiceLines/NF/2",
		"VoiceLines/NF/3",
		"VoiceLines/NF/4"
	};
	public string[] NightmareAddresses = new string[] {
		"VoiceLines/Nightmare/0",
		"VoiceLines/Nightmare/1",
		"VoiceLines/Nightmare/2",
		"VoiceLines/Nightmare/3",
		"VoiceLines/Nightmare/4"
	};
	public string[] JOCAddresses = new string[] {
		"VoiceLines/JOC/0",
		"VoiceLines/JOC/1",
		"VoiceLines/JOC/2",
		"VoiceLines/JOC/3",
		"VoiceLines/JOC/4"
	};
	public string[] NMangleAddresses = new string[] {
		"VoiceLines/NMangle/0",
		"VoiceLines/NMangle/1",
		"VoiceLines/NMangle/2"
	};
	public string[] CBAddresses = new string[] {
		"VoiceLines/CB/0",
		"VoiceLines/CB/1",
		"VoiceLines/CB/2",
		"VoiceLines/CB/3"
	};
	public string[] HFAddresses = new string[] {
		"VoiceLines/HappyFrog/0",
		"VoiceLines/HappyFrog/1",
		"VoiceLines/HappyFrog/2",
		"VoiceLines/HappyFrog/3",
		"VoiceLines/HappyFrog/4"
	};
	public string[] PPAddresses = new string[] {
		"VoiceLines/Pigpatch/0",
		"VoiceLines/Pigpatch/1",
		"VoiceLines/Pigpatch/2",
		"VoiceLines/Pigpatch/3",
		"VoiceLines/Pigpatch/4"
	};
	public string[] NeddBearAddresses = new string[] {
		"VoiceLines/NeddBear/0",
		"VoiceLines/NeddBear/1",
		"VoiceLines/NeddBear/2",
		"VoiceLines/NeddBear/3",
		"VoiceLines/NeddBear/4"
	};
	public string[] OrvilleAddresses = new string[] {
		"VoiceLines/Orville/0",
		"VoiceLines/Orville/1",
		"VoiceLines/Orville/2",
		"VoiceLines/Orville/3",
		"VoiceLines/Orville/4"
	};
	public string[] MrHippoAddresses = new string[] {
		"VoiceLines/MrHippo/0",
		"VoiceLines/MrHippo/1",
		"VoiceLines/MrHippo/2",
		"VoiceLines/MrHippo/3"
	};
	public Image MrHippoBG;
	public string[] MrHippoBGAddresses;
	public Image MrHippoDiscovery;
	public string[] MrHippoDiscoveryAddresses;
	public AudioSource RFRSource;
	public string[] MMAddresses = new string[] {
		"VoiceLines/MM/0",
		"VoiceLines/MM/1",
		"VoiceLines/MM/2",
		"VoiceLines/MM/3",
		"VoiceLines/MM/4"
	};
	public string[] TCAddresses = new string[] {
		"VoiceLines/TC/0",
		"VoiceLines/TC/1",
		"VoiceLines/TC/2",
		"VoiceLines/TC/3",
		"VoiceLines/TC/4"
	};
	public string[] LeftyAddresses = new string[] {
		"VoiceLines/Lefty/0",
		"VoiceLines/Lefty/1",
		"VoiceLines/Lefty/2",
		"VoiceLines/Lefty/3",
		"VoiceLines/Lefty/4"
	};
	public string[] ScrapAddresses = new string[] {
		"VoiceLines/Scrap/0",
		"VoiceLines/Scrap/1",
		"VoiceLines/Scrap/2",
		"VoiceLines/Scrap/3"
	};
	public string[] WBAddresses = new string[] {
		"VoiceLines/WB/0",
		"VoiceLines/WB/1",
		"VoiceLines/WB/2",
		"VoiceLines/WB/3",
		"VoiceLines/WB/4"
	};
	public string[] NightmareFreddyAddresses = new string[] {
		"VoiceLines/NightmareFreddy/0",
		"VoiceLines/NightmareFreddy/1",
		"VoiceLines/NightmareFreddy/2",
		"VoiceLines/NightmareFreddy/3",
		"VoiceLines/NightmareFreddy/4"
	};
	public string[] RBonnieAddresses = new string[] {
		"VoiceLines/RBonnie/0",
		"VoiceLines/RBonnie/1",
		"VoiceLines/RBonnie/2",
		"VoiceLines/RBonnie/3",
		"VoiceLines/RBonnie/4"
	};
	public string[] FredbearAddresses;
	public string[] BonnetAddresses = new string[] {
		"VoiceLines/Bonnet/0",
		"VoiceLines/Bonnet/1",
		"VoiceLines/Bonnet/2",
		"VoiceLines/Bonnet/3",
		"VoiceLines/Bonnet/4"
	};
	private bool done;
	private string lastCharacter;
	private float survivalTime;

	// Use this for initialization
	void Start () {
		lastCharacter = DataManager.GetValue<string>("LastDeath", "data:/");
        survivalTime = DataManager.GetValue<float>("LastSurvival", "data:/");
		float time = survivalTime;

		int minutes = Mathf.FloorToInt(time / 60f);
    	int seconds = Mathf.FloorToInt(time % 60f);
    	int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f / 10);

    	// Update the Timer UI with the formatted time
    	DeathTime.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
		if (230 + UnityEngine.Random.Range(0, 40) >= 255)
		{
			StartCoroutine(CassidyCoroutine());
		}
		int random = Random.Range(0, 5);
		switch (lastCharacter)
		{
			case "Puppet":
			MainAudioSource.clip = load_audioClip(PuppetVoiceLineAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Mangle":
			MainAudioSource.clip = load_audioClip(MangleVoiceLineAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Funtime Foxy":
			MainAudioSource.clip = load_audioClip(FuntimeFoxyVoiceLineAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Foxy":
			MainAudioSource.clip = load_audioClip(FoxyVoiceLineAddresses[random]);
			MainAudioSource.Play();
			break;
			case "NM":
			MainAudioSource.clip = load_audioClip(NMAdresses[random]);
			MainAudioSource.Play();
			break;
			case "Rockstar Chica":
			MainAudioSource.clip = load_audioClip(RCAdresses[random]);
			MainAudioSource.Play();
			break;
			case "Springtrap":
			MainAudioSource.clip = load_audioClip(SpringtrapAddress);
			MainAudioSource.Play();
			break;
			case "Ennard":
			MainAudioSource.clip = load_audioClip(ENAddress);
			MainAudioSource.Play();
			break;
			case "Afton":
			MainAudioSource.clip = load_audioClip("Afton/0");
			MainAudioSource.Play();
			break;
			case "NBB":
			MainAudioSource.clip = load_audioClip(NBBAdresses[random]);
			MainAudioSource.Play();
			break;
			case "Ballora":
			MainAudioSource.clip = load_audioClip(BalloraAdresses[random]);
			MainAudioSource.Play();
			break;
			case "RFR":
			RFRSource.clip = load_audioClip(RFRAddress);
			RFRSource.Play();
			break;
			case "TF":
			MainAudioSource.clip = load_audioClip(TFAddresses[random]);
			MainAudioSource.Play();
			break;
			case "WitheredChica":
			MainAudioSource.clip = load_audioClip(WCAddresses[random]);
			MainAudioSource.Play();
			break;
			case "NF":
			MainAudioSource.clip = load_audioClip(NFAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Nightmare":
			MainAudioSource.clip = load_audioClip(NightmareAddresses[random]);
			MainAudioSource.Play();
			break;
			case "JOC":
			MainAudioSource.clip = load_audioClip(JOCAddresses[random]);
			MainAudioSource.Play();
			break;
			case "NMangle":
			int nmanglerndm = Random.Range(0,3);
			MainAudioSource.clip = load_audioClip(NMangleAddresses[nmanglerndm]);
			MainAudioSource.Play();
			break;
			case "CB":
			int cbrndm = Random.Range(0,4);
			MainAudioSource.clip = load_audioClip(CBAddresses[cbrndm]);
			MainAudioSource.Play();
			break;
			case "HappyFrog":
			MainAudioSource.clip = load_audioClip(HFAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Pigpatch":
			MainAudioSource.clip = load_audioClip(PPAddresses[random]);
			MainAudioSource.Play();
			break;
			case "NeddBear":
			MainAudioSource.clip = load_audioClip(NeddBearAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Orville":
			MainAudioSource.clip = load_audioClip(OrvilleAddresses[random]);
			MainAudioSource.Play();
			break;
			case "MrHippo":
			MainAudioSource.clip = load_audioClip(MrHippoAddresses[Random.Range(0,MrHippoAddresses.Length)]);
			MainAudioSource.Play();
			DataManager.SaveValue<bool>("MustHippo", true, "data:/");
			StartCoroutine(MrHippoSequence(MainAudioSource.clip.length, 30f));
			StartCoroutine(MrHippoWait(MainAudioSource.clip.length));
			break;
			case "MM":
			MainAudioSource.clip = load_audioClip(MMAddresses[random]);
			MainAudioSource.Play();
			break;
			case "ToyChica":
			MainAudioSource.clip = load_audioClip(TCAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Lefty":
			MainAudioSource.clip = load_audioClip(LeftyAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Freddy":
			MainAudioSource.clip = load_audioClip("FF");
			MainAudioSource.Play();
			break;
			case "Scrapbaby":
			int sbrndm = Random.Range(0,4);
			MainAudioSource.clip = load_audioClip(ScrapAddresses[sbrndm]);
			MainAudioSource.Play();
			break;
			case "WBonnie":
			MainAudioSource.clip = load_audioClip(WBAddresses[random]);
			MainAudioSource.Play();
			break;
			case "NightmareFreddy":
			MainAudioSource.clip = load_audioClip(NightmareFreddyAddresses[random]);
			MainAudioSource.Play();
			break;
			case "RBonnie":
			MainAudioSource.clip = load_audioClip(RBonnieAddresses[random]);
			MainAudioSource.Play();
			break;
			case "Fredbear":
			MainAudioSource.clip = load_audioClip(FredbearAddresses[Random.Range(0,FredbearAddresses.Length)]);
			MainAudioSource.Play();
			break;
			case "Bonnet":
			MainAudioSource.clip = load_audioClip(BonnetAddresses[random]);
			MainAudioSource.Play();
			break;
		}
	}

	Sprite load_sprite(string target)
    {
        Sprite new_sprite = Resources.Load<Sprite>(target);
        return new_sprite;
    }

	private IEnumerator MrHippoSequence(float discoveryMoveDuration, float moveDuration = 30f)
    {
        // Step 1: Wait 5 seconds
        yield return new WaitForSeconds(5f);

        // Step 2: Blend in MrHippoBG over 10 seconds
		MrHippoBG.sprite = load_sprite(MrHippoBGAddresses[Random.Range(0, MrHippoBGAddresses.Length)]);
        StartCoroutine(FadeInImage(MrHippoBG, 10f));

        // Step 3: Wait for an additional 25 seconds (total 30 seconds)
        yield return new WaitForSeconds(25f);

        // Step 4: Blend in MrHippoDiscovery over 10 seconds
		MrHippoDiscovery.sprite = load_sprite(MrHippoDiscoveryAddresses[Random.Range(0, MrHippoDiscoveryAddresses.Length)]);
        StartCoroutine(FadeInImage(MrHippoDiscovery, 10f));

        // Step 5: Move MrHippoDiscovery across the screen
        Vector3 originalPosition = MrHippoDiscovery.transform.position;
        Vector3 targetPosition = new Vector3(-Screen.width, originalPosition.y, originalPosition.z); // Move to the negative x position
        float elapsedTime = 0f;

        while (elapsedTime < discoveryMoveDuration)
        {
            MrHippoDiscovery.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / discoveryMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MrHippoDiscovery.transform.position = targetPosition;
    }

    // Coroutine to fade in an image over a specified duration
    private IEnumerator FadeInImage(Image image, float duration)
    {
        Color startColor = image.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Set alpha to 1 (fully opaque)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            image.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = endColor; // Ensure the final color is fully opaque
    }

    IEnumerator MrHippoWait(float duration)
	{
		yield return new WaitForSeconds(duration);
		DataManager.SaveValue<bool>("MustHippo", false, "data:/");
		done = true;
		StartCoroutine(Quit());
	}

	IEnumerator CassidyCoroutine()
	{
		// Wait for 10 seconds, then start fading out
        float fadeDuration = 0.6f;
        float fadeSpeed = 1f / fadeDuration; // How much to fade per second
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime * fadeSpeed);
			Cassidy.alpha = alpha;
            yield return null; // Continue every frame
        }

        // Ensure alpha is set to 0 at the end
        Cassidy.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Return))
		{
			if (!done && lastCharacter != "MrHippo")
			{
				done = true;
				StartCoroutine(Quit());
			}
		}
	}

	AudioClip load_audioClip(string target)
    {
        AudioClip new_audioClip = Resources.Load<AudioClip>(target);
        return new_audioClip;
    }

	IEnumerator Quit()
	{
		GameOverAnimator.Play("GameOverDisappear");
		int Frigids = DataManager.GetValue<int>("Frigid", "data:/");
        int Coins = DataManager.GetValue<int>("Coins", "data:/");
        int Batteries = DataManager.GetValue<int>("Battery", "data:/");
        int DDRepels = DataManager.GetValue<int>("DDRepel", "data:/");
		int PUPOdds = DataManager.GetValue<int>("PowerUpOdds", "data:/");
		yield return new WaitForSeconds(0.683f);
		if ((Random.Range(0,11) <= (2+PUPOdds)) && (Frigids < 10 || Coins < 10 || Batteries < 10 || DDRepels < 10))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("PowerUp");
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuLoader");
		}
	}
}
