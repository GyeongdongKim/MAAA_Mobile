using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightController : MonoBehaviour {

    // The directional light which we manipulate as our sun.
    public Light sun;
    public Text currentTime;
    private bool dayChange = false, voteTrigger = false, nightTrigger = false,spawnTrigger=false;
    //[HideInInspector]public GameObject[] pointlights;
    [HideInInspector] public bool gameOver=true;
    public Text narrative;
    private GameManager gameManager;
    public BigMoniterControler bmc;
    public GameObject[] eventUIs;


    // The number of real-world seconds in one full game day.
    // Set this to 86400 for a 24-hour realtime day.
    public float secondsInFullDay = 480f;

    // The value we use to calculate the current time of day.
    // Goes from 0 (midnight) through 0.25 (sunrise), 0.5 (midday), 0.75 (sunset) to 1 (midnight).
    // We define ourself what value the sunrise sunrise should be etc., but I thought these 
    // values fit well. And now much of the script are hardcoded to these values.
    [Range(0,1)]
    public float currentTimeOfDay = 0.26f;

    // A multiplier other scripts can use to speed up and slow down the passing of time.
    [HideInInspector]
    public float timeMultiplier = 1f;
    private AudioSource audioSource;

    public GameObject barricade;
    public GameObject executionCollider;
    [Header("Sound Clip")]
    public AudioClip morningAudio;
    public AudioClip voteAudio;
    public AudioClip nightAudio;
    public AudioClip shootAudio;
    // Get the initial intensity of the sun so we remember it.
    float sunInitialIntensity;
    void Start() {
        //pointlights = GameObject.FindGameObjectsWithTag("POINTLIGHT");
        gameManager = GetComponent<GameManager>();
        //sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        sunInitialIntensity = sun.intensity;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        // Updates the sun's rotation and intensity according to the current time of day.
        if (!gameOver)
        {
            UpdateSun();

            // This makes currentTimeOfDay go from 0 to 1 in the number of seconds we've specified.
            currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
            currentTime.text = currentTimeOfDay.ToString();
            if (currentTimeOfDay > 0.25 && !dayChange)
            {
                executionCollider.SetActive(false);
                Debug.Log("dayPrint");
                eventUIs[0].SetActive(true);
                bmc.NoteUpdate();
                dayChange = true;
                gameManager.DayPrint();
                AudioManager._Instance.Fade(morningAudio, 0.3f, true);// I.ChangeBGM(morningAudio, false);
                barricade.SetActive(true);
                GetComponent<GameOverManager>().CheckGame();
            }
            if (currentTimeOfDay > 0.25f + 1f / 36f&&!spawnTrigger)
            {
                gameManager.localPlayer.transform.position = new Vector3(Random.Range(-70, -55), 10, Random.Range(-60, -40)); //execution location
                spawnTrigger = true;
            }
            // If currentTimeOfDay is 1 (midnight) set it to 0 again so we start a new day.
            if (currentTimeOfDay >= 1)
            {
                currentTimeOfDay = 0;
                dayChange = false;
                GetComponent<VoteManager>().trigger1 = false;
                GetComponent<VoteManager>().trigger2 = false;
                voteTrigger = false;nightTrigger = false;
                eventUIs[2].SetActive(false);
                spawnTrigger = false;
            }

            if (currentTimeOfDay > 23f / 36f && !voteTrigger)
            {
                executionCollider.SetActive(true);
                NarrationWhat("The vote will begin soon");
                eventUIs[0].SetActive(false);
                eventUIs[1].SetActive(true);
                voteTrigger = true;
                AudioManager._Instance.Fade(voteAudio, 0.3f, true);// I.ChangeBGM(voteAudio, true);
            }

            if (currentTimeOfDay > 13f / 18f && !nightTrigger)
            {
                executionCollider.SetActive(false);
                barricade.SetActive(false);
                NarrationWhat("Ready for the night");
                eventUIs[1].SetActive(false);
                eventUIs[2].SetActive(true);
                nightTrigger = true;
                GetComponent<GameOverManager>().CheckGame();
                AudioManager._Instance.Fade(nightAudio, 0.3f, true);// I.ChangeBGM(nightAudio, true);
            }
        }
    }

    void UpdateSun() {
        // Rotate the sun 360 degrees around the x-axis according to the current time of day.
        // We subtract 90 degrees from this to make the sun rise at 0.25 instead of 0.
        // I just found that easier to work with.
        // The y-axis determines where on the horizon the sun will rise and set.
        // The z-axis does nothing.
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
        /*if (currentTimeOfDay > 0.25 && currentTimeOfDay < 0.75)
        {
            for (int i = 0; i < pointlights.Length; i++)
            {
                pointlights[i].GetComponent<Light>().intensity = 0;
            }
        }
        else
        {
            for (int i = 0; i < pointlights.Length; i++)
            {
                pointlights[i].GetComponent<Light>().intensity = 1.5f;
            }
        }*/
        // The following determines the sun's intensity according to current time of day.
        // You'll notice I have hardcoded a bunch of values here. They were just the values
        // I felt worked best. This can obviously be made to be user configurable.
        // Also with some more clever code you can have different lengths for the day and
        // night as well.

        // The sun is full intensity during the day.
        float intensityMultiplier = 1;
        // Set intensity to 0 during the night night.
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f) {
            intensityMultiplier = 0;
        }
        // Fade in the sun when it rises.
        else if (currentTimeOfDay <= 0.25f) {
            // 0.02 is the amount of time between sunrise and the time we start fading out
            // the intensity (0.25 - 0.23). By dividing 1 by that value we we get get 50.
            // This tells us that we have to fade in the intensity 50 times faster than the
            // time is passing to be able to go from 0 to 1 intensity in the same amount of
            // time as the currentTimeOfDay variable goes from 0.23 to 0.25. That way we get
            // a perfect fade.
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        // And fade it out when it sets.
        else if (currentTimeOfDay >= 0.73f) {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
        }

        // Multiply the intensity of the sun according to the time of day.
        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }

    public void NarrationWhat(string what)
    {
        StartCoroutine(Narr(what));
    }

    IEnumerator Narr(string what)
    {
        narrative.text = what;
        narrative.GetComponent<Animator>().SetBool("FadeIn", true);
        yield return new WaitForSeconds(3.0f);
        narrative.GetComponent<Animator>().SetBool("FadeIn", false);
        //narrative.text = "";
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(currentTimeOfDay);
        }
        else
        {
            currentTimeOfDay = (float)stream.ReceiveNext();
        }
    }
}