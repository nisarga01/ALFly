using ALFly.Data;
using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.ModifyPermissionDTO;
using ALFly.DTO.PermissionDTO.AgentPermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionResponseDTO;
using ALFly.IRepository;
using ALFly.IServices;
using ALFly.Models;
using ALFly.Repository;
using ALFly.ServiceResponse;

namespace ALFly.Services
{
    public class PermissionServices : IPermissionServices
    {
        public readonly IPermissionRepository permissionRepository;
        public PermissionServices(IPermissionRepository permissionRepository)
        {
            this.permissionRepository = permissionRepository;
        }
        public async Task<ServiceResponse<PermissionResponseDTO>> addPermissionAsync(PermissionRequestDTO permissionRequestDTO)
        {
            var permissions = new Permissions()
            {
                Permission = permissionRequestDTO.Permission,
            };

            var Result = await permissionRepository.addPermissionAsync(permissions);
            var Response = new ServiceResponse<PermissionResponseDTO>()
            {
                Data = Result.Success ? new PermissionResponseDTO()
                {
                    PermissionId = Result.Data.PermissionId,
                    Permission = Result.Data.Permission,

                } : null,
                Success = Result.Success,
                ErrorMessage = Result.ErrorMessage,
                ResultMessage = Result.ResultMessage
            };
            return Response;
        }
        public async Task<ServiceResponse<AgentResponseDTO>> associatePermissionsAsync(AgentPermissionRequestDTO agentPermissionRequestDTO)
        {
            try
            {
                var result = await permissionRepository.associatePermissionsAsync(agentPermissionRequestDTO);

                if (!result.Success)
                {
                    // Handle any additional logic or error handling specific to the service layer
                    return new ServiceResponse<AgentResponseDTO>
                    {
                        Success = false,
                        ErrorMessage = result.ErrorMessage,
                        ResultMessage = "Agent Id not found"
                    };
                }

                // Additional logic or post-processing if needed

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AgentResponseDTO>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred in the service layer while associating permissions"
                };
            }
        }

        public async Task<ServiceResponse<List<GetAgentPermissionsDTO>>> getAgentsWithPermissionsAsync()
        {
            try
            {
                var agentsWithPermissions = await permissionRepository.getAgentsWithPermissionsAsync();
                if(agentsWithPermissions == null)
                {
                    return new ServiceResponse<List<GetAgentPermissionsDTO>>
                    {
                        Success = false,
                        ErrorCode= "404",
                        ErrorMessage = "Agents with permissions not found",
                        // You may include additional messages or properties in the response if needed
                    };
                }

                // Additional logic or post-processing if needed

                return new ServiceResponse<List<GetAgentPermissionsDTO>>
                {
                    Success = true,
                    Data = agentsWithPermissions,
                    ResultMessage = "Agents with permissions retrieved successfully"
                    // You may include additional messages or properties in the response if needed
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetAgentPermissionsDTO>>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred  while retrieving agents with permissions"
                };
            }
        }
        public async Task<ServiceResponse<GetAgentPermissionsDTO>> GetAgentPermissionsAsync(int agentId)
        {
            try
            {
                var agentPermissions = await permissionRepository.GetAgentPermissionsAsync(agentId);
                if(agentPermissions == null)
                {
                    return new ServiceResponse<GetAgentPermissionsDTO>
                    {
                        Success = false,
                        Data = null,
                        ErrorMessage="Agent with the id not found",
                        ErrorCode="404",
                    };

                }

                return new ServiceResponse<GetAgentPermissionsDTO>
                {
                    Success = true,
                    Data = agentPermissions,
                    ResultMessage = "Agent permissions retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetAgentPermissionsDTO>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode= "404",
                    ResultMessage = "Error occurred while retrieving agent permissions"
                };
            }
        }

        public async Task<ServiceResponse<GetAgentPermissionsDTO>> ModifyAgentPermissionsAsync(int agentId, ModifyPermissionDTO modifyPermissionDTO)
        {
            try
            {
                // Additional validation if needed

                var agentPermissions = await permissionRepository.ModifyAgentPermissionsAsync(agentId, modifyPermissionDTO);

                return new ServiceResponse<GetAgentPermissionsDTO>
                {
                    Success = true,
                    Data = agentPermissions,
                    ResultMessage = "Agent permissions modified successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetAgentPermissionsDTO>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred while modifying agent permissions"
                };
            }
        }

    }
}
