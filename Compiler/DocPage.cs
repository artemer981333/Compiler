using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    class DocPage
    {
        private string resultText;
        private string text;
        private string title;
        private string fileName;
        private bool saved;

        public string Text
        {
            get => text;
            set
            {
                saved = false;
                text = value;
            }
        }
        public string ResultText { get => resultText; }
        public string Title { get => title; }
        public bool Saved { get => saved; }
        public string FileName { get => fileName; set => fileName = value; }

        public static DocPage OpenFromFile(string fileName)
        {
            StreamReader file = new StreamReader(fileName);
            DocPage ret = new DocPage(fileName);
            ret.text = file.ReadToEnd();
            ret.fileName = fileName;
            ret.saved = true;
            return ret;
        }
        public DocPage(string title = "Новый документ")
        {
            text = "";
            resultText = "";
            fileName = null;
            this.title = title;
            saved = false;
        }

        public void Close()
        {
            if (saved)
                return;
            DialogResult res = MessageBox.Show("Сохранить файл " + title + "?", "Сохранение файла", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
                Save();
        }

        public void Save()
        {
            if (saved)
                return;
            if (fileName == null)
            {
                SaveAs();
                return;
            }
            StreamWriter file = new StreamWriter(fileName);
            file.Write(text);
            file.Close();
            saved = true;
        }
        public void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = title;
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
                return;
            fileName = saveFileDialog.FileName;
            title = fileName;
            StreamWriter file = new StreamWriter(fileName);
            file.Write(text);
            file.Close();
            saved = true;
        }
    }
}
