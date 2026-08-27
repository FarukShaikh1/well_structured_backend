using FMS_Collection.API.Authorization;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class FoodMenuController(IFoodMenuRepository foodMenuRepository) : ControllerBase
{
    private Guid CurrentUserId =>Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


    // -------------------- GET LIST --------------------

    [HttpGet]
    [RequirePermission("FoodMenu.View")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var result =
                await foodMenuRepository.GetByUserAsync(
                    CurrentUserId);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
        }
    }


    // -------------------- GET DETAILS --------------------

    [HttpGet("{foodMenuId:guid}")]
    [RequirePermission("FoodMenu.View")]
    public async Task<IActionResult> GetDetails(
        Guid foodMenuId)
    {
        try
        {
            var result =
                await foodMenuRepository.GetDetailsAsync(
                    foodMenuId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
        }
    }


    // -------------------- ADD --------------------

    [HttpPost]
    [RequirePermission("FoodMenu.Create")]
    public async Task<IActionResult> Add(
        [FromBody] FoodMenu foodMenu)
    {
        try
        {
            var newId =
                await foodMenuRepository.AddAsync(
                    foodMenu,
                    CurrentUserId);

            return Ok(newId);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
        }
    }


    // -------------------- UPDATE --------------------

    [HttpPut]
    [RequirePermission("FoodMenu.Update")]
    public async Task<IActionResult> Update(
        [FromBody] FoodMenu foodMenu)
    {
        try
        {
            await foodMenuRepository.UpdateAsync(
                foodMenu,
                CurrentUserId);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
        }
    }


    // -------------------- DELETE --------------------

    [HttpDelete("{foodMenuId:guid}")]
    [RequirePermission("FoodMenu.Delete")]
    public async Task<IActionResult> Delete(
        Guid foodMenuId)
    {
        try
        {
            await foodMenuRepository.DeleteAsync(
                foodMenuId,
                CurrentUserId);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
        }
    }
}