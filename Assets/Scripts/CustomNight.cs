using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class CustomNight : MonoBehaviour
{
    // Variables
    public int[] currentAI; // Array for AI levels

    // Animators for doors, vents, monitor, and fan
    public Animator LeftDoorAnimator, RightDoorAnimator, FrontVentAnimator, SideVentAnimator, MonitorAnimator, FanAnimator, MaskAnimator, CandyCadet;

    // GameObjects for UI systems and camera system exclusive UI
    public GameObject CamExclusiveUI, CameraSystemExclusive, VentSystemExclusive, DuctSystemExclusive, VentilationWarning, CameraExclusiveOverLap, PrizeCorner, Showtime, OutOfOrder;
    public Transform MainBG;
    public float MinX, MaxX;
    public float moveSpeed = 5f; // Speed at which MainBG moves
    public AudioSource DoorSFX;
    public AudioSource MaskOn;
    public AudioSource MaskOff;
    public AudioSource Fan;
    public AudioSource CamPutOn;
    public AudioSource SwitchCam;
    public AudioSource ACSound;
    public Sprite[] CamSprites;
    public Image CameraMainBG;
    public GameObject AudioOnly;
    public GameObject Cameras;
    public AudioSource[] CandyCadetVoiceLines;
    public GameObject Flashlight;
    public AudioSource FlashClick;

    // UI elements for snares and ventilation reset
    public Image Snare1, Snare2, Snare3, ResetVentilationButton;

    // Sprites for active/unactive snares and ventilation states
    public Sprite ActiveSnare, UnActiveSnare, VentilationFailing, NormalVentilation;

    // Canvas groups for fading effects
    public CanvasGroup[] Groups;

    // Ventilation system-related variables
    public Transform Off; // The off state parent for the ventilator
    public Transform GBM; // The GBM state parent for the ventilator
    public Transform PowerAC; // The Power AC state parent for the ventilator
    public Transform Heater; // The Heater state parent for the ventilator
    public Transform SilVentilation; // The Silent Ventilation state parent for the ventilator
    public Transform PowerGenerator; // The Power Generator state parent for the ventilator
    public Transform VentilatorChild; // The ventilator object to be re-parented
    public GameObject[] UsageBlocks;
    public GameObject[] SoundBlocks;
    public Text PercentText;
    private float PowerLessPer30Seconds;
    private int currentVentilator = 0; // Current ventilator state index
    public Text Timer;

    // Active snare index
    private int activeSnare = 0;

    // Coroutine reference for resetting ventilation
    private Coroutine ventilationCoroutine;
    private Coroutine ventilationFadeOutCoroutine;
    private bool camsActive;
    private int currentCam = 1;
    private bool leftDoorClosed;
    private bool rightDoorClosed;
    private bool frontVentClosed;
    private bool sideVentClosed;
    private bool maskActive;
    private bool FanOn = true;
    private int Usage = 1;
    private int Sound = 1;
    private float PercentLeft = 100;
    // Power consumption per second based on usage levels
    private float[] powerConsumptionRates = { 0f / 30f, 2.54f / 30f, 10f / 30f, 22.22f / 30f, 39.33f / 30f, 62f / 30f, 88.5f / 30f, 118.11f / 30f };
    private float TimerTime = 0f; // Variable to track the elapsed time
    public Text AMText;
    public Text DegreeText;
    public GameObject WindMusicBoxButton;
    public Image MusicBoxProgress;
    public AudioSource WindMusicBox;
    private bool windingMusicBox;
    private float mbProgress = 100f;
    private bool GBMActive;
    public AudioSource[] MusicBoxSounds;
    private int currentMusicBoxIndex;
    private int FazCoins;
    public Text FazCoinCounter;
    public GameObject SampleFazCoin;

    // Variables to track power drain
    private float powerDrainTimer = 0f;
    private float cameraMoveDirection = 1f; // 1 for right, -1 for left
    public float cameraMoveSpeed = 1.25f;
    public float FanDegreePerSecond;
    public float ACDegreePerSecond;
    public float HeaterDegreePerSecond;
    private float currentDegreePerSecond;
    private float currentDegrees;
    private bool flashlightActive;
    public AudioSource ErrorSound;
    private bool isLoadingWin;
    public AudioSource PowerOut;
    public GameObject LowerUI;
    public GameObject UpperUI;
    public GameObject DisableOnJumpscare;
    [Header("Office")]
    public Image MainOffice;
    public Animator Desk;
    public string[] MainOfficeSprites;
    public string[] DeskOfficeSprites;
    public string CandyCadetAddress;
    private Sprite OfficePowerOut;
    public Image SpringtrapFlash;
    public string SpringtrapFlashPath;
    [Header("Dee Dee")]
    public bool DeeDeeEnabled = true;
    public float DeeDeeChance = 0.1f;
    public float additionalXORChance = 0.01f;
    public int DeeDeeForceCharacter = -1;
    public string DeeDeeAddress;
    public string XORAddress;
    public string XORSoundAddress;
    public Animator DeeDeeAnimator;
    public Transform DeeDeeTransform;
    private bool is5020 = false;
    public string[] DeeDeeSoundAddress;
    public AudioSource DeeDeeSoundAS;
    public AudioSource DeeDeeSoundAS2;
    public Animator NewChallengerAnimator;
    public string NewChallengerAddress;
    public string NewChallengerAudioAddress;
    bool XORHappened = false;
    [Header("Secret Chars")]
    public string[] FredbearJumpscareAddresses;
    public string PlushtrapChairAddress;
    private int PlushtrapState = -1;
    private string PlushtrapJoinTime;
    public string[] PlushtrapJumpscareAddress;
    public GameObject PlushtrapContainer;
    public Animator PlushtrapAnimator;
    Coroutine MoveCor;
    [Header("Bonnet")]
    public Animator BonnetAnimator;
    public string BonnetAnimatorAddress;
    public string[] BonnetJumpscareAddresses;
    private bool bonnetActive = false;
    public string BonnetVoiceline;
    public string NoseBoopSoundAddress;
    public AudioSource bonnetSounds;
    [Header("LOLBit")]
    public Image LOLBit;
    public string LOLBitAddress;
    public AudioSource Beeping;
    public string BeepingAddress;
    private bool LOLBitActive;
    [Header("Nightmare Chica")]
    public RectTransform UpperNC;
    public RectTransform LowerNC;
    public string UpperJawAddress;
    public string LowerJawAddress;
    public string[] NightmareChicaJumpscareAddresses;
    [Header("Minireenas")]
    public Image Minireenas;
    public string MinireenasAddress;
    private bool ActivateMinireenasN = false;
    private float MinireenaTimer = 0f;
    [Header("RWQ")]
    public GameObject RWQCarrier;
    public Image RWQ;
    public string RWQAddress;
    private bool activateRWQNext;
    private float RWQTimer;
    [Header("Easter Eggs")]
    public GameObject Tangle;
    public GameObject Bouncepot;
    public GameObject WhiteRabbit;
    public GameObject DeskHat;
    [Header("Shop")]
    public Image DCoin;
    public Sprite DCoinClickSprite;
    public Sprite NormalDCoinSprite;
    public GameObject BuyDCoinB;
    public GameObject BuyNBPB;
    public GameObject BuyNMPB;
    public GameObject BuyCBPB;
    public AudioSource BuySFX;
    private bool DCoinActive;
    [Header("Animatronics")]
    private bool PlushieCoroutineActive = false;
    public AudioSource Block;
    public Image JumpscareImage;
    public string[] JumpscareSoundAdresses;
    public AudioSource JumpscareAS;
    public int JumpscareFPS;
    private Sprite[] jumpscareSprites;
    private bool isJumpscaring;
    public Transform[] ventPath1;
    public Transform[] ventPath2;
    public Transform[] ventPath3;
    public float PixelSteps;
    public Transform WetFloorSign;
    public Vector3 WetFloorSignPos0;
    public Vector3 WetFloorSignPos1;
    public Sprite LeftSideSprite;
    public Sprite RightSideSprite;
    public string[] OldManConsequencesAdresses;
    public Image[] OldManConsequencesImages;
    public AudioSource[] OldManConsequencesSounds;
    private bool MonitorBlocked;
    private bool puppetPrepared;
    public string[] PuppetJumpscareAddresses;
    public Image FuntimeChicaDistraction;
    public Image FuntimeChicaDistractionC;
    public string[] FuntimeChicaAddresses;
    public AudioSource FuntimeChicaAS;
    public AudioSource FuntimeChicaFlash;
    public Transform MangleIcon;
    public Image MangleInOffice;
    public AudioSource MangleInOfficeAS;
    public string[] MangleAddresses;
    public string[] MangleJumpscareAddresses;
    private bool mangleisOffice;
    private bool ElChipAdActive;
    public Image ElChipAdUpper;
    public AudioSource ElChipAdSound;
    public Image ElChipAdLower;
    public Image ElChipSkip;
    public string[] ElChipAddresses;
    private int currentShowTime;
    public string FunTimeFoxyCurtainAddress;
    public string[] FuntimeFoxyJumpscareAddresses;
    public Text AtWhatTimeIsShowtime;
    public Image FoxyLegs, FoxyTorso, FoxyHook, FoxyArm2, FoxyHead;
    public string[] FoxyAddresses;
    public string[] FoxyJumpscareAddresses;
    public Image CurrentFigurine;
    private int activeFigurine = 0;
    private int FoxyStage = 0;
    private int FoxyH = 0;
    public Image GoldenFreddy;
    public string GoldenFreddyAddress;
    public string[] GoldenFreddyJumpscareAddress;
    bool goldenFreddyInOffice;
    private int gfI;
    private bool gfP;
    public Image Nightmarionette;
    public string[] NightmarionetteAddresses;
    public string[] NightmarionetteJumpscareAddresses;
    public int NMLeftIndex, NMRightIndex, NMMiddleLeftIndex, NMMiddleRightIndex;
    public Transform NMLeft, NMRight, NMMiddleLeft, NMMiddleRight;
    public float[] RangeOfNightmarionne;
    private bool CallActive;
    public Image MuteCall;
    public Rigidbody2D MuteButtonRB;
    public string MuteButtonAddress;
    public string[] PhoneCallAddresses;
    public AudioSource PhoneCall;
    public Vector2 PositiveVelocity;
    public Vector2 NegativeVelocity;
    public string[] RockstarChicaAddresses;
    public Image RockstarChicaLeft;
    public Image RockstarChicaRight;
    public string[] RockstarChicaJumpscareAddresses;
    private bool RockstarChicaInHalls;
    private int RockstarChicaCam;
    private int WetFloorSignSide = 0;
    public Transform SpringtrapIcon;
    public string[] SpringtrapAddresses;
    public Image SpringtrapInVent;
    public string[] SpringtrapJumpscareAddresses;
    private bool SpringtrapInOffice;
    private bool MoltenFreddyInOffice;
    public AudioSource MoltenFreddyLaugh;
    public string[] MoltenFreddyAddresses;
    public string[] MoltenFreddyJumpscareAddresses;
    public Transform MoltenFreddyIcon;
    private bool EnnardInOffice;
    public AudioSource EnnardSqueack;
    public string[] EnnardAddresses;
    public string[] EnnardJumpscareAddresses;
    public Transform EnnardIcon;
    public Image MrCanDo;
    public Image CrateSmall;
    public AudioSource TatGJumpscare;
    public Image CrateJumpscare;
    public string[] TatGAddresses;
    public string[] TatGJumpscareAddresses;
    public string[] TatGWhispers;
    private int currentMrCanDoCam = 0;
    private bool TatGJumpscarecoroutineRunning;
    public AudioSource Helpy;
    public Image HelpyLowerScreen;
    public Image HelpyJumpscare;
    public string[] HelpyAddresses;
    private bool HelpyInOffice;
    public Image PhantomBBJumpscare;
    public Image PhantomBBOnCams;
    public string[] PhantomBBAddresses;
    private bool PhantomBBInOffice;
    private bool canPhantomBB = true;
    public Image PhantomMangleInOfficeImg;
    public Image PhantomMangleOnCamsImg;
    public string[] PhantomMangleAddresses;
    public AudioSource PhantomMangleScream;
    private bool PhantomMangleInOffice;
    private bool PhantomMangleOnCams;
    private bool canPhantomMangle = true;
    public AudioSource Breathing;
    public string BreathingAddress;
    public Image PhantomFreddyInOffice;
    public Image PhantomFreddyJumpscare;
    public string[] PhantomFreddyAddresses;
    private int PhantomFreddyFade = 255;
    public string AftonAttackAddress;
    public AudioSource AftonAttack;
    public string[] AftonJumpscareAddresses;
    public Image OfficeLights;
    public Image NightmareBBInActive;
    public Image NightmareBBActive;
    public string[] NightmareBBAddresses;
    public string[] NightmareBBJumpscareAddresses;
    private bool NightmareBBStandingUp;
    private bool NightmareBBInnocentJumpscare = false;
    public AudioSource BalloraMusic;
    public string BalloraAddress;
    public string[] BalloraJumpscareAddresses;
    public Image RockstarFreddy;
    public GameObject Deposit5Coins;
    public AudioSource RockstarFreddyAS;
    public string[] RockstarFreddyAddresses;
    public string[] RockstarFreddyJumpscareAddresses;
    private bool isAsking = false;
    private int TimesAsked = 0;
    private Coroutine RFRCoroutine;
    public GameObject Cam08Parent;
    public string[] ToyFreddyAddresses;
    private int MrHugsCamera;
    private int currentClosedDoor;
    private int currentToyFreddyCam = 1;
    public string[] ToyFreddyJumpscareAddresses;
    public Image ToyFreddyCloseDoor;
    public Sprite ToyFreddyCloseDoorNormal;
    public Sprite ToyFreddyCloseDoorClosed;
    private bool WitheredChicaStuck;
    public Transform WitheredChicaIcon;
    public Image WitheredChicaInOffice;
    public string[] WitheredChicaAddresses;
    public string[] WitheredChicaJumpscareAddresses;
    public AudioSource FredLaugh;
    public string[] NFAddresses;
    public string[] NightmareJumpscareAddresses;
    public string[] NightmareFredbearJumpscareAddresses;
    public Image FredbearEyes;
    public Image NightmareEyes;
    public Image JackOChicaLeft;
    public Image JackOChicaRight;
    private float JackOChicaManifest;
    public string JackOChicaAddress;
    public string[] JackOChicaJumpscareAddresses;
    private bool NightmareMangleBought;
    private bool NightmareMangleActive;
    private bool NightmareBonnieBought;
    private bool NightmareBonnieActive;
    private bool BabyBought;
    private bool BabyActive;
    public GameObject BabyBuy, NightmareBonnieBuy, NightmareMangleBuy;
    public Text BabyBuyText, NightmareBonnieBuyText, NightmareMangleBuyText;
    public string[] PlushieAddresses;
    public string[] NightmareBonnieJumpscareAddresses;
    public string[] NightmareMangleJumpscareAddresses;
    public string[] BabyJumpscareAddresses;
    public Transform DuctLure;
    public Transform DuctClosed;
    public Transform DuctOpen;
    private string currentClosedDuct = "RightDuct";
    public DuctCharacter HappyFrogDC;
    public DuctCharacter MrHippoDC;
    public DuctCharacter PigpatchDC;
    public DuctCharacter NeddBearDC;
    public DuctCharacter OrvilleDC;
    private int MusicManAgitation = 0;
    public AudioSource MusicManCymbals;
    public string MusicManAddress;
    public string[] MusicManJumpscareAddress;
    public string[] BonnieAddresses;
    public AudioSource BonnieJumpscare;
    public GameObject BonnieStatic;
    public Transform ToyBonnieChicaMoveTowards;
    public Transform ToyBonnie;
    public Transform ToyBonnieReturnPos;
    public string ToyBonnieAnimatorControllerAddress;
    public Animator ToyBonnieAnimator;
    public string[] ToyBonnieJumpscareAddresses;
    private bool ToyBonnieAttacking;
    public Transform ToyChica;
    public Transform ToyChicaReturnPos;
    public string ToyChicaAnimatorControllerAddress;
    public Animator ToyChicaAnimator;
    public string[] ToyChicaJumpscareAddresses;
    private bool ToyChicaAttacking;
    public AudioSource StareAS;
    public string StareAddress;
    public string[] LeftyAddresses;
    public string[] LeftyJumpscareAddresses;
    private int LeftyAgitation;
    public Image BBInOffice;
    public Image BBInVent;
    public string[] BBAddresses;
    public AudioSource BBLaugh;
    private bool BBActive;
    private bool BBVent;
    public Image JJInOffice;
    public Image JJInVent;
    public string[] JJAddresses;
    private bool JJActive;
    private bool JJVent;
    public AudioSource ChicaPotsAndPans;
    public string PotsAndPantsAddress;
    private bool ChicaHappy;
    private int ChicaMissedMusic;
    private bool ChicaAngry;
    public string[] ChicaJumpscareAddresses;
    public Image FreddyStage1;
    public Image FreddyStage2;
    public Image FreddyStage3;
    public Image FreddyStage4;
    public string[] FreddyAddresses;
    private float FreddyProgress;
    public string[] FreddyJumpscareAddresses;
    public Image ScrapbabyInOffice;
    public Image ShockPanel;
    public AudioSource Shock;
    public string[] ScrapbabyAddresses;
    private bool ScrapBabyStandingUp;
    private bool ScrapBabyIsInOffice;
    public string[] ScrapbabyJumpscareAddresses;
    public Image ToiletBowlBonnieInOffice;
    public string ToiletBowlBonnieAddress;
    private bool ToiletBowlBonniePrepared;
    private bool ToiletBowlBonnieIsInOffice;
    public string[] ToiletBowlBonnieJumpscareAddresses;
    public GameObject[] Freddles;
    public Transform[] FreddlePositionsOffice0;
    public Transform[] FreddlePositionsOffice1;
    public Transform[] FreddlePositionsOffice2;
    public Transform[] FreddlePositionsOffice3;
    public Transform FreddleSmoke;
    public AudioSource FreddlesSounds;
    public string[] NightmareFreddyAddresses;
    public string[] NightmareFreddyJumpscareAddresses;
    public Image RockstarBonnie;
    public Image RockstarBonnieGuitar;
    public string[] RockstarBonnieAddresses;
    private bool RBonnieInOffice;
    private int RBonnieCamera;
    public string[] RBonnieJumpscareAddresses;
    public GameObject RFoxyPrizes;
    public GameObject RFoxyObjects;
    public Transform RFoxyBird;
    public Transform RFoxyBirdGoTo;
    public Animator RockstarFoxyMain;
    public string[] RockstarFoxyJumpscareAddresses;
    private bool RFoxyInOffice;
    private bool RFoxyBirdActive;
    private Vector3 RFoxyBirdInitalPos;
    public AudioSource[] RandomBirdSounds;
    public AudioSource[] RFoxyHelpful;
    public AudioSource[] RFoxyUnHelpful;
    private bool isSoundProof;
    private bool trueCamsActive = false;
    int office = -1;
    private float lastValidDegrees = 60f;
    private float startTimer = 0f;

    void LoadOffice()
    {
        switch (DataManager.GetValue<int>("Office", "data:/"))
        {
            case 0:
            MainOffice.sprite = load_sprite(MainOfficeSprites[0]);
            Desk.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(DeskOfficeSprites[0]);
            CandyCadet.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(CandyCadetAddress);
            CandyCadet.gameObject.SetActive(true);
            OfficePowerOut = load_sprite(MainOfficeSprites[2]);
            office = 0;
            StartCoroutine(UCNOffice());
            break;
            case 1:
            MainOffice.sprite = load_sprite(MainOfficeSprites[3]);
            OfficePowerOut = load_sprite(MainOfficeSprites[3]);
            Desk.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(DeskOfficeSprites[1]);
            office = 1;
            break;
            case 2:
            MainOffice.sprite = load_sprite(MainOfficeSprites[4]);
            load_sprite(MainOfficeSprites[5]);
            OfficePowerOut = load_sprite(MainOfficeSprites[4]);
            Desk.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(DeskOfficeSprites[2]);
            office = 2;
            StartCoroutine(FNaF3Office());
            break;
            case 3:
            MainOffice.sprite = load_sprite(MainOfficeSprites[6]);
            OfficePowerOut = load_sprite(MainOfficeSprites[6]);
            Desk.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(DeskOfficeSprites[3]);
            office = 3;
            break;
        }
    }

    void LoadPowerUps()
    {
        bool frigid = DataManager.GetValue<bool>("UseFrigid", "data:/");
        bool coins = DataManager.GetValue<bool>("UseCoins", "data:/");
        bool battery = DataManager.GetValue<bool>("UseBattery", "data:/");
        bool ddRepel = DataManager.GetValue<bool>("UseDDRepel", "data:/");
        int Frigids = DataManager.GetValue<int>("Frigid", "data:/");
        int Coins = DataManager.GetValue<int>("Coins", "data:/");
        int Batteries = DataManager.GetValue<int>("Battery", "data:/");
        int DDRepels = DataManager.GetValue<int>("DDRepel", "data:/");
        if (battery == true)
        {
            PercentLeft = 102f;
            DataManager.SaveValue<int>("Battery", Batteries-1, "data:/");
            DataManager.SaveValue<bool>("UseBattery", false, "data:/");
            Debug.Log("Using A Battery");
        }
        if (ddRepel == true)
        {
            DeeDeeEnabled = false;
            DataManager.SaveValue<int>("DDRepel", DDRepels-1, "data:/");
            DataManager.SaveValue<bool>("UseDDRepel", false, "data:/");
            Debug.Log("Using A Diddy Repel");
        }
        if (coins == true)
        {
            FazCoins += 3;
            FazCoinCounter.text = FazCoins.ToString();
            DataManager.SaveValue<int>("Coins", Coins-1, "data:/");
            DataManager.SaveValue<bool>("UseCoins", false, "data:/");
            Debug.Log("Using 3 Coins");
        }
        if (frigid == true)
        {
            lastValidDegrees = 50f;
            currentDegrees = 50f;
            DegreeText.text = "50°";
            DataManager.SaveValue<int>("Frigid", Frigids-1, "data:/");
            DataManager.SaveValue<bool>("UseFrigid", false, "data:/");
            Debug.Log("Using Frigid");
        }
    }

    IEnumerator UCNOffice()
    {
        while (PercentLeft > 0f)
        {
            MainOffice.sprite = load_sprite(MainOfficeSprites[0]);
            yield return new WaitForSeconds(Random.Range(0.04f, 1f));
            MainOffice.sprite = load_sprite(MainOfficeSprites[1]);
            yield return new WaitForSeconds(Random.Range(0.06f, 0.19f));
        }
    }

    IEnumerator FNaF3Office()
    {
        while (PercentLeft > 0f)
        {
            MainOffice.sprite = load_sprite(MainOfficeSprites[4]);
            yield return new WaitForSeconds(Random.Range(0.04f, 0.2f));
            MainOffice.sprite = load_sprite(MainOfficeSprites[5]);
            yield return new WaitForSeconds(Random.Range(0.06f, 0.19f));
        }
    }

    void Start()
    {
        Resources.UnloadUnusedAssets();
		System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
        DataManager.SaveValue<int>("LastWonScore", DataManager.GetValue<int>("currentPoints", "data:/"), "data:/");
        ventilationCoroutine = StartCoroutine(VentilationCycle());
        LoadAILevels();
        LoadOffice();
        LoadPowerUps();
        StartCoroutine(PowerCoroutine());
        if (office == 0)
        {
            StartCoroutine(CandyCadetCoroutine());
        }
        currentDegreePerSecond = FanDegreePerSecond + 1;
        int AIOver50Count = 0;
        for (int i = 0; i < currentAI.Length; i++)
        {
            if (currentAI[i] >= 20 && i <= 49)
            {
                AIOver50Count++;
            }
        }
        if (AIOver50Count == 50)
        {
            is5020 = true;
            DeeDeeChance = 0.2f;
        }
        if (DataManager.GetValue<bool>("MustHippo", "data:/") == true)
        {
            MrHippoDC.PrependJumpscarePath();
            StartJumpscare(MrHippoDC.JumpscareAddresses, 5, "MrHippo");
        }
        if (currentAI[26] >= 1)
        {
            OldManConsequencesImages[0].sprite = load_sprite(OldManConsequencesAdresses[0]);
            OldManConsequencesImages[1].sprite = load_sprite(OldManConsequencesAdresses[1]);
            OldManConsequencesImages[2].sprite = load_sprite(OldManConsequencesAdresses[2]);
            OldManConsequencesSounds[0].clip = load_audioClip(OldManConsequencesAdresses[3]);
            OldManConsequencesSounds[1].clip = load_audioClip(OldManConsequencesAdresses[4]);
            OldManConsequencesSounds[2].clip = load_audioClip(OldManConsequencesAdresses[5]);
            StartCoroutine(OldManConsequencesLoop());
        }
        if (currentAI[44] >= 1)
        {
            FuntimeChicaDistraction.sprite = load_sprite(FuntimeChicaAddresses[6]);
            FuntimeChicaDistractionC.sprite = load_sprite(FuntimeChicaAddresses[7]);
            FuntimeChicaFlash.clip = load_audioClip(FuntimeChicaAddresses[0]);
            FuntimeChicaAS.clip = load_audioClip(FuntimeChicaAddresses[1]);
            StartCoroutine(FuntimeChicaLoop());
        }
        if (currentAI[43] >= 1)
        {
            ElChipAdSound.clip = load_audioClip(ElChipAddresses[4]);
            ElChipSkip.sprite = load_sprite(ElChipAddresses[3]);
            StartCoroutine(ElChipLoop());
        }
        if (currentAI[7] >= 1)
        {
            MangleInOfficeAS.clip = load_audioClip(MangleAddresses[2]);
            MangleInOffice.sprite = load_sprite(MangleAddresses[1]);
            MangleIcon.GetComponent<Image>().sprite = load_sprite(MangleAddresses[0]);
            MangleIcon.gameObject.SetActive(true);
            StartCoroutine(MangleLoop());
            MangleIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[29] >= 1)
        {
            StartCoroutine(FuntimeFoxyLoop());
        }
        if (currentAI[3] >= 1)
        {
            CurrentFigurine.sprite = load_sprite(FoxyAddresses[0]);
            CurrentFigurine.gameObject.SetActive(true);
            FoxyLegs.sprite = load_sprite(FoxyAddresses[1]);
            FoxyTorso.sprite = load_sprite(FoxyAddresses[2]);
            FoxyArm2.sprite = load_sprite(FoxyAddresses[4]);
            FoxyHook.sprite = load_sprite(FoxyAddresses[3]);
            FoxyHead.sprite = load_sprite(FoxyAddresses[5]);
            StartCoroutine(FoxyLoop());
            activeFigurine = 0;
        }
        if (currentAI[1] >= 1)
        {
            CurrentFigurine.sprite = load_sprite(BonnieAddresses[0]);
            CurrentFigurine.gameObject.SetActive(true);
            activeFigurine = 1;
            BonnieJumpscare.clip = load_audioClip(BonnieAddresses[3]);
        }
        if (currentAI[1] >= 1 && currentAI[3] >= 1)
        {
            int DiceRoll = Random.Range(0,2);
            activeFigurine = DiceRoll;
            if (activeFigurine == 0)
            {
                CurrentFigurine.sprite = load_sprite(FoxyAddresses[0]);
                CurrentFigurine.gameObject.SetActive(true);
            }
            else
            {
                CurrentFigurine.sprite = load_sprite(BonnieAddresses[0]);
                CurrentFigurine.gameObject.SetActive(true);
            }
            StartCoroutine(FigurineLoop());
        }
        if (currentAI[13] >= 1)
        {
            GoldenFreddy.sprite = load_sprite(GoldenFreddyAddress);
            StartCoroutine(GoldenFreddyInOffice());
        }
        if (currentAI[24] >= 1)
        {
            StartCoroutine(NightmarionneLoop());
        }
        if (currentAI[49] >= 1)
        {
            MuteCall.sprite = load_sprite(MuteButtonAddress);
            StartCoroutine(PhoneGuyLoop());
        }
        if (currentAI[40] >= 1)
        {
            RockstarChicaLeft.sprite = load_sprite(RockstarChicaAddresses[0]);
            RockstarChicaRight.sprite = load_sprite(RockstarChicaAddresses[1]);
            StartCoroutine(RockstarChicaLoop());
        }
        if (currentAI[14] >= 1)
        {
            SpringtrapInVent.sprite = load_sprite(SpringtrapAddresses[1]);
            SpringtrapIcon.GetComponent<Image>().sprite = load_sprite(SpringtrapAddresses[0]);
            SpringtrapIcon.gameObject.SetActive(true);
            StartCoroutine(SpringtrapLoop());
            SpringtrapIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[45] >= 1)
        {
            MoltenFreddyIcon.GetComponent<Image>().sprite = load_sprite(MoltenFreddyAddresses[0]);
            MoltenFreddyIcon.gameObject.SetActive(true);
            StartCoroutine(MoltenFreddyLoop());
            MoltenFreddyLaugh.clip = load_audioClip(MoltenFreddyAddresses[1]);
            MoltenFreddyIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[30] >= 1)
        {
            EnnardIcon.GetComponent<Image>().sprite = load_sprite(EnnardAddresses[0]);
            EnnardIcon.gameObject.SetActive(true);
            StartCoroutine(EnnardLoop());
            EnnardSqueack.clip = load_audioClip(EnnardAddresses[1]);
            EnnardIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[31] >= 1)
        {
            CrateSmall.sprite = load_sprite(TatGAddresses[0]);
            TatGJumpscare.clip = load_audioClip(TatGAddresses[2]);
            MrCanDo.sprite = load_sprite(TatGAddresses[1]);
        }
        if (currentAI[32] >= 1)
        {
            HelpyLowerScreen.sprite = load_sprite(HelpyAddresses[0]);
            HelpyJumpscare.sprite = load_sprite(HelpyAddresses[2]);
            Helpy.clip = load_audioClip(HelpyAddresses[4]);
        }
        if (currentAI[17] >= 1)
        {
            Breathing.clip = load_audioClip(BreathingAddress);
            PhantomBBOnCams.sprite = load_sprite(PhantomBBAddresses[0]);
            PhantomBBJumpscare.sprite = load_sprite(PhantomBBAddresses[1]);
        }
        if (currentAI[15] >= 1)
        {
            PhantomMangleScream.clip = load_audioClip(PhantomMangleAddresses[2]);
            PhantomMangleOnCamsImg.sprite = load_sprite(PhantomMangleAddresses[0]);
            PhantomMangleInOfficeImg.sprite = load_sprite(PhantomMangleAddresses[1]);
        }
        if (currentAI[16] >= 1)
        {
            Breathing.clip = load_audioClip(BreathingAddress);
            PhantomFreddyInOffice.sprite = load_sprite(PhantomFreddyAddresses[0]);
            PhantomFreddyJumpscare.sprite = load_sprite(PhantomFreddyAddresses[1]);
            StartCoroutine(PhantomFreddyLoop());
        }
        if (currentAI[47] >= 1)
        {
            AftonAttack.clip = load_audioClip(AftonAttackAddress);
            StartCoroutine(AftonLoop());
        }
        if (currentAI[25] >= 1)
        {
            NightmareBBInActive.sprite = load_sprite(NightmareBBAddresses[0]);
            NightmareBBActive.sprite = load_sprite(NightmareBBAddresses[1]);
            NightmareBBInActive.gameObject.SetActive(true);
            StartCoroutine(NightmareBBLoop());
        }
        if (currentAI[28] >= 1)
        {
            BalloraMusic.clip = load_audioClip(BalloraAddress);
            BalloraMusic.mute = true;
            BalloraMusic.Play();
            StartCoroutine(BalloraLoop());
        }
        if (currentAI[38] >= 1)
        {
            Deposit5Coins.GetComponent<Image>().sprite = load_sprite(RockstarFreddyAddresses[9]);
            RockstarFreddy.sprite = load_sprite(RockstarFreddyAddresses[0]);
            RockstarFreddy.gameObject.SetActive(true);
            RFRCoroutine = StartCoroutine(RockstarFreddyLoop());
            StartCoroutine(UseHeaterToRFR());
        }
        if (currentAI[4] >= 1)
        {
            StartCoroutine(ToyFreddyLoop());
        }
        if (currentAI[10] >= 1)
        {
            WitheredChicaIcon.GetComponent<Image>().sprite = load_sprite(WitheredChicaAddresses[0]);
            WitheredChicaIcon.GetComponent<Image>().preserveAspect = true;
            WitheredChicaInOffice.sprite = load_sprite(WitheredChicaAddresses[1]);
            WitheredChicaIcon.gameObject.SetActive(true);
            StartCoroutine(WitheredChicaLoop());
        }
        if (currentAI[20] >= 1)
        {
            FredbearEyes.sprite = load_sprite(NFAddresses[1]);
            StartCoroutine(NightmareFredbearLoop());
        }
        if (currentAI[21] >= 1)
        {
            NightmareEyes.sprite = load_sprite(NFAddresses[0]);
            StartCoroutine(NightmareLoop());
        }
        if (currentAI[22] >= 1)
        {
            JackOChicaLeft.sprite = load_sprite(JackOChicaAddress);
            JackOChicaRight.sprite = load_sprite(JackOChicaAddress);
            StartCoroutine(JackOChicaLoop());
        }
        if (currentAI[23] >= 1 || currentAI[27] >= 1 || currentAI[19] >= 1)
        {
            if (currentAI[23] >= 1)
            {
                NightmareMangleBuy.SetActive(true);
            }
            if (currentAI[27] >= 1)
            {
                BabyBuy.SetActive(true);
            }
            if (currentAI[19] >= 1)
            {
                NightmareBonnieBuy.SetActive(true);
            }

            StartCoroutine(PlushieCoroutine());
            PlushieCoroutineActive = true;
        }
        if (currentAI[33] >= 1)
        {
            HappyFrogDC.currentAILevel = currentAI[33];
            HappyFrogDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[34] >= 1)
        {
            MrHippoDC.currentAILevel = currentAI[34];
            MrHippoDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[35] >= 1)
        {
            PigpatchDC.currentAILevel = currentAI[35];
            PigpatchDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[36] >= 1)
        {
            NeddBearDC.currentAILevel = currentAI[36];
            NeddBearDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[37] >= 1)
        {
            OrvilleDC.currentAILevel = currentAI[37];
            OrvilleDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[42] >= 1)
        {
            MusicManCymbals.clip = load_audioClip(MusicManAddress);
            StartCoroutine(MusicManLoop());
            StartCoroutine(MusicManCymbalsCoroutine());
        }
        if (currentAI[5] >= 1)
        {
            StareAS.clip = load_audioClip(StareAddress);
            ToyBonnieAnimator.runtimeAnimatorController = Resources.Load(ToyBonnieAnimatorControllerAddress) as RuntimeAnimatorController;
            StartCoroutine(ToyBonnieLoop());
        }
        if (currentAI[6] >= 1)
        {
            StareAS.clip = load_audioClip(StareAddress);
            ToyChicaAnimator.runtimeAnimatorController = Resources.Load(ToyChicaAnimatorControllerAddress) as RuntimeAnimatorController;
            StartCoroutine(ToyChicaLoop());
        }
        if (currentAI[48] >= 1)
        {
            StartCoroutine(LeftyLoop());
        }
        if (currentAI[8] >= 1)
        {
            BBInOffice.sprite = load_sprite(BBAddresses[1]);
            BBInVent.sprite = load_sprite(BBAddresses[0]);
            BBLaugh.clip = load_audioClip(BBAddresses[2]);
        }
        if (currentAI[9] >= 1)
        {
            JJInOffice.sprite = load_sprite(JJAddresses[1]);
            JJInVent.sprite = load_sprite(JJAddresses[0]);
        }
        if (currentAI[2] >= 1)
        {
            ChicaPotsAndPans.clip = load_audioClip(PotsAndPantsAddress);
            ChicaPotsAndPans.Play();
            ChicaPotsAndPans.mute = false;
            ChicaHappy = true;
            StartCoroutine(ChicaLoop());
        }
        if (currentAI[0] >= 1)
        {
            FreddyStage1.sprite = load_sprite(FreddyAddresses[0]);
            FreddyStage2.sprite = load_sprite(FreddyAddresses[1]);
            FreddyStage3.sprite = load_sprite(FreddyAddresses[2]);
            FreddyStage4.sprite = load_sprite(FreddyAddresses[3]);
            StartCoroutine(FreddyLoop());
        }
        if (currentAI[46] >= 1)
        {
            ScrapbabyInOffice.sprite = load_sprite(ScrapbabyAddresses[0]);
            ShockPanel.sprite = load_sprite(ScrapbabyAddresses[2]);
            Shock.clip = load_audioClip(ScrapbabyAddresses[3]);
            StartCoroutine(ScrapBabyLoop());
        }
        if (currentAI[11] >= 1)
        {
            StareAS.clip = load_audioClip(StareAddress);
            ToiletBowlBonnieInOffice.sprite = load_sprite(ToiletBowlBonnieAddress);
            StartCoroutine(ToiletBowlBonnieLoop());
        }
        if (currentAI[18] >= 1)
        {
            Freddles[0].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[0]) as RuntimeAnimatorController;
            Freddles[1].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[1]) as RuntimeAnimatorController;
            Freddles[2].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[2]) as RuntimeAnimatorController;
            Freddles[3].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[3]) as RuntimeAnimatorController;
            Freddles[4].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[4]) as RuntimeAnimatorController;
            FreddlesSounds.clip = load_audioClip(NightmareFreddyAddresses[5]);
            FreddleSmoke.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[6]) as RuntimeAnimatorController;
            StartCoroutine(NightmareFreddyLoop());
            for (int i = 0; i < Freddles.Length; i++)
            {
                if (office == 0)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice0[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice0[i].rotation;
                }
                if (office == 1)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice1[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice1[i].rotation;
                }
                if (office == 2)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice2[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice2[i].rotation;
                }
                if (office == 3)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice3[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice3[i].rotation;
                }
            }
            FreddlesSounds.Play();
        }
        if (currentAI[39] >= 1)
        {
            RockstarBonnie.sprite = load_sprite(RockstarBonnieAddresses[0]);
            RockstarBonnieGuitar.sprite = load_sprite(RockstarBonnieAddresses[1]);
            StartCoroutine(RockstarBonnieLoop());
        }
        StartCoroutine(RFoxyLoop());
    }

    IEnumerator FlickerOfficeLights(float duration, float speed, float strength)
    {
    Color originalColor = OfficeLights.color;
    float startTime = Time.time;

    while (Time.time < startTime + duration)
    {
        // Calculate a random alpha value based on the strength of the flicker
        float flickerAlpha = Mathf.Clamp01(originalColor.a + Random.Range(-strength, strength));

        // Apply the flicker effect by adjusting the alpha of the OfficeLights image
        OfficeLights.color = new Color(originalColor.r, originalColor.g, originalColor.b, flickerAlpha);

        // Wait for a small interval before the next flicker
        yield return new WaitForSeconds(speed);
    }

    // Reset the alpha back to the original value after flickering
    OfficeLights.color = originalColor;
    }


    public void StartJumpscare(string[] addresses, int Sound, string Character, int JFPS = 24, float dur = 1.6f)
    {
        if (isJumpscaring)
            return;

        Resources.UnloadUnusedAssets();
		System.GC.WaitForPendingFinalizers();
        System.GC.Collect();

        isJumpscaring = true;
        LowerUI.SetActive(false);
        ElChipAdSound.volume = 0f;
        ElChipAdUpper.gameObject.SetActive(false);
        DisableOnJumpscare.SetActive(false);
        LowerNC.gameObject.SetActive(false);
        UpperNC.gameObject.SetActive(false);
        // Load the jumpscare sprites
        jumpscareSprites = LoadSprites(addresses);

        // Start the jumpscare coroutine
        StartCoroutine(PlayJumpscareAnimation(Sound, Character, JFPS, dur));
    }

    // Coroutine to play the jumpscare animation
    private IEnumerator PlayJumpscareAnimation(int Sound, string Character, int JFPS, float dur)
    {
    // Play the jumpscare sound
    if (camsActive)
    {
        DeActivateCams();
        yield return new WaitForSeconds(0.16f);
    }
    JumpscareAS.clip = Resources.Load<AudioClip>(JumpscareSoundAdresses[Sound]);
    JumpscareAS.Play();

    // Calculate the time per frame
    float timePerFrame = 1f / JFPS;
    float elapsedTime = 0f;

    // Repeat the animation for 2 seconds
    while (elapsedTime < dur)
    {
        for (int i = 0; i < jumpscareSprites.Length; i++)
        {
            JumpscareImage.sprite = jumpscareSprites[i];
            JumpscareImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(timePerFrame);
            elapsedTime += timePerFrame;

            // Break out of the loop if we've reached the 2-second limit
            if (elapsedTime >= 2f)
                break;
        }
    }

    // Save the data and load the Game Over scene
    DataManager.SaveValue<string>("LastDeath", Character, "data:/");
    DataManager.SaveValue<float>("LastSurvival", TimerTime, "data:/");
    int score = 0;
    for (int i = 0; i < currentAI.Length; i++)
    {
        score += (currentAI[i]*10);
    }
    DataManager.SaveValue<int>("PowerUpOdds", 0, "data:/");
    if (TimerTime >= 225f && score >= 1000)
    {
        DataManager.SaveValue<int>("PowerUpOdds", 7, "data:/");
    }
    else if (TimerTime >= 180f && score >= 500f)
    {
        DataManager.SaveValue<int>("PowerUpOdds", 6, "data:/");
    }
    else if (TimerTime >= 135f)
    {
        DataManager.SaveValue<int>("PowerUpOdds", 5, "data:/");
    }
    else if (TimerTime >= 90f)
    {
        DataManager.SaveValue<int>("PowerUpOdds", 4, "data:/");
    }
    else if (TimerTime >= 45f)
    {
        DataManager.SaveValue<int>("PowerUpOdds", 3, "data:/");
    }
    int animatronicsAt20 = 0;
    foreach (int i in currentAI)
    {
        if (i == 20)
        {
            animatronicsAt20++;
        }
    }
    if (animatronicsAt20 == 50)
    {
        float Best5020Time = DataManager.GetValue<float>("Best5020Time", "data:/");
        if (TimerTime >= Best5020Time)
        {
            DataManager.SaveValue<float>("Best5020Time", TimerTime, "data:/");
        }
        Debug.Log("???, It Saved");
    }
    else
    {
        Debug.Log(animatronicsAt20);
    }
    SceneManager.LoadScene("GameOverLoader");
    }

    // Function to load the sprites from Resources
    private Sprite[] LoadSprites(string[] addresses)
    {
        Sprite[] sprites = new Sprite[addresses.Length];
        for (int i = 0; i < addresses.Length; i++)
        {
            sprites[i] = Resources.Load<Sprite>(addresses[i]);
        }
        return sprites;
    }

    public void CollectFazCoin(GameObject ToDestroy)
    {
        Destroy(ToDestroy);
        FazCoins++;
        FazCoinCounter.text = FazCoins.ToString();
    }

    private void SpawnFazCoin()
    {
        int Amount = Random.Range(0,3);
        for (int i = 0; i < Amount; i++) {
            GameObject newCoin = Instantiate(SampleFazCoin, SampleFazCoin.transform.parent);
            newCoin.transform.position = new Vector3(Random.Range(0,400), Random.Range(0,240), 0);
            newCoin.SetActive(true);
        }
    }

    public void WindMusicBoxStart()
    {
        windingMusicBox = true;
    }

    public void WindMusicBoxStop()
    {
        windingMusicBox = false;
    }

    IEnumerator RFoxyLoop()
    {
        while (true)
        {
            while (!RFoxyInOffice)
            {
                yield return new WaitForSeconds(60f);
                if (MovementOppertunityC(currentAI[41]+10, 40) && !RFoxyInOffice && !RFoxyBirdActive)
                {
                    RFoxyInOffice = false;
                    RFoxyBirdActive = true;
                    StartCoroutine(RFoxyBirdCoroutine());
                }
                yield return null;
            }
            yield return null;
        }
    }

    public void AcceptFoxy()
    {
        RFoxyBird.position = RFoxyBirdInitalPos;
        RockstarFoxyMain.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        RockstarFoxyMain.Play("RFoxyGoUp");
        int RFoxyHelpfullness = Random.Range(1, 62)+currentAI[41]*2;
        RFoxyBirdActive = false;
        RFoxyInOffice = true;
        if (RFoxyHelpfullness <= 60)
        {
            int rndmIndex = Random.Range(0, RFoxyHelpful.Length);
            RFoxyHelpful[rndmIndex].Play();
            RFoxyPrizes.SetActive(true);
        }
        else
        {
            int rndmIndex = Random.Range(0, RFoxyUnHelpful.Length);
            RFoxyUnHelpful[rndmIndex].Play();
            StartCoroutine(RFoxyUnHepfulCor(rndmIndex));
        }
    }

    public void Plus1Power()
    {
        PercentLeft++;
        PercentText.text = ((int)PercentLeft).ToString();
        RockstarFoxyMain.Play("RFoxyGoDown");
        RFoxyInOffice = false;
        RFoxyPrizes.SetActive(false);
    }
    public void SixtyDegrees()
    {
        lastValidDegrees = 60f;
        currentDegrees = 60;
        if (!isJumpscaring)
        {
            DegreeText.text = ((int)currentDegrees).ToString();
        }
        RockstarFoxyMain.Play("RFoxyGoDown");
        RFoxyInOffice = false;
        RFoxyPrizes.SetActive(false);
    }
    public void Soundproof()
    {
        StartCoroutine(SoundProofHandler());
        RockstarFoxyMain.Play("RFoxyGoDown");
        RFoxyInOffice = false;
        RFoxyPrizes.SetActive(false);
    }

    public void Plus10FazCoins()
    {
        FazCoins += 10;
        FazCoinCounter.text = FazCoins.ToString();
        RockstarFoxyMain.Play("RFoxyGoDown");
        RFoxyInOffice = false;
        RFoxyPrizes.SetActive(false);
    }

    IEnumerator SoundProofHandler()
    {
        float t = 0f;
        while (t <= 10f)
        {
            t += Time.deltaTime;
            isSoundProof = true;
            yield return null;
        }
        isSoundProof = false;
    }

    IEnumerator RFoxyUnHepfulCor(int index)
    {
        yield return new WaitForSeconds(RFoxyHelpful[index].clip.length);
        StartJumpscare(RockstarFoxyJumpscareAddresses, 5, "RFoxy");
    }

    IEnumerator RFoxyBirdCoroutine()
    {
    // Record the initial position when the coroutine is called
    Vector3 initialPosition = RFoxyBird.position;
    RFoxyBirdInitalPos = initialPosition;
    // Calculate the target position with a negative X value
    Vector3 targetPosition = RFoxyBirdGoTo.position;
    RandomBirdSounds[Random.Range(0, RandomBirdSounds.Length)].Play();
    
    // Define the speed of movement
    float moveSpeed = 30f; // Adjust as necessary
    
    while (RFoxyBirdActive)
    {
        // Move towards the target position
        RFoxyBird.position = Vector3.MoveTowards(RFoxyBird.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the position is reached and break out of the loop
        if (RFoxyBird.position == targetPosition)
        {
            RFoxyBird.position = initialPosition;
            RFoxyBirdActive = false;
            yield break;
        }
        
        yield return null; // Wait until the next frame
    }
    }

    public void CollectGuitar()
    {
        RBonnieInOffice = false;
        RockstarBonnie.gameObject.SetActive(false);
        RockstarBonnieGuitar.gameObject.SetActive(false);
    }

    IEnumerator RockstarBonnieLoop()
    {
        int i = 0;
        while (currentAI[39] >= 1)
        {
            while (!RBonnieInOffice)
            {
                yield return new WaitForSeconds(13f);
                if (camsActive && MovementOppertunityC(currentAI[39], 29))
                {
                    RBonnieInOffice = true;
                    RockstarBonnie.gameObject.SetActive(true);
                    int[] options = { 1, 2, 6, 8 };
                    int randomIndex = Random.Range(0, options.Length);
                    RBonnieCamera = options[randomIndex];
                }
                yield return null;
            }
            while (RBonnieInOffice)
            {
                i++;
                if (i == 900)
                {
                    StartCoroutine(FlickerOfficeLights(1.4f, 0.01f, 0.4f));
                }
                if (i == 1000)
                {
                    StartJumpscare(RBonnieJumpscareAddresses, 5, "RBonnie");
                }
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator NightmareFreddyLoop()
    {
        int ActiveFreddles = 0;
        float timeOnCams = 0f;
        while (true)
        {
            if (ActiveFreddles >= 7 && camsActive)
            {
                StartJumpscare(NightmareFreddyJumpscareAddresses, 3, "NightmareFreddy");
            }
            if (camsActive)
            {
                timeOnCams += Time.deltaTime;
                if (timeOnCams >= 1f)
                {
                    timeOnCams = 0f;
                    if (MovementOppertunityC(currentAI[18], 30))
                    {
                    ActiveFreddles++;
                    for (int i = 0; i < Freddles.Length; i++)
                    {
                        Freddles[i].SetActive(i < Mathf.Clamp(ActiveFreddles, 0, Freddles.Length));
                    }
                    }
                }
            }
            if (flashlightActive)
            {
                if (ActiveFreddles < 3 && ActiveFreddles >= 1)
                {
                    if (Random.value <= 0.05f)
                    {
                        ActiveFreddles--;
                        if (ActiveFreddles < 5)
                        {
                            SpawnFreddleSmoke(Freddles[ActiveFreddles--].transform.position);   
                        }  
                        for (int i = 0; i < Freddles.Length; i++)
                        {
                            Freddles[i].SetActive(i < Mathf.Clamp(ActiveFreddles, 0, Freddles.Length));
                        }
                    }
                }
                else if (ActiveFreddles >= 3)
                {
                    if (Random.value <= 0.1f)
                    {
                        ActiveFreddles--;
                        if (ActiveFreddles < 5)
                        {
                            SpawnFreddleSmoke(Freddles[ActiveFreddles--].transform.position);   
                        }  
                        for (int i = 0; i < Freddles.Length; i++)
                        {
                            Freddles[i].SetActive(i < Mathf.Clamp(ActiveFreddles, 0, Freddles.Length));
                        }
                    }
                }
            }
            if (ActiveFreddles >= 1)
            {
                FreddlesSounds.mute = false;
            }
            else
            {
                FreddlesSounds.mute = true;
            }
            yield return null;
        }
    }

    void SpawnFreddleSmoke(Vector3 Position)
    {
        GameObject smoke = Instantiate(FreddleSmoke.gameObject, FreddleSmoke.transform.parent);
        smoke.transform.position = Position;
        smoke.SetActive(true);
        Destroy(smoke, 0.433f);
    }

    void ActivateBonnet()
    {
        StartCoroutine(activateBonnet());
    }

    IEnumerator activateBonnet()
    {
        BonnetAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(BonnetAnimatorAddress);
        bonnetSounds.clip = Resources.Load<AudioClip>(BonnetVoiceline);
        yield return new WaitForSeconds(4f);
        bonnetSounds.Play();
        BonnetAnimator.transform.localPosition = new Vector3(185.7f, -85.7f, 0f);
        MoveCor = StartCoroutine(MoveTransform(BonnetAnimator.transform, new Vector3( -185.7f, -85.7f,0), 4f ));
        StartCoroutine(BonnetFunction());
        bonnetActive = true;
    }

    IEnumerator BonnetFunction()
    {
        yield return new WaitForSeconds(4f);
        if (bonnetActive)
        {
            StartJumpscare(BonnetJumpscareAddresses, 4, "Bonnet");
        }
    }

    public void BoopBonnet()
    {
        if (!bonnetActive)
            return;

        bonnetActive = false;
        BonnetAnimator.Play("Boop");
        bonnetSounds.clip = Resources.Load<AudioClip>(NoseBoopSoundAddress);
        bonnetSounds.Play();
        StopCoroutine(MoveCor);
        StartCoroutine(MoveTransform(BonnetAnimator.transform, new Vector3( BonnetAnimator.transform.localPosition.x, -171.4f,0f), 1f ));
        StartCoroutine(UnloadBonnet());
    }

    IEnumerator EnableLOLBit(bool isXOR)
    {
        yield return new WaitForSeconds(isXOR ? 42f : 10f);
        LOLBitActive = true;
        int i = 0;
        LOLBit.sprite = load_sprite(LOLBitAddress);
        LOLBit.gameObject.SetActive(true);
        Beeping.clip = load_audioClip(BeepingAddress);
        Beeping.Play();
        Sound += 3;
        while (i < 1000 && Input.GetKeyDown(KeyCode.Alpha1) == false)
        {
            i++;
            yield return null;
        }
        while (i < 1000 && Input.GetKeyDown(KeyCode.Alpha2) == false)
        {
            i++;
            yield return null;
        }
        while (i < 1000 && Input.GetKeyDown(KeyCode.Alpha1) == false)
        {
            i++;
            yield return null;
        }
        Sound -= 3;
        Beeping.Stop();
        LOLBit.gameObject.SetActive(false);
        LOLBitActive = false;
    }

    IEnumerator UnloadBonnet()
    {
        yield return new WaitForSeconds(0.5f);
        BonnetAnimator.runtimeAnimatorController = null;
    }

    IEnumerator ToiletBowlBonnieLoop()
    {
        float camTime = 0f;
        while (true)
        {
            while (!ToiletBowlBonniePrepared && !ToiletBowlBonnieIsInOffice)
            {
                if (camsActive)
                {
                    camTime += Time.deltaTime;
                    if (camTime >= 13f)
                    {
                        ToiletBowlBonnieInOffice.gameObject.SetActive(true);
                        ToiletBowlBonniePrepared = true;
                        camTime = 0f;
                    }
                }
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator ScrapBabyLoop()
    {
        float timeOnCams = 0f;
        bool canAttack = false;
        while (!ScrapBabyIsInOffice)
        {
            if (camsActive)
            {
                timeOnCams += Time.deltaTime;
                if (timeOnCams >= 5f)
                {
                    timeOnCams = 0f;
                    if (MovementOppertunityC(currentAI[46], 30))
                    {
                        ScrapBabyIsInOffice = true;
                        ScrapbabyInOffice.gameObject.SetActive(true);
                        ShockPanel.gameObject.SetActive(true);
                    }
                }
            }
            yield return null;
        }

        while (ScrapbabyInOffice)
        {
            if (camsActive && !canAttack)
            {
                timeOnCams += Time.deltaTime;
                if (timeOnCams >= 5f)
                {
                    timeOnCams = 0f;
                    if (MovementOppertunityC(currentAI[46], 30))
                    {
                        canAttack = true;
                        ScrapbabyInOffice.sprite = load_sprite(ScrapbabyAddresses[1]);
                        ScrapBabyStandingUp = true;
                    }
                }
            }
            yield return null;
        }
    }

    public void ShockScrapBaby()
    {
        PercentLeft--;
        PercentText.text = ((int)PercentLeft).ToString();
        Shock.Play();
        StartCoroutine(FlickerOfficeLights(1f, 3f, 1f));
        if (ScrapBabyStandingUp)
        {
            ScrapBabyStandingUp = false;
            ScrapBabyIsInOffice = false;
            ScrapbabyInOffice.sprite = load_sprite(ScrapbabyAddresses[0]);
        }
    }

    IEnumerator ActivateRWQ()
    {
        RWQ.sprite = load_sprite(RWQAddress);
        activateRWQNext = true;
        while (RWQCarrier.activeSelf == false)
        {
            yield return null;
        }
        while (RWQTimer < 10f)
        {
            RWQTimer += Time.deltaTime;
            yield return null;
        }
        activateRWQNext = false;
    }

    IEnumerator ActivateMinireenas()
    {
        Minireenas.sprite = load_sprite(MinireenasAddress);
        ActivateMinireenasN = true;
        while (Minireenas.gameObject.activeSelf == false)
        {
            yield return null;
        }
        while (MinireenaTimer < 33.33f)
        {
            MinireenaTimer += Time.deltaTime;
            yield return null;
        }
        ActivateMinireenasN = false;
    }

    IEnumerator FreddyLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            FreddyProgress += (currentAI[0]+((currentDegrees-60))/5f);
            Debug.Log(FreddyProgress);
            if (FreddyProgress >= 400 && leftDoorClosed)
            {
                BlockAnimatronic();
                FreddyProgress = 0;
            }
            else if (FreddyProgress >= 500 && !leftDoorClosed && camsActive)
            {
                StartJumpscare(FreddyJumpscareAddresses, 0, "Freddy");
            }
            if (camsActive && currentCam == 1)
            {
                SwitchCamera(1, false);
            }
            yield return null;
        }
    }

    IEnumerator ChicaLoop()
    {
        while (!ChicaAngry)
        {
            yield return new WaitForSeconds(15f);
            if (MovementOppertunityC(currentAI[2], 40) && !GBMActive)
            {
                ChicaHappy = false;
                ChicaPotsAndPans.mute = true;
                ChicaMissedMusic++;
                if (ChicaMissedMusic >= 2)
                {
                    ChicaAngry = true;
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }

        while (ChicaAngry)
        {
            yield return new WaitForSeconds(0.5f);
            if (camsActive)
            {
                if (Random.value <= 0.2f)
                {
                    StartJumpscare(ChicaJumpscareAddresses, 0, "Chica");
                    yield break;
                }
            }
            yield return null;
        }
    }

    IEnumerator LeftyLoop()
    {
        while (currentAI[48] >= 1)
        {
            LeftyAgitation = Mathf.Clamp(LeftyAgitation, 0, 1800);
            if (MovementOppertunityC(currentAI[48], 100) && currentVentilator != 1 && !isSoundProof)
            {
                if (Sound >= 2)
                {
                    LeftyAgitation += Sound-1;
                }
                if (currentDegrees >= 80 && currentDegrees < 100)
                {
                    LeftyAgitation++;
                }
                else if (currentDegrees >= 100)
                {
                    LeftyAgitation += 2;
                }
            }
            else if (currentVentilator == 1)
            {
                LeftyAgitation--;
            }

            if (LeftyAgitation >= 1800)
            {
                if (camsActive && currentCam != 3)
                {
                    StartJumpscare(LeftyJumpscareAddresses, 5, "Lefty");
                    yield break;
                }
                else if (!camsActive)
                {
                    StartJumpscare(LeftyJumpscareAddresses, 5, "Lefty");
                    yield break;
                }
            }
            yield return null;
        }
    }

    IEnumerator ToyChicaLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            if (MovementOppertunityC(currentAI[6], 60) && !ToyChicaAttacking)
            {
                StareAS.Play();
                ToyChicaAttacking = true;
                StartCoroutine(ToyChicaAttackingCoroutine());
                StartCoroutine(FlickerOfficeLights(5f, 0.4f, 0.4f));
            }
            yield return null;
        }
    }

    IEnumerator ToyChicaAttackingCoroutine()
    {
        int i = 0;
        int MaskI = 0;
        MaskI = Random.Range(0, 100);
        while (ToyChicaAttacking)
        {
            i++;
            if (maskActive)
            {
                MaskI++;
            }
            if (Vector3.Distance(ToyChica.position, ToyBonnieChicaMoveTowards.position) <= 15f && maskActive)
            {
                MaskI++;
            }
            if (MaskI >= 200)
            {
                StartCoroutine(ToyChicaReturn());
                StareAS.Stop();
                ToyChicaAttacking = false;
                yield break;
            }
            if (i >= 300)
            {
                ToyChica.gameObject.SetActive(false);
                StartJumpscare(ToyChicaJumpscareAddresses, 1, "ToyChica");
                yield break;
            }
            ToyChica.position = Vector3.MoveTowards(ToyChica.position, ToyBonnieChicaMoveTowards.position, 2f);
            yield return null;
        }
    }

    IEnumerator ToyChicaReturn()
    {
        float dt = 0;
        while (dt <= 3f)
        {
            dt += Time.deltaTime;
            ToyChica.position = Vector3.MoveTowards(ToyChica.position, ToyChicaReturnPos.position, 6f);
            yield return null;
        }
    }

    IEnumerator ToyBonnieLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            if (MovementOppertunityC(currentAI[5], 60) && !ToyBonnieAttacking)
            {
                StareAS.Play();
                ToyBonnieAttacking = true;
                StartCoroutine(ToyBonnieAttackingCoroutine());
                StartCoroutine(FlickerOfficeLights(5f, 0.4f, 0.4f));
            }
            yield return null;
        }
    }

    IEnumerator ToyBonnieAttackingCoroutine()
    {
        int i = 0;
        int MaskI = 0;
        MaskI = Random.Range(0, 100);
        while (ToyBonnieAttacking)
        {
            i++;
            if (maskActive)
            {
                MaskI++;
            }
            if (Vector3.Distance(ToyBonnie.position, ToyBonnieChicaMoveTowards.position) <= 15f && maskActive)
            {
                MaskI++;
            }
            if (MaskI >= 200)
            {
                StartCoroutine(ToyBonnieReturn());
                StareAS.Stop();
                ToyBonnieAttacking = false;
                yield break;
            }
            if (i >= 300)
            {
                ToyBonnie.gameObject.SetActive(false);
                StartJumpscare(ToyBonnieJumpscareAddresses, 1, "ToyBonnie");
                yield break;
            }
            ToyBonnie.position = Vector3.MoveTowards(ToyBonnie.position, ToyBonnieChicaMoveTowards.position, 2f);
            yield return null;
        }
    }

    IEnumerator ToyBonnieReturn()
    {
        float dt = 0;
        while (dt <= 3f)
        {
            dt += Time.deltaTime;
            ToyBonnie.position = Vector3.MoveTowards(ToyBonnie.position, ToyBonnieReturnPos.position, 6f);
            yield return null;
        }
    }

    IEnumerator FigurineLoop()
    {
        float MonitorTime = 0f;
        while (currentAI[1] >= 1 && currentAI[3] >= 1)
        {
            if (camsActive)
            {
                MonitorTime += Time.deltaTime;

                if (MonitorTime >= 2f)
                {
                    MonitorTime = 0f;
                    if (Random.value <= 0.33f)
                    {
                        if (activeFigurine == 0)
                        {
                            activeFigurine = 1;
                            CurrentFigurine.sprite = load_sprite(BonnieAddresses[0]);
                            CurrentFigurine.gameObject.SetActive(true);
                        }
                        else
                        {
                            CurrentFigurine.sprite = load_sprite(FoxyAddresses[0]);
                            CurrentFigurine.gameObject.SetActive(true);
                            activeFigurine = 0;
                        }
                    }
                }
            }
            yield return null;
        }
        CurrentFigurine.gameObject.SetActive(false);
    }

    IEnumerator MusicManLoop()
    {
        while (true)
        {
            if (MusicManAgitation >= 10000)
            {
                StartJumpscare(MusicManJumpscareAddress, 0, "MM");
                yield break;
            }
            if (Sound <= 0 || isSoundProof)
            {
                if (MusicManAgitation > 0)
                {
                    MusicManAgitation -= 5;
                }
                else
                {
                    MusicManAgitation = 0;
                }
                yield return null;
            }
            if (Sound == 1 && !isSoundProof)
            {
                if (MusicManAgitation > 0)
                {
                    MusicManAgitation -= 1;
                }
                else
                {
                    MusicManAgitation = 0;
                }
                yield return null;
            }
            if (Sound == 2 && !isSoundProof)
            {
                MusicManAgitation += 1+(currentAI[42]/5);
                yield return new WaitForSeconds(0.02f);
                yield return null;
            }
            if (Sound >= 3 && !isSoundProof)
            {
                MusicManAgitation += 1+(currentAI[42]/5);
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator MusicManCymbalsCoroutine()
    {
        while (true)
        {
            if (MusicManAgitation >= 5000 && MusicManAgitation < 6500)
            {
                MusicManCymbals.Play();
                yield return new WaitForSeconds(3f);
                yield return null;
            }
            else if (MusicManAgitation >= 6500 && MusicManAgitation < 8000)
            {
                MusicManCymbals.Play();
                yield return new WaitForSeconds(2f);
                yield return null;
            }
            else if (MusicManAgitation >= 8000)
            {
                MusicManCymbals.Play();
                yield return new WaitForSeconds(1f);
                yield return null;
            }
            yield return null;
        }
    }

    public void SwitchOpenDuct()
    {
        if (currentClosedDuct == "RightDuct")
        {
            currentClosedDuct = "LeftDuct";
            Vector3 DuctOpenPos = DuctOpen.position;
            Vector3 DuctClosedPos = DuctClosed.position;
            DuctOpen.position = DuctClosedPos;
            DuctClosed.position = DuctOpenPos;
            HappyFrogDC.currentClosedDuct = "LeftDuct";
            MrHippoDC.currentClosedDuct = "LeftDuct";
            PigpatchDC.currentClosedDuct = "LeftDuct";
            NeddBearDC.currentClosedDuct = "LeftDuct";
            OrvilleDC.currentClosedDuct = "LeftDuct";
        }
        else
        {
            currentClosedDuct = "RightDuct";
            Vector3 DuctOpenPos = DuctOpen.position;
            Vector3 DuctClosedPos = DuctClosed.position;
            DuctOpen.position = DuctClosedPos;
            DuctClosed.position = DuctOpenPos;
            HappyFrogDC.currentClosedDuct = "RightDuct";
            MrHippoDC.currentClosedDuct = "RightDuct";
            PigpatchDC.currentClosedDuct = "RightDuct";
            NeddBearDC.currentClosedDuct = "RightDuct";
            OrvilleDC.currentClosedDuct = "RightDuct";
        }
    }

    public void SetLure(DuctNode DC)
    {
        DuctLure.position = DC.transform.position;
        DuctLure.gameObject.SetActive(true);
        HappyFrogDC.ActiveLure = DC;
        MrHippoDC.ActiveLure = DC;
        PigpatchDC.ActiveLure = DC;
        NeddBearDC.ActiveLure = DC;
        OrvilleDC.ActiveLure = DC;
    }

    public void BuyNightmareBonnie()
    {
        if (currentAI[19] < 10 && FazCoins >= 10)
        {
            FazCoins -= 10;
            FazCoinCounter.text = FazCoins.ToString();
            NightmareBonnieBuy.SetActive(false);
            NightmareBonnieBought = true;
            NightmareBonnieActive = false;
            BuySFX.Play();
        }
        else if (currentAI[19] < 20 && FazCoins >= 15)
        {
            FazCoins -= 15;
            FazCoinCounter.text = FazCoins.ToString();
            NightmareBonnieBuy.SetActive(false);
            NightmareBonnieBought = true;
            NightmareBonnieActive = false;
            BuySFX.Play();
        }
        else if (currentAI[19] == 20 && FazCoins >= 20)
        {
            FazCoins -= 20;
            FazCoinCounter.text = FazCoins.ToString();
            NightmareBonnieBuy.SetActive(false);
            NightmareBonnieBought = true;
            NightmareBonnieActive = false;
            BuySFX.Play();
        }
    }

    public void BuyNightmareMangle()
    {
        if (currentAI[23] < 10 && FazCoins >= 10)
        {
            FazCoins -= 10;
            FazCoinCounter.text = FazCoins.ToString();
            NightmareMangleBuy.SetActive(false);
            NightmareMangleBought = true;
            NightmareMangleActive = false;
            BuySFX.Play();
        }
        else if (currentAI[23] < 20 && FazCoins >= 15)
        {
            FazCoins -= 15;
            FazCoinCounter.text = FazCoins.ToString();
            NightmareMangleBuy.SetActive(false);
            NightmareMangleBought = true;
            NightmareMangleActive = false;
            BuySFX.Play();
        }
        else if (currentAI[23] == 20 && FazCoins >= 20)
        {
            FazCoins -= 20;
            FazCoinCounter.text = FazCoins.ToString();
            NightmareMangleBuy.SetActive(false);
            NightmareMangleBought = true;
            NightmareMangleActive = false;
            BuySFX.Play();
        }
    }

    public void BuyCircusBaby()
    {
        if (currentAI[27] < 10 && FazCoins >= 10)
        {
            FazCoins -= 10;
            FazCoinCounter.text = FazCoins.ToString();
            BabyBuy.SetActive(false);
            BabyBought = true;
            BabyActive = false;
            BuySFX.Play();
        }
        else if (currentAI[27] < 20 && FazCoins >= 15)
        {
            FazCoins -= 15;
            FazCoinCounter.text = FazCoins.ToString();
            BabyBuy.SetActive(false);
            BabyBought = true;
            BabyActive = false;
            BuySFX.Play();
        }
        else if (currentAI[27] == 20 && FazCoins >= 20)
        {
            FazCoins -= 20;
            FazCoinCounter.text = FazCoins.ToString();
            BabyBuy.SetActive(false);
            BabyBought = true;
            BabyActive = false;
            BuySFX.Play();
        }
    }

    IEnumerator PlushieCoroutine()
    {
        if (currentAI[19] < 10)
        {
            NightmareBonnieBuyText.text = "Nightmare Bonnie - 10";
        }
        else if (currentAI[19] < 20)
        {
            NightmareBonnieBuyText.text = "Nightmare Bonnie - 15";
        }
        else if (currentAI[19] == 20)
        {
            NightmareBonnieBuyText.text = "Nightmare Bonnie - 20";
        }

        if (currentAI[23] < 10)
        {
            NightmareMangleBuyText.text = "Nightmare Mangle - 10";
        }
        else if (currentAI[23] < 20)
        {
            NightmareMangleBuyText.text = "Nightmare Mangle - 15";
        }
        else if (currentAI[23] == 20)
        {
            NightmareMangleBuyText.text = "Nightmare Mangle - 20";
        }

        if (currentAI[27] < 10)
        {
            BabyBuyText.text = "Circus Baby - 10";
        }
        else if (currentAI[27] < 20)
        {
            BabyBuyText.text = "Circus Baby - 15";
        }
        else if (currentAI[27] == 20)
        {
            BabyBuyText.text = "Circus Baby - 20";
        }
        
        while (true)
        {
            yield return new WaitForSeconds(45f);
            if (getCurrentAMTime() == 1 || getCurrentAMTime() == 3 || getCurrentAMTime() == 5)
            {
                int rndm = Random.Range(0,3);
                if (rndm == 0)
                {
                    if (currentAI[19] >= 1 && !NightmareBonnieBought)
                    {
                        NightmareBonnieActive = true;
                        NightmareMangleActive = false;
                        BabyActive = false;
                    }
                }
                else if (rndm == 1)
                {
                    if (currentAI[23] >= 1 && !NightmareMangleBought)
                    {
                        NightmareBonnieActive = false;
                        NightmareMangleActive = true;
                        BabyActive = false;
                    }
                }
                else if (rndm == 2)
                {
                    if (currentAI[27] >= 1 && !BabyBought)
                    {
                        NightmareBonnieActive = false;
                        NightmareMangleActive = false;
                        BabyActive = true;
                    }
                }
                StartCoroutine(PlushieActiveHandler(rndm));
            }
        }
    }

    IEnumerator PlushieActiveHandler(int Plushie)
    {
        yield return new WaitForSeconds(20f);
        switch (Plushie)
        {
            case 0:
            if (!NightmareBonnieBought && NightmareBonnieActive)
            {
                if (!camsActive)
                {
                    StartJumpscare(NightmareBonnieJumpscareAddresses, 3, "NB");
                }
                else
                {
                    if (currentCam != 2)
                    {
                        StartJumpscare(NightmareBonnieJumpscareAddresses, 3, "NB");
                    }
                    else
                    {
                        while (currentCam == 2)
                        {
                            yield return null;
                        }
                        StartJumpscare(NightmareBonnieJumpscareAddresses, 3, "NB");
                    }
                }
            }
            break;
            case 1:
            if (!NightmareMangleBought && NightmareMangleActive)
            {
                if (!camsActive)
                {
                    StartJumpscare(NightmareMangleJumpscareAddresses, 3, "NMangle");
                }
                else
                {
                    if (currentCam != 2)
                    {
                        StartJumpscare(NightmareMangleJumpscareAddresses, 3, "NMangle");
                    }
                    else
                    {
                        while (currentCam == 2)
                        {
                            yield return null;
                        }
                        StartJumpscare(NightmareMangleJumpscareAddresses, 3, "NMangle");
                    }
                }
            }
            break;
            case 2:
            if (!BabyBought && BabyActive)
            {
                if (!camsActive)
                {
                    StartJumpscare(BabyJumpscareAddresses, 4, "CB");
                }
                else
                {
                    if (currentCam != 2)
                    {
                        StartJumpscare(BabyJumpscareAddresses, 4, "CB");
                    }
                    else
                    {
                        while (currentCam == 2)
                        {
                            yield return null;
                        }
                        StartJumpscare(BabyJumpscareAddresses, 4, "CB");
                    }
                }
            }
            break;
        }
        yield return null;
    }

    IEnumerator NightmareChicaLoop()
    {
        UpperNC.localPosition = new Vector3(0f, 230.8278f, 0f);
        LowerNC.localPosition = new Vector3(0f, -230.8278f, 0f);

        UpperNC.GetComponent<Image>().sprite = load_sprite(UpperJawAddress);
        LowerNC.GetComponent<Image>().sprite = load_sprite(LowerJawAddress);

        float currentY = 230.8278f;

        while (true)
        {
            if (currentVentilator != 2)
            {
                currentY -= 0.22222222222f * Time.deltaTime * 60f;
            }
            else
            {
                currentY += 0.88888888888f * Time.deltaTime * 60f;
            }

            UpperNC.localPosition = new Vector3(0f, currentY, 0f);
            LowerNC.localPosition = new Vector3(0f, -currentY, 0f);

            if (currentY < 60f)
            {
                StartJumpscare(NightmareChicaJumpscareAddresses, 3, "NightmareChica");
                yield break;
            }

            if (currentY > 230.8278f)
            {
                break;
            }

            yield return null;
        }
    }

    IEnumerator JackOChicaLoop()
    {
        while (true)
        {
            if (currentDegrees >= 90)
            {
                //JackOChicaManifest += (currentAI[22]*2)/(1.0f/Time.deltaTime);
                if (leftDoorClosed && rightDoorClosed && currentDegrees < 100)
                {
                    JackOChicaManifest -= 10;
                }
                else if ((!leftDoorClosed || !rightDoorClosed) && currentDegrees < 100)
                {
                    JackOChicaManifest += (currentAI[22]*2)/(1.0f/Time.deltaTime);
                }
                else if (currentDegrees >= 100)
                {
                    JackOChicaManifest += (currentAI[22]*2)/(1.0f/Time.deltaTime);
                }

                if (JackOChicaManifest >= 500)
                {
                    if ((!leftDoorClosed || !rightDoorClosed))
                    {
                        StartJumpscare(JackOChicaJumpscareAddresses, 3, "JOC");
                        yield break;
                    }
                }
                
                JackOChicaManifest = Mathf.Clamp(JackOChicaManifest, -50, 500);
                JackOChicaLeft.color = new Color(1,1,1,JackOChicaManifest/500);
                JackOChicaRight.color = new Color(1,1,1,JackOChicaManifest/500);
            }
            else
            {
                if (leftDoorClosed && rightDoorClosed && currentDegrees < 100)
                {
                    JackOChicaManifest -= 10;
                    JackOChicaManifest = Mathf.Clamp(JackOChicaManifest, -50, 500);
                    JackOChicaLeft.color = new Color(1,1,1,JackOChicaManifest/500);
                    JackOChicaRight.color = new Color(1,1,1,JackOChicaManifest/500);
                }
            }
            yield return null;
        }
    }

    IEnumerator NightmareLoop()
    {
        float timeOnCams = 0;
        int i = 0;
        bool atDoor = false;
        bool canAttack = false;
        while (true)
        {
            if (camsActive && !atDoor)
            {
                timeOnCams += Time.deltaTime;
            }
            if (timeOnCams >= 5f && !atDoor)
            {
                if (MovementOppertunityC(currentAI[21], 40))
                {
                    atDoor = true;
                    FredLaugh.clip = load_audioClip(NFAddresses[Random.Range(2,4)]);
                    FredLaugh.Play();
                    NightmareEyes.gameObject.SetActive(true);
                }
                else
                {
                    timeOnCams = 0f;
                }
            }

            while (atDoor && !rightDoorClosed && !canAttack)
            {
                i++;
                if (i >= 300 - (currentAI[21]*5))
                {
                    canAttack = true;
                }
                yield return null;
            }

            while (canAttack && !rightDoorClosed)
            {
                if (camsActive)
                {
                    StartJumpscare(NightmareJumpscareAddresses, 3, "Nightmare");
                    yield break;
                }
                yield return null;
            }

            if (rightDoorClosed && atDoor)
            {
                BlockAnimatronic();
                i = 0;
                timeOnCams = 0f;
                atDoor = false;
                canAttack = false;
                NightmareEyes.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator NightmareFredbearLoop()
    {
        float timeOnCams = 0;
        int i = 0;
        bool atDoor = false;
        bool canAttack = false;
        while (true)
        {
            if (camsActive && !atDoor)
            {
                timeOnCams += Time.deltaTime;
            }
            if (timeOnCams >= 5f && !atDoor)
            {
                if (MovementOppertunityC(currentAI[20], 40))
                {
                    atDoor = true;
                    FredLaugh.clip = load_audioClip(NFAddresses[Random.Range(2,4)]);
                    FredLaugh.Play();
                    FredbearEyes.gameObject.SetActive(true);
                }
                else
                {
                    timeOnCams = 0f;
                }
            }

            while (atDoor && !leftDoorClosed && !canAttack)
            {
                i++;
                if (i >= 300 - (currentAI[20]*5))
                {
                    canAttack = true;
                }
                yield return null;
            }

            while (canAttack && !leftDoorClosed)
            {
                if (camsActive)
                {
                    StartJumpscare(NightmareFredbearJumpscareAddresses, 3, "NF");
                    yield break;
                }
                yield return null;
            }

            if (leftDoorClosed && atDoor)
            {
                BlockAnimatronic();
                i = 0;
                timeOnCams = 0f;
                atDoor = false;
                canAttack = false;
                FredbearEyes.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator WitheredChicaLoop()
    {
        while (!WitheredChicaStuck)
        {
            yield return new WaitForSeconds(5f);
            if (MovementOppertunity(currentAI[10]))
            {
                // Select a random path index
                int pathIndex = 2;

                // Move the MangleIcon along the selected path
                yield return StartCoroutine(MoveWitheredChicaAlongPath(pathIndex));
            }
        }
    }

    // Coroutine to move MangleIcon along the selected path
    private IEnumerator MoveWitheredChicaAlongPath(int pathIndex)
    {

    Vector3 resetPosition = WitheredChicaIcon.position;

    // Select the path based on the index
    Transform[] selectedPath;

    switch (pathIndex)
    {
        case 0:
            selectedPath = ventPath1;
            break;
        case 1:
            selectedPath = ventPath2;
            break;
        case 2:
            selectedPath = ventPath3;
            break;
        default:
            yield break;
    }

    // Move along each transform in the selected path
    for (int i = 0; i < selectedPath.Length; i++)
    {
        Transform targetTransform = selectedPath[i];

        // Move towards the target transform
        while (Vector3.Distance(WitheredChicaIcon.position, targetTransform.position) > 0.01f)
        {
            WitheredChicaIcon.position = Vector3.MoveTowards(WitheredChicaIcon.position, targetTransform.position, (PixelSteps * Time.deltaTime)+(currentAI[7]*0.01f));
            if (IsInSnareRange(WitheredChicaIcon))
            {
                // Reset Mangle to the start of the path or a reset position
                WitheredChicaIcon.position = resetPosition;
                BlockAnimatronic();
                yield break;
            }
            yield return null;
        }
    }

        if (!frontVentClosed && camsActive)
        {
            WitheredChicaStuck = true;
            WitheredChicaInOffice.gameObject.SetActive(true);
            WitheredChicaIcon.gameObject.SetActive(false);
            StartCoroutine(WitheredChicaInOfficeCoroutine());
        }
        else if (frontVentClosed)
        {
            WitheredChicaIcon.position = resetPosition;
            BlockAnimatronic();
        }
        else if (!frontVentClosed && !camsActive)
        {
            while (!frontVentClosed && !camsActive)
            {
                yield return null;
            }
            if (!frontVentClosed && camsActive)
            {
                WitheredChicaStuck = true;
                WitheredChicaInOffice.gameObject.SetActive(true);
                WitheredChicaIcon.gameObject.SetActive(false);
                StartCoroutine(WitheredChicaInOfficeCoroutine());
            }
            else
            {
                WitheredChicaIcon.position = resetPosition;
                BlockAnimatronic();
            }
        }
    }

    IEnumerator WitheredChicaInOfficeCoroutine()
    {
        while (WitheredChicaStuck)
        {
            yield return new WaitForSeconds(1f);
            if (camsActive && Random.value <= 1/15)
            {
                StartJumpscare(WitheredChicaJumpscareAddresses, 1, "WitheredChica");
            }
        }
    }

    public void SwitchToyFreddyCamera(int Cam)
    {
        currentToyFreddyCam = Cam;
        if (currentToyFreddyCam == currentClosedDoor)
        {
            ToyFreddyCloseDoor.sprite = ToyFreddyCloseDoorClosed;
        }
        else
        {
            ToyFreddyCloseDoor.sprite = ToyFreddyCloseDoorNormal;
        }
        SwitchCamera(8);
    }

    public void CloseCurrentToyFreddyDoor()
    {
        DoorSFX.Play();
        currentClosedDoor = currentToyFreddyCam;
        ToyFreddyCloseDoor.sprite = ToyFreddyCloseDoorClosed;
    }

    IEnumerator ToyFreddyLoop()
    {
        MrHugsCamera = Random.Range(1, 4);
        bool jumpscared = false;
        while (currentAI[4] >= 1)
        {
            while (!jumpscared)
            {
                yield return new WaitForSeconds(( 1000 - (currentAI[4]*5))/60f);
                if (currentClosedDoor == MrHugsCamera && !(camsActive && currentCam == 8) && MovementOppertunityC(currentAI[4], 29))
                {
                    MrHugsCamera = Random.Range(1, 4);
                    yield return null;
                }
                else if (currentClosedDoor != MrHugsCamera && !(camsActive && currentCam == 8) && MovementOppertunityC(currentAI[4], 29))
                {
                    jumpscared = true;
                    MrHugsCamera = 5;
                }
                yield return null;
            }

            while (jumpscared)
            {
                yield return new WaitForSeconds(0.5f);
                if (camsActive)
                {
                    if (Random.value <= 0.1f)
                    {
                        StartJumpscare(ToyFreddyJumpscareAddresses, 1, "TF");
                    }   
                }
                yield return null;
            }
            yield return null;
        }
    }

    public void Pay5CoinsToRFR()
    {
        if (FazCoins >= 5)
        {
            FazCoins -= 5;
            TimesAsked = 0;
            isAsking = false;
            StopCoroutine(RFRCoroutine);
            RFRCoroutine = StartCoroutine(RockstarFreddyLoop());
            RockstarFreddyAS.clip = load_audioClip(RockstarFreddyAddresses[8]);
            RockstarFreddyAS.Play();
            BuySFX.Play();
            Deposit5Coins.SetActive(false);
            RockstarFreddy.sprite = load_sprite(RockstarFreddyAddresses[0]);
            FazCoinCounter.text = FazCoins.ToString();
        }
    }

    IEnumerator UseHeaterToRFR()
    {
        while (true)
        {
            while (currentVentilator == 3 && isAsking)
            {
            yield return new WaitForSeconds(0.4f);
            if (currentVentilator == 3 && Random.value <= 0.15f && isAsking)
            {
                TimesAsked = 0;
                isAsking = false;
                StopCoroutine(RFRCoroutine);
                RFRCoroutine = StartCoroutine(RockstarFreddyLoop());
                RockstarFreddyAS.clip = load_audioClip(RockstarFreddyAddresses[8]);
                RockstarFreddyAS.Play();
                RockstarFreddy.sprite = load_sprite(RockstarFreddyAddresses[0]);
                Deposit5Coins.SetActive(false);
            }
            else if (currentVentilator == 3 && isAsking)
            {
                int rndm = Random.Range(10, 13);
                int rndm2 = Random.Range(3, 6);
                RockstarFreddyAS.clip = load_audioClip(RockstarFreddyAddresses[rndm]);
                RockstarFreddyAS.Play();
                RockstarFreddy.sprite = load_sprite(RockstarFreddyAddresses[rndm2]);
            }
            yield return null;
            }
            yield return null;
        }
    }

    IEnumerator RockstarFreddyLoop()
    {
        Coroutine RFRHeaterCor = null;
        while (true)
        {
            while (!isAsking)
            {
                yield return new WaitForSeconds(30f);
                if (MovementOppertunityC(currentAI[38], 29))
                {
                    TimesAsked = 0;
                    isAsking = true;
                    RockstarFreddy.sprite = load_sprite(RockstarFreddyAddresses[1]);
                    Deposit5Coins.SetActive(true);
                }
                else
                {
                    yield return null;
                }
            }

            while (isAsking)
            {
                if (currentVentilator == 3 && RFRHeaterCor == null)
                {
                    RFRHeaterCor = StartCoroutine(UseHeaterToRFR());
                }
                if (TimesAsked <= 5)
                {
                    yield return new WaitForSeconds(3f);
                    if (currentVentilator == 3 && RFRHeaterCor == null)
                    {
                        RFRHeaterCor = StartCoroutine(UseHeaterToRFR());
                    }
                    RockstarFreddyAS.clip = load_audioClip(RockstarFreddyAddresses[6]);
                    RockstarFreddyAS.Play();
                    TimesAsked++;
                    yield return null;
                }
                else if (TimesAsked <= 9)
                {
                    yield return new WaitForSeconds(3f);
                    if (currentVentilator == 3 && RFRHeaterCor == null)
                    {
                        RFRHeaterCor = StartCoroutine(UseHeaterToRFR());
                    }
                    RockstarFreddyAS.clip = load_audioClip(RockstarFreddyAddresses[7]);
                    RockstarFreddyAS.Play();
                    TimesAsked++;
                    yield return null;
                }
                else if (TimesAsked <= 11)
                {
                    yield return new WaitForSeconds(1f);
                    if (currentVentilator == 3 && RFRHeaterCor == null)
                    {
                        RFRHeaterCor = StartCoroutine(UseHeaterToRFR());
                    }
                    RockstarFreddyAS.clip = load_audioClip(RockstarFreddyAddresses[7]);
                    RockstarFreddyAS.Play();
                    TimesAsked++;
                    yield return null;
                }
                else if (TimesAsked >= 12)
                {
                    yield return new WaitForSeconds(1f);
                    RockstarFreddyAS.Stop();
                    StartJumpscare(RockstarFreddyJumpscareAddresses, 5, "RFR");
                }
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator BalloraLoop()
    {
        bool attacking = false;
        int i = 0;
        string whatDoor = "Left";
        while (true)
        {
            while (!attacking)
            {
                i = 0;
                yield return new WaitForSeconds(14f);
                if (MovementOppertunityC(currentAI[28], 40))
                {
                    BalloraMusic.mute = false;
                    int rndm = Random.Range(0,2);
                    if (rndm == 0)
                    {
                        BalloraMusic.panStereo = 1;
                        whatDoor = "Right";
                    }
                    else
                    {
                        BalloraMusic.panStereo = -1;
                        whatDoor = "Left";
                    }
                    attacking = true;
                }
                else
                {
                    yield return null;
                }
            }

            while (attacking)
            {
                i++;
                if (i == 100)
                {
                    StartCoroutine(FlickerOfficeLights(1.16666666667f, 0.07f, 0.7f));
                }
                else if (i >= 170)
                {
                    if ((whatDoor == "Left" && leftDoorClosed) || (whatDoor == "Right" && rightDoorClosed))
                    {
                        BlockAnimatronic();
                        attacking = false;
                        BalloraMusic.mute = true;
                    }
                    else
                    {
                        StartJumpscare(BalloraJumpscareAddresses, 4, "Ballora");
                    }
                }
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator NightmareBBLoop()
    {
        while (true)
        {
            if (MainBG.position.x <= MinX && flashlightActive)
            {
                if (NightmareBBStandingUp)
                {
                    NightmareBBStandingUp = false;
                    NightmareBBInnocentJumpscare = false;
                    NightmareBBInActive.gameObject.SetActive(true);
                    NightmareBBActive.gameObject.SetActive(false);
                }
                else if (!NightmareBBStandingUp && NightmareBBInnocentJumpscare)
                {
                    StartJumpscare(NightmareBBJumpscareAddresses, 3, "NBB");
                }
            }
            yield return null;
        }
    }

    IEnumerator CanPhantomMangleIE()
    {
        yield return new WaitForSeconds(45f);
        canPhantomMangle = true;
    }

    IEnumerator CanPhantomBBIE()
    {
        yield return new WaitForSeconds(30f);
        canPhantomBB = true;
    }

    IEnumerator AftonLoop()
    {
        int i = 0;
        bool attacking = false;
        while (!attacking)
        {
            yield return new WaitForSeconds(29f);
            if (Random.value <= 0.25f)
            {
                AftonAttack.Play();
                StartCoroutine(FlickerOfficeLights(((130 - (currentAI[47]*3))/60), 0.07f, 0.7f));
                attacking = true;
            }
            yield return null;
        }
        while (attacking)
        {
            i++;
            if (i >= 130 - (currentAI[47]*3))
            {
                if (!sideVentClosed)
                {
                    StartJumpscare(AftonJumpscareAddresses, 5, "Afton");
                }
                else
                {
                    BlockAnimatronic();
                    yield break;
                }
            }
            yield return null;
        }
    }

    IEnumerator PhantomFreddyLoop()
    {
    while (true)
    {
        while (!flashlightActive)
    {
        yield return new WaitForSeconds(0.25f);

        // Reduce PhantomFreddyFade based on AI and currentDegrees
        PhantomFreddyFade -= (currentAI[16] / 5) + 1;
        if (currentDegrees == 120)
        {
            PhantomFreddyFade -= 5;
        }

        // Check if PhantomFreddyFade reaches the threshold for jumpscare
        if (PhantomFreddyFade <= 0 && !isJumpscaring)
        {
            PhantomFreddyJumpscare.color = new Color(1, 1, 1, 1);
            PhantomFreddyJumpscare.gameObject.SetActive(true);
            JumpscareAS.clip = load_audioClip(JumpscareSoundAdresses[2]);
            JumpscareAS.Play();
            Breathing.Play();
            camsActive = true;
            yield return new WaitForSeconds(1f / 60f);  // Simulate 1 frame delay
            camsActive = false;
            StartCoroutine(FadeOutPhantomBBJumpscare(PhantomFreddyJumpscare, 6f));
            PhantomFreddyFade = 1000;  // Reset PhantomFreddyFade to prevent immediate re-trigger
        }

        // Reverse the alpha for the color application (0 means fully visible, 255 means invisible)
        byte reversedAlpha = (byte)Mathf.Clamp(255 - PhantomFreddyFade, 0, 255);
        PhantomFreddyInOffice.color = new Color32(255, 255, 255, reversedAlpha);
        Debug.Log(PhantomFreddyFade);
        yield return null;
    }
    while (flashlightActive)
    {
        // Gradually increase PhantomFreddyFade when flashlight is active
        if (PhantomFreddyFade <= 400)
        {
            PhantomFreddyFade += 5;
        }

        // Reverse the alpha for the color application (0 means fully visible, 255 means invisible)
        byte reversedAlpha = (byte)Mathf.Clamp(255 - PhantomFreddyFade, 0, 255);
        PhantomFreddyInOffice.color = new Color32(255, 255, 255, reversedAlpha);
        Debug.Log(PhantomFreddyFade);
        yield return null;
    }
    }
    }

    public void ClickHelpy()
    {
        HelpyInOffice = false;
        HelpyLowerScreen.sprite = load_sprite(HelpyAddresses[1]);
        Helpy.clip = load_audioClip(HelpyAddresses[4]);
        Helpy.Play();
        StartCoroutine(WaitDeactivationHelpy());
        FazCoins++;
        FazCoinCounter.text = FazCoins.ToString();
    }

    IEnumerator WaitDeactivationHelpy()
    {
        yield return new WaitForSeconds(0.23f);
        HelpyLowerScreen.gameObject.SetActive(false);
    }

    IEnumerator HelpyInOfficeCoroutine()
    {
        int i = 0;
        while (HelpyInOffice)
        {
            i++;
            if (i >= 500 - (currentAI[32]*5) && !isJumpscaring)
            {
                if (camsActive)
                {
                    DeActivateCams();
                }
                HelpyInOffice = false;
                HelpyLowerScreen.gameObject.SetActive(false);
                HelpyJumpscare.gameObject.SetActive(true);
                Helpy.clip = load_audioClip(HelpyAddresses[3]);
                Helpy.Play();
                Sound += 2;
                MusicManAgitation += 2500;
                //LeftyAgitation += 500;
                yield return new WaitForSeconds(1.5f);
                Sound -= 2;
                HelpyJumpscare.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator EnnardLoop()
    {
        while (true)
        {

            if (!EnnardInOffice)
            {
                // Select a random path index
                int pathIndex = Random.Range(0, 3);

                // Move the MangleIcon along the selected path
                yield return StartCoroutine(MoveENAlongPath(pathIndex));
            }
            yield return null;
        }
    }

    private IEnumerator MoveENAlongPath(int pathIndex)
    {

    Vector3 resetPosition = EnnardIcon.position;

    // Select the path based on the index
    Transform[] selectedPath;

    switch (pathIndex)
    {
        case 0:
            selectedPath = ventPath1;
            break;
        case 1:
            selectedPath = ventPath2;
            break;
        case 2:
            selectedPath = ventPath3;
            break;
        default:
            yield break;
    }

    // Move along each transform in the selected path
    for (int i = 0; i < selectedPath.Length; i++)
    {
        Transform targetTransform = selectedPath[i];

        // Move towards the target transform
        while (Vector3.Distance(EnnardIcon.position, targetTransform.position) > 0.01f)
        {
            EnnardIcon.position = Vector3.MoveTowards(EnnardIcon.position, targetTransform.position, (PixelSteps * Time.deltaTime)+(currentAI[30]*0.01f));
            yield return null;
        }
    }

        if (!WitheredChicaStuck)
        {
            EnnardInOffice = true;
            EnnardIcon.gameObject.SetActive(false);
            StartCoroutine(ENInOfficeCoroutine(resetPosition));
            EnnardSqueack.Play();
        }
    }

    IEnumerator ENInOfficeCoroutine(Vector3 resetPosition)
    {
        int i = 0;
        while (EnnardInOffice)
        {
            i++;
            if (i >= 400 - (currentAI[14]*10) && !frontVentClosed)
            {
                if (camsActive)
                {
                    StartJumpscare(EnnardJumpscareAddresses, 4, "Ennard");
                    yield break;
                }
                else if (frontVentClosed)
                {
                    BlockAnimatronic();
                    EnnardIcon.position = resetPosition;
                    EnnardIcon.gameObject.SetActive(true);
                    EnnardInOffice = false;
                    yield break;
                }
            }
            else if (frontVentClosed)
            {
                BlockAnimatronic();
                MoltenFreddyIcon.position = resetPosition;
                MoltenFreddyIcon.gameObject.SetActive(true);
                MoltenFreddyInOffice = false;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator MoltenFreddyLoop()
    {
        while (true)
        {

            if (!MoltenFreddyInOffice)
            {
                // Select a random path index
                int pathIndex = Random.Range(0, 3);

                // Move the MangleIcon along the selected path
                yield return StartCoroutine(MoveMFAlongPath(pathIndex));
            }
            yield return null;
        }
    }

    private IEnumerator MoveMFAlongPath(int pathIndex)
    {

    Vector3 resetPosition = MoltenFreddyIcon.position;

    // Select the path based on the index
    Transform[] selectedPath;

    switch (pathIndex)
    {
        case 0:
            selectedPath = ventPath1;
            break;
        case 1:
            selectedPath = ventPath2;
            break;
        case 2:
            selectedPath = ventPath3;
            break;
        default:
            yield break;
    }

    // Move along each transform in the selected path
    for (int i = 0; i < selectedPath.Length; i++)
    {
        Transform targetTransform = selectedPath[i];

        // Move towards the target transform
        while (Vector3.Distance(MoltenFreddyIcon.position, targetTransform.position) > 0.01f)
        {
            MoltenFreddyIcon.position = Vector3.MoveTowards(MoltenFreddyIcon.position, targetTransform.position, (PixelSteps * Time.deltaTime)+(currentAI[45]*0.01f));
            yield return null;
        }
    }

        if (!WitheredChicaStuck)
        {
            MoltenFreddyInOffice = true;
            MoltenFreddyIcon.gameObject.SetActive(false);
            StartCoroutine(MFInOfficeCoroutine(resetPosition));
            MoltenFreddyLaugh.Play();
        }
    }

    IEnumerator MFInOfficeCoroutine(Vector3 resetPosition)
    {
        int i = 0;
        while (MoltenFreddyInOffice)
        {
            i++;
            if (i >= 400 - (currentAI[14]*10) && !frontVentClosed)
            {
                if (camsActive)
                {
                    StartJumpscare(MoltenFreddyJumpscareAddresses, 5, "MF");
                    yield break;
                }
                else if (frontVentClosed)
                {
                    BlockAnimatronic();
                    MoltenFreddyIcon.position = resetPosition;
                    MoltenFreddyIcon.gameObject.SetActive(true);
                    MoltenFreddyInOffice = false;
                    yield break;
                }
            }
            else if (frontVentClosed)
            {
                BlockAnimatronic();
                MoltenFreddyIcon.position = resetPosition;
                MoltenFreddyIcon.gameObject.SetActive(true);
                MoltenFreddyInOffice = false;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator SpringtrapLoop()
    {
        while (true)
        {

            if (!SpringtrapInOffice)
            {
                // Select a random path index
                int pathIndex = Random.Range(0, 3);

                // Move the MangleIcon along the selected path
                yield return StartCoroutine(MoveSpringtrapAlongPath(pathIndex));
            }
            yield return null;
        }
    }

    private IEnumerator MoveSpringtrapAlongPath(int pathIndex)
    {

    Vector3 resetPosition = SpringtrapIcon.position;

    // Select the path based on the index
    Transform[] selectedPath;

    switch (pathIndex)
    {
        case 0:
            selectedPath = ventPath1;
            break;
        case 1:
            selectedPath = ventPath2;
            break;
        case 2:
            selectedPath = ventPath3;
            break;
        default:
            yield break;
    }

    // Move along each transform in the selected path
    for (int i = 0; i < selectedPath.Length; i++)
    {
        Transform targetTransform = selectedPath[i];

        // Move towards the target transform
        while (Vector3.Distance(SpringtrapIcon.position, targetTransform.position) > 0.01f)
        {
            SpringtrapIcon.position = Vector3.MoveTowards(SpringtrapIcon.position, targetTransform.position, (PixelSteps * Time.deltaTime)+(currentAI[14]*0.01f));
            yield return null;
        }
    }

        if (!WitheredChicaStuck)
        {
            SpringtrapInOffice = true;
            SpringtrapIcon.gameObject.SetActive(false);
            StartCoroutine(SpringtrapInOfficeCoroutine(resetPosition));
            SpringtrapInVent.gameObject.SetActive(true);
        }
    }

    IEnumerator SpringtrapInOfficeCoroutine(Vector3 resetPosition)
    {
        int i = 0;
        while (SpringtrapInOffice)
        {
            i++;
            if (i >= 400 - (currentAI[14]*10) && !frontVentClosed)
            {
                if (camsActive)
                {
                    StartJumpscare(SpringtrapJumpscareAddresses, 2, "Springtrap");
                    yield break;
                }
                else if (frontVentClosed)
                {
                    BlockAnimatronic();
                    SpringtrapIcon.position = resetPosition;
                    SpringtrapIcon.gameObject.SetActive(true);
                    SpringtrapInOffice = false;
                    yield return new WaitForSeconds(0.12f);
                    SpringtrapInVent.gameObject.SetActive(false);
                    yield break;
                }
            }
            else if (frontVentClosed)
            {
                BlockAnimatronic();
                SpringtrapIcon.position = resetPosition;
                SpringtrapIcon.gameObject.SetActive(true);
                SpringtrapInOffice = false;
                yield return new WaitForSeconds(0.12f);
                SpringtrapInVent.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    public void ChangeWetSignSide()
    {
        if (WetFloorSignSide == 0)
        {
            WetFloorSignSide = 1;
            WetFloorSign.localPosition = WetFloorSignPos1;
            WetFloorSign.GetComponent<Image>().sprite = RightSideSprite;
        }
        else
        {
            WetFloorSignSide = 0;
            WetFloorSign.localPosition = WetFloorSignPos0;
            WetFloorSign.GetComponent<Image>().sprite = LeftSideSprite;
        }
    }

    IEnumerator RockstarChicaLoop()
    {
        float rcI = 0f;
        int rcH = 0;
        while (true)
        {
            if (camsActive && RockstarChicaCam == 0)
            {
                rcI += Time.deltaTime;
            }
            else if (RockstarChicaCam != 0)
            {
                rcI = 0f;
                rcH++;
            }
            if (rcI >= 10f)
            {
                rcI = 0f;
                if (MovementOppertunityC(currentAI[40], 30))
                {
                    int rndm = Random.Range(0,2);
                    if (rndm == 0)
                    {
                        if (currentCam != 1)
                        {
                            RockstarChicaCam = 1;
                        }
                        else
                        {
                            RockstarChicaCam = 2;
                        }
                    }
                    else
                    {
                        if (currentCam != 2)
                        {
                            RockstarChicaCam = 2;
                        }
                        else
                        {
                            RockstarChicaCam = 1;
                        }
                    }
                }
            }

            if (rcH >= 900)
            {
                if (RockstarChicaCam == 1 && !leftDoorClosed && WetFloorSignSide != 0 && camsActive)
                {
                    StartJumpscare(RockstarChicaJumpscareAddresses, 5, "Rockstar Chica");
                }
                else if (RockstarChicaCam == 1 && WetFloorSignSide == 0)
                {
                    RockstarChicaCam = 0;
                    BlockAnimatronic();
                    if (currentCam == 1 && camsActive) {SwitchCamera(1);}
                    rcI = 0f;
                    rcH = 0;
                }
                else if (RockstarChicaCam == 2 && !rightDoorClosed && WetFloorSignSide != 1 && camsActive)
                {
                    StartJumpscare(RockstarChicaJumpscareAddresses, 5, "Rockstar Chica");
                }
                else if (RockstarChicaCam == 2 && WetFloorSignSide == 1)
                {
                    RockstarChicaCam = 0;
                    BlockAnimatronic();
                    if (currentCam == 2 && camsActive) {SwitchCamera(2);}
                    rcI = 0f;
                    rcH = 0;
                }
            }
            yield return null;
        }
    }

    IEnumerator PhoneGuyLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            if (MovementOppertunityC(currentAI[49], 30) && !CallActive)
            {
                MuteCall.gameObject.SetActive(true);
                int rand = Random.Range(0,2);
                if (rand == 0)
                {
                    MuteButtonRB.AddForce(PositiveVelocity);
                }
                else
                {
                    MuteButtonRB.AddForce(NegativeVelocity);
                }
                CallActive = true;
                int rndm = Random.Range(0, 6);
                PhoneCall.clip = load_audioClip(PhoneCallAddresses[rndm]);
                PhoneCall.Play();
                Sound++;
                StartCoroutine(PhoneGuyTiming());
            }
            yield return null;
        }
    }

    public void MuteCallFunction()
    {
        MuteCall.gameObject.SetActive(false);
        CallActive = false;
        Sound--;
        PhoneCall.Stop();
    }

    IEnumerator PhoneGuyTiming()
    {
        int i = 0;
        while (CallActive)
        {
            i++;
            if (i >= (300 - (currentAI[49]*2)))
            {
                MuteCall.gameObject.SetActive(false);
                yield return new WaitForSeconds(PhoneCall.clip.length);
                CallActive = false;
                Sound--;
            }
            yield return null;
        }
    }

    IEnumerator NightmarionneLoop()
    {
    yield return new WaitForSeconds(2f);
    int lastSprite = 0;  // Track the last sprite set
    int curSprite = 0;
    float spriteChangeCooldown = 15f;  // 15 seconds cooldown
    float lastSpriteChangeTime = -spriteChangeCooldown;  // Initialize to allow immediate sprite change
    bool loopRunning = true;

    while (loopRunning)
    {
        Vector3 nightMarionneWP = new Vector3(Nightmarionette.transform.position.x - 200, Nightmarionette.transform.position.y - 120, Nightmarionette.transform.position.z);
        Nightmarionette.color = new Color(Nightmarionette.color.r, Nightmarionette.color.g, Nightmarionette.color.b, Mathf.Clamp(Nightmarionette.color.a, 0, 1));

        // Check if enough time has passed since the last sprite change
        if (Time.time - lastSpriteChangeTime >= spriteChangeCooldown && Nightmarionette.color.a == 0f)
        {
            // Determine which sprite to set based on the WP's X position
            if (nightMarionneWP.x >= 50 && nightMarionneWP.x <= 100)
            {
                curSprite = 4;  // Right sprite
            }
            else if (nightMarionneWP.x > 0 && nightMarionneWP.x < 50)
            {
                curSprite = 3;  // Middle-right sprite
            }
            else if (nightMarionneWP.x <= 0 && nightMarionneWP.x > -50)
            {
                curSprite = 2;  // Middle-left sprite
            }
            else if (nightMarionneWP.x <= -50 && nightMarionneWP.x >= -100)
            {
                curSprite = 1;  // Left sprite
            }

            // If the current sprite is different from the last sprite, update it
            if (curSprite != lastSprite && curSprite != 0)
            {
                lastSprite = curSprite;  // Update the last sprite set
                lastSpriteChangeTime = Time.time;  // Reset the cooldown timer

                switch (curSprite)
                {
                    case 1:
                        Nightmarionette.sprite = load_sprite(NightmarionetteAddresses[NMRightIndex]);
                        break;
                    case 2:
                        Nightmarionette.sprite = load_sprite(NightmarionetteAddresses[NMMiddleRightIndex]);
                        break;
                    case 3:
                        Nightmarionette.sprite = load_sprite(NightmarionetteAddresses[NMMiddleLeftIndex]);
                        break;
                    case 4:
                        Nightmarionette.sprite = load_sprite(NightmarionetteAddresses[NMLeftIndex]);
                        break;
                }
            }
        }

        // Adjust the alpha of Nightmarionette based on the corresponding range for the current sprite
        if (curSprite == 1 && nightMarionneWP.x <= -50 && nightMarionneWP.x >= -100 && !camsActive)
        {
            Nightmarionette.color += new Color(0, 0, 0, 0.004f + (currentAI[24]*0.0002f));
        }
        else if (curSprite == 2 && nightMarionneWP.x <= 0 && nightMarionneWP.x > -50 && !camsActive)
        {
            Nightmarionette.color += new Color(0, 0, 0, 0.004f + (currentAI[24]*0.0002f));
        }
        else if (curSprite == 3 && nightMarionneWP.x > 0 && nightMarionneWP.x < 50 && !camsActive)
        {
            Nightmarionette.color += new Color(0, 0, 0, 0.004f + (currentAI[24]*0.0002f));
        }
        else if (curSprite == 4 && nightMarionneWP.x >= 50 && nightMarionneWP.x <= 100 && !camsActive)
        {
            Nightmarionette.color += new Color(0, 0, 0, 0.004f + (currentAI[24]*0.0002f));
        }
        else
        {
            Nightmarionette.color -= new Color(0, 0, 0, 0.006f);
        }

        // Clamp the alpha value and check if it's reached 1
        Nightmarionette.color = new Color(Nightmarionette.color.r, Nightmarionette.color.g, Nightmarionette.color.b, Mathf.Clamp(Nightmarionette.color.a, 0, 1));

        // Break the loop if alpha reaches 1
        if (Nightmarionette.color.a == 1f)
        {
            // Trigger the event when alpha reaches 1
            // Add the action you want to perform here
            Nightmarionette.gameObject.SetActive(false);
            StartJumpscare(NightmarionetteJumpscareAddresses, 3, "NM");
            loopRunning = false;
        }

        yield return null;  // Continue to the next frame
    }
    }

    IEnumerator GoldenFreddyInOffice()
    {
        while (true)
        {
            while (goldenFreddyInOffice && !maskActive)
            {
                gfI++;
                DCoin.sprite = DCoinClickSprite;
                GoldenFreddy.gameObject.SetActive(true);

                if (gfI >= (80 - currentAI[13]))
                {
                    StartJumpscare(GoldenFreddyJumpscareAddress, 1, "Golden Freddy");
                    gfI = 0;  // Reset the counter after triggering the jumpscare
                }

                yield return null;  // Wait until the next frame
            }

            if (maskActive)
            {
                gfP = false;
                goldenFreddyInOffice = false;
            }

            GoldenFreddy.gameObject.SetActive(false);
            gfI = 0;  // Reset i when exiting the loop
            //DCoin.sprite = NormalDCoinSprite;
            yield return null;
        }
    }

    IEnumerator FoxyLoop()
    {
        int FoxyE = 0;
        while (currentAI[3] >= 1 && FoxyStage != 8)
        {
            FoxyH = Mathf.Clamp(FoxyH,0,3);
            FoxyE += (1 + FoxyH);
            if (FoxyE >= (1000 - (currentAI[3]*10)))
            {
                if (Random.value <=0.66f)
                {
                    if (camsActive && currentCam == 5)
                    {
                        if (FoxyStage < 5)
                        {
                            FoxyE = 0;
                            FoxyH = 0;
                            Debug.Log("Foxy Countered!");
                        }
                        else
                        {
                            FoxyStage++;
                            FoxyH++;
                            FoxyE = 0;
                        }
                    }
                    else
                    {
                        FoxyStage++;
                        FoxyH++;
                        FoxyE = 0;
                        Debug.Log("Foxy Progressed!");
                        switch (FoxyStage)
                        {
                            case 4:
                            if (camsActive)
                            {
                                FoxyLegs.gameObject.SetActive(true);
                            }
                            break;
                            case 5:
                            if (camsActive)
                            {
                                FoxyTorso.gameObject.SetActive(true);
                            }
                            break;
                            case 6:
                            if (camsActive)
                            {
                                FoxyHook.gameObject.SetActive(true);
                            }
                            break;
                            case 7:
                            if (camsActive)
                            {
                                FoxyArm2.gameObject.SetActive(true);
                            }
                            break;
                            case 8:
                            if (camsActive)
                            {
                                FoxyHead.gameObject.SetActive(true);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    FoxyE = 0;
                    Debug.Log("Foxy Unlucky!");
                }
            }
            while (camsActive && FoxyStage == 8)
            {
                if (Random.value <= 0.10f)
                {
                    DeActivateCams();
                    StartJumpscare(FoxyJumpscareAddresses, 0, "Foxy");
                    yield return null;
                }
            }
            if (FoxyStage >= 4 && camsActive)
            {
                FoxyLegs.gameObject.SetActive(true);
            }

            if (FoxyStage >= 5 && camsActive)
            {
                FoxyTorso.gameObject.SetActive(true);
            }

            if (FoxyStage >= 6 && camsActive)
            {
                FoxyHook.gameObject.SetActive(true);
            }

            if (FoxyStage >= 7 && camsActive)
            {
                FoxyArm2.gameObject.SetActive(true);
            }

            if (FoxyStage >= 8 && camsActive)
            {
                FoxyHead.gameObject.SetActive(true);
            }
            yield return null;
        }
    }

    IEnumerator FuntimeFoxyLoop()
    {
    while (currentAI[29] >= 1)
    {
        if (currentShowTime == 0)
        {
            // Set the next showtime randomly between 1 and 2 AM.
            int rndm = Random.Range(1, 4);
            currentShowTime = getCurrentAMTime() + rndm;
            AtWhatTimeIsShowtime.text = currentShowTime.ToString();
        }

        // Wait until the current time reaches the showtime.
        while (getCurrentAMTime() < currentShowTime)
        {
            yield return null; // Wait until the next frame before checking again
        }

        // Showtime has arrived, check the camera status.
        if (currentCam == 6 && camsActive)
        {
            currentShowTime = 0; // Reset showtime if the player is on the correct camera
        }
        else
        {
            // Start the jumpscare if conditions are not met
            if (currentAI[29] >= 1)
            {
                StartJumpscare(FuntimeFoxyJumpscareAddresses, 4, "Funtime Foxy");
            }
        }

        yield return new WaitForSeconds(1f); // Small delay to prevent rapid jumpscare triggering
    }
    }

    IEnumerator MangleInOfficeCoroutine()
    {
        while (true)
        {
            while (mangleisOffice && camsActive)
            {
                yield return new WaitForSeconds(1f);
                if (Random.value <= 1f / 30f)
                {
                    StartJumpscare(MangleJumpscareAddresses, 1, "Mangle");
                }
                yield return null;
            }
            yield return null;
        }
    }

    int getCurrentAMTime()
    {
        if (TimerTime >= 270f) // After 3:45 minutes (270s - 45s = 225s)
        {
            return 6;
        }
        else if (TimerTime >= 225f) // After 3:00 minutes (225s - 45s = 180s)
        {
            return 5;
        }
        else if (TimerTime >= 180f) // After 2:15 minutes (180s - 45s = 135s)
        {
            return 4;
        }
        else if (TimerTime >= 135f) // After 1:30 minutes (135s - 45s = 90s)
        {
            return 3;
        }
        else if (TimerTime >= 90f) // After 0:45 minutes (90s - 45s = 45s)
        {
            return 2;
        }
        else if (TimerTime >= 45f) // Starting from 0s to 45s, display 1 AM
        {
            return 1;
        }
        else if (TimerTime < 45f)
        {
            return 0;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator ElChipLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            if (!ElChipAdActive && MovementOppertunityC(currentAI[43], 29) && !isJumpscaring)
            {
                ElChipAdActive = true;
                int random = Random.Range(0,3);
                ElChipAdUpper.sprite = load_sprite(ElChipAddresses[random]);
                ElChipAdLower.sprite = load_sprite(ElChipAddresses[random]);
                ElChipAdUpper.gameObject.SetActive(true);
                ElChipAdLower.gameObject.SetActive(true);
            }
        }
    }

    public void SkipElChip()
    {
        ElChipAdActive = false;
        ElChipAdLower.gameObject.SetActive(false);
        ElChipAdUpper.gameObject.SetActive(false);
    }

    IEnumerator MangleLoop()
    {
        while (!mangleisOffice)
        {
            yield return new WaitForSeconds(5f);
            if (MovementOppertunity(currentAI[7]))
            {
                // Select a random path index
                int pathIndex = Random.Range(0, 3);

                // Move the MangleIcon along the selected path
                yield return StartCoroutine(MoveMangleAlongPath(pathIndex));
            }
        }
    }

    // Coroutine to move MangleIcon along the selected path
    private IEnumerator MoveMangleAlongPath(int pathIndex)
    {

    Vector3 resetPosition = MangleIcon.position;

    // Select the path based on the index
    Transform[] selectedPath;

    switch (pathIndex)
    {
        case 0:
            selectedPath = ventPath1;
            break;
        case 1:
            selectedPath = ventPath2;
            break;
        case 2:
            selectedPath = ventPath3;
            break;
        default:
            yield break;
    }

    // Move along each transform in the selected path
    for (int i = 0; i < selectedPath.Length; i++)
    {
        Transform targetTransform = selectedPath[i];

        // Move towards the target transform
        while (Vector3.Distance(MangleIcon.position, targetTransform.position) > 0.01f)
        {
            MangleIcon.position = Vector3.MoveTowards(MangleIcon.position, targetTransform.position, (PixelSteps * Time.deltaTime)+(currentAI[7]*0.01f));
            if (IsInSnareRange(MangleIcon))
            {
                // Reset Mangle to the start of the path or a reset position
                MangleIcon.position = resetPosition;
                BlockAnimatronic();
                yield break;
            }
            yield return null;
        }
    }

        mangleisOffice = true;
        MangleInOffice.gameObject.SetActive(true);
        Sound++;
        MangleIcon.gameObject.SetActive(false);
        StartCoroutine(MangleInOfficeCoroutine());
    }

    // Function to check if Mangle is within range of the active snare
    private bool IsInSnareRange(Transform t)
    {
    float range = 5f;

    switch (activeSnare)
    {
        case 1:
            return Vector3.Distance(t.position, Snare1.transform.position) <= range;
        case 2:
            return Vector3.Distance(t.position, Snare2.transform.position) <= range;
        case 3:
            return Vector3.Distance(t.position, Snare3.transform.position) <= range;
        default:
            return false;
    }
    }

    public void BuyDCoin()
    {
        if (FazCoins >= 10)
        {
            FazCoins -= 10;
            DCoinActive = true;
            DCoin.gameObject.SetActive(true);
            BuyDCoinB.SetActive(false);
            BuySFX.Play();
            FazCoinCounter.text = FazCoins.ToString();
        }
    }

    public void UseDCoin()
    {
        if (currentCam == 4 && currentAI[12] >= 1)
        {
            currentAI[12] = 0;
            Block.Play();
            DCoin.gameObject.SetActive(false);
        }
        else if (currentCam == 6 && currentAI[29] >= 1)
        {
            currentAI[29] = 0;
            Block.Play();
            DCoin.gameObject.SetActive(false);
            CameraMainBG.sprite = CamSprites[currentCam-1];
            StopCoroutine(FuntimeFoxyLoop());
            Showtime.SetActive(false);
            SwitchCamera(6);
        }
        else if (currentCam == 5)
        {
            if (currentAI[1] >= 1)
            {
                currentAI[1] = 0;
                Block.Play();
                DCoin.gameObject.SetActive(false);
            }
            if (currentAI[3] >= 1)
            {
                currentAI[3] = 0;
                Block.Play();
                DCoin.gameObject.SetActive(false);
                StopCoroutine(FoxyLoop());
            }
            SwitchCamera(5);
        }
        else if (goldenFreddyInOffice)
        {
            goldenFreddyInOffice = false;
            GoldenFreddy.gameObject.SetActive(false);
            DCoin.gameObject.SetActive(false);
            bool gf1 = false;
            int all0C = 0;
            for (int i = 0; i < currentAI.Length; i++)
            {
                if (i == 13 && currentAI[13] == 1)
                {
                    gf1 = true;
                }
                else if (i != 13 && currentAI[i] == 0)
                {
                    all0C++;
                }
            }
            if (all0C == 55 && gf1 == true)
            {
                StartJumpscare(FredbearJumpscareAddresses, 6, "Fredbear", 45, 1.5f);
            }
            else
            {
                currentAI[13] = 0;
                StopCoroutine(GoldenFreddyInOffice());
                Block.Play();
            }
        }
        else if (currentCam == 8 && currentAI[4] >= 1)
        {
            currentAI[4] = 0;
            StopCoroutine(ToyFreddyLoop());
            DCoin.gameObject.SetActive(false);
            SwitchCamera(8);
            Block.Play();
        }
        else if (currentCam == 3 && currentAI[48] >= 1)
        {
            currentAI[48] = 0;
            DCoin.gameObject.SetActive(false);
            SwitchCamera(3);
            Block.Play();
        }
    }

    IEnumerator FuntimeChicaLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(35f);
            
            if (MovementOppertunityC(currentAI[44], 30))
            {
                CanvasGroup mg = FuntimeChicaDistraction.GetComponent<CanvasGroup>();
                
                // Set the sprite and audio clip
                FuntimeChicaDistractionC.sprite = load_sprite(FuntimeChicaAddresses[Random.Range(7, 11)]);
                FuntimeChicaAS.clip = load_audioClip(FuntimeChicaAddresses[Random.Range(1, 6)]);
                
                // Play the flash effect and audio
                FuntimeChicaFlash.Play();
                FuntimeChicaAS.Play();
                
                // Set initial alpha to 1 (fully visible)
                mg.alpha = 1f;

                // Wait for 10 seconds, then start fading out
                float fadeDuration = 10f;
                float fadeSpeed = 1f / fadeDuration; // How much to fade per second
                float elapsedTime = 0f;

                while (elapsedTime < fadeDuration)
                {
                    elapsedTime += Time.deltaTime;
                    mg.alpha = Mathf.Lerp(1f, 0f, elapsedTime * fadeSpeed);
                    yield return null; // Continue every frame
                }

                // Ensure alpha is set to 0 at the end
                mg.alpha = 0f;
            }
        }
    }

    IEnumerator OldManConsequencesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.5f);

            if (MovementOppertunity(currentAI[26]))
            {
                // Activate the Fisher GameObject
                OldManConsequencesImages[1].gameObject.SetActive(true);
                OldManConsequencesImages[0].gameObject.SetActive(true); // Activate the Fish

                float startTime = Time.time;
                bool fishCaught = false;
                Vector3 startPosition = OldManConsequencesImages[0].transform.localPosition;

                // Fish movement loop for 4 seconds
                float moveDirection = 1f; // 1 for right, -1 for left
                float moveSpeed = 100f; // Adjust this to control speed

                while (Time.time - startTime < 4f)
                {
                    // Move fish continuously in the x direction
                    OldManConsequencesImages[0].transform.localPosition += new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);

                    // If the fish reaches the edge of the movement range, reverse direction
                    if (OldManConsequencesImages[0].transform.localPosition.x >= startPosition.x + 141.1f || 
                        OldManConsequencesImages[0].transform.localPosition.x <= startPosition.x - 0.01f)
                    {
                        moveDirection *= -1f;

                        // Flip fish scale to simulate turning around
                        Vector3 fishScale = OldManConsequencesImages[0].transform.localScale;
                        fishScale.x *= -1f;
                        OldManConsequencesImages[0].transform.localScale = fishScale;
                    }

                    // Check if the fish is within the catch range
                    if (Mathf.Abs(OldManConsequencesImages[0].transform.localPosition.x) <= 8.5f && 
                        Input.GetKeyDown(KeyCode.Y))
                    {
                        fishCaught = true;
                        OldManConsequencesSounds[1].Play();
                        bool gf1 = false;
                        int all0C = 0;
                        for (int i = 0; i < currentAI.Length; i++)
                        {
                            if (i == 26 && currentAI[26] == 1)
                            {
                                gf1 = true;
                            }
                            else if (i != 26 && currentAI[i] == 0)
                            {
                                all0C++;
                            }
                        }
                        if (all0C == 55 && gf1 == true)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene("OldManConsequences");
                        }
                        break;
                    }

                    yield return null; // Continue every frame without delay
                }

                if (!fishCaught)
                {
                    // Fish was not caught, show the error image for 7.5 seconds
                    OldManConsequencesImages[2].gameObject.SetActive(true);
                    MonitorBlocked = true;
                    if (camsActive) {DeActivateCams();}
                    OldManConsequencesSounds[2].Play();
                    OldManConsequencesImages[1].gameObject.SetActive(false);
                    yield return new WaitForSeconds(7.5f);
                    OldManConsequencesImages[2].gameObject.SetActive(false);

                    // Reset MonitorBlocked
                    MonitorBlocked = false;
                }

                // Reset the mini-game by deactivating the Fisher and Fish
                OldManConsequencesImages[1].gameObject.SetActive(false);
                OldManConsequencesImages[0].gameObject.SetActive(false);
                OldManConsequencesImages[0].transform.localPosition = startPosition;
                Vector3 fishScale2 = OldManConsequencesImages[0].transform.localScale;
                fishScale2.x = 0.5f;
                OldManConsequencesImages[0].transform.localScale = fishScale2;
            }
        }
    }


    bool MovementOppertunity(int AI)
    {
        int diceRoll = UnityEngine.Random.Range(0, 21);
        if (AI >= diceRoll && AI >= 1)
        {
            return true;
        }
        return false;
    }
    bool MovementOppertunityC(int AI, int Max)
    {
        int diceRoll = UnityEngine.Random.Range(0, Max);
        if (AI >= diceRoll && AI >= 1)
        {
            return true;
        }
        return false;
    }

    public void BlockAnimatronic()
    {
        Block.Play();
        FazCoins++;
        FazCoinCounter.text = FazCoins.ToString();
    }

    Sprite load_sprite(string target)
    {
        Sprite new_sprite = Resources.Load<Sprite>(target);
        return new_sprite;
    }

    AudioClip load_audioClip(string target)
    {
        AudioClip new_audioClip = Resources.Load<AudioClip>(target);
        return new_audioClip;
    }

    IEnumerator CandyCadetCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            if (Random.value <= 0.49f)
            {
                CandyCadet.Play("CandyCadetAction 0");
                CandyCadet.Play("CandyCadetAction");
                int random = Random.Range(0, CandyCadetVoiceLines.Length);
                CandyCadetVoiceLines[random].Play();
            }
        }
    }

    public void ChangeMusic()
    {
    // Stop the current music source if any
    if (MusicBoxSounds[currentMusicBoxIndex].isPlaying)
    {
        MusicBoxSounds[currentMusicBoxIndex].Stop();
        MusicBoxSounds[currentMusicBoxIndex].mute = true;
    }

    if (!ChicaHappy && !ChicaAngry)
    {
        ChicaHappy = true;
        ChicaPotsAndPans.mute = false;
    }
    else
    {
        ChicaAngry = true;
        ChicaPotsAndPans.mute = true;
    }

    // Increment the index to go to the next audio source
    currentMusicBoxIndex++;

    // If the index exceeds the length of the array, reset it to 0
    if (currentMusicBoxIndex >= MusicBoxSounds.Length)
    {
        currentMusicBoxIndex = 0;
    }

    // Start playing the next audio source
    MusicBoxSounds[currentMusicBoxIndex].Play();
    MusicBoxSounds[currentMusicBoxIndex].mute = false;
    }

    IEnumerator PowerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            PercentLeft -= GetPowerConsumptionRate();
            PercentText.text = ((int)PercentLeft).ToString();
            currentDegrees += currentDegreePerSecond;
            if (lastValidDegrees >= 60)
            {
                Debug.Log("Is Over 60!");
                lastValidDegrees = currentDegrees;

                currentDegrees = Mathf.Clamp(currentDegrees, 60f, 120);
            }
            else
            {
                Debug.Log("Is Under 60! CD: "+currentDegrees+" LVD");
                currentDegrees = Mathf.Clamp(currentDegrees, currentDegrees-currentDegreePerSecond, 60);
                lastValidDegrees = currentDegrees;
                Debug.Log("LVD: "+lastValidDegrees+" CD: "+currentDegrees);
            }
            //currentDegrees = Mathf.Clamp(currentDegrees, 60, 120);
            if (!isJumpscaring)
            {
                DegreeText.text = ((int)currentDegrees).ToString() + "°";
            }
            if (windingMusicBox)
            {
                mbProgress += 33.33f;
                WindMusicBox.Play();
                
            }
            else
            {
                if (currentAI[12] >= 1)
                {
                    mbProgress -= (currentAI[12]/10)+1;
                }
            }

            if (GBMActive)
            {
                mbProgress += 5;
            }
            mbProgress = Mathf.Clamp(mbProgress, 0, 100f);
            MusicBoxProgress.fillAmount = mbProgress/100;
            if (mbProgress == 0)
            {
                if (currentAI[12] >= 1) {puppetPrepared = true;}
            }
            if (PercentLeft <= 0)
            {
                PowerOut.Play();
                MainBG.GetComponent<Image>().sprite = OfficePowerOut;
                StartCoroutine(PowerOutCoroutine());
                StopCoroutine(VentilationCycle());
                yield break;
            }
        }
    }

    IEnumerator PowerOutCoroutine()
    {
        LowerUI.SetActive(false);
        UpperUI.SetActive(false);
        if (frontVentClosed)
        {
            FrontVent();
        }
        if (leftDoorClosed)
        {
            LeftDoor();
        }
        if (rightDoorClosed)
        {
            RightDoor();
        }
        if (sideVentClosed)
        {
            SideVent();
        }
        if (FanOn)
        {
            ToggleDeskFan();
        }
        if (camsActive)
        {
            DeActivateCams();
        }
        if (flashlightActive)
        {
            Flashlight.SetActive(false);
            FlashClick.Play();
        }
        SwitchCurrentVentilatorState(0);
        float duration = 7.0f; // Duration of fade-out effect
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);

            foreach (CanvasGroup group in Groups)
            {
                group.alpha = 0f + alpha; // Fade out effect
            }
            yield return null;
        }
        camsActive = true;
    }

    private void LoadAILevels()
    {
        if (DataManager.ValueExists("currentAI", "data:/"))
        {
            currentAI = DataManager.GetValue<int[]>("currentAI", "data:/");
        }
        else
        {
            for (int i = 0; i < currentAI.Length; i++)
            {
                currentAI[i] = 0;
            }
        }
    }

    void Update()
    {
        UpdateTimerUI(TimerTime);
        if (!isJumpscaring)
        {
            TimerTime += Time.deltaTime;
        }
        if (PercentLeft <= 0)
            return;
        if (isJumpscaring)
            return;
        if (Input.GetKeyUp(KeyCode.A) && !camsActive && !maskActive && flashlightActive)
        {
            FlashClick.Play();
            Usage--;
        }
        MoveCameraMainBG(cameraMoveSpeed);
        // Toggle cameras on/off with Numpad2
        if (Input.GetKeyDown(KeyCode.Keypad2) && !maskActive && !MonitorBlocked && !flashlightActive)
        {
            if (!camsActive) 
            {
                ActivateCams();
            }
            else 
            {
                DeActivateCams();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) && !maskActive && MonitorBlocked && !flashlightActive)
        {
            ErrorSound.Play();
        }

        // Toggle mask with Numpad8
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Mask();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !JJActive)
        {
            LeftDoor();
            DoorSFX.Play();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && JJActive)
        {
            ErrorSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !JJActive)
        {
            RightDoor();
            DoorSFX.Play();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && JJActive)
        {
            ErrorSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !JJActive)
        {
            SideVent();
            DoorSFX.Play();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && JJActive)
        {
            ErrorSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !JJActive)
        {
            FrontVent();
            DoorSFX.Play();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && JJActive)
        {
            ErrorSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleDeskFan();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (FanOn) {ToggleDeskFan();}
            SwitchCurrentVentilatorState(0);
        }
        if (Input.GetKey(KeyCode.A) && !camsActive && !maskActive)
        {
            if (!BBActive)
            {
                flashlightActive = true;
                Flashlight.SetActive(true);
            }
        }
        else
        {
            if (!BBActive)
            {
                flashlightActive = false;
                Flashlight.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.A) && !camsActive && !maskActive && !BBActive)
        {
            FlashClick.Play();
            Usage++;
        }
        else if (BBActive && Input.GetKeyDown(KeyCode.A) && !camsActive && !maskActive)
        {
            ErrorSound.Play();
        }

        // Move MainBG left with Numpad4
        if (Input.GetKey(KeyCode.Keypad4))
        {
            MoveMainBG(-moveSpeed);
        }

        // Move MainBG right with Numpad6
        if (Input.GetKey(KeyCode.Keypad6))
        {
            MoveMainBG(moveSpeed);
        }

        if (Input.GetKeyDown(KeyCode.L) && camsActive)
        {
            int switchTo = currentCam-1;
            if (switchTo <= 0)
            {
                switchTo = 8;
            }
            SwitchCamera(switchTo);
        }
        if (Input.GetKeyDown(KeyCode.R) && camsActive)
        {
            int switchTo = currentCam+1;
            if (switchTo >= 9)
            {
                switchTo = 1;
            }
            SwitchCamera(switchTo);
        }
        if (Input.GetKey(KeyCode.Return))
        {
            startTimer += Time.deltaTime;
            if (startTimer >= 2f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuLoader");
            }
        }
        else if (Input.GetKey(KeyCode.RightAlt))
        {
            startTimer += Time.deltaTime;
            if (startTimer >= 3f)
            {
                AMText.text = "6";
            if (!isLoadingWin)
            {
                isLoadingWin = true;
                int animatronicsAt20 = 0;
                foreach (int i in currentAI)
                {
                    if (i == 20)
                    {
                        animatronicsAt20++;
                    }
                }
                if (animatronicsAt20 >= 50)
                {
                    DataManager.SaveValue<float>("Best5020Time", TimerTime, "data:/");
                }
                int score = 0;
                for (int i = 0; i < currentAI.Length; i++)
                {
                    score += (currentAI[i]*10);
                }
                DataManager.SaveValue<int>("LastWonScore", score, "data:/");
                SceneManager.LoadScene("WinNightLoader");
            }
            }
        }
        else
        {
            startTimer = 0f;
        }

        // Update usage and sound blocks based on the current usage and sound values
        UpdateUsageBlocks();
        UpdateSoundBlocks();
    }

    private void UpdateTimerUI(float time)
    {    
    // Convert time to minutes, seconds, and milliseconds
    int minutes = Mathf.FloorToInt(time / 60f);
    int seconds = Mathf.FloorToInt(time % 60f);
    int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f / 10);

    // Update the Timer UI with the formatted time
    Timer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

    // Calculate the current hour based on the elapsed time (assuming the game goes for 4:30 minutes)
    float totalGameTime = 270f; // 4 minutes 30 seconds = 270 seconds
        if (time >= 270f) // After 3:45 minutes (270s - 45s = 225s)
        {
            AMText.text = "6";
            if (!isLoadingWin)
            {
                isLoadingWin = true;
                int animatronicsAt20 = 0;
                foreach (int i in currentAI)
                {
                    if (i == 20)
                    {
                        animatronicsAt20++;
                    }
                }
                if (animatronicsAt20 >= 50)
                {
                    DataManager.SaveValue<float>("Best5020Time", TimerTime, "data:/");
                }
                int score = 0;
                for (int i = 0; i < currentAI.Length; i++)
                {
                    score += (currentAI[i]*10);
                }
                DataManager.SaveValue<int>("LastWonScore", score, "data:/");
                SceneManager.LoadScene("WinNightLoader");
            }
        }
        else if (time >= 225f) // After 3:00 minutes (225s - 45s = 180s)
        {
            if (AMText.text != "5")
            {
                AmChange();
            }
            AMText.text = "5";
        }
        else if (time >= 180f) // After 2:15 minutes (180s - 45s = 135s)
        {
            if (AMText.text != "4")
            {
                AmChange();
            }
            AMText.text = "4";
        }
        else if (time >= 135f) // After 1:30 minutes (135s - 45s = 90s)
        {
            if (AMText.text != "3")
            {
                AmChange();
            }
            AMText.text = "3";
        }
        else if (time >= 90f) // After 0:45 minutes (90s - 45s = 45s)
        {
            if (AMText.text != "2")
            {
                AmChange();
            }
            AMText.text = "2";
        }
        else if (time >= 45f) // Starting from 0s to 45s, display 1 AM
        {
            if (AMText.text != "1")
            {
                AmChange();
            }
            AMText.text = "1";
        }
    }

    void AmChange()
    {
        Resources.UnloadUnusedAssets();
		System.GC.WaitForPendingFinalizers();
        System.GC.Collect();
        if (DeeDeeEnabled)
        {
            if (UnityEngine.Random.value <= DeeDeeChance)
            {
                StartCoroutine(DeeDeeSequence());
            }
        }
        else
        {
            if (is5020)
            {
                if (UnityEngine.Random.value <= DeeDeeChance)
                {
                    StartCoroutine(DeeDeeSequence());
                }
            }
        }
    }

    IEnumerator DeeDeeSequence()
    {
        bool isXOR = false;
        if (UnityEngine.Random.value > additionalXORChance && !is5020)
        {
            DeeDeeAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(DeeDeeAddress);
            StartCoroutine(MoveTransform(DeeDeeTransform, new Vector3(DeeDeeTransform.localPosition.x, -120f, DeeDeeTransform.localPosition.z), 1f));
            DeeDeeSoundAS2.clip = (Resources.Load<AudioClip>(DeeDeeSoundAddress[UnityEngine.Random.Range(0,2)]));
            DeeDeeSoundAS2.Play();
        }
        else if (!XORHappened)
        {
            DeeDeeAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(XORAddress);
            StartCoroutine(MoveTransform(DeeDeeTransform, new Vector3(DeeDeeTransform.localPosition.x, -120f, DeeDeeTransform.localPosition.z), 1f));
            DeeDeeSoundAS.clip = (Resources.Load<AudioClip>(XORSoundAddress));
            DeeDeeSoundAS.Play();
            isXOR = true;
            XORHappened = true;
        }
        yield return new WaitForSeconds(12.5f);
        DeeDeeSoundAS.Stop();
        StartCoroutine(MoveTransform(DeeDeeTransform, new Vector3(DeeDeeTransform.localPosition.x, -280f, DeeDeeTransform.localPosition.z), 1f));
        if (!isXOR)
        {
            if (UnityEngine.Random.value >= 0.1f)
            {
                List<int> animatronicsBelow5 = new List<int>();
                for (int i = 0; i < currentAI.Length; i++)
                {
                    if (currentAI[i] < 5)
                    {
                        animatronicsBelow5.Add(i);
                    }
                }

                if (animatronicsBelow5.Count == 0)
                    yield break;

                int selectedAnimatronic = animatronicsBelow5[Random.Range(0, animatronicsBelow5.Count)];
                if (DeeDeeForceCharacter != -1)
                {
                    selectedAnimatronic = DeeDeeForceCharacter;
                }

                bool was0 = currentAI[selectedAnimatronic] == 0;
                currentAI[selectedAnimatronic] = Mathf.Clamp(currentAI[selectedAnimatronic] + Random.Range(5, 11), 0, 20);
                UpdateAnimatronicLevel(selectedAnimatronic, was0, false);

                StartCoroutine(DisplayNewChallenger());
            }
        }
        else
        {
            StartCoroutine(DisplayNewChallenger());
            for (int i = 50; i < 56; i++)
            {
                float delay = GetDelayForAnimatronic(i);
                StartCoroutine(HandleAnimatronicDelay(i, delay));
            }
        }
    }

    float GetDelayForAnimatronic(int animatronic)
    {
        switch (animatronic)
        {
            case 50: return 14f;
            case 51: return 24f;
            case 52: return 0f;
            case 53: return 21f;
            case 54: return 35f;
            case 55: return 7f;
            default: return 0f;
        }
    }

    IEnumerator HandleAnimatronicDelay(int animatronic, float delay)
    {
        yield return new WaitForSeconds(delay);

        bool was0 = currentAI[animatronic] <= 0;
        currentAI[animatronic] = 10;
        UpdateAnimatronicLevel(animatronic, was0, true);
    }

    void UpdateAnimatronicLevel(int index, bool was0, bool isXor)
    {
        if (currentAI[26] >= 1 && was0 && index == 26)
        {
            OldManConsequencesImages[0].sprite = load_sprite(OldManConsequencesAdresses[0]);
            OldManConsequencesImages[1].sprite = load_sprite(OldManConsequencesAdresses[1]);
            OldManConsequencesImages[2].sprite = load_sprite(OldManConsequencesAdresses[2]);
            OldManConsequencesSounds[0].clip = load_audioClip(OldManConsequencesAdresses[3]);
            OldManConsequencesSounds[1].clip = load_audioClip(OldManConsequencesAdresses[4]);
            OldManConsequencesSounds[2].clip = load_audioClip(OldManConsequencesAdresses[5]);
            StartCoroutine(OldManConsequencesLoop());
        }
        if (currentAI[44] >= 1 && was0 && index == 44)
        {
            FuntimeChicaDistraction.sprite = load_sprite(FuntimeChicaAddresses[6]);
            FuntimeChicaDistractionC.sprite = load_sprite(FuntimeChicaAddresses[7]);
            FuntimeChicaFlash.clip = load_audioClip(FuntimeChicaAddresses[0]);
            FuntimeChicaAS.clip = load_audioClip(FuntimeChicaAddresses[1]);
            StartCoroutine(FuntimeChicaLoop());
        }
        if (currentAI[43] >= 1 && was0 && index == 43)
        {
            ElChipAdSound.clip = load_audioClip(ElChipAddresses[4]);
            ElChipSkip.sprite = load_sprite(ElChipAddresses[3]);
            StartCoroutine(ElChipLoop());
        }
        if (currentAI[7] >= 1 && was0 && index == 7)
        {
            MangleInOfficeAS.clip = load_audioClip(MangleAddresses[2]);
            MangleInOffice.sprite = load_sprite(MangleAddresses[1]);
            MangleIcon.GetComponent<Image>().sprite = load_sprite(MangleAddresses[0]);
            MangleIcon.gameObject.SetActive(true);
            StartCoroutine(MangleLoop());
            MangleIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[29] >= 1 && was0 && index == 29)
        {
            StartCoroutine(FuntimeFoxyLoop());
        }
        if (currentAI[3] >= 1 && was0 && index == 3)
        {
            CurrentFigurine.sprite = load_sprite(FoxyAddresses[0]);
            CurrentFigurine.gameObject.SetActive(true);
            FoxyLegs.sprite = load_sprite(FoxyAddresses[1]);
            FoxyTorso.sprite = load_sprite(FoxyAddresses[2]);
            FoxyArm2.sprite = load_sprite(FoxyAddresses[4]);
            FoxyHook.sprite = load_sprite(FoxyAddresses[3]);
            FoxyHead.sprite = load_sprite(FoxyAddresses[5]);
            StartCoroutine(FoxyLoop());
            activeFigurine = 0;
        }
        if (currentAI[1] >= 1 && was0 && index == 1)
        {
            CurrentFigurine.sprite = load_sprite(BonnieAddresses[0]);
            CurrentFigurine.gameObject.SetActive(true);
            activeFigurine = 1;
            BonnieJumpscare.clip = load_audioClip(BonnieAddresses[3]);
        }
        if (currentAI[1] >= 1 && currentAI[3] >= 1 && was0 && (index == 1 || index == 3))
        {
            int DiceRoll = Random.Range(0,2);
            activeFigurine = DiceRoll;
            if (activeFigurine == 0)
            {
                CurrentFigurine.sprite = load_sprite(FoxyAddresses[0]);
                CurrentFigurine.gameObject.SetActive(true);
            }
            else
            {
                CurrentFigurine.sprite = load_sprite(BonnieAddresses[0]);
                CurrentFigurine.gameObject.SetActive(true);
            }
            StartCoroutine(FigurineLoop());
        }
        if (currentAI[13] >= 1 && was0 && index == 13)
        {
            GoldenFreddy.sprite = load_sprite(GoldenFreddyAddress);
            StartCoroutine(GoldenFreddyInOffice());
        }
        if (currentAI[24] >= 1 && was0 && index == 24)
        {
            StartCoroutine(NightmarionneLoop());
        }
        if (currentAI[49] >= 1 && was0 && index == 49)
        {
            MuteCall.sprite = load_sprite(MuteButtonAddress);
            StartCoroutine(PhoneGuyLoop());
        }
        if (currentAI[40] >= 1 && was0 && index == 40)
        {
            RockstarChicaLeft.sprite = load_sprite(RockstarChicaAddresses[0]);
            RockstarChicaRight.sprite = load_sprite(RockstarChicaAddresses[1]);
            StartCoroutine(RockstarChicaLoop());
        }
        if (currentAI[14] >= 1 && was0 && index == 14)
        {
            SpringtrapInVent.sprite = load_sprite(SpringtrapAddresses[1]);
            SpringtrapIcon.GetComponent<Image>().sprite = load_sprite(SpringtrapAddresses[0]);
            SpringtrapIcon.gameObject.SetActive(true);
            StartCoroutine(SpringtrapLoop());
            SpringtrapIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[45] >= 1 && was0 && index == 45)
        {
            MoltenFreddyIcon.GetComponent<Image>().sprite = load_sprite(MoltenFreddyAddresses[0]);
            MoltenFreddyIcon.gameObject.SetActive(true);
            StartCoroutine(MoltenFreddyLoop());
            MoltenFreddyLaugh.clip = load_audioClip(MoltenFreddyAddresses[1]);
            MoltenFreddyIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[30] >= 1 && was0 && index == 30)
        {
            EnnardIcon.GetComponent<Image>().sprite = load_sprite(EnnardAddresses[0]);
            EnnardIcon.gameObject.SetActive(true);
            StartCoroutine(EnnardLoop());
            EnnardSqueack.clip = load_audioClip(EnnardAddresses[1]);
            EnnardIcon.GetComponent<Image>().preserveAspect = true;
        }
        if (currentAI[31] >= 1 && was0 && index == 31)
        {
            CrateSmall.sprite = load_sprite(TatGAddresses[0]);
            TatGJumpscare.clip = load_audioClip(TatGAddresses[2]);
            MrCanDo.sprite = load_sprite(TatGAddresses[1]);
        }
        if (currentAI[32] >= 1 && was0 && index == 32)
        {
            HelpyLowerScreen.sprite = load_sprite(HelpyAddresses[0]);
            HelpyJumpscare.sprite = load_sprite(HelpyAddresses[2]);
            Helpy.clip = load_audioClip(HelpyAddresses[4]);
        }
        if (currentAI[17] >= 1 && was0 && index == 17)
        {
            Breathing.clip = load_audioClip(BreathingAddress);
            PhantomBBOnCams.sprite = load_sprite(PhantomBBAddresses[0]);
            PhantomBBJumpscare.sprite = load_sprite(PhantomBBAddresses[1]);
        }
        if (currentAI[15] >= 1 && was0 && index == 15)
        {
            PhantomMangleScream.clip = load_audioClip(PhantomMangleAddresses[2]);
            PhantomMangleOnCamsImg.sprite = load_sprite(PhantomMangleAddresses[0]);
            PhantomMangleInOfficeImg.sprite = load_sprite(PhantomMangleAddresses[1]);
        }
        if (currentAI[16] >= 1 && was0 && index == 16)
        {
            Breathing.clip = load_audioClip(BreathingAddress);
            PhantomFreddyInOffice.sprite = load_sprite(PhantomFreddyAddresses[0]);
            PhantomFreddyJumpscare.sprite = load_sprite(PhantomFreddyAddresses[1]);
            StartCoroutine(PhantomFreddyLoop());
        }
        if (currentAI[47] >= 1 && was0 && index == 47)
        {
            AftonAttack.clip = load_audioClip(AftonAttackAddress);
            StartCoroutine(AftonLoop());
        }
        if (currentAI[25] >= 1 && was0 && index == 25)
        {
            NightmareBBInActive.sprite = load_sprite(NightmareBBAddresses[0]);
            NightmareBBActive.sprite = load_sprite(NightmareBBAddresses[1]);
            NightmareBBInActive.gameObject.SetActive(true);
            StartCoroutine(NightmareBBLoop());
        }
        if (currentAI[28] >= 1 && was0 && index == 28)
        {
            BalloraMusic.clip = load_audioClip(BalloraAddress);
            BalloraMusic.mute = true;
            BalloraMusic.Play();
            StartCoroutine(BalloraLoop());
        }
        if (currentAI[38] >= 1 && was0 && index == 38)
        {
            Deposit5Coins.GetComponent<Image>().sprite = load_sprite(RockstarFreddyAddresses[9]);
            RockstarFreddy.sprite = load_sprite(RockstarFreddyAddresses[0]);
            RockstarFreddy.gameObject.SetActive(true);
            RFRCoroutine = StartCoroutine(RockstarFreddyLoop());
            StartCoroutine(UseHeaterToRFR());
        }
        if (currentAI[4] >= 1 && was0 && index == 4)
        {
            StartCoroutine(ToyFreddyLoop());
        }
        if (currentAI[10] >= 1 && was0 && index == 10)
        {
            WitheredChicaIcon.GetComponent<Image>().sprite = load_sprite(WitheredChicaAddresses[0]);
            WitheredChicaIcon.GetComponent<Image>().preserveAspect = true;
            WitheredChicaInOffice.sprite = load_sprite(WitheredChicaAddresses[1]);
            WitheredChicaIcon.gameObject.SetActive(true);
            StartCoroutine(WitheredChicaLoop());
        }
        if (currentAI[20] >= 1 && was0 && index == 20)
        {
            FredbearEyes.sprite = load_sprite(NFAddresses[1]);
            StartCoroutine(NightmareFredbearLoop());
        }
        if (currentAI[21] >= 1 && was0 && index == 21)
        {
            NightmareEyes.sprite = load_sprite(NFAddresses[0]);
            StartCoroutine(NightmareLoop());
        }
        if (currentAI[22] >= 1 && was0 && index == 22)
        {
            JackOChicaLeft.sprite = load_sprite(JackOChicaAddress);
            JackOChicaRight.sprite = load_sprite(JackOChicaAddress);
            StartCoroutine(JackOChicaLoop());
        }
            if (currentAI[23] >= 1)
            {
                NightmareMangleBuy.SetActive(true);
            }
            if (currentAI[27] >= 1)
            {
                BabyBuy.SetActive(true);
            }
            if (currentAI[19] >= 1)
            {
                NightmareBonnieBuy.SetActive(true);
            }

            if (!PlushieCoroutineActive)
            {
                StartCoroutine(PlushieCoroutine());
                PlushieCoroutineActive = true;
            }
        if (currentAI[33] >= 1 && was0 && index == 33)
        {
            HappyFrogDC.currentAILevel = currentAI[33];
            HappyFrogDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[34] >= 1 && was0 && index == 34)
        {
            MrHippoDC.currentAILevel = currentAI[34];
            MrHippoDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[35] >= 1 && was0 && index == 35)
        {
            PigpatchDC.currentAILevel = currentAI[35];
            PigpatchDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[36] >= 1 && was0 && index == 36)
        {
            NeddBearDC.currentAILevel = currentAI[36];
            NeddBearDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[37] >= 1 && was0 && index == 37)
        {
            OrvilleDC.currentAILevel = currentAI[37];
            OrvilleDC.toTransport.gameObject.SetActive(true);
        }
        if (currentAI[42] >= 1 && was0 && index == 42)
        {
            MusicManCymbals.clip = load_audioClip(MusicManAddress);
            StartCoroutine(MusicManLoop());
            StartCoroutine(MusicManCymbalsCoroutine());
        }
        if (currentAI[5] >= 1 && was0 && index == 5)
        {
            StareAS.clip = load_audioClip(StareAddress);
            ToyBonnieAnimator.runtimeAnimatorController = Resources.Load(ToyBonnieAnimatorControllerAddress) as RuntimeAnimatorController;
            StartCoroutine(ToyBonnieLoop());
        }
        if (currentAI[6] >= 1 && was0 && index == 6)
        {
            StareAS.clip = load_audioClip(StareAddress);
            ToyChicaAnimator.runtimeAnimatorController = Resources.Load(ToyChicaAnimatorControllerAddress) as RuntimeAnimatorController;
            StartCoroutine(ToyChicaLoop());
        }
        if (currentAI[48] >= 1 && was0 && index == 48)
        {
            StartCoroutine(LeftyLoop());
        }
        if (currentAI[8] >= 1 && was0 && index == 8)
        {
            BBInOffice.sprite = load_sprite(BBAddresses[1]);
            BBInVent.sprite = load_sprite(BBAddresses[0]);
            BBLaugh.clip = load_audioClip(BBAddresses[2]);
        }
        if (currentAI[9] >= 1 && was0 && index == 9)
        {
            JJInOffice.sprite = load_sprite(JJAddresses[1]);
            JJInVent.sprite = load_sprite(JJAddresses[0]);
        }
        if (currentAI[2] >= 1 && was0 && index == 2)
        {
            ChicaPotsAndPans.clip = load_audioClip(PotsAndPantsAddress);
            ChicaPotsAndPans.Play();
            ChicaPotsAndPans.mute = false;
            ChicaHappy = true;
            StartCoroutine(ChicaLoop());
        }
        if (currentAI[0] >= 1 && was0 && index == 0)
        {
            FreddyStage1.sprite = load_sprite(FreddyAddresses[0]);
            FreddyStage2.sprite = load_sprite(FreddyAddresses[1]);
            FreddyStage3.sprite = load_sprite(FreddyAddresses[2]);
            FreddyStage4.sprite = load_sprite(FreddyAddresses[3]);
            StartCoroutine(FreddyLoop());
        }
        if (currentAI[46] >= 1 && was0 && index == 46)
        {
            ScrapbabyInOffice.sprite = load_sprite(ScrapbabyAddresses[0]);
            ShockPanel.sprite = load_sprite(ScrapbabyAddresses[2]);
            Shock.clip = load_audioClip(ScrapbabyAddresses[3]);
            StartCoroutine(ScrapBabyLoop());
        }
        if (currentAI[11] >= 1 && was0 && index == 11)
        {
            StareAS.clip = load_audioClip(StareAddress);
            ToiletBowlBonnieInOffice.sprite = load_sprite(ToiletBowlBonnieAddress);
            StartCoroutine(ToiletBowlBonnieLoop());
        }
        if (currentAI[18] >= 1 && was0 && index == 18)
        {
            Freddles[0].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[0]) as RuntimeAnimatorController;
            Freddles[1].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[1]) as RuntimeAnimatorController;
            Freddles[2].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[2]) as RuntimeAnimatorController;
            Freddles[3].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[3]) as RuntimeAnimatorController;
            Freddles[4].GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[4]) as RuntimeAnimatorController;
            FreddlesSounds.clip = load_audioClip(NightmareFreddyAddresses[5]);
            FreddleSmoke.GetComponent<Animator>().runtimeAnimatorController = Resources.Load(NightmareFreddyAddresses[6]) as RuntimeAnimatorController;
            StartCoroutine(NightmareFreddyLoop());
            for (int i = 0; i < Freddles.Length; i++)
            {
                if (office == 0)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice0[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice0[i].rotation;
                }
                if (office == 1)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice1[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice1[i].rotation;
                }
                if (office == 2)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice2[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice2[i].rotation;
                }
                if (office == 3)
                {
                    Freddles[i].transform.position = FreddlePositionsOffice3[i].position;
                    Freddles[i].transform.rotation = FreddlePositionsOffice3[i].rotation;
                }
            }
            FreddlesSounds.Play();
        }
        if (currentAI[39] >= 1 && was0 && index == 39)
        {
            RockstarBonnie.sprite = load_sprite(RockstarBonnieAddresses[0]);
            RockstarBonnieGuitar.sprite = load_sprite(RockstarBonnieAddresses[1]);
            StartCoroutine(RockstarBonnieLoop());
        }
        if (currentAI[50] >= 1 && was0 && index == 50)
        {
            PlushtrapState = 0;
            StartCoroutine(PlushtrapLoop());
        }  
        if (currentAI[51] >= 1 && was0 && index == 51)
        {
            ActivateBonnet();
        }
        if (currentAI[52] >= 1 && was0 && index == 52)
        {
            StartCoroutine(EnableLOLBit(isXor));
        }
        if (currentAI[53] >= 1 && was0 && index == 53)
        {
            StartCoroutine(NightmareChicaLoop());
            Debug.Log("Uh Huh");
        }  
        if (currentAI[54] >= 1 && was0 && index == 54)
        {
            StartCoroutine(ActivateMinireenas());
        }
        if (currentAI[55] >= 1 && was0 && index == 55)
        {
            StartCoroutine(ActivateRWQ());
        }
    }

    IEnumerator PlushtrapLoop()
    {
        int i = 0;
        int frameCount = Random.Range(700, 1199);
        while (i < frameCount)
        {
            i++;
            yield return null;
        }
        if (PlushtrapState == 0)
        StartJumpscare(PlushtrapJumpscareAddress, 0, "Plushtrap");
    }

    IEnumerator DisplayNewChallenger()
    {
        NewChallengerAnimator.gameObject.SetActive(true);
        NewChallengerAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(NewChallengerAddress);
        DeeDeeSoundAS.PlayOneShot(Resources.Load<AudioClip>(NewChallengerAudioAddress));
        yield return new WaitForSeconds(2.5f);
        NewChallengerAnimator.gameObject.SetActive(false);
    }

    public IEnumerator MoveTransform(Transform target, Vector3 destination, float duration)
    {
        Vector3 startPosition = target.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            target.localPosition = Vector3.Lerp(startPosition, destination, progress);
            yield return null;
        }

        target.localPosition = destination;
    }

    private float GetPowerConsumptionRate()
    {
    int usageIndex = Mathf.Clamp(Usage + 1, 0, powerConsumptionRates.Length - 1);
    return powerConsumptionRates[usageIndex];
    }

    void Mask()
    {
        if (camsActive)
            return;
        maskActive = !maskActive;
        if (maskActive) { MaskAnimator.Play("MaskOn"); MaskOn.Play(); } else { MaskAnimator.Play("MaskOff"); MaskOff.Play(); }
    }

    void ToggleDeskFan()
    {
        FanOn = !FanOn;
        if (FanOn) {FanAnimator.speed = 1; Fan.Play(); Usage++; Sound++; currentDegreePerSecond += FanDegreePerSecond;} else {FanAnimator.speed = 0; Fan.Pause(); Usage--; Sound--; currentDegreePerSecond -= FanDegreePerSecond;}
    }

    void LeftDoor()
    {
        if (!leftDoorClosed)
        {
            leftDoorClosed = true;
            LeftDoorAnimator.Play("LeftDoorClose");
            Usage++;
        }
        else
        {
            leftDoorClosed = false;
            LeftDoorAnimator.Play("LeftDoorOpen");
            Usage--;
        }
    }

    void RightDoor()
    {
        if (!rightDoorClosed)
        {
            rightDoorClosed = true;
            RightDoorAnimator.Play("RightDoorClose");
            Usage++;
        }
        else
        {
            rightDoorClosed = false;
            RightDoorAnimator.Play("RightDoorOpen");
            Usage--;
        }
    }

    void FrontVent()
    {
        if (!frontVentClosed)
        {
            frontVentClosed = true;
            FrontVentAnimator.Play("FrontVentClose");
            Usage++;
        }
        else
        {
            frontVentClosed = false;
            FrontVentAnimator.Play("FrontVentOpen");
            Usage--;
        }
    }

    void SideVent()
    {
        if (!sideVentClosed)
        {
            sideVentClosed = true;
            SideVentAnimator.Play("SideVentClose");
            Usage++;
        }
        else
        {
            sideVentClosed = false;
            SideVentAnimator.Play("SideVentOpen");
            Usage--;
        }
    }

    // Function to move MainBG
    private void MoveMainBG(float direction)
    {
        Vector3 newPosition = MainBG.position + new Vector3(direction * Time.deltaTime, 0f, 0f);
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX); // Clamp the position within MinX and MaxX
        MainBG.position = newPosition;
    }

    // Function to switch the current ventilator state
    public void SwitchCurrentVentilatorState(int Index)
    {
        if (Index == currentVentilator)
            return;

        // Save the current local position before changing parent
        Vector3 localPosition = VentilatorChild.localPosition;
        if (currentVentilator == 5) {Usage++; Sound -= 2;}
        if (currentVentilator == 3) {Usage--; Sound--; currentDegreePerSecond -= HeaterDegreePerSecond;}
        if (currentVentilator == 2) {Usage--; Sound--; currentDegreePerSecond -= ACDegreePerSecond; ACSound.Stop();}
        if (currentVentilator == 4) {Usage--; currentDegreePerSecond -= FanDegreePerSecond;}
        if (currentVentilator == 1) {Usage--; GBMActive = false; if (!camsActive){MusicBoxSounds[currentMusicBoxIndex].mute = true;}}

        // Switch parent based on the Index
        switch (Index)
        {
            case 0:
                VentilatorChild.SetParent(Off);
                break;
            case 1:
                VentilatorChild.SetParent(GBM);
                GBMActive = true;
                if (!camsActive)
                {
                    MusicBoxSounds[currentMusicBoxIndex].mute = false;
                    MusicBoxSounds[currentMusicBoxIndex].volume = 0.325f;
                }
                Usage++;
                break;
            case 2:
                VentilatorChild.SetParent(PowerAC);
                Usage++;
                Sound++;
                ACSound.Play();
                currentDegreePerSecond += ACDegreePerSecond;
                break;
            case 3:
                VentilatorChild.SetParent(Heater);
                currentDegreePerSecond += HeaterDegreePerSecond;
                Usage++;
                Sound++;
                break;
            case 4:
                VentilatorChild.SetParent(SilVentilation);
                Usage++;
                currentDegreePerSecond += FanDegreePerSecond;
                break;
            case 5:
                VentilatorChild.SetParent(PowerGenerator);
                Usage--;
                Sound += 2;
                break;
            default:
                Debug.LogWarning("Invalid ventilator index!");
                break;
        }

        // Restore the local position after changing parent
        currentVentilator = Index;
        VentilatorChild.localPosition = localPosition;
        HappyFrogDC.currentVentilator = Index;
        MrHippoDC.currentVentilator = Index;
        PigpatchDC.currentVentilator = Index;
        NeddBearDC.currentVentilator = Index;
        OrvilleDC.currentVentilator = Index;
    }

    // Function to switch to the vent system
    public void SwitchToVentSystem()
    {
        CameraSystemExclusive.SetActive(false);
        DuctSystemExclusive.SetActive(false);
        VentSystemExclusive.SetActive(true);
        CamExclusiveUI.SetActive(true);
    }

    // Function to switch to the duct system
    public void SwitchToDuctSystem()
    {
        CameraSystemExclusive.SetActive(false);
        VentSystemExclusive.SetActive(false);
        DuctSystemExclusive.SetActive(true);
        CamExclusiveUI.SetActive(true);
    }

    // Function to switch to the camera system (default)
    public void SwitchToCameraSystem()
    {
        VentSystemExclusive.SetActive(false);
        DuctSystemExclusive.SetActive(false);
        CameraSystemExclusive.SetActive(true);
        CamExclusiveUI.SetActive(true);
    }

    // Function to reset ventilation and restore normal state
    public void ResetVentilation()
    {
        if (ventilationCoroutine != null)
        {
            StopCoroutine(ventilationCoroutine);
            if (ventilationFadeOutCoroutine != null)
            {
                StopCoroutine(ventilationFadeOutCoroutine);
                foreach (CanvasGroup group in Groups)
                {
                    group.alpha = 0f; // Fade out effect
                }
            }
        }
        
        ResetVentilationButton.sprite = NormalVentilation;
        VentilationWarning.SetActive(false);
        ventilationCoroutine = StartCoroutine(VentilationCycle());
    }

    public void SwitchCamera(int Cam)
    {
        bool awardCoins = true;
        DCoin.sprite = NormalDCoinSprite;
        currentCam = Cam;
        SwitchCam.Play();
        if (currentCam == 4)
        {
            ChicaPotsAndPans.volume = 1f;
            MusicBoxSounds[currentMusicBoxIndex].mute = false;
            MusicBoxSounds[currentMusicBoxIndex].volume = 0.75f;
        }
        if (currentCam == 4){AudioOnly.SetActive(true);WindMusicBoxButton.SetActive(true); MusicBoxSounds[currentMusicBoxIndex].mute = false; if(currentAI[12] >= 1 && DCoinActive){DCoin.sprite = DCoinClickSprite;}}else{AudioOnly.SetActive(false);WindMusicBoxButton.SetActive(false); MusicBoxSounds[currentMusicBoxIndex].mute = true;}
        if (currentCam == 7){PrizeCorner.SetActive(true);}else{PrizeCorner.SetActive(false);}
        if (currentCam == 6){if (currentAI[29] >= 1){Showtime.SetActive(true);if (DCoinActive){DCoin.sprite = DCoinClickSprite;}}else{OutOfOrder.SetActive(true);}}else{Showtime.SetActive(false);OutOfOrder.SetActive(false);}
        CameraMainBG.sprite = CamSprites[currentCam-1];
        if (currentCam == 6 && currentAI[29] >= 1) {CameraMainBG.sprite = load_sprite(FunTimeFoxyCurtainAddress);}
        if (currentCam == 5 && currentAI[3] >= 1 && activeFigurine == 0)
        {
            FoxyH = 0;
            switch (FoxyStage)
            {
                case 0:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[6]);
                break;
                case 1:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[7]);
                break;
                case 2:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[8]);
                break;
                case 3:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[9]);
                break;
                case 4:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[10]);
                break;
                default:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[10]);
                break;
            }
            if (DCoinActive)
            {
                DCoin.sprite = DCoinClickSprite;
            }
        }
        else if (currentCam == 5 && currentAI[1] >= 1 && activeFigurine == 1)
        {
            StartCoroutine(BonnieJumpscareCoroutine());
        }
        RockstarChicaLeft.gameObject.SetActive(false);
        RockstarChicaRight.gameObject.SetActive(false);
        FreddyStage1.gameObject.SetActive(false);
        FreddyStage2.gameObject.SetActive(false);
        FreddyStage3.gameObject.SetActive(false);
        FreddyStage4.gameObject.SetActive(false);
        if (currentCam == 1)
        {
            if (RockstarChicaCam == 1)
            {
                RockstarChicaLeft.gameObject.SetActive(true);
            }
            if (FreddyProgress >= 100 && FreddyProgress < 200)
            {
                FreddyStage1.gameObject.SetActive(true);
            }
            else if (FreddyProgress >= 200 && FreddyProgress < 300)
            {
                FreddyStage2.gameObject.SetActive(true);
            }
            else if (FreddyProgress >= 300 && FreddyProgress < 400)
            {
                FreddyStage3.gameObject.SetActive(true);
            }
            else if (FreddyProgress >= 400)
            {
                FreddyStage4.gameObject.SetActive(true);
            }
        }
        if (currentCam == 2)
        {
            if (RockstarChicaCam == 2)
            {
                RockstarChicaRight.gameObject.SetActive(true);
            }
        }
        MrCanDo.gameObject.SetActive(false);
        if (currentCam == currentMrCanDoCam && currentAI[31] >= 1)
        {
            MrCanDo.gameObject.SetActive(true);
        }
        if (awardCoins)
        {
            SpawnFazCoin();
        }
        if (MovementOppertunityC(currentAI[32], 40) && !HelpyInOffice)
        {
            HelpyLowerScreen.sprite = load_sprite(HelpyAddresses[0]);
            HelpyLowerScreen.gameObject.SetActive(true);
            HelpyInOffice = true;
            StartCoroutine(HelpyInOfficeCoroutine());
        }
        PhantomBBInOffice = false;
        PhantomBBOnCams.gameObject.SetActive(false);
        PhantomMangleOnCams = false;
        PhantomMangleOnCamsImg.gameObject.SetActive(false);
        Cam08Parent.SetActive(false);
        if (currentCam == 8 && currentAI[4] >= 1)
        {
            Cam08Parent.SetActive(true);
            DCoin.sprite = DCoinClickSprite;
            if (MrHugsCamera != 5)
            {
            switch (currentToyFreddyCam)
            {
                case 1:
                if (MrHugsCamera == 1)
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[0]);
                }
                else
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[1]);
                }
                break;
                case 2:
                if (MrHugsCamera == 2)
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[4]);
                }
                else
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[5]);
                }
                break;
                case 3:
                if (MrHugsCamera == 3)
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[2]);
                }
                else
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[3]);
                }
                break;
            }
            }
            else
            {
                CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[6]);
            }
        }

        
        JackOChicaLeft.gameObject.SetActive(false);
        JackOChicaRight.gameObject.SetActive(false);
        if (currentCam == 1)
        {
            JackOChicaLeft.gameObject.SetActive(true);
        }
        if (currentCam == 2)
        {
            JackOChicaRight.gameObject.SetActive(true);
            if (NightmareBonnieActive)
            {
                CameraMainBG.sprite = load_sprite(PlushieAddresses[0]);
            }
            else if (NightmareMangleActive)
            {
                CameraMainBG.sprite = load_sprite(PlushieAddresses[1]);
            }
            else if (BabyActive)
            {
                CameraMainBG.sprite = load_sprite(PlushieAddresses[2]);
            }
        }

        if (currentCam == 3 && currentAI[48] >= 1)
        {
            DCoin.sprite = DCoinClickSprite;
            Debug.Log(LeftyAgitation);
            if (LeftyAgitation < 300)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[0]);
            }
            else if (LeftyAgitation < 600)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[1]);
            }
            else if (LeftyAgitation < 900)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[2]);
            }
            else if (LeftyAgitation < 1200)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[3]);
            }
            else if (LeftyAgitation >= 1200)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[4]);
            }
        }
        RockstarBonnieGuitar.gameObject.SetActive(false);
        if (currentCam == RBonnieCamera)
        {
            RockstarBonnieGuitar.gameObject.SetActive(true);
        }

        if (currentCam == 6 && PlushtrapState == 0)
        {
            PlushtrapContainer.SetActive(true);
            PlushtrapAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(PlushtrapChairAddress);
            StartCoroutine(PlushtrapCamLoop());
        }
        else
        {
            PlushtrapContainer.SetActive(false);
        }
    }

    public void SwitchCamera(int Cam, bool awardCoins)
    {
        DCoin.sprite = NormalDCoinSprite;
        currentCam = Cam;
        if (awardCoins)
        {
            SwitchCam.Play();
        }
        if (currentCam == 4)
        {
            ChicaPotsAndPans.volume = 1f;
            MusicBoxSounds[currentMusicBoxIndex].mute = false;
            MusicBoxSounds[currentMusicBoxIndex].volume = 0.75f;
        }
        if (currentCam == 4){AudioOnly.SetActive(true);WindMusicBoxButton.SetActive(true); MusicBoxSounds[currentMusicBoxIndex].mute = false; if(currentAI[12] >= 1 && DCoinActive){DCoin.sprite = DCoinClickSprite;}}else{AudioOnly.SetActive(false);WindMusicBoxButton.SetActive(false); MusicBoxSounds[currentMusicBoxIndex].mute = true;}
        if (currentCam == 7){PrizeCorner.SetActive(true);}else{PrizeCorner.SetActive(false);}
        if (currentCam == 6){if (currentAI[29] >= 1){Showtime.SetActive(true);if (DCoinActive){DCoin.sprite = DCoinClickSprite;}}else{OutOfOrder.SetActive(true);}}else{Showtime.SetActive(false);OutOfOrder.SetActive(false);}
        CameraMainBG.sprite = CamSprites[currentCam-1];
        if (currentCam == 6 && currentAI[29] >= 1) {CameraMainBG.sprite = load_sprite(FunTimeFoxyCurtainAddress);}
        if (currentCam == 5 && currentAI[3] >= 1 && activeFigurine == 0)
        {
            FoxyH = 0;
            switch (FoxyStage)
            {
                case 0:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[6]);
                break;
                case 1:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[7]);
                break;
                case 2:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[8]);
                break;
                case 3:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[9]);
                break;
                case 4:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[10]);
                break;
                default:
                CameraMainBG.sprite = load_sprite(FoxyAddresses[10]);
                break;
            }
            if (DCoinActive)
            {
                DCoin.sprite = DCoinClickSprite;
            }
        }
        else if (currentCam == 5 && currentAI[1] >= 1 && activeFigurine == 1)
        {
            StartCoroutine(BonnieJumpscareCoroutine());
        }
        RockstarChicaLeft.gameObject.SetActive(false);
        RockstarChicaRight.gameObject.SetActive(false);
        FreddyStage1.gameObject.SetActive(false);
        FreddyStage2.gameObject.SetActive(false);
        FreddyStage3.gameObject.SetActive(false);
        FreddyStage4.gameObject.SetActive(false);
        if (currentCam == 1)
        {
            if (RockstarChicaCam == 1)
            {
                RockstarChicaLeft.gameObject.SetActive(true);
            }
            if (FreddyProgress >= 100 && FreddyProgress < 200)
            {
                FreddyStage1.gameObject.SetActive(true);
            }
            else if (FreddyProgress >= 200 && FreddyProgress < 300)
            {
                FreddyStage2.gameObject.SetActive(true);
            }
            else if (FreddyProgress >= 300 && FreddyProgress < 400)
            {
                FreddyStage3.gameObject.SetActive(true);
            }
            else if (FreddyProgress >= 400)
            {
                FreddyStage4.gameObject.SetActive(true);
            }
        }
        if (currentCam == 2)
        {
            if (RockstarChicaCam == 2)
            {
                RockstarChicaRight.gameObject.SetActive(true);
            }
        }
        MrCanDo.gameObject.SetActive(false);
        if (currentCam == currentMrCanDoCam && currentAI[31] >= 1)
        {
            MrCanDo.gameObject.SetActive(true);
        }
        if (awardCoins)
        {
            SpawnFazCoin();
        }
        if (MovementOppertunityC(currentAI[32], 40) && !HelpyInOffice)
        {
            HelpyLowerScreen.sprite = load_sprite(HelpyAddresses[0]);
            HelpyLowerScreen.gameObject.SetActive(true);
            HelpyInOffice = true;
            StartCoroutine(HelpyInOfficeCoroutine());
        }
        PhantomBBInOffice = false;
        PhantomBBOnCams.gameObject.SetActive(false);
        PhantomMangleOnCams = false;
        PhantomMangleOnCamsImg.gameObject.SetActive(false);
        Cam08Parent.SetActive(false);
        if (currentCam == 8 && currentAI[4] >= 1)
        {
            Cam08Parent.SetActive(true);
            DCoin.sprite = DCoinClickSprite;
            if (MrHugsCamera != 5)
            {
            switch (currentToyFreddyCam)
            {
                case 1:
                if (MrHugsCamera == 1)
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[0]);
                }
                else
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[1]);
                }
                break;
                case 2:
                if (MrHugsCamera == 2)
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[4]);
                }
                else
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[5]);
                }
                break;
                case 3:
                if (MrHugsCamera == 3)
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[2]);
                }
                else
                {
                    CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[3]);
                }
                break;
            }
            }
            else
            {
                CameraMainBG.sprite = load_sprite(ToyFreddyAddresses[6]);
            }
        }

        
        JackOChicaLeft.gameObject.SetActive(false);
        JackOChicaRight.gameObject.SetActive(false);
        if (currentCam == 1)
        {
            JackOChicaLeft.gameObject.SetActive(true);
        }
        if (currentCam == 2)
        {
            JackOChicaRight.gameObject.SetActive(true);
            if (NightmareBonnieActive)
            {
                CameraMainBG.sprite = load_sprite(PlushieAddresses[0]);
            }
            else if (NightmareMangleActive)
            {
                CameraMainBG.sprite = load_sprite(PlushieAddresses[1]);
            }
            else if (BabyActive)
            {
                CameraMainBG.sprite = load_sprite(PlushieAddresses[2]);
            }
        }

        if (currentCam == 3 && currentAI[48] >= 1)
        {
            DCoin.sprite = DCoinClickSprite;
            Debug.Log(LeftyAgitation);
            if (LeftyAgitation < 300)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[0]);
            }
            else if (LeftyAgitation < 600)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[1]);
            }
            else if (LeftyAgitation < 900)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[2]);
            }
            else if (LeftyAgitation < 1200)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[3]);
            }
            else if (LeftyAgitation >= 1200)
            {
                CameraMainBG.sprite = load_sprite(LeftyAddresses[4]);
            }
        }
        RockstarBonnieGuitar.gameObject.SetActive(false);
        if (currentCam == RBonnieCamera)
        {
            RockstarBonnieGuitar.gameObject.SetActive(true);
        }

        if (currentCam == 6 && PlushtrapState == 0)
        {
            PlushtrapContainer.SetActive(true);
            PlushtrapAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(PlushtrapChairAddress);
            StartCoroutine(PlushtrapCamLoop());
        }
        else
        {
            PlushtrapContainer.SetActive(false);
        }
    }

    IEnumerator PlushtrapCamLoop()
    {
        float time = 0f;
        while (currentCam == 6 && PlushtrapState == 0 && time < 1.6f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (currentCam == 6 && PlushtrapState == 0 && camsActive)
        {
            PlushtrapAnimator.Play("Plushtrap_off");
            PlushtrapState = -1;
        }
    }

    IEnumerator BonnieJumpscareCoroutine()
    {
        int i = 0;
        BonnieJumpscare.Play();
        CameraMainBG.sprite = load_sprite(BonnieAddresses[1]);
        yield return new WaitForSeconds(0.2f);
        CameraMainBG.sprite = load_sprite(BonnieAddresses[2]);
        yield return new WaitForSeconds(0.2f);
        CameraMainBG.sprite = load_sprite(BonnieAddresses[1]);
        yield return new WaitForSeconds(0.2f);
        CameraMainBG.sprite = load_sprite(BonnieAddresses[2]);
        yield return new WaitForSeconds(0.2f);
        CameraMainBG.sprite = load_sprite(BonnieAddresses[1]);
        yield return new WaitForSeconds(0.2f);
        BonnieStatic.SetActive(true);
        while (BonnieStatic.activeSelf)
        {
            i++;
            if (i >= 250+(currentAI[1]*20))
            {
                BonnieStatic.SetActive(false);
                if (camsActive)
                {
                    SwitchCamera(currentCam);
                }
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator PhantomBBInOfficeCoroutine()
    {
    int i = 0;
    while (PhantomBBInOffice && camsActive)
    {
        i++;
        if (i >= 100 - (currentAI[17] * 2))
        {
            // Play jumpscare sound and activate jumpscare UI
            JumpscareAS.clip = Resources.Load<AudioClip>(JumpscareSoundAdresses[2]);
            JumpscareAS.Play();
            canPhantomBB = false;
            DeActivateCams();
            PhantomBBJumpscare.color = new Color(1,1,1,1);
            PhantomBBJumpscare.gameObject.SetActive(true);
            Breathing.Play();
            StartCoroutine(CanPhantomBBIE());

            // Start fade-out coroutine
            StartCoroutine(FadeOutPhantomBBJumpscare(PhantomBBJumpscare, 6f)); // Fade out over 6 seconds
        }
        yield return null;
    }
    }

    // Coroutine to fade out the jumpscare image over a specified duration
    IEnumerator FadeOutPhantomBBJumpscare(Image jumpscareImage, float duration)
    {
    float startAlpha = jumpscareImage.color.a;
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
        jumpscareImage.color = new Color(jumpscareImage.color.r, jumpscareImage.color.g, jumpscareImage.color.b, newAlpha);
        yield return null;
    }

    // Ensure the alpha is set to 0 after the fade-out
    jumpscareImage.color = new Color(jumpscareImage.color.r, jumpscareImage.color.g, jumpscareImage.color.b, 0f);
    PhantomBBJumpscare.gameObject.SetActive(false); // Deactivate the jumpscare object
    }

    // Function to set Snare 1 and deactivate others
    public void SetSnare1()
    {
        activeSnare = 1;
        UpdateSnareSprites();
    }

    // Function to set Snare 2 and deactivate others
    public void SetSnare2()
    {
        activeSnare = 2;
        UpdateSnareSprites();
    }

    // Function to set Snare 3 and deactivate others
    public void SetSnare3()
    {
        activeSnare = 3;
        UpdateSnareSprites();
    }

    // Function to activate cameras
    public void ActivateCams()
    {
        if (ScrapBabyStandingUp)
        {
            StartJumpscare(ScrapbabyJumpscareAddresses, 5, "Scrapbaby");
            return;
        }
        if (NightmareBBStandingUp)
        {
            StartJumpscare(NightmareBBJumpscareAddresses, 3, "NBB");
            return;
        }
        if (puppetPrepared)
        {
            StartJumpscare(PuppetJumpscareAddresses, 1, "Puppet");
            return;
        }
        StartCoroutine(CamsHandler(true));
        camsActive = true;
        MonitorAnimator.Play("MonitorFlipUp");
        Usage++;
    }

    // Function to deactivate cameras
    public void DeActivateCams()
    {
        CamExclusiveUI.SetActive(false);
        CameraExclusiveOverLap.SetActive(false);
        camsActive = false;
        trueCamsActive = false;
        if (Random.value <= 0.0001f)
        {
            Bouncepot.SetActive(true);
        }
        if (Random.value <= 0.0001f)
        {
            Tangle.SetActive(true);
        }
        if (Random.value <= 0.0001f)
        {
            WhiteRabbit.SetActive(true);
        }
        if (Random.value <= 0.00001f)
        {
            DeskHat.SetActive(true);
        }
        if (ActivateMinireenasN == true)
        {
            Minireenas.gameObject.SetActive(true);
        }
        if (activateRWQNext)
        {
            RWQCarrier.SetActive(true);
        }
        if (Random.value <= 0.15f && office == 2)
        {
            SpringtrapFlash.sprite = load_sprite(SpringtrapFlashPath);
            SpringtrapFlash.gameObject.SetActive(true);
        }
        LowerNC.gameObject.SetActive(true);
        UpperNC.gameObject.SetActive(true);
        MonitorAnimator.Play("MonitorFlipDown");
        Cameras.SetActive(false);
        RFoxyObjects.SetActive(true);
        Usage--;
        if (!GBMActive)
        {
            MusicBoxSounds[currentMusicBoxIndex].mute = true;
        }
        else
        {
            MusicBoxSounds[currentMusicBoxIndex].mute = false;
            MusicBoxSounds[currentMusicBoxIndex].volume = 0.325f;
        }
        WetFloorSign.gameObject.SetActive(true);

        if (puppetPrepared)
        {
            StartJumpscare(PuppetJumpscareAddresses, 1, "Puppet");
        }
        if (gfP)
        {
            goldenFreddyInOffice = true;
        }
        if (MovementOppertunityC(currentAI[25], 41))
        {
            NightmareBBStandingUp = true;
            NightmareBBActive.gameObject.SetActive(true);
            NightmareBBInActive.gameObject.SetActive(false);
            NightmareBBInnocentJumpscare = false;
        }
        else
        {
            NightmareBBInnocentJumpscare = true;
        }
        if (ToiletBowlBonniePrepared)
        {
            ToiletBowlBonniePrepared = false;
            StartCoroutine(FlickerOfficeLights((250 - (currentAI[11]*4))/60f,2f, 0.6f));
            ToiletBowlBonnieIsInOffice = true;
            StartCoroutine(ToiletBowlBonnieInOfficeCoroutine());
            StareAS.Play();
        }
        int rndm = Random.Range(0, 29);
        rndm -= currentAI[31];
        rndm++;
        if (currentAI[31] >= 1)
        {
            currentMrCanDoCam = rndm;
        }
        ChicaPotsAndPans.volume = 0.194f;
    }

    IEnumerator CamsHandler(bool active)
    {
        yield return new WaitForSeconds(0.317f);
        if (camsActive)
        {
            trueCamsActive = true;
            goldenFreddyInOffice = false;
            gfP = false;
            SpringtrapFlash.gameObject.SetActive(false);
            RWQCarrier.SetActive(false);
            Minireenas.gameObject.SetActive(false);
            LowerNC.gameObject.SetActive(false);
        UpperNC.gameObject.SetActive(false);
            RFoxyObjects.SetActive(false);
            CamExclusiveUI.SetActive(active);
            CameraExclusiveOverLap.SetActive(active);
        CamPutOn.Play();
        Cameras.SetActive(true);
        ChicaPotsAndPans.volume = 0.5f;
        SwitchCamera(currentCam);
        if (MovementOppertunityC(currentAI[13], 40) && currentAI[13] >= 1)
        {
            GoldenFreddy.gameObject.SetActive(true);
            gfP = true;
        }
        if (ScrapBabyStandingUp == false && ScrapBabyIsInOffice == false)
        {
            ScrapbabyInOffice.gameObject.SetActive(false);
            ShockPanel.gameObject.SetActive(false);
        }
        WetFloorSign.gameObject.SetActive(false);
        if (MovementOppertunityC(currentAI[31], 30) && !TatGJumpscarecoroutineRunning)
        {
            StartCoroutine(TatGJumpscareCoroutine());
        }
        if (MovementOppertunityC(currentAI[17], 50) && canPhantomBB)
        {
            PhantomBBInOffice = true;
            PhantomBBOnCams.gameObject.SetActive(true);
            StartCoroutine(PhantomBBInOfficeCoroutine());
        }
        if (MovementOppertunityC(currentAI[15], 60) && canPhantomMangle)
        {
            PhantomMangleOnCams = true;
            PhantomMangleOnCamsImg.gameObject.SetActive(true);
            StartCoroutine(PhantomMangleOnCamsCoroutine());
        }
        if (MovementOppertunityC(currentAI[8], 30) && !BBActive && !BBVent && !JJVent)
        {
            BBInVent.gameObject.SetActive(true);
            BBVent = true;
            StartCoroutine(BBInVentCoroutine());
        }
        if (MovementOppertunityC(currentAI[9], 30) && !BBVent && !JJActive && !JJVent)
        {
            JJInVent.gameObject.SetActive(true);
            JJVent = true;
            StartCoroutine(JJInVentCoroutine());
        }
        }
    }

    IEnumerator ToiletBowlBonnieInOfficeCoroutine()
    {
        int i = 0;
        while (ToiletBowlBonnieIsInOffice)
        {
            i++;
            if (maskActive)
            {
                Debug.Log("Erm What the Schmegma");
                if (i < 60)
                {
                    if (Random.value <= 0.033f)
                    {
                        ToiletBowlBonnieInOffice.gameObject.SetActive(false);
                        ToiletBowlBonnieIsInOffice = false;
                        StareAS.Stop();
                        yield break;
                    }
                }
                else if (i >= 60)
                {
                    if (Random.value <= 0.066f)
                    {
                        ToiletBowlBonnieInOffice.gameObject.SetActive(false);
                        ToiletBowlBonnieIsInOffice = false;
                        StareAS.Stop();
                        yield break;
                    }
                }
            }
            if (i >= 250 - (currentAI[11]*4))
            {
                ToiletBowlBonnieInOffice.gameObject.SetActive(false);
                StartJumpscare(ToiletBowlBonnieJumpscareAddresses, 1, "WBonnie");
                StareAS.Stop();
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator JJInVentCoroutine()
    {
        int i = 0;
        while (!sideVentClosed)
        {
            i++;
            if (i >= 300 - (currentAI[9]*5) && camsActive)
            {
                JJInVent.gameObject.SetActive(false);
                JJActive = true;
                JJInOffice.gameObject.SetActive(true);
                StartCoroutine(JJInOfficeCoroutine());
                JJVent = false;
                yield break;
            }
            yield return null;
        }
        JJVent = false;
        BlockAnimatronic();
        yield return new WaitForSeconds(0.12f);
        JJInVent.gameObject.SetActive(false);
    }

    IEnumerator JJInOfficeCoroutine()
    {
        int i = 0;
        while (JJActive)
        {
            i++;
            if (i >= 600)
            {
                JJActive = false;
                JJInOffice.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator BBInVentCoroutine()
    {
        int i = 0;
        while (!sideVentClosed)
        {
            i++;
            if (i >= 300 - (currentAI[8]*5) && camsActive)
            {
                BBInVent.gameObject.SetActive(false);
                BBActive = true;
                BBInOffice.gameObject.SetActive(true);
                StartCoroutine(BBInOfficeCoroutine());
                BBVent = false;
                flashlightActive = false;
                Flashlight.SetActive(false);
                yield break;
            }
            yield return null;
        }
        BBVent = false;
        BlockAnimatronic();
        yield return new WaitForSeconds(0.12f);
        BBInVent.gameObject.SetActive(false);
    }

    IEnumerator BBInOfficeCoroutine()
    {
        int i = 0;
        while (BBActive)
        {
            i++;
            if (i >= 600)
            {
                BBActive = false;
                BBInOffice.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator PhantomMangleOnCamsCoroutine()
    {
    int i = 0;
    while (PhantomMangleOnCams && camsActive)
    {
        i++;
        if (i >= 100 - (currentAI[15] * 2))
        {
            PhantomMangleScream.Play();
            DeActivateCams();
            PhantomMangleInOfficeImg.gameObject.SetActive(true);
            Sound++;
            canPhantomMangle = false;
            StartCoroutine(CanPhantomMangleIE());
            yield return new WaitForSeconds((400+ (currentAI[16]*10))/30);
            PhantomMangleInOfficeImg.gameObject.SetActive(false);
            Sound--;
            PhantomMangleScream.Stop();
        }
        yield return null;
    }
    }

    IEnumerator TatGJumpscareCoroutine()
    {
    // Set up initial variables
    TatGJumpscarecoroutineRunning = true;
    CrateSmall.gameObject.SetActive(true); // Activate CrateSmall
    AudioSource rndmWhisperAudioSource = TatGJumpscare; // Assuming the AudioSource component is attached to the same GameObject
    int whisperCount = 0; // Track whispers
    bool jumpscareTriggered = false; // Track if the jumpscare has been triggered

    // Whisper upon entry
    rndmWhisperAudioSource.clip = load_audioClip(TatGWhispers[Random.Range(0, TatGWhispers.Length)]);
    rndmWhisperAudioSource.Play();
    whisperCount++;

    // Main loop while in the office
    while (!jumpscareTriggered)
    {
        yield return new WaitForSeconds(3f); // Wait 3 seconds

        // 50% chance of initializing a jumpscare
        if (Random.value <= 0.5f)
        {
            // Trigger jumpscare
            jumpscareTriggered = true; // Set jumpscare triggered flag

            // Play jumpscare audio
            rndmWhisperAudioSource.clip = load_audioClip(TatGAddresses[2]);
            rndmWhisperAudioSource.Play();

            // Animate the jumpscare sprites
            for (int i = 0; i < TatGJumpscareAddresses.Length; i++)
            {
                CrateJumpscare.gameObject.SetActive(true);
                CrateJumpscare.sprite = load_sprite(TatGJumpscareAddresses[i]);
                yield return new WaitForSeconds(0.2f); // Wait for 1/60th of a second (60 FPS), resulting in 50 frames of jumpscare animation
            }

            CrateJumpscare.gameObject.SetActive(false);
            yield return new WaitForSeconds(30f);
            TatGJumpscarecoroutineRunning = false;
            break; // Exit the coroutine after the jumpscare
        }

        // Whisper every 7 seconds
        if (whisperCount % 2 == 0) // 3 seconds have passed twice (6 seconds) + 1 second delay after whispering makes it 7 seconds
        {
            rndmWhisperAudioSource.clip = load_audioClip(TatGWhispers[Random.Range(0, TatGWhispers.Length)]);
            rndmWhisperAudioSource.Play();
            whisperCount++;
        }
    }
    CrateSmall.gameObject.SetActive(false); // Deactivate CrateSmall
    yield return new WaitForSeconds(30f);
    TatGJumpscarecoroutineRunning = false;
    }

    // Coroutine to handle ventilation cycle and fading effect
    private IEnumerator VentilationCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f); // Wait for 30 seconds

            // Set ventilation to failing state and start fading effect
            ResetVentilationButton.sprite = VentilationFailing;
            VentilationWarning.SetActive(true);
            ventilationFadeOutCoroutine = StartCoroutine(FadeOutGroups());
        }
    }

    // Coroutine to fade out canvas groups (ventilation failing effect)
    private IEnumerator FadeOutGroups()
    {
        if (PercentLeft <= 0)
            yield break;
        float duration = 7.0f; // Duration of fade-out effect
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);

            foreach (CanvasGroup group in Groups)
            {
                group.alpha = 0f + alpha; // Fade out effect
            }

            yield return null;
        }
    }

    private void MoveCameraMainBG(float speed)
    {
    Vector3 cameraNewPosition = CameraMainBG.transform.position + new Vector3(cameraMoveDirection * speed * Time.deltaTime, 0f, 0f);
    
    // Ensure the CameraMainBG stays within the defined boundaries
    cameraNewPosition.x = Mathf.Clamp(cameraNewPosition.x, MinX, MaxX);
    
    CameraMainBG.transform.position = cameraNewPosition;

    // Reverse direction when reaching boundaries (bounce effect)
    if (cameraNewPosition.x <= MinX || cameraNewPosition.x >= MaxX)
    {
        cameraMoveDirection *= -1f;
    }
    }

    // Function to update snare sprites based on the active snare
    private void UpdateSnareSprites()
    {
        Snare1.sprite = (activeSnare == 1) ? ActiveSnare : UnActiveSnare;
        Snare2.sprite = (activeSnare == 2) ? ActiveSnare : UnActiveSnare;
        Snare3.sprite = (activeSnare == 3) ? ActiveSnare : UnActiveSnare;
    }

    // Function to update usage blocks based on the current usage value
    private void UpdateUsageBlocks()
    {
        for (int i = 0; i < UsageBlocks.Length; i++)
        {
            UsageBlocks[i].SetActive(i < Usage);
        }
    }

    // Function to update sound blocks based on the current sound value
    private void UpdateSoundBlocks()
    {
        for (int i = 0; i < SoundBlocks.Length; i++)
        {
            SoundBlocks[i].SetActive(i < Sound);
        }
    }
}
