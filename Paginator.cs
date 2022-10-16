﻿using System;
using System.Text;

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
    /// <a class=page-link href=/Admin/Users/@pager.First>First</a>
    /// </summary>
    public readonly int First;

    /// <summary>
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.Previous>Previous</a>
    /// </summary>
    public readonly int Previous;
    public readonly int PreviousTen;
    public readonly int PreviousTwenty;
    public readonly int PreviousFifty;
    public readonly int PreviousHundred;

    /// <summary>
    /// Example:
    /// @foreach (var b in pager.BetweenPages)
	/// {
	///     <li class=page-item @b.Value><a class=page-link href=/Admin/Users/@b.Key>@b.Key</a></li>
	/// }
    /// </summary>
    public readonly Dictionary<int, string> BetweenPages = new();

    /// <summary>
    /// Example:
    /// @foreach (var b in pager.AllPages)
	/// {
	///     <li class=page-item @b.Value><a class=page-link href=/Admin/Users/@b.Key>@b.Key</a></li>
	/// }
    /// </summary>
    public Dictionary<int, string> AllPages { get { return GetAllPages(); } }

    /// <summary>
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.Next>Next</a>
    /// </summary>
    public readonly int Next;
    public readonly int NextTen;
    public readonly int NextTwenty;
    public readonly int NextFifty;
    public readonly int NextHundred;

    /// <summary>
    /// Last Page
    /// Example:
    /// <a class=page-link href=/Admin/Users/@pager.Last>Last</a>
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
        PageCount = GetPageCount();

        if (CurrentPage > PageCount)
            throw new ArgumentOutOfRangeException($"The current page '{CurrentPage}' is greater than the page count '{PageCount}.'");

        // Get the page numbers
        First = GetFirst();
        Previous = GetPrevious();
        PreviousTen = GetPreviousTen();
        PreviousTwenty = GetPreviousTwenty();
        PreviousFifty = GetPreviousFifty();
        PreviousHundred = GetPreviousHundred();
        BetweenPages = GetBetweenPages(ButtonCount);
        Next = GetNext();
        NextTen = GetNextTen();
        NextTwenty = GetNextTwenty();
        NextFifty = GetNextFifty();
        NextHundred = GetNextHundred();
        Last = GetLast();

        Take = GetTake();
        Skip = GetSkip();
        CurrentCount = GetCurrentCount();
        TotalCount = GetTotalCount();
    }

    public Paginator()
    {
        _currentPage = 1;
        _resultsCount = 0;
        _pageLimit = 10;
        BetweenPages = new();
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
        if (PageCount > 1)
        {
            if (_pageStart > 0)
                return _currentPage - 1;
        }
        return -1;
    }

    private int GetPreviousTen()
    {
        if (PageCount > 1)
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
        if (PageCount > 1)
        {
            checked
            {
                if (_pageStart > 0 && (_currentPage - 20) > 0)
                    return _currentPage - 20;
            }
        }
        return -1;
    }

    private int GetPreviousFifty()
    {
        if (PageCount > 1)
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
        if (PageCount > 1)
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
        if (PageCount > 1 && ButtonCount >= 1)
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
                // and reset the values if they are
                if (Start <= 0)
                {
                    Start = 1;
                    End = ButtonCount;
                }

                // See if the page is close to th ending
                // and reset the values if they are
                if (End >= PageCount)
                {
                    if (ButtonCount % 2 == 0)
                        Start = PageCount - ButtonCount + 1;
                    else
                        Start = PageCount - ButtonCount;

                    End = PageCount;
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
        if (PageCount > 1)
        {
            for (var x = 1; x <= PageCount; x++)
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
        if (PageCount > 1)
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
        if (PageCount > 1)
        {
            checked
            {
                if (PageCount >= (_currentPage + 10))
                    return _currentPage + 10;
            }
        }
        return -1;
    }

    private int GetNextTwenty()
    {
        if (PageCount > 1)
        {
            checked
            {
                if (PageCount >= (_currentPage + 20))
                    return _currentPage + 20;
            }
        }
        return -1;
    }

    private int GetNextFifty()
    {
        if (PageCount > 1)
        {
            checked
            {
                if (PageCount >= (_currentPage + 50))
                    return _currentPage + 50;
            }
        }
        return -1;
    }

    private int GetNextHundred()
    {
        if (PageCount > 1)
        {
            checked
            {
                if (PageCount >= (_currentPage + 100))
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
        return PageCount;
    }
}
