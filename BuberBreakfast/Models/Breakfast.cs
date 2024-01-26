using BuberBreakfast.Contracts.Breakfast;
using ErrorOr;
public class Breakfast
{
    public const int MinNameLength = 1;
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 150;
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }

    public Breakfast(
        Guid id, 
        string name, 
        string description, 
        DateTime startDatetime, 
        DateTime endDatetime, 
        DateTime lastModifiedDateTime, 
        List<string> savory, 
        List<string> sweet)
    {

        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startDatetime;
        EndDateTime = endDatetime;
        LastModifiedDateTime = lastModifiedDateTime;
        Savory = savory;
        Sweet = sweet;
    }
    public static ErrorOr<Breakfast> Create(
        string name, 
        string description, 
        DateTime startDatetime, 
        DateTime endDatetime, 
        List<string> savory, 
        List<string> sweet,
        Guid? id = null)
    {
        List<Error> errors = new();
        if (name.Length is < MinNameLength or > MaxNameLength)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }
        if (description.Length > MaxDescriptionLength)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
        }
        if (errors.Any())
        {
            return errors;
        }
        return new Breakfast(
            id ?? Guid.NewGuid(),
            name,
            description,
            startDatetime,
            endDatetime,
            DateTime.UtcNow,
            savory,
            sweet
        );
    }

    public static ErrorOr<Breakfast> From(CreateBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );
    }
    public static ErrorOr<Breakfast> From(Guid id, UpsertBreakfastRequest request)
    {
        return Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet,
            id
        );
    }
}