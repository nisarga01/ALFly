using ALFly.DTO.AgentPatchDTO;
using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.GetAgentDetailsDTO;
using ALFly.Models;
using ALFly.ServiceResponse;

namespace ALFly.IServices
{
    public interface IAgentServices
    {
        Task<ServiceResponse<AgentResponseDTO>> addAgentsAsync(AgentRequestDTO agentRequestDTO);
        Task<ServiceResponse<List<GetAgentDetailsDTO>>> getAgentDetailsAsync();
        Task<ServiceResponse<Agents>> EditAgentsAsync(int id, AgentPatchDTO agentPatchDTO);

        Task<ServiceResponse<string>> DeleteAgentAsync(int id);
    }
}
