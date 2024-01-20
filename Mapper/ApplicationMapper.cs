using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.Models;
using AutoMapper;

namespace ALFly.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<AgentRequestDTO, Agents>();
            CreateMap<AgentResponseDTO,Agents>();
        }
    }
}
        

