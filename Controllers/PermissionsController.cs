using ALFly.DTO.ModifyPermissionDTO;
using ALFly.DTO.PermissionDTO.AgentPermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionRequestDTO;
using ALFly.IServices;
using ALFly.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ALFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        public readonly IPermissionServices permissionServices;
        public PermissionsController(IPermissionServices permissionServices)
        {
            this.permissionServices = permissionServices;
        }

        [EnableCors("CORSPolicy")]
        [HttpPost("AddPermission")]
        public async Task<IActionResult> addPermissionAsync([FromBody] PermissionRequestDTO permissionRequestDTO)
        {
            var Result = await permissionServices.addPermissionAsync(permissionRequestDTO);
            if (Result.Success)
                return Ok(Result);
            if (!Result.Success && Result.ErrorCode == "ValidationFailed")
                return UnprocessableEntity(Result);  // 422 Unprocessable Entity
            if (!Result.Success && Result.ErrorCode == "NotFound")
                return NotFound(Result);  // 404 Not Found
            if (!Result.Success && Result.ErrorCode == "Unauthorized")
                return Unauthorized(Result);  // 401 Unauthorized   
            if (!Result.Success && Result.ErrorCode == "Conflict")
                return Conflict(Result);  // 409 Conflict
            if (!Result.Success && Result.ErrorCode == "InternalServerError")
                return StatusCode(500, Result);  // 500 Internal Server Error
            return BadRequest(Result);
        }

        [EnableCors("CORSPolicy")]
        [HttpPost("AssociatePermissions")]
        public async Task<IActionResult> associatePermissionsAsync([FromBody] AgentPermissionRequestDTO agentPermissionRequestDTO)
        {
            var Result = await permissionServices.associatePermissionsAsync(agentPermissionRequestDTO);
            if (Result.Success)
                return Ok(Result);
            if (!Result.Success && Result.ErrorCode == "ValidationFailed")
                return UnprocessableEntity(Result);  // 422 Unprocessable Entity
            if (!Result.Success && Result.ErrorCode == "NotFound")
                return NotFound(Result);  // 404 Not Found
            if (!Result.Success && Result.ErrorCode == "Unauthorized")
                return Unauthorized(Result);  // 401 Unauthorized   
            if (!Result.Success && Result.ErrorCode == "Conflict")
                return Conflict(Result);  // 409 Conflict
            if (!Result.Success && Result.ErrorCode == "InternalServerError")
                return StatusCode(500, Result);  // 500 Internal Server Error
            return BadRequest(Result);
        }

        [EnableCors("CORSPolicy")]
        [HttpGet("Permissions")]
        public async Task<IActionResult> getAgentsWithPermissionsAsync()
        {
            var result = await permissionServices.getAgentsWithPermissionsAsync();
            if (result.Success)
                return Ok(result);
            // Handle other status codes...
            return BadRequest(result);
        }

        [EnableCors("CORSPolicy")]
        [HttpGet("{agentId}")]
        public async Task<IActionResult> GetAgentPermissions(int agentId)
        {
            var result = await permissionServices.GetAgentPermissionsAsync(agentId);

            if (result.Success)
                return Ok(result);

            // Handle other status codes...
            return BadRequest(result);
        }

        [EnableCors("CORSPolicy")]
        [HttpPut("{agentId}")]
        public async Task<IActionResult> ModifyAgentPermissions(int agentId, [FromBody] ModifyPermissionDTO modifyPermissionsDTO)
        {
            var result = await permissionServices.ModifyAgentPermissionsAsync(agentId, modifyPermissionsDTO);

            if (result.Success)
                return Ok(result);

            // Handle other status codes...
            return BadRequest(result);
        }
        [EnableCors("CORSPolicy")]
        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultPermissions()
        {
            var result = await permissionServices.GetDefaultPermissionsAsync();

            if (result.Success)
                return Ok(result);

            // Handle other status codes...
            return BadRequest(result);
        }


    }
}
