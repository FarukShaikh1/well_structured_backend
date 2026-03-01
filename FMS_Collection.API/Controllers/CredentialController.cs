using FMS_Collection.Core.Common;
using FMS_Collection.Core.Entities;
using FMS_Collection.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CredentialController : ControllerBase
{
    private readonly ICredentialRepository _credentialRepository;

    public CredentialController(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    // ==============================
    // GET: api/credential
    // ==============================
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<Credential>>> GetAll()
    {
        var result = await _credentialRepository.GetAllAsync();
        return Ok(result);
    }

    // ==============================
    // GET: api/credential/user/{userId}
    // ==============================
    [HttpGet("GetList")]
    public async Task<ActionResult<List<Credential>>> GetByUser(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("Invalid UserId");

        var result = await _credentialRepository.GetByUserAsync(userId);
        return Ok(result);
    }

    // ==============================
    // GET: api/credential/{credentialId}
    // ==============================
    [HttpGet("Details/{credentialId}")]
    public async Task<ActionResult<Credential>> GetDetails(Guid credentialId)
    {
        if (credentialId == Guid.Empty)
            return BadRequest("Invalid CredentialId");

        var result = await _credentialRepository.GetDetailsAsync(credentialId);

        if (result == null)
            return NotFound("Credential not found");

        return Ok(result);
    }

    // ==============================
    // POST: api/credential
    // ==============================
    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add([FromBody] Credential credential)
    {
        if (credential == null)
            return BadRequest("Credential data is required");

        if (credential.UserId == Guid.Empty)
            return BadRequest("UserId is required");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = credential.UserId;

        var newId = await _credentialRepository.AddAsync(credential, loggedInUserId);
        return Ok(newId);
    }

    // ==============================
    // PUT: api/credential/{credentialId}
    // ==============================
    [HttpPut("Update")]
    public async Task<ActionResult> Update([FromBody] Credential credential)
    {
        if (credential == null)
            return BadRequest("Credential data is required");

        if (credential.Id == Guid.Empty)
            return BadRequest("Invalid CredentialId");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = credential.UserId;

        await _credentialRepository.UpdateAsync(credential, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Credential updated successfully"
        });
    }

    // ==============================
    // PUT: api/credential/{credentialId}
    // ==============================
    [HttpGet("Delete")]
    public async Task<ActionResult> Delete(Guid credentialId, Guid UserId)
    {

        if (credentialId == Guid.Empty)
            return BadRequest("Invalid CredentialId");

        // TODO: Replace with Logged-in UserId
        Guid loggedInUserId = UserId;

        await _credentialRepository.DeleteAsync(credentialId, loggedInUserId);
        return Ok(new
        {
            success = true,
            message = "Credential Entry Deleted successfully."
        });
    }

}
