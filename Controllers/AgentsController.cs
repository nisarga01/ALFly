using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.EditAgentDetailsDTO;
using ALFly.IServices;
using ALFly.ServiceResponse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ALFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        public readonly IAgentServices AgentServices;
        public AgentsController(IAgentServices agentServices)
        {
            AgentServices = agentServices;
        }

        [EnableCors("CORSPolicy")]
        [HttpPost("AddAgents")]
        //[RequestFormLimits(MultipartBodyLengthLimit = 10 * 1024 * 1024)] // 10 MB

        public async Task<IActionResult>addAgentsAsync([FromForm]AgentRequestDTO agentRequestDTO)
        {
            var Result = await AgentServices.addAgentsAsync(agentRequestDTO);
            if (Result.Success)
                return Ok(Result);
            if (!Result.Success && Result.ErrorCode == "ValidationFailed")
                return UnprocessableEntity(Result);
            return BadRequest(Result);
        }

        [EnableCors("CORSPolicy")]
        [HttpGet("GetAgentDetails")]
        public async Task<IActionResult> getAgentDetailsAsync()
        {
            var Result = await AgentServices.getAgentDetailsAsync();
            if (Result.Success)
                return Ok(Result);
            if (!Result.Success && Result.ErrorCode == "ValidationFailed")
                return UnprocessableEntity(Result);
            return BadRequest(Result);
        }

        [EnableCors("CORSPolicy")]
        [HttpPut("EditAgentDetails")]
        public async Task<IActionResult> EditAgentsAsync(int id, [FromForm] AgentRequestDTO agentRequestDTO)
        {
            var Result = await AgentServices.EditAgentsAsync(id,agentRequestDTO);

            if (Result.Success)
                return Ok(Result);

            if (!Result.Success && Result.ErrorCode == "ValidationFailed")
                return UnprocessableEntity(Result);

            return BadRequest(Result);
        }

        [EnableCors("CORSPolicy")]
        [HttpDelete("DeleteAgent")]
        public async Task<IActionResult> DeleteAgentAsync(int id)
        {
            var deleteResult = await AgentServices.DeleteAgentAsync(id);

            if (deleteResult.Success)
            {
                return Ok(deleteResult);
            }
            else
            {
                return BadRequest(deleteResult);
            }
        }
    }
}
