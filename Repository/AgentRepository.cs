using ALFly.Data;
using ALFly.IRepository;
using ALFly.Models;
using ALFly.ServiceResponse;
using Microsoft.EntityFrameworkCore;

namespace ALFly.Repository
{
    public class AgentRepository : IAgentRepository
    {
        public readonly ALFlyDBContext alflyDBContext;
        public AgentRepository(ALFlyDBContext alflyDBContext)
        {
            this.alflyDBContext = alflyDBContext;
        }
        public async Task<ServiceResponse<Agents>> addAgentsAsync(Agents agent)
        {
            try
            {
                await alflyDBContext.Agents.AddAsync(agent);
                await alflyDBContext.SaveChangesAsync();

                return new ServiceResponse<Agents>()
                {
                    Success = true,
                    Data = agent,
                    ResultMessage = "Agent details added successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Agents>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occured while adding, please try again later"
                };
            }
        }
    }
}

//        public async Task<List<Agents>> getAgentDetailsAsync()
//        {
//            return await alflyDBContext.Agents
//        .ToListAsync();
//        }
//        public async Task<Agents> GetAgentByIdAsync(int id)
//        {
//            var agent = await alflyDBContext.Agents.FindAsync(id);
//            return agent;
//        }

//        public async Task<ServiceResponse<Agents>> EditAgentsAsync(int id, Agents updatedAgent)
//        {
//            try
//            {
//                var existingAgent = await alflyDBContext.Agents.FindAsync(id);

//                if (existingAgent == null)
//                {
//                    return new ServiceResponse<Agents>()
//                    {
//                        Success = false,
//                        ErrorMessage = "Agent not found",
//                        ResultMessage = "The specified agent does not exist"
//                    };
//                }

//                // Save changes to the database
//                await alflyDBContext.SaveChangesAsync();

//                return new ServiceResponse<Agents>()
//                {
//                    Success = true,
//                    Data = existingAgent,
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ServiceResponse<Agents>()
//                {
//                    Data = null,
//                    Success = false,
//                    ErrorMessage = ex.Message,
//                    ResultMessage = "Error occurred while updating, please try again later"
//                };
//            }
//        }
//        public async Task<ServiceResponse<string>> DeleteAgentAsync(Agents agent)
//        {
//            try
//            {
//                alflyDBContext.Agents.Remove(agent);
//                await alflyDBContext.SaveChangesAsync();

//                return new ServiceResponse<string>()
//                {
//                    Success = true,
//                    ResultMessage = "Agent deleted successfully"
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ServiceResponse<string>()
//                {
//                    Success = false,
//                    ErrorMessage = ex.Message,
//                    ResultMessage = "Error occurred while deleting the agent"
//                };
//            }
//        }
//    }
//}
