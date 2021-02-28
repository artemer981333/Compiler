using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    class CodeHandler
    {
        private RichTextBox richTextBox;

        private Regex integer;
        public CodeHandler(RichTextBox textBox)
        {
            richTextBox = textBox;
            integer = new Regex(@"\bint\b");
        }

        public void HandleText()
        {
            MatchCollection matchCollection = integer.Matches(richTextBox.Text);
            richTextBox.Enabled = false;
            richTextBox.Visible = false;
            Control control = richTextBox.Parent;
            while (control.GetType().Name != "Form1")
            {
                control = control.Parent;
            }
            int selectionStart = richTextBox.SelectionStart;
            int selectionLength = richTextBox.SelectionLength;
            richTextBox.SelectAll();
            richTextBox.SelectionColor = Color.Black;
            foreach (Match match in matchCollection)
            {
                richTextBox.Select(match.Index, match.Length);
                richTextBox.SelectionColor = Color.Red;
            }
            richTextBox.Select(selectionStart, selectionLength);
            richTextBox.Visible = true;
            richTextBox.Enabled = true;
            (control as Form).ActiveControl = richTextBox;

        }
    }
}
