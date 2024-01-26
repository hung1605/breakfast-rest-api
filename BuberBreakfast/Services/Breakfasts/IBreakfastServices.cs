using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;

public interface IBreakfastServices
{
    ErrorOr<Created> CreateBreakfast(Breakfast breakfast);
    ErrorOr<Deleted> DeleteBreakfast(Guid id);
    ErrorOr<Breakfast> GetBreakfast(Guid id);
    ErrorOr<UpsertBreakfast> UpsertBreakfast(Breakfast breakfast);
}