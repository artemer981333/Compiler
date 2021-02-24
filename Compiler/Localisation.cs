using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compiler
{
    struct LocalisationElement
    {
        public string Name;
        public string Text;

        public LocalisationElement(string Name, string Text)
        {
            this.Name = Name;
            this.Text = Text;
        }
    }
    class Localisation
    {
        public class NameComparer
        {
            string _s;

            public NameComparer(string s)
            {
                _s = s;
            }

            public bool cmp(LocalisationElement elem)
            {
                return elem.Name == _s;
            }
        }

        List<LocalisationElement> localisationElements;

        public Localisation()
        {
            localisationElements = new List<LocalisationElement>();
        }

        public void LoadFromFile(string fileName)
        {
            StreamReader file;
            try
            {
                file = new StreamReader(fileName);
            }
            catch (Exception e)
            {
                throw new Exception("Can't open file");
            }
            localisationElements.Clear();
            while (!file.EndOfStream)
                localisationElements.Add(new LocalisationElement(file.ReadLine(), file.ReadLine()));
        }
        public string this[string Name]
        {
            get
            {
                NameComparer nc = new NameComparer(Name);
                int index = localisationElements.FindIndex(nc.cmp);
                if (index == -1)
                    return "NOT FOUND";
                else
                    return localisationElements[index].Text;
            }
        }
    }
}
