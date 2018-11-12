using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/*************************************************
 * 
 *      By Alan Mattanó
 * 
 *      Start play the video
 *      Fade in and out
 *      Destroy game objects
 * 
 * ***********************************************/

public class StreamVideo : MonoBehaviour {

    [Space]
    [Header("OnStart > Play video content, fade out and destroy itself")]
    [Space]
    public RawImage rawImage;
    
    public VideoPlayer videoPlayer;
    public GameObject blackImageGO;
    public float puseStart = 1f;
    public float lerpDuration = 5f;
    public float lerpInitial = 1f;
    public float lerpStop = 3f;
    public float lerpEnd = 3f;
    public float lerpFinish= 7f;
    public bool degugConsole;

    private bool stopCorutineAndEnd, lerpToTransparent, lerpToWhite;
    private float alpha, timeStartedLearping;
    private Image blackImage;

    void Awake()
    {
        // activo este GO para poder trabajar en el UI editor
        blackImageGO.SetActive(true);
        blackImage = blackImageGO.GetComponent<Image>();
    }

    void Start ()
    {
        // Lerp from black to transparent
        alpha = 1f;
        StartCoroutine(PlayVideo());
	}
	

	void Update ()
    {
        if (lerpToWhite)
        {
            rawImage.color = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 3));
        }

        if (lerpToTransparent)
        {
            float timeSinceStarted = Time.time - timeStartedLearping;
            float percentageToComplite = timeSinceStarted / lerpDuration;
            // from: https://www.youtube.com/watch?v=62IFyHUdH9U
            alpha = Mathf.Lerp(1.0f, 0.0f, percentageToComplite);
            rawImage.color = new Color(1f, 1f, 1f, alpha);
        }

		if (stopCorutineAndEnd)
        {
            StopPlayingAndDestroy();
        }
	}

    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(puseStart);
        while (!videoPlayer.isPrepared)
        {
            
            yield return waitForSeconds;
            break;
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        rawImage.color = new Color(1f, 1f, 1f, 1f);
        if (degugConsole) Debug.Log(lerpInitial + Time.timeSinceLevelLoad + "\n");

        yield return new WaitForSeconds(lerpStop);
        if (degugConsole) Debug.Log(lerpStop + Time.timeSinceLevelLoad + "\n");
        // Stop lerp
        lerpToWhite = false;
        blackImage.color = new Color(0f, 0f, 0f, 0f);

        yield return new WaitForSeconds(lerpEnd);
        if (degugConsole) Debug.Log(lerpEnd + Time.timeSinceLevelLoad + "  Vanish\n");
        // Start lerp
        timeStartedLearping = Time.time;
        lerpToTransparent = true;

        yield return new WaitForSeconds(lerpFinish);
        if (degugConsole) Debug.Log(lerpFinish + Time.timeSinceLevelLoad + "  Destroy\n");
        stopCorutineAndEnd = true;
    }

    public void StopPlayingAndDestroy()
    {
        videoPlayer.Stop();

        StopCoroutine(PlayVideo());
        Destroy(blackImage.gameObject, 1f);
        Destroy(rawImage.gameObject, 1f);
        Destroy(this.gameObject, 1f);
    }
}