using MCPMappingEditor.Model;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;

namespace MCPMappingEditor.ViewModel
{
    public class MethodMapViewModel : BaseClassMemberMapEntryViewModel
    {
        private static Dictionary<char, string> typeChars = new Dictionary<char, string>() { { 'B', "byte" },
                                                                                      { 'S', "short" },
                                                                                      { 'I', "int" },
                                                                                      { 'J', "long" },
                                                                                      { 'F', "float" },
                                                                                      { 'D', "double" },
                                                                                      { 'Z', "boolean" },
                                                                                      { 'C', "char" },
                                                                                      { 'V', "void" } };

        public string SignatureString
        {
            get
            {
                MethodMap methodMap = mapEntry as MethodMap;
                string returnType = TypeCharStringToNames(methodMap.Signature.Split(')')[1]);
                string parameterTypes = TypeCharStringToNames(methodMap.Signature.Split(')')[0][1..]);

                return $"{returnType} {Substitute}({parameterTypes})";
            }
        }

        public override string OriginalLabel => $"({OriginalMember})";
        public override string ListLabelText => $"_{SignatureString}";

        public MethodMapViewModel(BaseMapEntry mapEntry, MapCollection mappings, ClassMap classMapEntry) : base(mapEntry, mappings, classMapEntry)
        {
            if (mapEntry is MethodMap == false)
            {
                throw new Exception($"Created entry viewmodel with wrong entry type! (Type is {mapEntry.GetType().Name}, expected MethodMap)");
            }
            mapEntry.PropertyChanged += MapEntry_PropertyChanged;
        }

        private void MapEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Substitute")
                NotifyPropertyChanged("SignatureString");
        }

        private string TypeCharStringToNames(string typeCharString)
        {
            if (typeCharString == "") return "";

            Regex regex = new Regex(@"(\[*?)(([BSIJFDZCJV])|(L)(.+?);)");
            MatchCollection matches = regex.Matches(typeCharString);

            List<string> typeCharParts = new List<string>();

            foreach (Match match in matches)
            {
                string matchString = "";
                if (match.Groups[4].Value == "L") // Is reference type name
                {
                    ClassMap map = mappings.GetClassMapByOriginalName(match.Groups[5].Value);
                    if (map != null)
                    {
                        matchString = map.Substitute;
                    }
                    else
                    {
                        matchString = match.Groups[5].Value;
                    }
                }
                else // Is value type name
                {
                    matchString = typeChars[match.Groups[3].Value[0]];
                }

                foreach (char _ in match.Groups[1].Value)
                {
                    matchString += "[]";
                }

                typeCharParts.Add(matchString);
            }

            return string.Join(", ", typeCharParts);
        }
    }
}
