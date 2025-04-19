using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OMC : MonoBehaviour
{
    public Transform WeirdBear;
    public float[] WeirdBearYPositions;
    public float WeirdBearSpeed;
    private float Timer;
    [Header("Main Gameplay")]
    public GameObject GamePlay;
    public Image Player;
    public Rigidbody2D PlayerRB;
    public float PlayerSpeed;
    public float StepSoundDelay;
    public Sprite[] PlayerSprites;
    public AudioSource I;
    public AudioSource PlayerStep;
    public float AnimationDelay;
    bool gameActive = false;
    private Vector3 moveDirection;
    private int currentSpriteIndex = 0;
    bool canMove = false;
    [Header("Dialog")]
    public GameObject MainDialog;
    public GameObject[] Dialogs;
    public GameObject NextButton;
    int currentdialog = 0;
    bool nextActive = false;
    [Header("Wiggle Down Mechanic")]
    public Transform PlayerTransform;
    public float WiggleSpeed = 1f;
    public GameObject P2;

    void Awake()
    {
        Resources.UnloadUnusedAssets();
		System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
    }

    void Start()
    {
        StartCoroutine(OMCIE());
        StartCoroutine(PlayerMovement());
        StartCoroutine(PlayerAnimation());
        StartCoroutine(PlayerSound());
    }

    void Update()
    {
        Timer += Time.deltaTime;
        P2.transform.position = PlayerTransform.position;
    }

    //UnityEngine.N3DS.Debug.Crash("This is your hell.");

    public void StartDialog()
    {
        canMove = false;
        MainDialog.SetActive(true);
        StartCoroutine(DialogCoroutine());
    }

    IEnumerator DialogCoroutine()
    {
        nextActive = false;
        if (currentdialog == 0)
        {
            currentdialog = 1;
            Dialogs[0].SetActive(true);
            yield return new WaitForSeconds(2f);
            NextButton.SetActive(true);
            nextActive = false;
            while (!nextActive)
            {
                yield return null;
            }
            nextActive = false;
            NextButton.SetActive(false);
            Dialogs[0].SetActive(false);
            Dialogs[1].SetActive(true);
            yield return new WaitForSeconds(2f);
            NextButton.SetActive(true);
            nextActive = false;
            while (!nextActive)
            {
                yield return null;
            }
            nextActive = false;
            NextButton.SetActive(false);
            MainDialog.SetActive(false);
            canMove = true;
        }
        else
        {
            Dialogs[1].SetActive(false);
            Dialogs[2].SetActive(true);
            yield return new WaitForSeconds(2f);
            NextButton.SetActive(true);
            nextActive = false;
            while (!nextActive)
            {
                yield return null;
            }
            nextActive = false;
            NextButton.SetActive(false);
            MainDialog.SetActive(false);
            canMove = true;
        }
    }

    public void Next()
    {
        nextActive = true;
    }

    IEnumerator OMCIE()
    {
        int i = 0;
        while (Timer < 13f)
        {
            WeirdBear.localPosition = new Vector3(WeirdBear.localPosition.x, WeirdBearYPositions[i], 0f);
            i++;
            if (i >= WeirdBearYPositions.Length)
            {
                i = 0;
            }
            yield return new WaitForSeconds(WeirdBearSpeed);
        }
        I.Play();
        GamePlay.SetActive(true);
        WeirdBear.gameObject.SetActive(false);
        gameActive = true;
        canMove = true;
        StartCoroutine(WiggleMechanic());
    }

    IEnumerator PlayerMovement()
    {
        while (true)
        {
            if (gameActive && canMove)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                moveDirection = new Vector3(horizontal, vertical, 0).normalized;

                if (moveDirection != Vector3.zero)
                {
                    PlayerRB.MovePosition(PlayerRB.position + (Vector2)(moveDirection * PlayerSpeed * Time.deltaTime));
                }
            }

            yield return null;
        }
    }

    IEnumerator PlayerAnimation()
    {
        int lastDirection = 0;

        while (true)
        {
            if (gameActive)
            {
                if (moveDirection.x > 0)
                {
                    currentSpriteIndex = (currentSpriteIndex == 0) ? 1 : 0;
                    lastDirection = 0;
                }
                else if (moveDirection.x < 0)
                {
                    currentSpriteIndex = (currentSpriteIndex == 2) ? 3 : 2;
                    lastDirection = 1;
                }
                else if (moveDirection.y != 0)
                {
                    if (lastDirection == 0)
                    {
                        currentSpriteIndex = (currentSpriteIndex == 0) ? 1 : 0;
                    }
                    else
                    {
                        currentSpriteIndex = (currentSpriteIndex == 2) ? 3 : 2;
                    }
                }
                else
                {
                    if (lastDirection == 0)
                    {
                        currentSpriteIndex = (currentSpriteIndex == 0) ? 1 : 0;
                    }
                    else
                    {
                        currentSpriteIndex = (currentSpriteIndex == 2) ? 3 : 2;
                    }
                }

                Player.sprite = PlayerSprites[currentSpriteIndex];
            }

            yield return new WaitForSeconds(AnimationDelay);
        }
    }

    IEnumerator PlayerSound()
    {
        while (true)
        {
            if (gameActive && moveDirection != Vector3.zero && canMove)
            {
                PlayerStep.Play();
                yield return new WaitForSeconds(StepSoundDelay);
            }
            else
            {
                PlayerStep.Stop();
                yield return null;
            }
        }
    }

    IEnumerator WiggleMechanic()
    {
        string lastInput = "";
        int successfulWiggles = 0;
        Vector3 initialPosition = PlayerTransform.position;

        while (successfulWiggles < 50)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (lastInput != "Left")
                {
                    lastInput = "Left";
                    successfulWiggles++;
                    PlayerTransform.position -= new Vector3(0, WiggleSpeed, 0);
                }
                else
                {
                    successfulWiggles = 0;
                    canMove = true;
                    PlayerRB.simulated = true;
                    P2.SetActive(false);
                    Player.enabled = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (lastInput != "Right")
                {
                    lastInput = "Right";
                    successfulWiggles++;
                    PlayerTransform.position -= new Vector3(0, WiggleSpeed, 0);
                }
                else
                {
                    successfulWiggles = 0;
                    canMove = true;
                    PlayerRB.simulated = true;
                    P2.SetActive(false);
                    Player.enabled = true;
                }
            }

            if (successfulWiggles >= 10)
            {
                canMove = false;
                PlayerRB.simulated = false;
                P2.SetActive(true);
                Player.enabled = false;
            }

            yield return null;
        }

        Debug.Log("Game is crashing...");
        UnityEngine.N3DS.Debug.Crash("This is your hell.");
    }

    //
}
