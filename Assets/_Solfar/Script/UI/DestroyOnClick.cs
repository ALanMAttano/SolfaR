using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyOnClick : MonoBehaviour,IPointerClickHandler
{
    [Space]
    [Header("When mouse click destroy this video")]
    [Space]
    public GameObject videoManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        videoManager.GetComponent<StreamVideo>().StopPlayingAndDestroy();
    }
}
