using BuberBreakfast.Contracts.Breakfast;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class BreakfastController : ApiControllers
{
    private readonly IBreakfastServices _breakfastServices;

    public BreakfastController(IBreakfastServices breakfastServices)
    {
        _breakfastServices = breakfastServices;
    }

    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var requestToBreakfastResult = Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );
        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }

        var breakfast = requestToBreakfastResult.Value;

        ErrorOr<Created> createBreakfastResult = _breakfastServices.CreateBreakfast(breakfast);
        return createBreakfastResult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            error => Problem(error)
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastServices.GetBreakfast(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastRespone(breakfast)),
            error => Problem(error)
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var requestToBreakfastResult = Breakfast.From(id ,request);

        if (requestToBreakfastResult.IsError)
        {
            return Problem(requestToBreakfastResult.Errors);
        }
        var breakfast = requestToBreakfastResult.Value;

        var upsertBreakfastResult = _breakfastServices.UpsertBreakfast(breakfast);
        
        return upsertBreakfastResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            error => Problem(error)
        );
    }
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> deleteBreakfastResult = _breakfastServices.DeleteBreakfast(id);
        
        return deleteBreakfastResult.Match(
            breakfast => NoContent(),
            error => Problem(error)
        );
    }
    private static BreakfastRespone MapBreakfastRespone(Breakfast breakfast)
    {
        return new BreakfastRespone(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );
    }
    private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
        => CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapBreakfastRespone(breakfast));
}

