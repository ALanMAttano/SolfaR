using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenUrlWebBrowser : MonoBehaviour
{
    [Space]
    [Header("When FROM button is press open the web browser url")]
    [Space]
    public InputField inputField;
    public InfoDataManager database;

    private string urlPath;

    void Start ()
    {
        inputField = inputField.GetComponent<InputField>();
        urlPath = database.data.downloadFromUrlWebPath;
        inputField.text = urlPath;
    }
	

	void Update ()
    {
		
	}

    // This functin is call [ FROM ] button
    public void OpenWebBrowserToUrl ()
    {
        // ex "http://google.com"
        System.Diagnostics.Process.Start(urlPath);
    }
}
