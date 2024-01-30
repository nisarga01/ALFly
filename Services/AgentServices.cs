using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.EditAgentDetailsDTO;
using ALFly.DTO.GetAgentDetailsDTO;
using ALFly.IRepository;
using ALFly.IServices;
using ALFly.Models;
using ALFly.ServiceResponse;
using System.Text.RegularExpressions;

namespace ALFly.Services
{
    public class AgentServices : IAgentServices
    {
        public readonly IAgentRepository agentRepository;
        public AgentServices(IAgentRepository agentRepository)
        {
            this.agentRepository = agentRepository;
        }
        public async Task<ServiceResponse<AgentResponseDTO>> addAgentsAsync(AgentRequestDTO agentRequestDTO)
        {
            if (string.IsNullOrEmpty(agentRequestDTO.FullName) || !Regex.IsMatch(agentRequestDTO.FullName.Trim(), @"^[A-Za-z]+(?:\s+[A-Za-z]+)*$"))
            {
                var errorResponse = new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "Enter correct Name",
                    ResultMessage = "Name should not be empty and contain only letters"
                };
                return errorResponse;
            }
            if (string.IsNullOrEmpty(agentRequestDTO.EmailAddress) || !Regex.IsMatch(agentRequestDTO.EmailAddress, @"^[a-z0-9._-]+@[a-z0-9.-]+\.[a-z]{2,}$"))
            {
                var errorResponse = new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ResultMessage = "Enter Valid Name",
                    ErrorMessage = "email should contains" + "lowercase letters,digits,special characters" + "@ symbol and . symbol"
                };
                return errorResponse;

            }
            if (string.IsNullOrEmpty(agentRequestDTO.Password) || !Regex.IsMatch(agentRequestDTO.Password, @"^(?=.*[A-Z])(?=.*[!@#$%^&*()\-_=+\\|/?,.<>:;""`~])(?=.*[0-9]).{8,16}$"))
            {
                var errorResponse = new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "password should contains minimum of 8 characteres with  at least 1 digit, 1 special character,1 upper and lower case",
                    ResultMessage = "Enter correct password"
                };
                return errorResponse;
            }
            if (string.IsNullOrEmpty(agentRequestDTO.Password) || !Regex.IsMatch(agentRequestDTO.Password, @"^(?=.*[A-Z])(?=.*[!@#$%^&*()\-_=+\\|/?,.<>:;""`~])(?=.*[0-9]).{8,10}$")
                || agentRequestDTO.Password != agentRequestDTO.ConfirmPassword)
            {
                var errorResponse = new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "confirm Password has to match exactly with the password",
                    ResultMessage = "Enter correct password"
                };
                return errorResponse;
            }

            byte[] photo = HandleFileUpload(agentRequestDTO.Photo);

            //var agent = mapper.Map<Agents>(agentRequestDTO);
            var agent = new Agents()
            {
                FullName = agentRequestDTO.FullName,
                Photo = photo,
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
                    Id = Result.Data.Id,
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
        private byte[] HandleFileUpload(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                // Handle the case where no file is provided or the file is empty
                return null; // Or throw an exception or handle it based on your requirements
            }

            using (var memoryStream = new MemoryStream())
            {
                photo.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<ServiceResponse<List<GetAgentDetailsDTO>>> getAgentDetailsAsync()
        {
            try
            {
                var latestAgent = await agentRepository.getAgentDetailsAsync();

                if (latestAgent != null && latestAgent.Any())
                {
                    // Mapping the list of entities to DTOs 
                    var getAgentDetailsDTO = latestAgent.Select(agent => new GetAgentDetailsDTO
                    {
                        Id = agent.Id,
                        FullName = agent.FullName,
                        Photo = agent.Photo, // Assuming PhotoData contains byte[] image data
                        EmailAddress = agent.EmailAddress,
                        Role = agent.Role
                    }).ToList();

                    // Return a success response with the agent details
                    return new ServiceResponse<List<GetAgentDetailsDTO>>
                    {
                        Data = getAgentDetailsDTO,
                        Success = true,
                        ResultMessage = "agent details retrieved successfully"
                    };
                }
                else
                {
                    // Return a response indicating that no agents were found
                    return new ServiceResponse<List<GetAgentDetailsDTO>>
                    {
                        Data = null,
                        Success = false,
                        ResultMessage = "No agents found in the database"
                    };
                }
            }
            catch (Exception ex)
            {
                // Return an error response if an exception occurs
                return new ServiceResponse<List<GetAgentDetailsDTO>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = ex.Message,
                    ResultMessage = "Error occurred while retrieving agent details"
                };
            }
        }
        public async Task<ServiceResponse<AgentResponseDTO>> EditAgentsAsync(int id, AgentRequestDTO agentRequestDTO)
        {
            // Check if the agent with the given id exists
            var existingAgent = await agentRepository.GetAgentByIdAsync(id);

            if (existingAgent == null)
            {
                return new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "Agent not found",
                    ResultMessage = "The specified agent does not exist"
                };
            }

            // Perform validation and update logic similar to addAgentsAsync
            if (string.IsNullOrEmpty(agentRequestDTO.FullName) || !Regex.IsMatch(agentRequestDTO.FullName.Trim(), @"^[A-Za-z]+(?:\s+[A-Za-z]+)*$"))
            {
                return new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "ValidationFailed",
                    ResultMessage = "Name should not be empty and contain only letters"
                };
            }

            // Validate EmailAddress
            if (string.IsNullOrEmpty(agentRequestDTO.EmailAddress) || !Regex.IsMatch(agentRequestDTO.EmailAddress, @"^[a-z0-9._-]+@[a-z0-9.-]+\.[a-z]{2,}$"))
            {
                return new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "ValidationFailed",
                    ResultMessage = "Email should contain lowercase letters, digits, special characters, @ symbol, and . symbol"
                };
            }

            // Validate Password
            if (string.IsNullOrEmpty(agentRequestDTO.Password) || !Regex.IsMatch(agentRequestDTO.Password, @"^(?=.*[A-Z])(?=.*[!@#$%^&*()\-_=+\\|/?,.<>:;""`~])(?=.*[0-9]).{8,16}$"))
            {
                return new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "ValidationFailed",
                    ResultMessage = "Password should contain a minimum of 8 characters with at least 1 digit, 1 special character, 1 upper and lower case"
                };
            }

            // Validate ConfirmPassword
            if (string.IsNullOrEmpty(agentRequestDTO.ConfirmPassword) || !Regex.IsMatch(agentRequestDTO.ConfirmPassword, @"^(?=.*[A-Z])(?=.*[!@#$%^&*()\-_=+\\|/?,.<>:;""`~])(?=.*[0-9]).{8,10}$") || agentRequestDTO .Password != agentRequestDTO.ConfirmPassword)
            {
                return new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = "ValidationFailed",
                    ResultMessage = "Confirm Password has to match exactly with the password"
                };
            }

            // Update the existing agent with the new details
            existingAgent.FullName = agentRequestDTO.FullName;
            existingAgent.EmailAddress = agentRequestDTO.EmailAddress;
            existingAgent.Password = agentRequestDTO.Password;  
            existingAgent.ConfirmPassword = agentRequestDTO.ConfirmPassword;
            existingAgent.Role = agentRequestDTO.Role;

            
            // Handle photo update
            if (agentRequestDTO.Photo != null)
            {
                existingAgent.Photo = HandleFileUpload(agentRequestDTO.Photo);
            }

            // Save changes to the database
            var updateResult = await agentRepository.EditAgentsAsync(id, existingAgent); // Pass the 'id' and 'existingAgent'

            if (updateResult.Success)
            {
                var updatedResponse = new ServiceResponse<AgentResponseDTO>()
                {
                    Success = true,
                    Data = new AgentResponseDTO()
                    {
                        Id = existingAgent.Id,
                        FullName = existingAgent.FullName,
                        Photo = existingAgent.Photo,
                        EmailAddress = existingAgent.EmailAddress,
                        Role = existingAgent.Role,
                        // Include other properties...
                    },
                    ResultMessage = "Agent details updated successfully"
                };

                return updatedResponse;
            }
            else
            {
                return new ServiceResponse<AgentResponseDTO>()
                {
                    Success = false,
                    ErrorMessage = updateResult.ErrorMessage,
                    ResultMessage = "Error occurred while updating, please try again later"
                };
            }
        }

        public async Task<ServiceResponse<string>> DeleteAgentAsync(int id)
        {
            // Check if the agent with the given id exists
            var existingAgent = await agentRepository.GetAgentByIdAsync(id);

            if (existingAgent == null)
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    ErrorMessage = "Agent not found",
                    ResultMessage = "The specified agent does not exist"
                };
            }

            // Delete the agent from the database
            var deleteResult = await agentRepository.DeleteAgentAsync(existingAgent);

            if (deleteResult.Success)
            {
                return new ServiceResponse<string>()
                {
                    Success = true,
                    ResultMessage = "Agent deleted successfully"
                };
            }
            else
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    ErrorMessage = deleteResult.ErrorMessage,
                    ResultMessage = "Error occurred while deleting the agent"
                };
            }
        }
    }
}











