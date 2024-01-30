using ALFly.Models;
using ALFly.ServiceResponse;

namespace ALFly.IRepository
{
    public interface IAgentRepository
    {
        Task<ServiceResponse<Agents>> addAgentsAsync(Agents agents);
        //Task<List<Agents>> getAgentDetailsAsync();
        //Task<Agents> GetAgentByIdAsync(int id);
        //Task<ServiceResponse<Agents>> EditAgentsAsync(int id, Agents updatedAgent);
        //Task<ServiceResponse<string>> DeleteAgentAsync(Agents agent);
    }
}
