using ALFly.Models;
using ALFly.ServiceResponse;

namespace ALFly.IRepository
{
    public interface IAgentRepository
    {
        Task<ServiceResponse<Agents>> addAgentsAsync(Agents agents);
    }
}
