# RetouchAgency

## Project Overview
This is a multi-layered ASP.NET Core project with:
- **API Layer** (Controllers)
- **Business Logic Layer (BLL)**
- **Data Access Layer (DAL)**

All Models and repositories are already implemented. To add new features, you only need to:
- Create a DTO (Data Transfer Object)
- Implement a Manager (business logic)
- Add an API endpoint (controller)

## Prerequisites
- [.NET SDK 8.0 or higher](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

## Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone <your-repo-url>
   cd RetouchAgency
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the project:**
   ```bash
   cd RetouchAgency
   dotnet run
   ```
   The API will be available at `https://localhost:<port>` or `http://localhost:<port>`.

---

## How to Add a New API Endpoint (For This Project)

### 1. Create a DTO (Data Transfer Object)
**What is a DTO?**
A DTO is a simple class used to transfer data between layers (e.g., from the API to the BLL, or from the BLL to the API). It usually contains only the properties you want to expose or accept from the client, and not navigation properties or sensitive fields.

- Example: `BLL/DTOs/ProductDTO.cs`
  ```csharp
  public class ProductDTO
  {
      public int Id { get; set; }
      public string Name { get; set; }
      public decimal Price { get; set; }
  }
  ```

### 2. Implement the Manager (Business Logic Layer)
- Create an interface in `BLL/Manager/Interfaces/IProductManager.cs`:
  ```csharp
  public interface IProductManager
  {
      Task<IEnumerable<ProductDTO>> GetAllAsync();
      Task<ProductDTO?> GetByIdAsync(int id);
      Task<int> CreateAsync(ProductDTO dto);
      Task UpdateAsync(int id, ProductDTO dto);
      Task DeleteAsync(int id);
  }
  ```
- Implement the manager in `BLL/Manager/ProductManager.cs`:
  ```csharp
  public class ProductManager(IUnitOfWork uow) : IProductManager
  {
      private readonly IUnitOfWork _uow = uow;
      public async Task<IEnumerable<ProductDTO>> GetAllAsync() =>
          (await _uow.Products.GetAllAsync()).Select(p => new ProductDTO { Id = p.Id, Name = p.Name, Price = p.Price });
      public async Task<ProductDTO?> GetByIdAsync(int id) =>
          (await _uow.Products.GetByIdAsync(id)) is Product p ? new ProductDTO { Id = p.Id, Name = p.Name, Price = p.Price } : null;
      public async Task<int> CreateAsync(ProductDTO dto)
      {
          var product = new Product { Name = dto.Name, Price = dto.Price };
          await _uow.Products.AddAsync(product);
          await _uow.SaveAllAsync();
          return product.Id;
      }
      public async Task UpdateAsync(int id, ProductDTO dto)
      {
          var product = await _uow.Products.GetByIdAsync(id) ?? throw new KeyNotFoundException();
          product.Name = dto.Name;
          product.Price = dto.Price;
          _uow.Products.Update(product);
          await _uow.SaveAllAsync();
      }
      public async Task DeleteAsync(int id)
      {
          await _uow.Products.DeleteAsync(id);
          await _uow.SaveAllAsync();
      }
  }
  ```

### 3. Register the Manager in Dependency Injection (Program.cs)
- In `RetouchAgency/Program.cs`, add:
  ```csharp
  builder.Services.AddScoped<IProductManager, ProductManager>();
  ```

### 4. Add the Controller (API Layer)
- In `RetouchAgency/Controllers/ProductController.cs`:
  ```csharp
  [ApiController]
  [Route("api/[controller]")]
  public class ProductController(IProductManager manager) : ControllerBase
  {
      [HttpGet]
      public async Task<IActionResult> GetAll() => Ok(await manager.GetAllAsync());
      [HttpGet("{id}")]
      public async Task<IActionResult> GetById(int id) =>
          (await manager.GetByIdAsync(id)) is ProductDTO dto ? Ok(dto) : NotFound();
      [HttpPost]
      public async Task<IActionResult> Create(ProductDTO dto)
      {
          var id = await manager.CreateAsync(dto);
          return CreatedAtAction(nameof(GetById), new { id }, dto);
      }
      [HttpPut("{id}")]
      public async Task<IActionResult> Update(int id, ProductDTO dto)
      {
          await manager.UpdateAsync(id, dto);
          return NoContent();
      }
      [HttpDelete("{id}")]
      public async Task<IActionResult> Delete(int id)
      {
          await manager.DeleteAsync(id);
          return NoContent();
      }
  }
  ```

### 5. Test Your Endpoint
- Run the project and use Postman or Swagger UI to test your new API endpoints.

---

## Tips for Beginners
- DTOs are for data transfer only—never put business logic in them.
- Managers (BLL) contain business rules and use the repositories via `IUnitOfWork`.
- Controllers should be thin: just call the manager and return results.
- Always run `dotnet restore` after pulling new code.
- Use `dotnet ef migrations add <Name>` and `dotnet ef database update` to manage database changes.
- Use dependency injection to keep your code clean and testable.

---

## Useful Commands
- `dotnet run` — Run the API
- `dotnet build` — Build the solution
- `dotnet ef migrations add <Name>` — Add a new migration
- `dotnet ef database update` — Apply migrations to the database

---

## Need Help?
If you get stuck, check the official [ASP.NET Core documentation](https://learn.microsoft.com/aspnet/core/) or ask for help in the project issues.
