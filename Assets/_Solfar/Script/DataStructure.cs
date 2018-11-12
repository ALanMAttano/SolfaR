/*************************************************
 * 
 *      2018 By Alan Mattano´ at SOARING STARS lab
*       for SergioSolfa Request and Vuelo a Vela
 * 
 *      A structure of file to save inspector data 
 *      
 *      from: https://www.udemy.com/saving-and-loading-game-data-in-unity3d/learn/v4/t/lecture/4792508?start=0
 * 
 * ***********************************************/

[System.Serializable]
public class DataStructure
{
    // Use this for declaration
    public AppStages appStage;
    public float updateTimeSeconds = 120f;
    public string downloadFromUrlWebPath = "http://mattano.com/dad/css/";
    public string saveToPcFolderPath = "U:/Cestino/DeleteNow";
    public string fileExtension = "igc";
}

public enum AppStages
{
    ON_active,
    Downloading,
    Saving,
    OFF_stop
}