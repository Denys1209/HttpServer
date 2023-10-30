using HttpDemo.Client;
var client = new Client("https://localhost:7083/");
var userRequestHandler = new UserRequestHandler(client);

Console.WriteLine("Instructions:");
Console.WriteLine("-1. Exit: exit");
Console.WriteLine($"{(int)UserRequest.CreateFilm}. Create a new film: create <name> <categories>");
Console.WriteLine($"{(int)UserRequest.CreateCategory}. Create a new category: create <name>");
Console.WriteLine($"{(int)UserRequest.UpdateFilm}. Update an existing film: update <id> <name> <categories>");
Console.WriteLine($"{(int)UserRequest.DeleteFilm}. Delete an existing film: delete <id>");
Console.WriteLine($"{(int)UserRequest.UpdateCategory}. Update an existing category: update <id> <name>");
Console.WriteLine($"{(int)UserRequest.DeleteCategory}. Delete an existing category: delete <id>");
Console.WriteLine($"{(int)UserRequest.GetFilm}. Get a film: get <id>");
Console.WriteLine($"{(int)UserRequest.GetAllFilms}. Get all existing films: getallfilms");
Console.WriteLine($"{(int)UserRequest.GetCategory}. Get a category: getcategory <id>");
Console.WriteLine($"{(int)UserRequest.GetAllCategories}. Get all existing categories: getallcategories"); 
Console.WriteLine($"{(int)UserRequest.GetAllFilmWithTheCategory}. Get all film with the category: GetAllFilmWithTheCategory <name>"); 

while (true)
{
    Console.WriteLine("Enter what you need...");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var instruction) || !Enum.IsDefined(typeof(UserRequest), instruction))
    {
        Console.WriteLine("Invalid input");
        continue;
    }
    
    var result = await userRequestHandler.HandleAsync((UserRequest)instruction);
    if (!result)
        break;
}

