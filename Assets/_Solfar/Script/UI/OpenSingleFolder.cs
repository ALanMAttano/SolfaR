using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

public class OpenSingleFolder : MonoBehaviour
{
    [Space]
    [Header("When TO button is press asige a folder path to the input field")]
    [Space]
    public InputField inputField;
    public InfoDataManager database;

	void Start ()
    {
        inputField = inputField.GetComponent<InputField>();
        inputField.text = database.data.saveToPcFolderPath;
    }
	

	void Update ()
    {
		
	}

    public void GetFolderPath()
    {
        string path = FileBrowser.OpenSingleFolder("Open Folder");
        Debug.Log("Selected folder: " + path);
        inputField.text = path;
    }

}
