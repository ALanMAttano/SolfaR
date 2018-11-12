using UnityEngine;
using System.IO;    // FILE: Read and Save data

/*************************************************
 * 
 *      By Alan Mattano´ at SOARING STARS lab 2018
 *      for SergioSolfa and Vuelo a Vela
 *      
 *      Inspector public App data 
 *      that can be save and load 
 *      to a file name AppSettings.txt
 *      
 *      from: https://www.udemy.com/saving-and-loading-game-data-in-unity3d/learn/v4/t/lecture/4799336?start=0
 * 
 * ***********************************************/

public class InfoDataManager : MonoBehaviour
{
    [Space]
    [Header("DATABASE controll by the UI and used by APP-MANAGER-CONTROLLER")]
    [Space]
    public DataStructure data;
    [Space]
    [Header("FIX VARIABLES")]
    [Space]
    public bool systemActivationStatus = false;
    public string settingsFilePath = "/settings/AppSettings.txt";
    public string appSettingsFileName = "AppSettings.txt";
    public string saveThisAppDataTo = "/settings/"; // /settings/AppSettings.txt  was working if added in the inspector
    public bool saveAppDataToDiskNow,
                loadAppDataToDiskNow,
                settingsStreamingFolder,
                debugConsole;

    private string pathRoot;

    void Awake()
    {
        #region Take Out One Option <----------<<< NEED WORK
        if (settingsStreamingFolder)
        {
            // For iOS or Android look https://docs.unity3d.com/Manual/StreamingAssets.html
            pathRoot = Application.dataPath + @"/StreamingAssets/settings/AppSettings.txt";// + saveThisAppDataTo + appSettingsFileName;
            if (debugConsole) Debug.LogWarning("Setting Location " + pathRoot + "\n");
        }
        else
        {
            // We use "/settings/" working folder to store AppSettings but is not induced in to the build
            pathRoot = Application.dataPath + settingsFilePath;
            Debug.LogError("MATTANO > This Option will not work into the build.\nPlease include settings/AppSettings.txt by copy and past to de build");
            if (debugConsole) Debug.LogWarning("Setting Location " + pathRoot + "\n");
        }
        #endregion

        data.appStage = AppStages.OFF_stop;

        LOAD_AppInfoAndSettings();
    }

