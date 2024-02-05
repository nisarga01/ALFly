using ALFly.DTO.AgentRequestDTO;
using ALFly.IServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ALFly.DTO.AgentPatchDTO;
using ALFly.DTO.PermissionDTO.PermissionRequestDTO;


namespace ALFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        public readonly IAgentServices agentServices;
        public AgentsController(IAgentServices agentServices)
        {
            this.agentServices = agentServices;
        }

        [EnableCors("CORSPolicy")]
        [HttpPost("AddAgents")]
        //[RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)] // 10 MB

        public async Task<IActionResult> addAgentsAsync([FromForm] AgentRequestDTO agentRequestDTO)
        {
            var Result = await agentServices.addAgentsAsync(agentRequestDTO);
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
        [HttpGet("GetAgentDetails")]
        public async Task<IActionResult> getAgentDetailsAsync()
        {
            var Result = await agentServices.getAgentDetailsAsync();
            if (Result.Success)
                return Ok(Result);  // 200 OK
            if (!Result.Success && Result.ErrorCode == "NotFound")
                return NotFound(Result);  // 404 Not Found
            if (!Result.Success && Result.ErrorCode == "InternalServerError")
                return StatusCode(500, Result);  // 500 Internal Server Error
            return BadRequest(Result);  // 400 Bad Request
        }

        [EnableCors("CORSPolicy")]
        [HttpPatch("EditAgentDetails")]
        public async Task<IActionResult> EditAgentsAsync(int id, [FromBody] AgentPatchDTO agentPatchDTO)
        {
            var Result = await agentServices.EditAgentsAsync(id, agentPatchDTO);
            if (Result.Success)
                return Ok(Result);  // 200 OK
            if (!Result.Success && Result.ErrorCode == "NotFound")
                return NotFound(Result);  // 404 Not Found
            if (!Result.Success && Result.ErrorCode == "UnprocessableEntity")
                return UnprocessableEntity(Result);  // 422 Unprocessable Entity
            if (!Result.Success && Result.ErrorCode == "InternalServerError")
                return StatusCode(500, Result);  // 500 Internal Server Error
            return BadRequest(Result);  // 400 Bad Request
        }

        [EnableCors("CORSPolicy")]
        [HttpDelete("DeleteAgent")]
        public async Task<IActionResult> DeleteAgentAsync(int id)
        {
            var Result = await agentServices.DeleteAgentAsync(id);
            if (Result.Success)
                return Ok(Result);  // 200 OK
            if (!Result.Success && Result.ErrorCode == "NotFound")
                return NotFound(Result);  // 404 Not Found
            if (!Result.Success && Result.ErrorCode == "InternalServerError")
                return StatusCode(500, Result);  // 500 Internal Server Error
            return BadRequest(Result);  // 400 Bad Request
        }
    }
}


