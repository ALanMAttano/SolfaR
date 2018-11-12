using UnityEngine;
using UnityEngine.UI;

public class UI_SliderSeconds : MonoBehaviour
{
    private Text display;
    public InfoDataManager database;

	void Start ()
    {
        display = this.GetComponent<Text>();
        display.text = "APAGADO";
        UpdateValue(database.data.updateTimeSeconds);
    }
	
	// Update is called once per frame
	public void UpdateValue (float seconds)
    {
        float min = seconds / 60f;
        int minInt = (int)min;
        float sec = seconds - ((float)minInt * 60f);
        display.text = minInt + " Min  : " + sec + " Sec";
    }
}
