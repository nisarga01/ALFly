namespace ALFly.DTO.AgentPatchDTO
{
    public class AgentPatchDTO
    {
        public List<PropertyPatch> PropertyPatches { get; set; }

    }
    public class PropertyPatch
    {
        public string PropertyName { get; set; }
        public object NewValue { get; set; }
    }
}
