using UnityEngine;

/*************************************************
 * 
 *      2018 By Alan Mattano at SOARING STARS lab
 *      for SergioSolfa Request and Vuelo a Vela
 *      
 *      Controls all the information for this app
 *
 * ***********************************************/

public class AppManagerController : MonoBehaviour
{
    [Space]
    [Header("Look for the database and make actions")]
    [Space]
    public bool AppON = false;
    public InfoManager displayInformation;

    private InfoDataManager database;
    private bool callTimerToEnable, callTimerToDisable;


	void Start ()
    {
        database = this.GetComponent<InfoDataManager>();
        AppON = database.systemActivationStatus;
        callTimerToEnable = true;
    }


    void OnApplicationQuit()
    {
        database.SAVE_AppInfoAndSettings();
    }


    void Update ()
    {
        // ALAN: USE APP STAGE INSTEAD                          <-------------------<<<
        AppON = database.systemActivationStatus;

        // A toggle ON OFF that now is ON
        if (AppON)
        {
            if (callTimerToEnable)
            {
                callTimerToEnable = false;
                callTimerToDisable = true;
                this.GetComponent<TimeController>().EnableTimer();
            }
        } else
        {
            // A toggle ON OFF that now is OFF
            if (callTimerToDisable)
            {
                callTimerToDisable = false;
                callTimerToEnable = true;
                this.GetComponent<TimeController>().DisableTimer();
            }
        }
	}


    // ------------------------------- PUBLIC FNCTIONS -----------------------------

    // [ UPDATE ] was button is press or it was call from Time Controller
    public void NowDownloadFIlesAndSaveToFolder()
    {
        database.data.appStage = AppStages.OFF_stop;

        displayInformation.additionalStatus = "Downloading ... ";

        // GET URL
        string path_FromURL = database.data.downloadFromUrlWebPath;
        // GET PC PATH
        string path_ToFolder = database.data.saveToPcFolderPath;
        // GET EXTENSION
        string file_extension = database.data.fileExtension;

        //Check if the last character contain / if not add one
        path_FromURL = CheckIfUrlIsCorrect(path_FromURL);

        // DOWNLOAD FILES
        //                    <------------<<<  Change it to coroutine?
        this.GetComponent<GetFileNamesFromUrl>().GET_FilesFromUrlWithExtension(path_FromURL, path_ToFolder, file_extension);
    }

    // if is missing "/" add it
    string CheckIfUrlIsCorrect(string url)
    {
        // obtain the last character
        string last = url.Substring(url.Length - 1);

        if (last != "/" )
        {
            url = url + "/";
        }

        return url;
    }
}