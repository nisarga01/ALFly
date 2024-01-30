using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.EditAgentDetailsDTO;
using ALFly.DTO.GetAgentDetailsDTO;
using ALFly.ServiceResponse;
using ALFly.Services;

namespace ALFly.IServices
{
    public interface IAgentServices
    {
        Task<ServiceResponse<AgentResponseDTO>> addAgentsAsync(AgentRequestDTO agentRequestDTO);
        //Task<ServiceResponse<List<GetAgentDetailsDTO>>> getAgentDetailsAsync();
        //Task<ServiceResponse<EditAgentResponseDTO>> EditAgentsAsync(int id, EditAgentRequestDTO editAgentRequestDTO);
        //Task<ServiceResponse<string>> DeleteAgentAsync(int id);
    }
}
