using UnityEngine;
using UnityEngine.EventSystems;

public class OpenFullScreenButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PageUI _pageUI; 
    [SerializeField] private FullScreenPicture _fullScreenPicture;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        _fullScreenPicture.Instantiate(_pageUI.Page);
    }
}
