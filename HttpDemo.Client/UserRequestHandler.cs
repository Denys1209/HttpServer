using HttpDemo.Shared.Dtos;
using System.Linq;

namespace HttpDemo.Client;

public sealed class UserRequestHandler
{
    private readonly Client client;
    
    public UserRequestHandler(Client client)
    {
        this.client = client;
    }
    private async Task<string[]> GetCategoriesByIds() 
    {
        var categoriesDiction = await client.GetCategoris();
        foreach (var category in categoriesDiction)
        {
            Console.WriteLine($"{category.Key} = {category.Value}");
        }
        Console.WriteLine("Enter categories ids(separated by spaces):");
        var ids = Console.ReadLine().Trim().Split(" ").Distinct().ToList();
        List<string> categories = new List<string>();
        foreach (var id in ids)
        {
            if (!(int.TryParse(id, out var parsedId) || (categoriesDiction.ContainsKey(parsedId))))
            {
                Console.WriteLine($"Invalid id {id}");
                throw new Exception($"Invalid id {id}");
            }
            categories.Add(categoriesDiction[parsedId]);
        }
        return categories.ToArray();
    }
    public async Task<bool> HandleAsync(UserRequest request)
    {
        return request switch
        {
            UserRequest.CreateFilm => await CreateFilmAsync(),
            UserRequest.CreateCategory => await CreateCategoryAsync(),
            UserRequest.UpdateFilm => await UpdateFilmAsync(),
            UserRequest.DeleteFilm => await DeleteFilmAsync(),
            UserRequest.GetFilm => await GetFilmAsync(),
            UserRequest.GetAllFilms => await GetAllFilmsAsync(),
            UserRequest.UpdateCategory => await UpdateCategoryAsync(),
            UserRequest.DeleteCategory => await DeleteCategoryAsync(),
            UserRequest.GetCategory => await GetCategoryAsync(),
            UserRequest.GetAllCategories => await GetAllCategoriesAsync(),
            UserRequest.GetAllFilmWithTheCategory => await GetAllFilmByCategory(),
            UserRequest.Exit => false,
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
        };
    }
    
    private async Task<bool> CreateFilmAsync()
    {
        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name");
            return false;
        }
        string[] categories = null;
        while (categories == null) 
        {
            try
            {
                categories = await GetCategoriesByIds() ?? new string[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        var message = await client.CreateAsyncFilm(new CreateFilmDto
        {
            Name = name,
            Categories = categories,
        });
        Console.WriteLine(message);
        return true;
    }
    
    private async Task<bool> UpdateFilmAsync()
    {
        Console.WriteLine("Enter id:");
        var id = Console.ReadLine();
        if (!int.TryParse(id, out var parsedId))
        {
            Console.WriteLine("Invalid id");
            return false;
        }
        
        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name");
            return false;
        }
        
        string[] categories = null;
        while (categories == null) 
        {
            try
            {
                categories = await GetCategoriesByIds();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        var message = await client.UpdateAsyncFilm(parsedId, new FilmDto(parsedId, name, categories));
        Console.WriteLine(message);
        return true;
    }
    
    private async Task<bool> DeleteFilmAsync()
    {
        Console.WriteLine("Enter id:");
        var id = Console.ReadLine();
        if (!int.TryParse(id, out var parsedId))
        {
            Console.WriteLine("Invalid id");
            return false;
        }
        
        var message = await client.DeleteFilmAsync(parsedId);
        Console.WriteLine(message);
        return true;
    }
    
    private async Task<bool> GetFilmAsync()
    {
        Console.WriteLine("Enter id:");
        var id = Console.ReadLine();
        if (!int.TryParse(id, out var parsedId))
        {
            Console.WriteLine("Invalid id");
            return false;
        }
        
        var message = await client.GetFilmAsync(parsedId);
        Console.WriteLine(message);
        return true;
    }
    
    private async Task<bool> GetAllFilmsAsync()
    {
        var message = await client.GetAllFilmsAsync();
        Console.WriteLine(message);
        return true;
    }
    private async Task<bool> CreateCategoryAsync()
    {
        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name");
            return false;
        }

        var message = await client.CreateAsyncCategory(new CreateCategoryDto
        {
            Name = name
        });
        Console.WriteLine(message);
        return true;
    }

    private async Task<bool> UpdateCategoryAsync()
    {
        Console.WriteLine("Enter id:");
        var id = Console.ReadLine();
        if (!int.TryParse(id, out var parsedId))
        {
            Console.WriteLine("Invalid id");
            return false;
        }

        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name");
            return false;
        }

        var message = await client.UpdateAsyncCategory(parsedId, new CategoryDto(parsedId, name));
        Console.WriteLine(message);
        return true;
    }

    private async Task<bool> DeleteCategoryAsync()
    {
        Console.WriteLine("Enter id:");
        var id = Console.ReadLine();
        if (!int.TryParse(id, out var parsedId))
        {
            Console.WriteLine("Invalid id");
            return false;
        }

        var message = await client.DeleteCategoryAsync(parsedId);
        Console.WriteLine(message);
        return true;
    }

    private async Task<bool> GetCategoryAsync()
    {
        Console.WriteLine("Enter id:");
        var id = Console.ReadLine();
        if (!int.TryParse(id, out var parsedId))
        {
            Console.WriteLine("Invalid id");
            return false;
        }

        var message = await client.GetCategoryAsync(parsedId);
        Console.WriteLine(message);
        return true;
    }

    private async Task<bool> GetAllCategoriesAsync()
    {
        var message = await client.GetAllCategoriesAsync();
        Console.WriteLine(message);
        return true;
    }

    private async Task<bool> GetAllFilmByCategory() 
    {
        Console.WriteLine("Enter name:");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name");
            return false;
        }

        var message = await client.GetAllFilmsThatContainTheCategoryAsync(name);
        Console.WriteLine(message);
        return true;
    }



}