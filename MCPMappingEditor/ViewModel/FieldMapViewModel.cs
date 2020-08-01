using MCPMappingEditor.Model;
using System;

namespace MCPMappingEditor.ViewModel
{
    public class FieldMapViewModel : BaseClassMemberMapEntryViewModel
    {
        public FieldMapViewModel(BaseMapEntry mapEntry, MapCollection mappings, ClassMap classMapEntry) : base(mapEntry, mappings, classMapEntry)
        {
            if (mapEntry is FieldMap == false)
            {
                throw new Exception($"Created entry viewmodel with wrong entry type! (Type is {mapEntry.GetType().Name}, expected FieldMap)");
            }
        }

        public override string OriginalLabel => $"({OriginalMember})";
        public override string ListLabelText => $"_{Substitute}";
    }
}
