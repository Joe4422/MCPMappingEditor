using MCPMappingEditor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCPMappingEditor
{
    public class MapFileManager
    {
        private List<string> fileContents = new List<string>();
        public MapCollection MapCollection { get; private set; }

        public MapCollection Deserialize(string filePath)
        {
            MapCollection = new MapCollection();

            if (File.Exists(filePath) == false) throw new FileNotFoundException();

            using StreamReader sr = File.OpenText(filePath);

            string s;
            while ((s = sr.ReadLine()) != null)
            {
                fileContents.Add(s);
            }

            sr.Close();

            for (int i = 0; i < fileContents.Count; i++)
            {
                string line = fileContents[i];

                BaseMapEntry bme = LineToBaseMapEntry(line, i);
                if (bme != null) MapCollection.Add(bme);
            }

            return MapCollection;
        }

        public void Serialise(string filePath)
        {
            foreach(BaseMapEntry entry in MapCollection)
            {
                fileContents[entry.LineNumber] = entry.ToString();
            }

            using StreamWriter sw = new StreamWriter(File.OpenWrite(filePath));

            foreach (string s in fileContents)
            {
                sw.WriteLine(s);
            }

            sw.Close();
        }

        private BaseMapEntry LineToBaseMapEntry(string line, int lineNum)
        {
            if (line.StartsWith(".class_map"))
            {
                return new ClassMap(line[".class_map ".Length..], lineNum);
            }
            else if (line.StartsWith(".field_map"))
            {
                return new FieldMap(line[".field_map ".Length..], lineNum);
            }
            else if (line.StartsWith(".method_map"))
            {
                return new MethodMap(line[".method_map ".Length..], lineNum);
            }
            else
            {
                return null;
            }
        }
    }
}
