using ALFly.DTO.AgentRequestDTO;
using ALFly.IServices;
using Microsoft.AspNetCore.Http;
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
        [HttpPost("AddAgents")]
        public async Task<IActionResult>addAgentsAsync([FromBody]AgentRequestDTO agentRequestDTO)
        {
            var Result = await AgentServices.addAgentsAsync(agentRequestDTO);
            if (Result.Success)
                return Ok(Result);
            if (!Result.Success && Result.ErrorCode == "ValidationFailed")
                return UnprocessableEntity(Result);
            return BadRequest(Result);
        }
    }
}
