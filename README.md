# RailcarTrips
## Pages
- RailcarTrips - shows all successful finished trips
- Trips - displays currently on the sql server
- Cities - displays currently on the sql server
- Equipment Events - displays currently on the sql server
- Trip details - displays all events in order for a trip

- DST does work! (previous assumption fixed)

# Solution Deployed to Azure  
- WebApp deployment: https://railcartripsclient.azurewebsites.net/ 

# Tech Stack:
- Blazor Global WebAssembly Webapp - .net8.0
- Azure Sql Server, Azure Sql Database, and Azure Key Vault
- Azure WebService Plan and WebApp for client side (sql connected to server side)
- Entity Framework Core
- Github repository
- Runtime .NET 8.0

# Additional additions if not for time constraints
- Logging!
- add Azure Logs and app insights
- authorization and authentication
- Figure out good solution for daylight savings time changing at different times for different locations.
- Different functionality for A/D event codes
- Add data binding
- Unit testing
- Pagination instead of loading entire table at once - but have virtualization and @key
- Print out pending trips somewhere
- Not print out multiple trips in the same trip details page.
- Add data binding
- Sort all the pages and make them look better
- improve encapsulation of fields with sensitive information
- Show ongoing trips

# Assumption to Solve Solution
- There can only be one active trip at a given time per railcar (can't double up)
- A railcar can make multiple trips overtime, not at the same time
- Sorting timestamps is required before processing csv file; processed in order after sorting.
- If a city updates its time zone rule, it will only affect future trip calculations
- Railcar trips page only shows completed trips

# Learning Resources
- https://www.udemy.com/course/blazor-deep-dive-from-beginner-to-advanced/
