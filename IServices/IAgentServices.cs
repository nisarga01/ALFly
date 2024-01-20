using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.ServiceResponse;
using ALFly.Services;

namespace ALFly.IServices
{
    public interface IAgentServices
    {
        Task<ServiceResponse<AgentResponseDTO>> addAgentsAsync(AgentRequestDTO agentRequestDTO);
    }
}
