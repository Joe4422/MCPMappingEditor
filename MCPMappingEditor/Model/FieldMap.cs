namespace MCPMappingEditor.Model
{
    public class FieldMap : BaseMapEntry
    {
        public override string MappingType => "field_map";

        public FieldMap(string line, int lineNumber) : base(line, lineNumber)
        {
            Original = sourceString[0];
            Substitute = sourceString[1];
        }

        public override string ToString()
        {
            return $".{MappingType} {Original} {Substitute}";
        }
    }
}
