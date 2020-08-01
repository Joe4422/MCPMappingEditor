using MCPMappingEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MCPMappingEditor.ViewModel
{
    public class ClassMapViewModel : BaseMapEntryViewModel
    {
        public ObservableCollection<BaseClassMemberMapEntryViewModel> Members { get; } = new ObservableCollection<BaseClassMemberMapEntryViewModel>();

        public override string OriginalLabel => $"({Original})";

        public override string ListLabelText => $"_{Substitute}";

        public ClassMapViewModel(BaseMapEntry mapEntry, MapCollection mappings, List<BaseMapEntry> members) : base(mapEntry, mappings)
        {
            if (mapEntry is ClassMap == false)
            {
                throw new Exception($"Created entry viewmodel with wrong entry type! (Type is {mapEntry.GetType().Name}, expected ClassMap)");
            }

            foreach(BaseMapEntry entry in members)
            {
                if (entry is FieldMap)
                {
                    Members.Add(new FieldMapViewModel(entry, mappings, mapEntry as ClassMap));
                }
                else if (entry is MethodMap)
                {
                    Members.Add(new MethodMapViewModel(entry, mappings, mapEntry as ClassMap));
                }
                else
                {
                    throw new ArgumentException("Member in <members> cannot be added to class!");
                }
            }
        }
    }
}
