# RailcarTrips
## Pages
- RailcarTrips - allows you to start trips with 'W' and end with 'Z'
- Trips - displays currently on the sql server
- Cities - displays currently on the sql server
- Equipment Events - displays currently on the sql server

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

# Assumption to Solve Solution
- There can only be one active trip at a given time per railcar (can't double up)
- A railcar can make multiple trips overtime, not at the same time
- Sorting timestamps is required before processing csv file; processed in order after sorting.
- No room errors within Trips
- Trip timezone stays in UTC
- Equipment IDs are unique

# Learning Resources
- https://www.udemy.com/course/blazor-deep-dive-from-beginner-to-advanced/
