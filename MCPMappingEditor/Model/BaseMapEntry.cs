using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCPMappingEditor.Model
{

    public abstract class BaseMapEntry : INotifyPropertyChanged
    {
        protected string[] sourceString;

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract string MappingType { get; }
        public string Original { get; protected set; }
        private string _Substitute;
        public string Substitute
        {
            get => _Substitute;
            set
            {
                if (_Substitute != value)
                {
                    _Substitute = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int LineNumber { get; }

        public BaseMapEntry(string line, int lineNumber)
        {
            sourceString = line.Split(' ');
            LineNumber = lineNumber;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
