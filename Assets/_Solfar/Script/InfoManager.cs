using UnityEngine;
using UnityEngine.UI;

/*************************************************
 * 
 *      2018 By Alan Mattano� at SOARING STARS lab
 *      for SergioSolfa Request and Vuelo a Vela
 *      
 *      Controls of the information that is display UI
 *
 * ***********************************************/

public class InfoManager : MonoBehaviour
{
    public GameObject manager;
    public Text displayInfo;
    public string additionalWarning;// messages from manager scripts
    public string additionalStatus;// messages from timer scripts

    private InfoDataManager database;
    private string messageToDisplay;


	void Start ()
    {
        messageToDisplay = "Start Up...";
        database    = manager.GetComponent<InfoDataManager>();
    }
	

	void Update ()
    {
        displayInfo.text = additionalStatus + messageToDisplay + additionalWarning;

        if (database.systemActivationStatus)
        {
            messageToDisplay = "System is Active";
        } else
        {
            messageToDisplay = ">> APAGADO <<";
            additionalStatus = "";
        }
    }
}