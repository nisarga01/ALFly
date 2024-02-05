using ALFly.Data;
using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.ModifyPermissionDTO;
using ALFly.DTO.PermissionDTO.AgentPermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionResponseDTO;
using ALFly.IRepository;
using ALFly.Models;
using ALFly.ServiceResponse;
using Microsoft.EntityFrameworkCore;

namespace ALFly.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        public readonly ALFlyDBContext alflyDBContext;
        public PermissionRepository(ALFlyDBContext alflyDBContext)
        {
            this.alflyDBContext = alflyDBContext;
        }
        public async Task<ServiceResponse<Permissions>> addPermissionAsync(Permissions permissions)
        {
            try
            {
                await alflyDBContext.Permissions.AddAsync(permissions);
                await alflyDBContext.SaveChangesAsync();

                return new ServiceResponse<Permissions>()
                {
                    Success = true,
                    Data = permissions,
                    ResultMessage = "Permission added successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Permissions>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occured while adding, please try again later"
                };
            }
        }
        public async Task<ServiceResponse<AgentResponseDTO>> associatePermissionsAsync(AgentPermissionRequestDTO agentPermissionRequestDTO)
        {
            try
            {
                // Find the agent by ID
                var agent = await alflyDBContext.Agents.FindAsync(agentPermissionRequestDTO.AgentId);

                if (agent == null)
                {
                    return new ServiceResponse<AgentResponseDTO>
                    {
                        Success = false,
                        ErrorMessage = "Agent not found",
                        ResultMessage = "Agent not found"
                    };
                }

                // Clear existing permissions (if any) for a clean update
                agent.AgentPermissions?.Clear();

                // Associate new permissions
                if (agentPermissionRequestDTO.PermissionIds != null && agentPermissionRequestDTO.PermissionIds.Any())
                {
                    foreach (var permissionId in agentPermissionRequestDTO.PermissionIds)
                    {
                        var permission = await alflyDBContext.Permissions.FindAsync(permissionId);

                        if (permission != null)
                        {
                            agent.AgentPermissions ??= new List<AgentPermission>();
                            agent.AgentPermissions.Add(new AgentPermission { Permission = permission });
                        }
                    }
                }

                // Save changes to the database
                await alflyDBContext.SaveChangesAsync();

                // Return success response
                return new ServiceResponse<AgentResponseDTO>
                {
                    Success = true,
                    ResultMessage = "Permissions associated successfully"
                    // You may include additional data in the response if needed
                };
            }
            catch (Exception ex)
            {
                // Return error response in case of exceptions
                return new ServiceResponse<AgentResponseDTO>
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred while associating permissions"
                };
            }
        }
        public async Task<List<GetAgentPermissionsDTO>> getAgentsWithPermissionsAsync()
        {
            var agentsWithPermissions = await alflyDBContext.Agents
                .Include(a => a.AgentPermissions)
                .ThenInclude(ap => ap.Permission)
                .Select(agent => new GetAgentPermissionsDTO
                {
                    AgentId = agent.Id,
                    FullName = agent.FullName,
                    Permissions = agent.AgentPermissions.Any()
                        ? agent.AgentPermissions.Select(ap => new PermissionResponseDTO
                        {
                            PermissionId = ap.Permission.PermissionId,
                            Permission = ap.Permission.Permission,
                            // Include other properties as needed
                        }).ToList()
                        : null
                })
                    .ToListAsync();

                return agentsWithPermissions;

        }
        public async Task<GetAgentPermissionsDTO> GetAgentPermissionsAsync(int agentId)
        {
            var agentPermissions = await alflyDBContext.Agents
                .Where(a => a.Id == agentId)
                .Include(a => a.AgentPermissions)
                .ThenInclude(ap => ap.Permission)
                .Select(agent => new GetAgentPermissionsDTO
                {
                    AgentId = agent.Id,
                    FullName = agent.FullName,
                    Permissions = agent.AgentPermissions.Select(ap => new PermissionResponseDTO
                    {
                        PermissionId = ap.Permission.PermissionId,
                        Permission = ap.Permission.Permission,
                        // Include other properties as needed
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return agentPermissions;
        }

        public async Task<GetAgentPermissionsDTO> ModifyAgentPermissionsAsync(int agentId, ModifyPermissionDTO modifyPermissionDTO)
        {
            // Fetch the agent from the database
            var agent = await alflyDBContext.Agents
                .Include(a => a.AgentPermissions)
                .ThenInclude(ap => ap.Permission)
                .FirstOrDefaultAsync(a => a.Id == agentId);


            // Update agent's permissions based on modifyPermissionsDTO
            if (modifyPermissionDTO.PermissionIds != null && modifyPermissionDTO.PermissionIds.Any())
            {
                // Clear existing permissions (if any) for a clean update

                foreach (var permissionId in modifyPermissionDTO.PermissionIds)
                {
                    var permission = await alflyDBContext.Permissions.FindAsync(permissionId);

                    if (permission != null)
                    {
                        agent.AgentPermissions ??= new List<AgentPermission>();
                        agent.AgentPermissions.Add(new AgentPermission { Permission = permission });
                    }
                }
            }

            // Save changes to the database
            await alflyDBContext.SaveChangesAsync();

            // Return the updated agent's permissions
            return await GetAgentPermissionsAsync(agentId);
        }
        public async Task<List<PermissionResponseDTO>> GetDefaultPermissionsAsync()
{
    // Implement the logic to retrieve default permissions from the Permission table
    var defaultPermissions = await alflyDBContext.Permissions
        .Select(permission => new PermissionResponseDTO
        {
            PermissionId = permission.PermissionId,
            Permission = permission.Permission,
            // Include other properties as needed
        })
        .ToListAsync();

    return defaultPermissions;
}

    }

}

