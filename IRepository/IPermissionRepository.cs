using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.ModifyPermissionDTO;
using ALFly.DTO.PermissionDTO.AgentPermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionResponseDTO;
using ALFly.Models;
using ALFly.ServiceResponse;

namespace ALFly.IRepository
{
    public interface IPermissionRepository
    {
        Task<ServiceResponse<Permissions>> addPermissionAsync(Permissions permissions);
        Task<ServiceResponse<AgentResponseDTO>> associatePermissionsAsync(AgentPermissionRequestDTO agentPermissionRequestDTO);
        Task<List<GetAgentPermissionsDTO>> getAgentsWithPermissionsAsync();
        Task<GetAgentPermissionsDTO> GetAgentPermissionsAsync(int agentId);
        Task<GetAgentPermissionsDTO> ModifyAgentPermissionsAsync(int agentId, ModifyPermissionDTO modifyPermissionDTO);
    }
}
