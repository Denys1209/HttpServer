namespace HttpDemo.Client;

public enum UserRequest
{
    Exit = -1,
    CreateFilm = 1,
    CreateCategory,
    UpdateFilm,
    DeleteFilm,
    UpdateCategory,
    DeleteCategory,
    GetFilm,
    GetAllFilms,
    GetCategory,
    GetAllCategories,
    GetAllFilmWithTheCategory
}