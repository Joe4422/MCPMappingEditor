using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCPMappingEditor.Model
{
    public class MapCollection : ICollection<BaseMapEntry>
    {
        protected List<BaseMapEntry> entries = new List<BaseMapEntry>();

        public ClassMap GetClassMapByOriginalName(string className)
        {
            BaseMapEntry[] classesOfName = ClassMaps.Where((x) => x.Original == className).ToArray();
            if (classesOfName.Length == 1)
            {
                return classesOfName[0] as ClassMap;
            }
            else
            {
                if (classesOfName.Length > 1) Console.WriteLine($"Multiple mappings to class with original name {className}!");
                return null;
            }
        }

        public bool CollectionModified { get; set; }

        public List<ClassMap> ClassMaps => entries.Where((x) => x is ClassMap).Select((x) => x as ClassMap).ToList();
        public List<FieldMap> FieldMaps => entries.Where((x) => x is FieldMap).Select((x) => x as FieldMap).ToList();
        public List<MethodMap> MethodMaps => entries.Where((x) => x is MethodMap).Select((x) => x as MethodMap).ToList();

        public List<BaseMapEntry> ClassMembers(ClassMap classMap) => entries.Where((x) => (x is ClassMap == false) && (string.Join('/', x.Original.Split('/')[..^1]) == classMap.Original)).ToList();

        public int Count => ((ICollection<BaseMapEntry>)entries).Count;

        public bool IsReadOnly => ((ICollection<BaseMapEntry>)entries).IsReadOnly;

        public void Add(BaseMapEntry item)
        {
            ((ICollection<BaseMapEntry>)entries).Add(item);
        }

        public void Clear()
        {
            ((ICollection<BaseMapEntry>)entries).Clear();
        }

        public bool Contains(BaseMapEntry item)
        {
            return ((ICollection<BaseMapEntry>)entries).Contains(item);
        }

        public void CopyTo(BaseMapEntry[] array, int arrayIndex)
        {
            ((ICollection<BaseMapEntry>)entries).CopyTo(array, arrayIndex);
        }

        public IEnumerator<BaseMapEntry> GetEnumerator()
        {
            return ((IEnumerable<BaseMapEntry>)entries).GetEnumerator();
        }

        public bool Remove(BaseMapEntry item)
        {
            return ((ICollection<BaseMapEntry>)entries).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)entries).GetEnumerator();
        }
    }
}
