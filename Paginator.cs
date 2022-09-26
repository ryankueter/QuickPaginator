namespace QuickPaginator;

public sealed class Paginator
{
    private readonly int _pageStart;
    private readonly int _pageLimit;
    private readonly int _resultsCount;
    private readonly int _currentPage;

    /// <summary>
    /// First Page
    /// Example:
    /// <a class=page-link href=~/Admin/Users/@pager.First>First</a>
    /// </summary>
    public readonly int First;

    /// <summary>
    /// Example:
    /// <a class=page-link href=~/Admin/Users/@pager.Previous>Previous</a>
    /// </summary>
    public readonly int Previous;

    /// <summary>
    /// Example:
    /// @foreach (var b in pager.BetweenPages)
	/// {
	///     <li class=page-item @b.Value><a class=page-link href=~/Admin/Users/@b.Key>@b.Key</a></li>
	/// }
    /// </summary>
    public readonly Dictionary<int, string> BetweenPages;

    /// <summary>
    /// Example:
    /// <a class=page-link href=~/Admin/Users/@pager.Next>Next</a>
    /// </summary>
    public readonly int Next;

    /// <summary>
    /// Last Page
    /// Example:
    /// <a class=page-link href=~/Admin/Users/@pager.Last>Last</a>
    /// </summary>
    public readonly int Last;

    /// <summary>
    /// The page count
    /// Example:
    /// @if (pager.PageCount >= 1)
	/// {
	/// }
    /// </summary>
    public readonly int PageCount;

    /// <summary>
    /// Example:
    /// response.Users.Where(user => user.IsDisabled == false).Skip(pager.Skip).Take(pager.Take).ToList();
    /// </summary>
    public readonly int Take;

    /// <summary>
    /// Example:
    /// response.Users.Where(user => user.IsDisabled == false).Skip(pager.Skip).Take(pager.Take).ToList();
    /// </summary>
    public readonly int Skip;

    /// <summary>
    /// Example:
    /// (2 of 20 results)
    /// (pager.CurrentCount of pager.TotalCount results)
    /// </summary>
    public readonly int CurrentCount;

    /// <summary>
    /// Example:
    /// (2 of 20 results)
    /// (pager.CurrentCount of pager.TotalCount results)
    /// </summary>
    public readonly int TotalCount;   

    /// <summary>
    /// Initializes the pager.
    /// </summary>
    /// <param name="CurrentPage">The current page, e.g., page 1.</param>
    /// <param name="ResultsCount">The number of results in the list.</param>
    /// <param name="PageLimit">The number of items per page.</param>
    public Paginator(int? CurrentPage, int ResultsCount, int PageLimit = 10)
    {
        // Make some initial calcluations
        _currentPage = GetCurrentPage(CurrentPage);
        _resultsCount = ResultsCount;
        _pageLimit = PageLimit;
        _pageStart = GetPageStart();
        PageCount = GetPageCount();

        // Get the page numbers
        First = GetFirst();
        Previous = GetPrevious();
        BetweenPages = GetBetweenPages();
        Next = GetNext();
        Last = GetLast();

        Take = GetTake();
        Skip = GetSkip();
        CurrentCount = GetCurrentCount();
        TotalCount = GetTotalCount();        
    }

    private int GetCurrentPage(int? CurrentPage)
    {
        if (CurrentPage is not null && CurrentPage > 0)
            return (int)CurrentPage;

        return 1;
    }

    private int GetPageStart()
    { 
        if (_currentPage > 0)
            return (_currentPage * _pageLimit) - _pageLimit;

        return 0;
    }

    private int GetPageCount()
    {
        if (_resultsCount == 0)
            return 0;

        double count = _resultsCount / _pageLimit;
        return (int)Math.Ceiling(count);
    }

    private int GetPrevious()
    {
        if (PageCount > 1)
        {
            if (_pageStart > 0)
            {
                return _currentPage - 1;
            }
        }
        return -1;
    }
    
    private Dictionary<int, string> GetBetweenPages()
    {
        Dictionary<int, string> result = new();

        if (PageCount > 1)
        {
            int n = 4;
            switch (_currentPage)
            {
                case 1:
                    n = 7;
                    break;
                case 2:
                    n = 6;
                    break;
                case 3:
                    n = 5;
                    break;
                case 4:
                    n = 4;
                    break;
            }

            if (_currentPage == PageCount)
                n = 7;

            if (_currentPage == PageCount - 1)
                n = 6;

            if (_currentPage == PageCount - 2)
                n = 5;

            if (_currentPage == PageCount - 3)
                n = 4;

            var i = 1;
            for (var x = 0; x < _resultsCount; x = x + _pageLimit)
            {
                if (x > (_pageStart - (n * _pageLimit)) && (x < (_pageStart + (n * _pageLimit))))
                {
                    var active = String.Empty;
                    if (_pageStart == x)
                    {
                        active = "active";
                    }
                    result.Add(i, active);
                }
                i++;
            }
        }
        return result;
    }

    private int GetNext()
    {
        if (PageCount > 1)
        {
            if ((_pageStart + _pageLimit) < _resultsCount)
            {
                return _currentPage + 1;
            }
        }
        return -1;
    }

    private int GetTake()
    {
        return _pageLimit;
    }

    private int GetSkip()
    {
        int skip = 0;
        if (_currentPage > 1)
            skip = (_currentPage - 1) * _pageLimit;

        return skip;
    }

    private int GetCurrentCount()
    {
        var next = _pageStart + _pageLimit;
        if (next < _resultsCount)
        {
            return next;
        }
        return _resultsCount;
    }

    private int GetTotalCount()
    {
        return _resultsCount;
    }

    private int GetFirst()
    {
        return 1;
    }

    private int GetLast()
    {
        return PageCount;
    }
}