    void Update()
    {
        if (saveAppDataToDiskNow)
        {
            saveAppDataToDiskNow = false;
            SAVE_AppInfoAndSettings();
        }

        if (loadAppDataToDiskNow)
        {
            loadAppDataToDiskNow = false;
            LOAD_AppInfoAndSettings();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            if (debugConsole) Debug.Log("KeyCode LeftControl + S \n SAVING");

            SAVE_AppInfoAndSettings();
        }


        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            if (debugConsole) Debug.Log("KeyCode LeftControl + L");

            LOAD_AppInfoAndSettings();
        }
    }

    // ------------------------------- PUBLIC FNCTIONS -----------------------------


    public void SET_UrlPath (string url)
    {
        data.downloadFromUrlWebPath = url;
    }

    public void SET_FolderPath(string folder)
    {
        data.saveToPcFolderPath = folder;
    }

    public void SET_SystemActivation ( bool active)
    {
        systemActivationStatus = active;
    }


    public void SAVE_AppInfoAndSettings()
    {
        // ENUMs
        /* no queremos guardar los estados pero dejamos estas lineas como ejemplo de Enum
         * SetValue("ActualStage", appData.appStage.ToString());*/

        // Floats
        SetValue("UpdateTimeSeconds", data.updateTimeSeconds.ToString());

        // Strings
        SetValue("DownloadFromUrlWebPath", data.downloadFromUrlWebPath);
        SetValue("SaveToPcFolderPath", data.saveToPcFolderPath);
        SetValue("FileExtension", data.fileExtension);
        //SetValue("SaveThisAppDataTo", saveThisAppDataTo);

        // Bool ??
        //SetValue("SystemActivationStatus", systemActivationStatus.ToString());

        if (debugConsole) Debug.Log("You have saved all the App settings to disk to location:\n" + pathRoot);
}

    public void LOAD_AppInfoAndSettings()
    {
        // ENUMs
        /* no queremos guardar los estados pero dejamos estas lineas como ejemplo de Enum
         * var ActualStageController = GetValue("ActualStage", AppStages.OFF_stop.ToString());// ,Default
         * appData.appStage = (AppStages)System.Enum.Parse(typeof(AppStages), ActualStageController);*/

        // Floats
        var timeValue = GetValue("UpdateTimeSeconds", 128.ToString());
        float.TryParse(timeValue, out data.updateTimeSeconds);

        // Strings
        var pathToUrl = GetValue("DownloadFromUrlWebPath", "AddHTMLPath");
        data.downloadFromUrlWebPath = pathToUrl;
        var pathToFolder = GetValue("SaveToPcFolderPath", "AddFolderPath");
        data.saveToPcFolderPath = pathToFolder;
        var fileExtension = GetValue("FileExtension", "igc");
        data.fileExtension = fileExtension;
        //var appSettings = GetValue("SaveThisAppDataTo", @"/settings/AppSettings.txt");
        //saveThisAppDataTo = appSettings;

        if (debugConsole) Debug.Log("You have loaded the settings");
    }


    // ------------------------------- INTERNAL FNCTIONS -----------------------------

    #region Set Get
    void SetValue(string key, string value)
    {
        var fileLines = ReadAllLines(saveThisAppDataTo);

        for (int i = 0; i < fileLines.Length; i++)
        {
            var line = fileLines[i];

            if (line.Contains(key))
            {
                var endOfKeyIndex = line.IndexOf(key) + key.Length;
                var data = line.Substring(endOfKeyIndex, line.Length - endOfKeyIndex);
                data = data.Trim(' ', ':');

                fileLines[i] = line.Replace(data, value);
            }
        }

        WriteAllLines(saveThisAppDataTo, fileLines);
    }

    string GetValue(string key, string defaultValue)
    {
        var fileLines = ReadAllLines(saveThisAppDataTo);

        foreach (var line in fileLines)
        {
            if (debugConsole) Debug.LogWarning(line + "\n");
            if (line.Contains(key))
            {
                var endOfKeyIndex = line.IndexOf(key) + key.Length;
                var data = line.Substring(endOfKeyIndex, line.Length - endOfKeyIndex);

                return data.Trim(' ', ':');
            }
        }

        return defaultValue;
    }
    #endregion


    #region Write Read
    string[] ReadAllLines(string saveThisAppDataTo)
    {
        var lines = new string[1];

        #region Take Out One Option <----------<<< NEED WORK
        if (settingsStreamingFolder)
        {
            // We use StreamingAssets folder to store AppSettings

            // For iOS or Android look https://docs.unity3d.com/Manual/StreamingAssets.html
            if (File.Exists(pathRoot))
            {
                lines = File.ReadAllLines(pathRoot);
            }
            else
            {
                if (debugConsole) Debug.LogWarning("File " + appSettingsFileName + " Do Not Exist ! \n");
                if (debugConsole) Debug.LogWarning("Location " + pathRoot + "\n");
            }
        }
        else
        {
            // We use "/settings/" folder to store AppSettings

            if (File.Exists(pathRoot))
            {
                lines = File.ReadAllLines(pathRoot);
            }
            else
            {
                if (debugConsole) Debug.LogWarning("File " + appSettingsFileName + " Do Not Exist ! \n");
                if (debugConsole) Debug.LogWarning("Location " + pathRoot + "\n");
            }
        }
        #endregion

        return lines;
    }

    void WriteAllLines(string path, string[] lines)
    {
        #region Take Out One Option <----------<<< NEED WORK
        if (settingsStreamingFolder)
        {
            // For iOS or Android look https://docs.unity3d.com/Manual/StreamingAssets.html
            // We use StreamingAssets folder to store AppSettings

            if (File.Exists(pathRoot))
            {
                File.WriteAllLines(pathRoot, lines);
            }
            else
            {
                if (debugConsole) Debug.LogWarning("File " + appSettingsFileName + " Do Not Exist ! \n");
                if (debugConsole) Debug.LogWarning("Location " + pathRoot + "\n");
            }
        }
        else
        {
            // We use "/settings/" folder to store AppSettings

            if (File.Exists(pathRoot))
            {
                File.WriteAllLines(pathRoot, lines);
            }
            else
            {
                if (debugConsole) Debug.LogWarning("File " + appSettingsFileName + " Do Not Exist ! \n");
                if (debugConsole) Debug.LogWarning("Location " + pathRoot + "\n");
            }
        }
        #endregion
    }
    #endregion
}
