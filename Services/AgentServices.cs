using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.IRepository;
using ALFly.IServices;
using ALFly.Models;
using ALFly.ServiceResponse;
using AutoMapper;

namespace ALFly.Services
{
    public class AgentServices : IAgentServices
    {
        public readonly IAgentRepository agentRepository;
        public AgentServices (IAgentRepository agentRepository)
        {
            this.agentRepository = agentRepository;
        }
        public async Task<ServiceResponse<AgentResponseDTO>> addAgentsAsync(AgentRequestDTO agentRequestDTO)
        {
            //var agent = mapper.Map<Agents>(agentRequestDTO);
            var agent = new Agents()
            {
                FullName = agentRequestDTO.FullName,
                Photo = agentRequestDTO.Photo,
                EmailAddress = agentRequestDTO.EmailAddress,
                Password = agentRequestDTO.Password,
                ConfirmPassword = agentRequestDTO.ConfirmPassword,
                Role = agentRequestDTO.Role,
            };
            var Result = await agentRepository.addAgentsAsync(agent);
            var Response = new ServiceResponse<AgentResponseDTO>()
            {
                Data = Result.Success ? new AgentResponseDTO()
                {
                   FullName = Result.Data.FullName,
                   Photo = Result.Data.Photo,
                   EmailAddress = Result.Data.EmailAddress,
                   Password = Result.Data.Password,
                   ConfirmPassword = Result.Data.ConfirmPassword,
                   Role = Result.Data.Role,

                } : null,
                Success = Result.Success,
                ErrorMessage = Result.ErrorMessage,
                ResultMessage = Result.ResultMessage
            };
            return Response;
        }
    }
}
