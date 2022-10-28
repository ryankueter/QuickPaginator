using System;
using System.Text;

namespace QuickPaginator;

public sealed class Paginator
{
    private readonly int _pageStart;
    private readonly int _pageLimit;
    private readonly int _resultsCount;
    private readonly int _currentPage;
    private readonly int _buttonCount;
    private readonly int _pageCount;

    /// <summary>
    /// First Page
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.First>First</a>
    /// </summary>
    public int First => GetFirst();

    /// <summary>
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.Previous>Previous</a>
    /// </summary>
    public int Previous => GetPrevious();
    public int PreviousTen => GetPreviousTen();
    public int PreviousTwenty => GetPreviousTwenty();
    public int PreviousThirty => GetPreviousThirty();
    public int PreviousFourty => GetPreviousFourty();
    public int PreviousFifty => GetPreviousFifty();
    public int PreviousHundred => GetPreviousHundred();

    /// <summary>
    /// Example:
    /// @foreach (var b in pager.BetweenPages)
	/// {
	///     <li class=page-item @b.Value><a class=page-link href=/Admin/Users/@b.Key>@b.Key</a></li>
	/// }
    /// </summary>
    public Dictionary<int, string> BetweenPages => GetBetweenPages(_buttonCount);

    /// <summary>
    /// Example:
    /// @foreach (var b in pager.AllPages)
	/// {
	///     <li class=page-item @b.Value><a class=page-link href=/Admin/Users/@b.Key>@b.Key</a></li>
	/// }
    /// </summary>
    public Dictionary<int, string> AllPages => GetAllPages();

    /// <summary>
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.Next>Next</a>
    /// </summary>
    public int Next => GetNext();
    public int NextTen => GetNextTen();
    public int NextTwenty => GetNextTwenty();
    public int NextThirty => GetNextThirty();
    public int NextFourty => GetNextFourty();
    public int NextFifty => GetNextFifty();
    public int NextHundred => GetNextHundred();

    /// <summary>
    /// Last Page
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.Last>Last</a>
    /// </summary>
    public int Last => GetLast();

    /// <summary>
    /// The page count
    /// Example:
    /// @if (pager.PageCount >= 1)
	/// {
	/// }
    /// </summary>
    public int PageCount => _pageCount;

    /// <summary>
    /// Example:
    /// response.Users.Where(user => user.IsDisabled == false).Skip(pager.Skip).Take(pager.Take).ToList();
    /// </summary>
    public int Take => GetTake();

    /// <summary>
    /// Example:
    /// response.Users.Where(user => user.IsDisabled == false).Skip(pager.Skip).Take(pager.Take).ToList();
    /// </summary>
    public int Skip => GetSkip();

    /// <summary>
    /// Example:
    /// (2 of 20 results)
    /// (pager.CurrentCount of pager.TotalCount results)
    /// </summary>
    public int CurrentCount => GetCurrentCount();

    /// <summary>
    /// Example:
    /// (2 of 20 results)
    /// (pager.CurrentCount of pager.TotalCount results)
    /// </summary>
    public int TotalCount => GetTotalCount();

    /// <summary>
    /// Initializes the pager.
    /// </summary>
    /// <param name="CurrentPage">The current page, e.g., page 1.</param>
    /// <param name="ResultsCount">The number of results in the list.</param>
    /// <param name="PageLimit">The number of items per page.</param>
    public Paginator(int? CurrentPage, int ResultsCount, int PageLimit = 10, int ButtonCount = 7)
    {
        // Make certain the numbers are positive
        if (CurrentPage <= 0 || ResultsCount < 0 || PageLimit <= 0 || ButtonCount < 1)
            throw new ArgumentOutOfRangeException();

        // Make some initial calcluations
        _currentPage = GetCurrentPage(CurrentPage);
        _resultsCount = ResultsCount;
        _pageLimit = PageLimit;
        _pageStart = GetPageStart();
        _buttonCount = ButtonCount;
        _pageCount = GetPageCount();

        // Make sure the current page isn't greater than the page count
        if (_currentPage > _pageCount)
            throw new ArgumentOutOfRangeException($"The current page '{_currentPage}' is greater than the page count '{_pageCount}.'");
    }

    /// <summary>
    /// Add a default constructor with some defaults,
    /// only used for initialization
    /// </summary>
    public Paginator()
    {
        _currentPage = 1;
        _resultsCount = 0;
        _pageLimit = 10;
    }

