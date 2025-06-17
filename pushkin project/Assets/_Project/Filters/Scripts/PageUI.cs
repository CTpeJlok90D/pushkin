using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PageUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Image _imageMask;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TMP_Text _author;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previewButton;
    
    public Page Page { get; private set; }
    private PageLoader _pageLoader;
    private Page[] _activePages;
    private int _index;

    public PageUI Instantiate(Page page, PageLoader pageLoader)
    {
        gameObject.SetActive(false);
        PageUI instance = Instantiate(this);
        gameObject.SetActive(true);
        
        instance._pageLoader = pageLoader;
        instance.Init(page);
        return instance;
    }
    
    private void Init(Page page)
    {
        Page = page;
        _image.sprite = Page.Image;
        _image.sprite = Page.Image;
        
        InitSize();
        InitButtons();
    }

    private void InitSize()
    {
        if (Page.Author == null)
        {
            Debug.LogError($"[<color=yellow>ОШИБКА ЗАПОЛНЕНИЯ КАРТИН</color>] Автор картины {Page} не найден", Page);
            _author.text = $"Автор неизвестен  {Page.BornDate}г.";
        }
        else
        {
            _author.text = $"{Page.Author.Name} {Page.Name} {Page.BornDate}г."; 
        }
        
        gameObject.SetActive(true);
        
        int imageWight = Page.Image.texture.width;
        int imageHeight = Page.Image.texture.height;
        
        Vector2 resultSize = new Vector2(750, 750);

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

    private void InitButtons()
    {
        _activePages = _pageLoader.Pages.Where(x => x.gameObject.activeInHierarchy).Select(x => x.Page).ToArray();
        _index = Array.IndexOf(_activePages, Page);
        
        _nextButton.interactable = _index != _activePages.Length - 1;
        _previewButton.interactable = _index != 0;
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseClick);
        _nextButton.onClick.AddListener(OnNextButtonClick);
        _previewButton.onClick.AddListener(OnPreviewButtonClick);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseClick);
        _nextButton.onClick.RemoveListener(OnNextButtonClick);
        _previewButton.onClick.RemoveListener(OnPreviewButtonClick);
    }

    private void OnPreviewButtonClick()
    {
        MovePage(-1);
    }

    private void OnNextButtonClick()
    {
        MovePage(1);
    }

    private void MovePage(int offset)
    {
        Page page = _activePages[_index + offset];
        
        Init(page);
    }

    private void OnCloseClick()
    {
        Destroy(gameObject);
    }
}
