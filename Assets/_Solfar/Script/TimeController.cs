using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*************************************************
 * 
 *      2018 By Alan Mattano at SOARING STARS lab
 *      for SergioSolfa Request and Vuelo a Vela
 *      
 *      Controls of the information that is display UI
 *
 * ***********************************************/

public class TimeController : MonoBehaviour
{
    public Slider timebar;
    public InfoManager displayInformation;
    public bool debugConsole;

    private InfoDataManager database;
    private AppManagerController managerContoller;
    private IEnumerator coroutine;
    private float pauseForSeconds;
    private float startTime, actualTime, deltaTime;
    private bool AppON = false;


    void Awake ()
    {
        database            = this.GetComponent<InfoDataManager>();
        managerContoller    = this.GetComponent<AppManagerController>();
    }

    void Start()
    {
        // For UI TimerBar: Give the maximum value in seconds
        timebar.maxValue = database.data.updateTimeSeconds;
    }

    void Update()
    {
        // ALAN: USE APP STAGE INSTEAD        <-------------------<<< NEED WORK ?
        AppON = database.systemActivationStatus;

        // A toggle ON OFF that now is ON
        if (AppON)
        {
            //timebar.maxValue = pauseForSeconds;
            timebar.value = Time.realtimeSinceStartup - startTime;
        } else
        {
            timebar.value = 0f;
        }
    }


    // this is enable using the Manager Controller
    public void EnableTimer()
    {
        if (debugConsole) Debug.Log("EnableTimer \nUpdate Now");

        // First make a save of all settings
        database.SAVE_AppInfoAndSettings();

        // For UI TimerBar: Give the maximum value in seconds
        timebar.maxValue = pauseForSeconds;
        startTime = Time.realtimeSinceStartup;
        if (debugConsole) Debug.Log(pauseForSeconds + "Timer \n no se actualiza cuando muevo el handle");

        pauseForSeconds = database.data.updateTimeSeconds;
        StartTimer();
    }


    public void DisableTimer()
    {
        StopCoroutine(coroutine);
    }


    // This function is call from the slider
    public void SetSeconds(float updatedValue)
    {
        pauseForSeconds = updatedValue;
        database.data.updateTimeSeconds = updatedValue;
    }


    void StartTimer()
    {
        displayInformation.additionalStatus = " Timer is running... ";
        coroutine = ActivateAfter();
        StartCoroutine(coroutine);
    }


    IEnumerator ActivateAfter()
    {
        // It will call a coroutine after some amount of seconds
        while (true)
        {
            // Make a Pause...


            yield return new WaitForSeconds(pauseForSeconds);
            // --->> DOWNLOAD !! <<---
            displayInformation.additionalStatus = " Downloading... | ";

            yield return new WaitForSeconds(0.1f);
            if (debugConsole) Debug.Log("Update Now \nALAN CHANGE THE ACTIVE STAGE TO DOWNLOAD                           <-------------<<<");

            // Control the slider, Reset the time-bar
            startTime = Time.realtimeSinceStartup;

            // NOW ... call function to make the download
            managerContoller.NowDownloadFIlesAndSaveToFolder();
        }
    }
}
