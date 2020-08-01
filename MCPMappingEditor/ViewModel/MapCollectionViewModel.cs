using MCPMappingEditor.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MCPMappingEditor.ViewModel
{
    public class MapCollectionViewModel
    {
        private readonly MapCollection mappings;

        public ObservableCollection<ClassMapViewModel> ClassMaps { get; } = new ObservableCollection<ClassMapViewModel>();

        public MapCollectionViewModel(MapCollection mappings)
        {
            this.mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));

            InitialiseClassMaps();
        }

        private void InitialiseClassMaps()
        {
            foreach (BaseMapEntry entry in mappings)
            {
                if (entry is ClassMap == false) continue;

                ClassMap classMap = entry as ClassMap;
                List<BaseMapEntry> members = new List<BaseMapEntry>();

                foreach (BaseMapEntry memberEntry in mappings)
                {
                    if (memberEntry is FieldMap || memberEntry is MethodMap)
                    {
                        if (string.Join('/', memberEntry.Original.Split('/')[..^1]) == classMap.Original)
                        {
                            members.Add(memberEntry);
                        }
                    }
                    else continue;
                }

                ClassMaps.Add(new ClassMapViewModel(entry, mappings, members));
            }
        }
    }
}
