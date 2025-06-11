using UnityEngine;
using UnityEngine.UI;

public class PageGalleryPrefab : MonoBehaviour
{
    [SerializeField] private Image image;
    public Page Page { get; private set; }
    public PageLoader PageLoader { get; private set; }
    
    public void Init(Page pageObject, PageLoader loader)
    {
        Page = pageObject;
        PageLoader = loader;

        int imageWight = pageObject.Image.texture.width;
        int imageHeight = pageObject.Image.texture.height;
        
        RectTransform imageRectTransform = (RectTransform)image.transform.parent;
        
        Vector2 resultSize = new Vector2(imageRectTransform.rect.width, imageRectTransform.rect.height);

        if (imageWight < imageHeight)
        {
            float modifier = imageRectTransform.rect.width / imageWight;
            resultSize.y = imageHeight * modifier;
        }
        else
        {
            float modifier = imageRectTransform.rect.height / imageHeight;
            resultSize.x = imageWight * modifier;
        }
        
        image.sprite = pageObject.Image;
        image.rectTransform.sizeDelta = resultSize;
    }
}
