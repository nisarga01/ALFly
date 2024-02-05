using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.ModifyPermissionDTO;
using ALFly.DTO.PermissionDTO.AgentPermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionResponseDTO;
using ALFly.ServiceResponse;

namespace ALFly.IServices
{
    public interface IPermissionServices
    {
        Task<ServiceResponse<PermissionResponseDTO>> addPermissionAsync(PermissionRequestDTO permissionRequestDTO);
        Task<ServiceResponse<AgentResponseDTO>> associatePermissionsAsync(AgentPermissionRequestDTO agentPermissionRequestDTO);
        Task<ServiceResponse<List<GetAgentPermissionsDTO>>> getAgentsWithPermissionsAsync();
        Task<ServiceResponse<GetAgentPermissionsDTO>> GetAgentPermissionsAsync(int agentId);
        Task<ServiceResponse<GetAgentPermissionsDTO>> ModifyAgentPermissionsAsync(int agentId, ModifyPermissionDTO modifyPermissionDTO);
        Task<ServiceResponse<List<PermissionResponseDTO>>> GetDefaultPermissionsAsync();
    }
}
