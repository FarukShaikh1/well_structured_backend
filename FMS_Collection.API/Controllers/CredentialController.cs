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
public class CredentialController(ICredentialRepository credentialRepository) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [RequirePermission("Credential.View")]
    public async Task<IActionResult> GetList()
    {
        var result = await credentialRepository.GetByUserAsync(CurrentUserId);
        return Ok(result);
    }

    [HttpGet("{credentialId:guid}")]
    [RequirePermission("Credential.View")]
    public async Task<IActionResult> GetDetails(Guid credentialId)
    {
        var result = await credentialRepository.GetDetailsAsync(credentialId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [RequirePermission("Credential.Create")]
    public async Task<IActionResult> Add([FromBody] Credential credential)
    {
        var newId = await credentialRepository.AddAsync(credential, CurrentUserId);
        return Ok(newId);
    }

    [HttpPut]
    [RequirePermission("Credential.Update")]
    public async Task<IActionResult> Update([FromBody] Credential credential)
    {
        await credentialRepository.UpdateAsync(credential, CurrentUserId);
        return Ok();
    }

    [HttpDelete("{credentialId:guid}")]
    [RequirePermission("Credential.Delete")]
    public async Task<IActionResult> Delete(Guid credentialId)
    {
        await credentialRepository.DeleteAsync(credentialId, CurrentUserId);
        return Ok();
    }
}
