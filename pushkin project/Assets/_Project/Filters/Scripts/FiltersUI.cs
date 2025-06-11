using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UIRangeSliderNamespace;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FiltersUI : MonoBehaviour
{
    [field: SerializeField] private UIRangeSlider _yearsSlider;
    [field: SerializeField] private TMP_Dropdown _authorDropdown;
    [field: SerializeField] private TMP_InputField _searchInput;
    [field: SerializeField] private Author _anyAuthor;
    [field: SerializeField] private AssetLabelReference _authorsLabel;
    [field: SerializeField] private PageLoader _pageLoader;
    [field: SerializeField] private TMP_Text _minYear;
    [field: SerializeField] private TMP_Text _maxYear;

    private List<Author> _loadedAuthors = new();
    
    private void Awake()
    {
        LoadAuthors();
        _ = UpdateYears();
    }

    private void OnEnable()
    {
        _yearsSlider.onValuesChanged.AddListener(OnDateRangeChange);
        _authorDropdown.onValueChanged.AddListener(OnAuthorChange);
        _searchInput.onValueChanged.AddListener(OnSearchChange);
    }

    private void OnDisable()
    {
        _yearsSlider.onValuesChanged.RemoveListener(OnDateRangeChange);
        _authorDropdown.onValueChanged.RemoveListener(OnAuthorChange);
        _searchInput.onValueChanged.RemoveListener(OnSearchChange);
    }

    private void OnSearchChange(string searchText)
    {
        _pageLoader.SearchString = searchText;
    }

    private void OnAuthorChange(int index)
    {
        Filters filters = _pageLoader.Filters;
        
        Author author = _loadedAuthors[index];
        if (_anyAuthor == author)
        {
            filters.Authors = new List<Author>();
        }
        else
        {
            filters.Authors = new List<Author> { author };
        }
        
        _pageLoader.Filters = filters;
    }

    private void OnDateRangeChange(float minYear, float maxYear)
    {
        _minYear.text = ((int)minYear).ToString();
        _maxYear.text = ((int)maxYear).ToString();

        Filters filters = _pageLoader.Filters;
        filters.MaxDateTime = (int)maxYear;
        filters.MinDateTime = (int)minYear;
        _pageLoader.Filters = filters;
    }

    private async UniTask UpdateYears()
    {
        await _pageLoader.AwaitPagesLoad();
        
        Page[] pages = _pageLoader.Pages.Select(x => x.Page).ToArray();
        int minYear = pages.Min(x => x.BornDate);
        int maxYear = pages.Max(x => x.BornDate);

        _yearsSlider.minLimit = minYear;
        _yearsSlider.maxLimit = maxYear;
        
        _yearsSlider.valueMin = minYear;
        _yearsSlider.valueMax = maxYear;
    }

    private void LoadAuthors()
    {
        Addressables.LoadAssetsAsync<Author>(_authorsLabel, OnAuthorsLoad);
    }

    private void OnAuthorsLoad(Author author)
    {
        _loadedAuthors.Add(author);
        _loadedAuthors.Remove(_anyAuthor);
        _loadedAuthors = _loadedAuthors.OrderBy(x => x.name).ToList();
        _loadedAuthors.Insert(0, _anyAuthor);

        _authorDropdown.options = _loadedAuthors.Select(x => new TMP_Dropdown.OptionData() { text = x.Name }).ToList();
    }
}