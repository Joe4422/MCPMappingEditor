using MCPMappingEditor.Model;

namespace MCPMappingEditor.ViewModel
{
    public abstract class BaseClassMemberMapEntryViewModel : BaseMapEntryViewModel
    {
        protected ClassMap parentClass;
        public string OriginalClass => string.Join('/', mapEntry.Original.Split('/')[..^1]);
        public string OriginalMember => mapEntry.Original.Split('/')[^1];
        public string SubstituteClass => parentClass == null ? OriginalClass : parentClass.Substitute;

        public BaseClassMemberMapEntryViewModel(BaseMapEntry mapEntry, MapCollection mappings, ClassMap classMapEntry) : base(mapEntry, mappings)
        {
            parentClass = classMapEntry ?? throw new System.ArgumentNullException(nameof(classMapEntry));
            if (parentClass != null) parentClass.PropertyChanged += ParentPropertyChanged;
        }

        private void ParentPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }
    }
}
