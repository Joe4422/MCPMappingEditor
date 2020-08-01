using System;
using System.Collections.Generic;
using System.Text;

namespace MCPMappingEditor.Model
{
    public class MethodMap : BaseMapEntry
    {
        public override string MappingType => "method_map";
        public string Signature { get; }

        public MethodMap(string line, int lineNumber) : base(line, lineNumber)
        {
            Original = sourceString[0];
            Substitute = sourceString[2];
            Signature = sourceString[1];
        }

        public override string ToString()
        {
            return $".{MappingType} {Original} {Signature} {Substitute}";
        }
    }
}
