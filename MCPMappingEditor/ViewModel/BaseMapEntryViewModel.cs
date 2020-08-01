using MCPMappingEditor.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace MCPMappingEditor.ViewModel
{
    public abstract class BaseMapEntryViewModel : INotifyPropertyChanged
    {
        protected readonly BaseMapEntry mapEntry;
        protected readonly MapCollection mappings;

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract string ListLabelText { get; }

        private Visibility _IsSubstituteTextBoxVisible = Visibility.Hidden;
        public Visibility IsSubstituteTextBoxVisible
        {
            get => _IsSubstituteTextBoxVisible;
            set
            {
                if (_IsSubstituteTextBoxVisible != value)
                {
                    _IsSubstituteTextBoxVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Visibility IsSubstituteLabelVisible => IsSubstituteTextBoxVisible == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        private bool _IsTextBoxFocused = false;
        public bool IsTextBoxFocused
        {
            get => _IsTextBoxFocused;
            set
            {
                if (_IsTextBoxFocused != value)
                {
                    _IsTextBoxFocused = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public abstract string OriginalLabel { get; }

        public string Original => mapEntry.Original;

        public string Substitute
        {
            get => mapEntry.Substitute;
            set
            {
                if (mapEntry.Substitute != value)
                {
                    mapEntry.Substitute = value;
                    mappings.CollectionModified = true;
                }
            }
        }

        public BaseMapEntryViewModel(BaseMapEntry mapEntry, MapCollection mappings)
        {
            this.mapEntry = mapEntry ?? throw new ArgumentNullException(nameof(mapEntry));
            this.mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
            mapEntry.PropertyChanged += MapEntry_PropertyChanged;
        }

        private void MapEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Substitute")
            {
                NotifyPropertyChanged("Substitute");
                NotifyPropertyChanged("ListLabelText");
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
