# Quick Paginator (.NET)

Author: Ryan Kueter  
Updated: September, 2022

## About

**Quick Paginator** is a free .NET library, available from the [NuGet Package Manager](https://www.nuget.org/packages/QuickPaginator), that provides a quick way to paginate a list of items.  

### Targets:
- .NET 6

## Use Case

The following is an example of how you would initialize the Paginator:
```csharp
using QuickPaginator;

public class UsersModel : PageModel
{
    private readonly IUsersDataService _usersDataService;
	
    [BindProperty]
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
                UserPager = new Paginator(Number, result.Count(), PageCount);

                // Active Users
                ActiveUsers = result.Skip(UserPager.Skip).Take(UserPager.Take).ToList();
            }
        }
    }
}
```

The following is an example of how you would use the Paginator in a Razor pages application.
```csharp
<nav class="app-pagination">
	@{
		var pager = Model.UserPager;
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
					<a class="page-link" href="~/Admin/Users/@pager.First">First</a>
				</li>
				<li class="page-item">
					<a class="page-link" href="~/Admin/Users/@pager.Previous">Previous</a>
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
				<li class="page-item @b.Value"><a class="page-link" href="~/Admin/Users/@b.Key">@b.Key</a></li>
			}
			@if (pager.Next is not -1)
			{
				<li class="page-item">
					<a class="page-link" href="~/Admin/Users/@pager.Next">Next</a>
				</li>
				<li class="page-item">
					<a class="page-link" href="~/Admin/Users/@pager.Last">Last</a>
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