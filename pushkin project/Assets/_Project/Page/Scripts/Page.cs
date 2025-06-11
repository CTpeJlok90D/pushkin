using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Page", menuName = "Game/Page")]
[Icon("Assets/_Project/Page/Icons/image.png")]
public class Page : ScriptableObject
{
    
    [field: Tooltip("Будет отображаться вверху после перехода на неё.")]
    [field: DisplayName("Название картины")]
    [field: SerializeField] public string Name { get; private set; }
    
    [field: TextArea(3, 10)]
    [field: SerializeField] public string Description { get; private set; }
    
    [field: Tooltip("Сама картина. Отсюда будет браться изображения для превью и для страницы")]
    [field: DisplayName("Текстура картины")]
    [field: SerializeField] public Sprite Image { get; private set; }
    
    [field: Tooltip("Кто её написал?")]
    [field: DisplayName("Автор картины")]
    [field: SerializeField] public Author Author { get; private set; }

    [field: Tooltip("Когда тот, кто её написал, её написал?")]
    [field: DisplayName("Год написания")]
    [field: SerializeField] public int BornDate { get; private set; } = 1900;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Image == null)
        {
            Debug.LogError($"<color=red>[Page filling error]</color> Image for {this} page was not found", this);
        }

        if (Author == null)
        {
            Debug.LogError($"<color=red>[Page filling error]</color> Author for {this} page was not found", this);
        }
    }
#endif
}