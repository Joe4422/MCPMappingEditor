namespace MCPMappingEditor.Model
{
    public class ClassMap : BaseMapEntry
    {
        public override string MappingType => "class_map";

        public ClassMap(string line, int lineNumber) : base(line, lineNumber)
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