    private int GetCurrentPage(int? CurrentPage)
    {
        if (CurrentPage is not null && CurrentPage > 0)
            return (int)CurrentPage;

        return 1;
    }

    private int GetPageStart()
    {
        checked
        {
            if (_currentPage > 0)
                return (_currentPage * _pageLimit) - _pageLimit;
        }
        return 0;
    }

    private int GetPageCount()
    {
        if (_resultsCount <= 0)
            return 0;

        checked
        {
            double count = (double)_resultsCount / _pageLimit;
            return (int)Math.Ceiling(count);
        }
    }

    private int GetPrevious()
    {
        if (_pageCount > 1)
        {
            if (_pageStart > 0)
                return _currentPage - 1;
        }
        return -1;
    }

    private int GetPreviousTen()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 10) > 0)
                    return _currentPage - 10;
            }
        }
        return -1;
    }

    private int GetPreviousTwenty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 20) > 0)
                    return _currentPage - 20;
            }
        }
        return -1;
    }

    private int GetPreviousThirty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 30) > 0)
                    return _currentPage - 30;
            }
        }
        return -1;
    }

    private int GetPreviousFourty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 40) > 0)
                    return _currentPage - 40;
            }
        }
        return -1;
    }

    private int GetPreviousFifty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 50) > 0)
                    return _currentPage - 50;
            }
        }
        return -1;
    }

    private int GetPreviousHundred()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 100) > 0)
                    return _currentPage - 100;
            }
        }
        return -1;
    }

    private Dictionary<int, string> GetBetweenPages(int ButtonCount)
    {
        Dictionary<int, string> result = new();
        if (_pageCount > 1 && ButtonCount >= 1)
        {
            checked
            {
                // Give or take
                int mod = ButtonCount / 2;

                // Calculate start
                int Start;
                if (ButtonCount % 2 == 0)
                    Start = _currentPage - mod + 1;
                else
                    Start = _currentPage - mod;

                // Calculate end
                int End = _currentPage + mod;

                // See if the page is close to the begining
                if (Start <= 0)
                {
                    Start = 1;
                    if (_pageCount >= ButtonCount)
                        End = ButtonCount;
                    else
                        End = _pageCount;
                }

                // See if the page is close to the ending
                if (End >= _pageCount && Start is not 1)
                {
                    Start = _pageCount - ButtonCount + 1;
                    End = _pageCount;
                }

                // Iterate only the pages that need to be added
                for (var x = Start; x <= End; x++)
                {
                    var active = String.Empty;
                    if (_currentPage == x)
                    {
                        active = "active";
                    }
                    result.Add(x, active);
                }
            }
        }
        return result;
    }

    private Dictionary<int, string> GetAllPages()
    {
        Dictionary<int, string> result = new();
        if (_pageCount > 1)
        {
            for (var x = 1; x <= _pageCount; x++)
            {
                if (_currentPage == x)
                    result.Add(x, "active");
                else
                    result.Add(x, String.Empty);
            }
        }
        return result;
    }

    private int GetNext()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if ((_pageStart + _pageLimit) < _resultsCount)
                    return _currentPage + 1;
            }
        }
        return -1;
    }

    private int GetNextTen()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageCount >= (_currentPage + 10))
                    return _currentPage + 10;
            }
        }
        return -1;
    }

    private int GetNextTwenty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageCount >= (_currentPage + 20))
                    return _currentPage + 20;
            }
        }
        return -1;
    }

    private int GetNextThirty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageCount >= (_currentPage + 30))
                    return _currentPage + 30;
            }
        }
        return -1;
    }

    private int GetNextFourty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageCount >= (_currentPage + 40))
                    return _currentPage + 40;
            }
        }
        return -1;
    }

    private int GetNextFifty()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageCount >= (_currentPage + 50))
                    return _currentPage + 50;
            }
        }
        return -1;
    }

    private int GetNextHundred()
    {
        if (_pageCount > 1)
        {
            checked
            {
                if (_pageCount >= (_currentPage + 100))
                    return _currentPage + 100;
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
        checked
        {
            if (_currentPage > 1)
                skip = (_currentPage - 1) * _pageLimit;
        }
        return skip;
    }

    private int GetCurrentCount()
    {
        checked
        {
            var next = _pageStart + _pageLimit;
            if (next < _resultsCount)
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
        return _pageCount;
    }
}
