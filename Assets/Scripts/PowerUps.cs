using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public Animator PowerUpsA;

    // Use this for initialization
    void Start()
    {
        int Frigids = DataManager.GetValue<int>("Frigid", "data:/");
        int Coins = DataManager.GetValue<int>("Coins", "data:/");
        int Batteries = DataManager.GetValue<int>("Battery", "data:/");
        int DDRepels = DataManager.GetValue<int>("DDRepel", "data:/");

        // Use KeyValuePair for compatibility
        List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Frigid", Frigids),
            new KeyValuePair<string, int>("Coins", Coins),
            new KeyValuePair<string, int>("Battery", Batteries),
            new KeyValuePair<string, int>("DDRepel", DDRepels)
        };

        // Filter items under 10
        List<KeyValuePair<string, int>> itemsUnder10 = items.FindAll(item => item.Value < 10);

        if (itemsUnder10.Count > 0)
        {
            int randomIndex = Random.Range(0, itemsUnder10.Count);
            var selectedItem = itemsUnder10[randomIndex];

            DataManager.SaveValue(selectedItem.Key, selectedItem.Value + 1, "data:/");

            // Play the appropriate animation
            switch (selectedItem.Key)
            {
                case "Frigid":
                    PowerUpsA.Play("PowerUp_Frigid");
                    break;
                case "Coins":
                    PowerUpsA.Play("PowerUp_Coins");
                    break;
                case "Battery":
                    PowerUpsA.Play("PowerUp_Battery");
                    break;
                case "DDRepel":
                    PowerUpsA.Play("PowerUp_DDRepel");
                    break;
            }
			StartCoroutine(Wait());
        }
        else
        {
            Debug.Log("All values are at or above 10. No changes made.");
        }
    }

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(2.5f);
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuLoader");
	}
}