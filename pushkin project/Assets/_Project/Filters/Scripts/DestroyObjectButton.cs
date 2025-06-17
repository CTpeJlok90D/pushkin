using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyObjectButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _target;
    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(_target);
    }
}
