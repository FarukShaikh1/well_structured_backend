// API/Controllers/FamilyController.cs
using FMS_Collection.API.Authorization;
using FMS_Collection.Application.Services;
using FMS_Collection.Core.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FMS_Collection.API.Controllers;

[ApiController]
//[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class FamilyController(FamilyService service) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ── Search ────────────────────────────────────────────────────────────────

    /// <summary>Search persons by name (max 20 results).</summary>
    [HttpGet("search")]
    //[RequirePermission("Family.View")]
    public async Task<IActionResult> Search([FromQuery] string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "name query parameter is required." });

            var result = await service.SearchPersonsAsync(name.Trim());
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Graph ─────────────────────────────────────────────────────────────────

    /// <summary>Return full graph (nodes + edges) for a given root person.</summary>
    [HttpGet("{personId:guid}/graph")]
    //[RequirePermission("Family.View")]
    public async Task<IActionResult> GetGraph(Guid personId, [FromQuery] int maxDepth = 5)
    {
        try
        {
            var result = await service.GetGraphAsync(personId, maxDepth);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Person CRUD ───────────────────────────────────────────────────────────
    [Authorize]
    [HttpPost("person")]
    [RequirePermission("Family.Create")]
    public async Task<IActionResult> AddPerson([FromBody] FamilyPersonRequest request)
    {
        try
        {
            var result = await service.AddPersonAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpPut("person")]
    [RequirePermission("Family.Update")]
    public async Task<IActionResult> UpdatePerson([FromBody] FamilyPersonRequest request)
    {
        try
        {
            if (request.PersonId == null)
                return BadRequest(new { message = "PersonId is required for update." });

            var result = await service.UpdatePersonAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // ── Relationship CRUD ─────────────────────────────────────────────────────

    [Authorize]
    [HttpPost("relationship")]
    [RequirePermission("Family.Create")]
    public async Task<IActionResult> AddRelationship([FromBody] FamilyRelationshipRequest request)
    {
        try
        {
            var result = await service.AddRelationshipAsync(request, CurrentUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [Authorize]
    [HttpDelete("relationship/{relationshipId:guid}")]
    [RequirePermission("Family.Delete")]
    public async Task<IActionResult> DeleteRelationship(Guid relationshipId)
    {
        try
        {
            var result = await service.DeleteRelationshipAsync(relationshipId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
}
