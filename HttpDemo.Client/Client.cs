using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using HttpDemo.Shared.Dtos;

namespace HttpDemo.Client;

public sealed class Client
{
    private readonly string mainFolder;
    private  readonly string fullPath;
    private readonly string categories;
    private readonly string films;
    private readonly HttpClient httpClient;
    
    public Client(string baseUrl)
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        this.mainFolder = "cinema";
        this.fullPath = $"/api/{mainFolder}";
        this.categories = "Category";
        this.films = "Film";

    }

    
    public async Task<Dictionary<int, string>> GetCategoris()
    {
        var response = await httpClient.GetAsync($"{this.fullPath}/{this.categories}");
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Your request failed with status code {response.StatusCode}");
        var categorisDtos = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        Dictionary<int, string> returnValue = new Dictionary<int, string>();
        foreach (var item in categorisDtos)
        {
            returnValue.Add(item.Id, item.Name);
        }
        return returnValue;
    }


    public async Task<string> CreateAsyncFilm(CreateFilmDto createFilmDto)
    {
        var content = JsonContent.Create(createFilmDto);
        
        var response = await httpClient.PostAsync(this.fullPath+$"/{films}", content);
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var filmDto = await response.Content.ReadFromJsonAsync<FilmDto>();
        return $"Film with id {filmDto?.Id} was created";
    }
    
    public async Task<string> UpdateAsyncFilm(int id, FilmDto updateFilmDto)
    {
        var content = JsonContent.Create(updateFilmDto);
        
        var response = await httpClient.PutAsync($"{fullPath}/{films}/{id}", content);
        return response.IsSuccessStatusCode
            ? $"Film with id {id} was updated"
            : $"Your request failed with status code {response.StatusCode}";
    }
    
    public async Task<string> DeleteFilmAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{fullPath}/{films}/{id}");
        return response.IsSuccessStatusCode
            ? $"Film with id {id} was deleted"
            : $"Your request failed with status code {response.StatusCode}";
    }

      
    public async Task<string> GetFilmAsync(int id)
    {
        var response = await httpClient.GetAsync($"{fullPath}/{films}/{id}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var filmDto = await response.Content.ReadFromJsonAsync<FilmDto>();
        return $"Film with id {filmDto?.Id} (Name: {filmDto?.Name}, Categories: {String.Join(", ", filmDto?.Categories)}) was found";
    }
    
    public async Task<string> GetAllFilmsAsync()
    {
        var response = await httpClient.GetAsync($"{fullPath}/{films}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var filmDtos = await response.Content.ReadFromJsonAsync<List<FilmDto>>();
        var result = filmDtos?.Aggregate("Film list:", (current, filmDto) => 
            current + $"\nFilm with id {filmDto.Id} (Name: {filmDto.Name}, Categories: {String.Join(", ", filmDto.Categories)})");
        return result ?? "Film list is empty";
    }

    public async Task<string> CreateAsyncCategory(CreateCategoryDto createCategoryDto)
    {
        var content = JsonContent.Create(createCategoryDto);

        var response = await httpClient.PostAsync($"{fullPath}/{categories}", content);
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";

        var categoryDto = await response.Content.ReadFromJsonAsync<CategoryDto>();
        return $"Category with id {categoryDto?.Id} was created";
    }

    public async Task<string> UpdateAsyncCategory(int id, CategoryDto updateCategoryDto)
    {
        var content = JsonContent.Create(updateCategoryDto);

        var response = await httpClient.PutAsync($"{fullPath}/{categories}/{id}", content);
        return response.IsSuccessStatusCode
            ? $"Category with id {id} was updated"
            : $"Your request failed with status code {response.StatusCode}";
    }

    public async Task<string> DeleteCategoryAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{fullPath}/{categories}/{id}");
        return response.IsSuccessStatusCode
            ? $"Category with id {id} was deleted"
            : $"Your request failed with status code {response.StatusCode}";
    }

    public async Task<string> GetCategoryAsync(int id)
    {
        var response = await httpClient.GetAsync($"{fullPath}/{categories}/{id}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";

        var CategoryDto = await response.Content.ReadFromJsonAsync<CategoryDto>();
        return $"Category with id {CategoryDto?.Id} (Name: {CategoryDto?.Name}) was found";
    }

    public async Task<string> GetAllCategoriesAsync()
    {
        var response = await httpClient.GetAsync($"{fullPath}/{this.categories}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";

        var categoryDtos = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        var result = categoryDtos?.Aggregate("Category list:", (current, categoryDto) =>
            current + $"\nCategory with id {categoryDto.Id} (Name: {categoryDto.Name})");
        return result ?? "Category list is empty";
    }
    public async Task<string> GetAllFilmsThatContainTheCategoryAsync(string category)
    {
        var response = await httpClient.GetAsync($"{fullPath}/{this.films}/{category}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";

        var filmDtos = await response.Content.ReadFromJsonAsync<List<FilmDto>>();
        var result = filmDtos?.Aggregate("Film list:", (current, FilmDto) =>
            current + $"\nFilm with id {FilmDto.Id} (Name: {FilmDto.Name}, Categories: {String.Join(", ", FilmDto?.Categories)})");
        return result ?? "Film list is empty";
    }
    

}