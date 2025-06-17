using UnityEngine;
using UnityEngine.UI;

public class FullScreenPicture : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Page _page;
    
    public FullScreenPicture Instantiate(Page page)
    {
        gameObject.SetActive(false);
        FullScreenPicture picture = Instantiate(this);
        gameObject.SetActive(true);

        picture._page = page;
        picture._image.sprite = page.Image;
        picture.gameObject.SetActive(true);
        picture.InitPictureSize();
        return picture;
    }

    private void InitPictureSize()
    {
        int imageWight = _page.Image.texture.width;
        int imageHeight = _page.Image.texture.height;
        
        Vector2 resultSize = new Vector2(3414, 1920);

        if (imageWight < imageHeight)
        {
            float modifier = resultSize.y / imageHeight;
            resultSize.x = imageWight * modifier;
        }
        else
        {
            float modifier = resultSize.x / imageWight;
            resultSize.y = imageHeight * modifier;
        }
        _image.rectTransform.sizeDelta = resultSize;
    }
}