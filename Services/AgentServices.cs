using ALFly.DTO.AgentPatchDTO;
using ALFly.DTO.AgentRequestDTO;
using ALFly.DTO.AgentResponseDTO;
using ALFly.DTO.GetAgentDetailsDTO;
using ALFly.DTO.PermissionDTO.PermissionRequestDTO;
using ALFly.DTO.PermissionDTO.PermissionResponseDTO;
using ALFly.Enums;
using ALFly.IRepository;
using ALFly.IServices;
using ALFly.Models;
using ALFly.Repository;
using ALFly.ServiceResponse;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
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

            //byte[] photo = HandleFileUpload(agentRequestDTO.Photo);
            string photoUrl = HandleFileUpload(agentRequestDTO.Photo);


            var agent = new Agents()
            {
                FullName = agentRequestDTO.FullName,
                Photo = photoUrl,
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
        private string HandleFileUpload(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return null;
            }

            // Define a folder path to store the photos
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Generate a unique filename for the photo
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            // Save the photo to the file system
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            // Return the URL of the saved photo
            return "/photos/" + fileName; // Adjust this based on your project's URL structure
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

        public async Task<ServiceResponse<Agents>> EditAgentsAsync(int id, AgentPatchDTO agentPatchDTO)
        {
            // Check if the agent with the given id exists
            var existingAgent = await agentRepository.GetAgentByIdAsync(id);

            if (existingAgent == null)
            {
                return new ServiceResponse<Agents>()
                {
                    Success = false,
                    ErrorMessage = "Agent not found",
                    ResultMessage = "The specified agent does not exist"
                };
            }

            // Validate property patches
            var validationResponse = ValidatePropertyPatches(agentPatchDTO);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }

            ApplyPropertyPatches(existingAgent, agentPatchDTO);

            // Save changes to the database
            var updateResult = await agentRepository.EditAgentsAsync(existingAgent);

            if (updateResult.Success)
            {
                var updatedResponse = new ServiceResponse<Agents>()
                {
                    Success = true,
                    ResultMessage = "Agent details updated successfully"
                };

                return updatedResponse;
            }
            else
            {
                return new ServiceResponse<Agents>()
                {
                    Success = false,
                    ErrorMessage = updateResult.ErrorMessage,
                    ResultMessage = "Error occurred while updating, please try again later"
                };
            }
        }
        private ServiceResponse<Agents> ValidatePropertyPatches(AgentPatchDTO patchDto)
        {
            if (patchDto == null || patchDto.PropertyPatches == null)
            {
                return new ServiceResponse<Agents>()
                {
                    Success = false,
                    ErrorMessage = "Invalid patch data",
                    ResultMessage = "Property patches cannot be null."
                };
            }

            foreach (var propertyPatch in patchDto.PropertyPatches)
            {
                switch (propertyPatch.PropertyName)
                {
                    case "FullName":
                        if (!IsValidFullName(propertyPatch.NewValue))
                        {
                            return new ServiceResponse<Agents>()
                            {
                                Success = false,
                                ErrorMessage = "Invalid value for FullName",
                                ResultMessage = "FullName should not contain numbers,specialcharacters."
                            };
                        }
                        break;
                    case "EmailAddress":
                        if (!IsValidEmailAddress(propertyPatch.NewValue))
                        {
                            return new ServiceResponse<Agents>()
                            {
                                Success = false,
                                ErrorMessage = "Invalid value for EmailAddress",
                                ResultMessage = "Invalid email format ! email should not be empty."
                            };
                        }
                        break;
                    case "Password":
                        if (!IsValidPassword(propertyPatch.NewValue))
                        {
                            return new ServiceResponse<Agents>()
                            {
                                Success = false,
                                ErrorMessage = "Invalid value for Password",
                                ResultMessage = "Password does not meet the criteria. ! password should not be empty"
                            };
                        }
                        break;
                }
            }
            return new ServiceResponse<Agents>() { Success = true };
        }
        
        private bool IsValidFullName(object newValue)
        {
            if (newValue is string fullName && !string.IsNullOrWhiteSpace(fullName))
            {
                // Check if the full name contains only letters and spaces
                return fullName.All(char.IsLetter) && !fullName.Any(char.IsDigit) && !fullName.Any(char.IsPunctuation);
            }
            return false;
        }
        private bool IsValidEmailAddress(object newValue)
        {
            if (newValue is string emailAddress && !string.IsNullOrWhiteSpace(emailAddress))
            {
                return Regex.IsMatch(emailAddress, @"^[a-z0-9._-]+@[a-z0-9.-]+\.[a-z]{2,}$");
            }
            return false;
        }
        private bool IsValidPassword(object newValue)
        {
            if (newValue is string password && !string.IsNullOrWhiteSpace(password))
            {
                return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[!@#$%^&*()\-_=+\\|/?,.<>:;""`~])(?=.*[0-9]).{8,16}$");
            }
            return false;
        }
        private void ApplyPropertyPatches(Agents existingAgent, AgentPatchDTO patchDto)
        {
            if (patchDto == null || patchDto.PropertyPatches == null)
            {

                throw new ArgumentNullException(nameof(patchDto), "ALflyPatchDTO or PropertyPatches cannot be null.");
            }

            foreach (var propertyPatch in patchDto.PropertyPatches)
            {
                var property = typeof(Agents).GetProperty(propertyPatch.PropertyName);

                if (property != null)
                {
                    try
                    {
                        if (property.PropertyType.BaseType == typeof(Enum))
                        {
                            Enum.TryParse(propertyPatch.NewValue.ToString(), out Role role);
                            propertyPatch.NewValue = role;
                        }
                        var convertedValue = Convert.ChangeType(propertyPatch.NewValue, property.PropertyType);
                        property.SetValue(existingAgent, convertedValue);
                    }
                    catch (InvalidCastException ex)
                    {
                        Console.Error.WriteLine($"Conversion error: {ex.Message}");
                        throw;
                    }
                }
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
