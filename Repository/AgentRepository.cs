using ALFly.Data;
using ALFly.IRepository;
using ALFly.Models;
using ALFly.ServiceResponse;

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
            if (string.IsNullOrWhiteSpace(agent.FullName) || string.IsNullOrWhiteSpace(agent.EmailAddress) || string.IsNullOrWhiteSpace(Convert.ToBase64String(agent.Photo)) || string.IsNullOrWhiteSpace(agent.Password) || string.IsNullOrWhiteSpace(agent.ConfirmPassword) || string.IsNullOrWhiteSpace(agent.Role.ToString()))
            {
                return new ServiceResponse<Agents>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Enter all the fields correctly",
                };
            }
            try
            {
                await alflyDBContext.Agents.AddAsync(agent);
                await alflyDBContext.SaveChangesAsync();

                return new ServiceResponse<Agents>()
                {
                    Data = agent,
                    Success = true,
                    ResultMessage = "Crop details added successfully"

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

