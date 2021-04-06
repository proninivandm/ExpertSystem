using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ExpertSystem
{
    public partial class Form1 : Form
    {
        int current;
        List<Node> nodes;
        public class Answer
        {
            public string answer;
            public int nextNode;
        }

        public class Node
        {
            public string title;
            public int prevNode;
            public List<Answer> answers;
            public string help;
        }

        public Form1()
        {
            InitializeComponent();
            using (StreamReader r = new StreamReader(new MemoryStream(Properties.Resources.nodes)))
            {
                string json = r.ReadToEnd();
                nodes = JsonConvert.DeserializeObject<List<Node>>(json);
            }
            current = 1;
            Show_node(current);
        }

        private void next_b_Click(object sender, EventArgs e)
        {
            if ((nodes[current].title != "ans")&&(checkedListBox1.CheckedIndices.Count != 0))
            {
                int next = nodes[current].answers[checkedListBox1.CheckedIndices[0]].nextNode;
                if (nodes[next].title != "ans")
                {
                    Show_node(next);
                    current = next;
                }
                else
                {
                    Show_ans(next);
                    current = next;
                }
            }
            
        }
        private void back_b_Click(object sender, EventArgs e)
        {
            int prev = nodes[current].prevNode;
            if (prev != 0)
            {
                Show_node(prev);
                current = prev;
            }
        }

        private void start_b_Click(object sender, EventArgs e)
        {
            Show_node(1);
            current = 1;
        }

        void Show_node(int cur)
        {
            button2.Visible = false;
            checkedListBox1.Visible = true;
            Qtext.Text = nodes[cur].title;
            checkedListBox1.Items.Clear();
            List<Answer> anses = nodes[cur].answers;
            for(int i = 0; i<anses.Count; i++)
            {
                checkedListBox1.Items.Add(anses[i].answer, false);
            }
        }

        void Show_ans(int cur)
        {
            Qtext.Text = "Ответ :  " + nodes[cur].answers[0].answer;
            checkedListBox1.Visible = false;
            button2.Visible = true;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 1)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
                checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
            }
        }

        private void Qtext_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(nodes[current].help, "Помощь", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "https://yandex.ru/search/?text=" + nodes[current].answers[0].answer;
            System.Diagnostics.Process.Start(url);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
