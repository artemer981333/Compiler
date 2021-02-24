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
        private static string defaultTitle = "";
        private string resultText;
        private string text;
        private string title;
        private string fileName;
        private bool saved;
        private Stack<string> States, CanceledStates;

        public string Text
        {
            get => text;
            set
            {
                saved = false;
                text = value;
                if (text != States.Peek())
                    SaveState();
            }
        }
        public string ResultText { get => resultText; }
        public string Title { get => title; }
        public bool Saved { get => saved; }
        public bool CanCancel { get => States.Count > 1; }
        public bool CanRepeat { get => CanceledStates.Count > 0; }
        public string FileName { get => fileName; set => fileName = value; }
        public static string DefaultTitle { get => defaultTitle; set => defaultTitle = value; }

        public static DocPage OpenFromFile(string fileName)
        {
            StreamReader file = new StreamReader(fileName);
            DocPage ret = new DocPage(fileName);
            ret.text = file.ReadToEnd();
            ret.fileName = fileName;
            ret.saved = true;
            return ret;
        }
        public DocPage(string title)
        {
            text = "";
            resultText = "";
            fileName = null;
            this.title = title;
            saved = false;
            States = new Stack<string>();
            CanceledStates = new Stack<string>();
            SaveState();
        }
        public DocPage()
        {
            text = "";
            resultText = "";
            fileName = null;
            this.title = defaultTitle;
            saved = false;
            States = new Stack<string>();
            CanceledStates = new Stack<string>();
            SaveState();
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
        private void SaveState()
        {
            CanceledStates.Clear();
            States.Push(text);
        }
        public void CancelState()
        {
            if (States.Count == 1)
                return;
            CanceledStates.Push(States.Pop());
            text = States.Peek();
        }
        public void RepeatState()
        {
            if (CanceledStates.Count == 0)
                return;
            States.Push(CanceledStates.Pop());
            text = States.Peek();
        }

    }
}
