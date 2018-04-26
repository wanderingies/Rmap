using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rmap
{
    public partial class RForm : Form
    {
        public RForm()
        {
            InitializeComponent();
        }

        int verion = 0;
        float width = 0;
        float length = 0;

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void textBox_DoubleClick(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "")
                return;

            if (listBox.Items.Count >= 3)
                return;

            string value = textBox1.Text + ";" + textBox2.Text + ";" + textBox3.Text + ";" + textBox4.Text + ";" + textBox5.Text;

            if (listBox.Items.Contains(value))
                return;

            if (currentSelectedIndex != -1)
                listBox.Items.RemoveAt(currentSelectedIndex);

            listBox.Items.Add(value);
            currentSelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "wmap|*.wmap";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = File.OpenRead(openFileDialog.FileName))
                    {
                        using (BinaryReader binaryReader = new BinaryReader(stream))
                        {
                            verion = binaryReader.ReadInt32();
                            width = binaryReader.ReadSingle();
                            length = binaryReader.ReadSingle();

                            int count = binaryReader.ReadInt32();

                            for (int i = 0; i < count; i++)
                            {
                                listBox.Items.Add(
                                    binaryReader.ReadSingle().ToString() + ";" +
                                    binaryReader.ReadSingle().ToString() + ";" +
                                    binaryReader.ReadSingle().ToString() + ";" +
                                    binaryReader.ReadSingle().ToString() + ";" +
                                    binaryReader.ReadSingle().ToString());
                            }
                        }
                    }
                }
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (verion == 0 && width == 0 && length == 0) return;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "wmap|*.wmap";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using(Stream stream = File.Create(saveFileDialog.FileName))
                    {
                        using (BinaryWriter binaryWriter = new BinaryWriter(stream))
                        {
                            binaryWriter.Write(verion);
                            binaryWriter.Write(width);
                            binaryWriter.Write(length);

                            for (int i = 0; i < listBox.Items.Count; i++)
                            {
                                string[] line = listBox.Items[i].ToString().Split(';');

                                binaryWriter.Write(float.Parse(line[0]));
                                binaryWriter.Write(float.Parse(line[1]));
                                binaryWriter.Write(float.Parse(line[2]));
                                binaryWriter.Write(float.Parse(line[3]));
                                binaryWriter.Write(float.Parse(line[4]));

                            }
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex <= -1) return;

            listBox.Items.RemoveAt(listBox.SelectedIndex);
        }

        private int currentSelectedIndex = -1;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex <= -1) return;

            currentSelectedIndex = listBox.SelectedIndex;
            string[] line = listBox.SelectedItem.ToString().Split(';');

            textBox1.Text = line[0];
            textBox2.Text = line[1];
            textBox3.Text = line[2];
            textBox4.Text = line[3];
            textBox5.Text = line[4];
        }
    }
}
