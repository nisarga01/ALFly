namespace ALFly.DTO.PermissionDTO.PermissionResponseDTO
{
    public class PermissionResponseDTO
    {
        public int PermissionId { get; set; }
        public string Permission { get; set; }
    }
    public class GetAgentPermissionsDTO
    {
        public int AgentId { get; set; }
        public string FullName { get; set; }
        public List<PermissionResponseDTO> Permissions { get; set; }
    }
}
