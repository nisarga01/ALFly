using ALFly.Data;
using ALFly.IRepository;
using ALFly.Models;
using ALFly.ServiceResponse;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ALFly.Repository
{
    public class AgentRepository : IAgentRepository
    {
        public readonly ALFlyDBContext alflyDBContext;
        public readonly IMapper mapper;
        public AgentRepository(ALFlyDBContext alflyDBContext, IMapper mapper)
        {
            this.alflyDBContext = alflyDBContext;
            this.mapper = mapper;
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
        public async Task<List<Agents>> getAgentDetailsAsync()
        {
            return await alflyDBContext.Agents
        .ToListAsync();
        }
        public async Task<Agents> GetAgentByIdAsync(int id)
        {
            var agent = await alflyDBContext.Agents.FindAsync(id);
            return agent;
        }
        public async Task<ServiceResponse<Agents>> EditAgentsAsync(Agents updatedAgent)
        {
            try
            {
                if (alflyDBContext.Entry(updatedAgent).State == EntityState.Detached)
                {
                    alflyDBContext.Attach(updatedAgent);
                }

                await alflyDBContext.SaveChangesAsync();

                return new ServiceResponse<Agents>()
                {
                    Success = true,
                    Data = updatedAgent,
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Agents>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred while updating, please try again later"
                };
            }
        }
        public async Task<ServiceResponse<string>> DeleteAgentAsync(Agents agent)
        {
            try
            {
                alflyDBContext.Agents.Remove(agent);
                await alflyDBContext.SaveChangesAsync();

                return new ServiceResponse<string>()
                {
                    Success = true,
                    ResultMessage = "Agent deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred while deleting the agent"
                };
            }
        }
    }
}
    





