using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class PageLoader : MonoBehaviour
{
    [field: SerializeField] public AssetLabelReference PageLabel { get; private set; }
    [field: SerializeField] public PageGalleryPrefab PageObject_PREFAB { get; private set; }
    [field: SerializeField] private Transform pagesRoot;
    [field: SerializeField] private Filters _filters;

    private List<PageGalleryPrefab> _pages = new();

    public IReadOnlyList<PageGalleryPrefab> Pages => _pages;

    private int _pageLoadingCount = int.MaxValue;
    
    private string _searchString = "";
    
    public Filters Filters
    {
        get
        {
            return _filters;
        }
        set
        {
            _filters = value;

            FilterPages();
        }
    }

    public string SearchString
    {
        get
        {
            return _searchString;
        }
        set
        {
            _searchString = value;
            
            FilterPages();
        }
    }

    private void FilterPages()
    {
        foreach (PageGalleryPrefab galleryPrefab in _pages)
        {
            Page page = galleryPrefab.Page;
            bool isUnderFilter = page.IsValid && (_filters.Authors.Contains(page.Author) || _filters.Authors.Count == 0) && 
                                 page.BornDate <= _filters.MaxDateTime && page.BornDate >=  _filters.MinDateTime
                                 && (string.IsNullOrEmpty(SearchString) || page.Author.Name.Contains(SearchString) || page.Name.Contains(SearchString));
            
            galleryPrefab.gameObject.SetActive(isUnderFilter);
        }
    }
    
    public void Start()
    {
        _ = LoadGallery();
    }

    private async UniTask LoadGallery()
    {
        IList<IResourceLocation> galleryAssets = await GetAssetReferences();
        _pageLoadingCount = galleryAssets.Count;
        foreach (IResourceLocation variable in galleryAssets)
        {
            PageGalleryPrefab pageObject = Instantiate(PageObject_PREFAB, pagesRoot);
            _pages.Add(pageObject);
            
            AsyncOperationHandle<Page> handle = Addressables.LoadAssetAsync<Page>(variable);
            handle.Completed += (handle) =>
            {
                _pageLoadingCount -= 1;
                pageObject.Init(handle.Result, this);
            };
        }
    }

    public async UniTask AwaitPagesLoad()
    {
        while (_pageLoadingCount != 0)
        {
            await Awaitable.NextFrameAsync();
        }
    }

    private async UniTask<IList<IResourceLocation>> GetAssetReferences()
    {
        return await (Addressables.LoadResourceLocationsAsync(new[] { PageLabel }, Addressables.MergeMode.Intersection).ToUniTask());
    }

    private void OnValidate()
    {
        
        Filters = _filters;
    }
}
