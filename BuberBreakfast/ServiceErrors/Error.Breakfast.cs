using ErrorOr;

public static class Errors
{
    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
            code: $"Breakfast.InvalidName",
            description: $"Breakfast name must be between {1} and {50} characters long."
        );
        public static Error InvalidDescription => Error.Validation(
            code: $"Breakfast.InvalidDescription",
            description: $"Breakfast description must be less than {150} characters long."
        );

        public static Error NotFound => Error.NotFound(
            code: $"Breakfast was not found.",
            description: "breakfast not found"
        );
    }   
}