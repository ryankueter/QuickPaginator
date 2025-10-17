# Quick Paginator (.NET)

Author: Ryan Kueter  
Updated: April, 2025

## About

**Quick Paginator** is a free .NET library, available from the [NuGet Package Manager](https://www.nuget.org/packages/QuickPaginator), that provides a quick way to paginate a list of items.  

### Targets:
- .NET 6 - .NET 9

### Features
- Create a simple or complex paginator using the buttons of your choice.
- Change the maximum number of buttons displayed, the default is seven.
- Easily work with Linq's Skip and Take functionality.
- List all buttons for any pagination scenario.

## Properties

The QuickPaginator provides a number of properties that contain calculated values for each page.

### Instantiation

```csharp
// CurrentPage, ResultsCount, PageLimit (optional - default: 10), ButtonCount (optional - default: 7)
var Pager = new Paginator(50, 1000, 10, 11);

// CurrentPage, ResultsCount, PageLimit (optional - default: 10)
var Pager = new Paginator(10, 100, 10);

// If you want 10 per page and seven buttons:
var Pager = new Paginator(10, 100);

// Or to satisfy a nullability check:
var Pager = new Paginator();
```

### Skip & Take
These values work with Linq skip and take:
```csharp
Pager.Skip
// Output: 90

Pager.Take
// Output: 10

Example:
result.Skip(Pager.Skip).Take(Pager.Take).ToList();
```

### First & Last
The first and last pages:
```csharp
Pager.First
// Output: 1

Pager.Last
// Output: 10
```

### Previous & Next
Since we are on page 10 and don't have a next page, Next returns a value of -1.
```csharp
Pager.Previous
// Output: 9

Pager.Next
// Output: -1

// Additionally:
Pager.PreviousTen
Pager.PreviousTwenty
Pager.PreviousThirty
Pager.PreviousFourty
Pager.PreviousFifty
Pager.PreviousHundred
Pager.GetNext(120) // a custom next button

Pager.NextTen
Pager.NextTwenty
Pager.NextThirty
Pager.NextFourty
Pager.NextFifty
Pager.NextHundred
Pager.GetPrevious(120) // a custom previous button
```

### Current Count & Total Count
The current count contains the number of total items viewed. And the total count contains the total number of items.

In the following example, since we are on page ten with ten items per page, we have viewed the total number of items.
```csharp
Pager.CurrentCount
// Output: 100

Pager.TotalCount
// Output: 100
```

### BetweenPages
BetweenPages is a dictionary array that stores a list of buttons between the previous and next buttons. The between pages array by default only returns 7 buttons. For example, if you are on page 1 of 100 pages, you will see pages 1 - 7. If you are on page 8 of 100 pages, you will see pages 5 - 11 to keep the current page centered.

BetweenPages has a default of 7 buttons. But you can change this default by specifying the fourth parameter in the constructor. 

The following example produces 100 pages of results and the current page is 20.
```csharp
foreach (var b in Pager.BetweenPages)
{
    Console.WriteLine($"Key: {b.Key}, Value:{b.Value}");
}

// Output (default 7 buttons)
Key: 17, Value:
Key: 18, Value:
Key: 19, Value:
Key: 20, Value:active
Key: 21, Value:
Key: 22, Value:
Key: 23, Value:

// If you want to specify the number of buttons, include the fourth parameter:
new Paginator(20, 1000, 10, 11);

// Output 
Key: 15, Value:
Key: 16, Value:
Key: 17, Value:
Key: 18, Value:
Key: 19, Value:
Key: 20, Value:active
Key: 21, Value:
Key: 22, Value:
Key: 23, Value:
Key: 24, Value:
Key: 25, Value:
```

### AllPages
AllPages is a dictionary array that stores a list of all the pages.

In this example, page 10 is the active page.
```csharp
foreach (var b in Pager.AllPages)
{
    Console.WriteLine($"Key: {b.Key}, Value:{b.Value}");
}

// Output
Key: 1, Value:
Key: 2, Value:
Key: 3, Value:
Key: 4, Value:
Key: 5, Value:
Key: 6, Value:
Key: 7, Value:
Key: 8, Value:
Key: 9, Value:
Key: 10, Value:active
```

## A Razor Pages Example

The following is an example of how you would initialize the Paginator:
```csharp
using QuickPaginator;

public class UsersModel : PageModel
{
    private readonly IUsersDataService _usersDataService;
	
    public Paginator Pager { get; set; }

    [BindProperty]
    public List<User> ActiveUsers { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Number { get; set; } = 1;

    public int PageCount { get; set; } = 10;

    public UsersModel(IUsersDataService usersDataService)
    {
        _usersDataService = usersDataService;
        ActiveUsers = new();
    }

	public async Task OnGetAsync()
    {
        if (ModelState.IsValid)
        {
            var response = await _usersDataService.GetAllUsers();
            if (response.Response is not null && response.Response.IsSuccess())
            {
                var result = response.Users.ToList();

                // Paginator(<CurrentPageNumber>, <ResultCount>, <PageCount>)
                Pager = new Paginator(Number, result.Count(), PageCount);

                // Active Users
                ActiveUsers = result.Skip(Pager.Skip).Take(Pager.Take).ToList();
            }
        }
    }
}
```

The following is an example of how you would use the Paginator in a Razor pages application.
```csharp
<nav class="app-pagination">
	@{
		var pager = Model.Pager;
	}
	<ul class="pagination justify-content-center">
		@if (pager.PageCount <= 1)
		{
			<li class="page-item active">No more results.</li>
		}
		else
		{
			@if (pager.Previous is not -1)
			{
				<li class="page-item">
					<a class="page-link" href="/Admin/Users/@pager.First">First</a>
				</li>
				<li class="page-item">
					<a class="page-link" href="/Admin/Users/@pager.Previous">Previous</a>
				</li>
			}
			else
			{
				<li class="page-item disabled">
					<a class="page-link" tabindex="-1" aria-disabled="true">First</a>
				</li>
				<li class="page-item disabled">
					<a class="page-link" tabindex="-1" aria-disabled="true">Previous</a>
				</li>
			}
			@foreach (var b in pager.BetweenPages)
			{
				<li class="page-item @b.Value"><a class="page-link" href="/Admin/Users/@b.Key">@b.Key</a></li>
			}
			@if (pager.Next is not -1)
			{
				<li class="page-item">
					<a class="page-link" href="/Admin/Users/@pager.Next">Next</a>
				</li>
				<li class="page-item">
					<a class="page-link" href="/Admin/Users/@pager.Last">Last</a>
				</li>
			}
			else
			{
				<li class="page-item disabled">
					<a class="page-link" tabindex="-1" aria-disabled="true">Next</a>
				</li>
				<li class="page-item disabled">
					<a class="page-link" tabindex="-1" aria-disabled="true">Last</a>
				</li>
			}
		}
	</ul>
	<div class="d-flex justify-content-center">
		@if (pager.PageCount >= 1)
		{
			<span>(@pager.CurrentCount of @pager.TotalCount results)</span>
		}
	</div>
</nav>
```
  
###
## Contributions

Quick Paginator is being developed for free by me, Ryan Kueter, in my spare time. So, if you use this library and see a need for improvement, please send your ideas.