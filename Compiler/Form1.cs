using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Form1 : Form
    {
        List<DocPage> Pages;
        private Stack<string> States, CanceledStates;
        private string CopyBuffer;

        public Form1()
        {
            InitializeComponent();
            States = new Stack<string>();
            CanceledStates = new Stack<string>();
            Pages = new List<DocPage>();
            Pages.Add(new DocPage());
            PagesTab.TabPages.Add(new TabPage(Pages[0].Title));
            SaveState();
        }
        private void SaveState()
        {
            CanceledStates.Clear();
            States.Push(CodeField.Text);
        }
        private void SaveFile(int pageNumber)
        {
            Pages[pageNumber].Save();
            PagesTab.TabPages[pageNumber].Text = Pages[pageNumber].Title;
        }
        private void SaveAsFile(int pageNumber)
        {
            Pages[pageNumber].SaveAs();
            PagesTab.TabPages[pageNumber].Text = Pages[pageNumber].Title;
        }
        private void UpdateText(object sender, EventArgs e)
        {
            Pages[PagesTab.SelectedIndex].Text = CodeField.Text;
            if (CanceledStates.Count == 0)
                SaveState();
        }
        private void CreateClick(object sender, EventArgs e)
        {
            Pages.Add(new DocPage());
            PagesTab.TabPages.Add(new TabPage(Pages[Pages.Count - 1].Title));
        }
        private void OpenClick(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult res = openFileDialog.ShowDialog();
            if (res == DialogResult.Cancel)
                return;
            try
            {
                Pages.Add(DocPage.OpenFromFile(openFileDialog.FileName));
                PagesTab.TabPages.Add(new TabPage(Pages[Pages.Count - 1].Title));
            }
            catch (Exception err)
            {
                ResultField.Text = err.Message;
            }
        }

        private void SaveClick(object sender, EventArgs e)
        {
            SaveFile(PagesTab.SelectedIndex);
        }

        private void SaveAsClick(object sender, EventArgs e)
        {
            SaveAsFile(PagesTab.SelectedIndex);
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void CancelClick(object sender, EventArgs e)
        {
            if (States.Count == 1)
                return;
            CanceledStates.Push(States.Pop());
            CodeField.Text = States.Peek();
            toolStripButton5.Enabled = true;
            if (States.Count == 1)
                toolStripButton4.Enabled = false;
        }

        private void RepeatClick(object sender, EventArgs e)
        {
            if (CanceledStates.Count == 0)
                return;
            States.Push(CanceledStates.Pop());
            CodeField.Text = States.Peek();
            toolStripButton4.Enabled = true;
            if (CanceledStates.Count == 0)
                toolStripButton5.Enabled = false;
        }

        private void CutClick(object sender, EventArgs e)
        {
            if (CodeField.SelectionLength == 0)
                return;
            int SelectionStart = CodeField.SelectionStart;
            CopyBuffer = CodeField.SelectedText;
            CodeField.Text = CodeField.Text.Remove(CodeField.SelectionStart, CodeField.SelectionLength);
            CodeField.SelectionStart = SelectionStart;
            SaveState();
        }

        private void CopyClick(object sender, EventArgs e)
        {
            if (CodeField.SelectionLength == 0)
                return;
            CopyBuffer = CodeField.SelectedText;
        }

        private void PasteClick(object sender, EventArgs e)
        {
            int SelectionStart;
            if(CodeField.SelectionLength != 0)
            {
                SelectionStart = CodeField.SelectionStart;
                CodeField.Text = CodeField.Text.Remove(CodeField.SelectionStart, CodeField.SelectionLength);
                CodeField.SelectionStart = SelectionStart;
            }
            SelectionStart = CodeField.SelectionStart + CopyBuffer.Length;
            CodeField.Text = CodeField.Text.Insert(CodeField.SelectionStart, CopyBuffer);
            CodeField.SelectionStart = SelectionStart;
            SaveState();
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            if (CodeField.SelectionLength == 0)
                return;
            int SelectionStart = CodeField.SelectionStart;
            CodeField.Text = CodeField.Text.Remove(CodeField.SelectionStart, CodeField.SelectionLength);
            CodeField.SelectionStart = SelectionStart;
            SaveState();
        }

        private void TabChanged(object sender, EventArgs e)
        {
            CodeField.Text = Pages[PagesTab.SelectedIndex].Text;
            ResultField.Text = Pages[PagesTab.SelectedIndex].ResultText;
        }

        private void CloseForm(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < Pages.Count; i++)
                Pages[i].Close();
        }

        private void CodeFontUp(object sender, EventArgs e)
        {
            CodeField.Font = new Font(CodeField.Font.FontFamily, Math.Min(CodeField.Font.Size + 1, 20));
        }

        private void CodeFontDown(object sender, EventArgs e)
        {
            CodeField.Font = new Font(CodeField.Font.FontFamily, Math.Max(CodeField.Font.Size - 1, 10));
        }

        private void OutFontUp(object sender, EventArgs e)
        {
            ResultField.Font = new Font(ResultField.Font.FontFamily, Math.Min(ResultField.Font.Size + 1, 20));
        }

        private void OutFontDown(object sender, EventArgs e)
        {
            ResultField.Font = new Font(ResultField.Font.FontFamily, Math.Min(ResultField.Font.Size - 1, 20));
        }

        private void Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var fileNames = data as string[];
                for (int i = 0; i < fileNames.Length; i++)
                {
                    try
                    {
                        Pages.Add(DocPage.OpenFromFile(fileNames[i]));
                        PagesTab.TabPages.Add(new TabPage(Pages[Pages.Count - 1].Title));
                    }
                    catch (Exception err)
                    {
                        ResultField.Text = err.Message;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CodeField.AllowDrop = true;
            CodeField.DragDrop += Drop;
        }

        private void About(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("help\\index.html");
        }

        private void SelectAllClick(object sender, EventArgs e)
        {
            CodeField.SelectionStart = 0;
            CodeField.SelectionLength = CodeField.Text.Length;
        }
    }
}
