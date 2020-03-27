using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace C1_proyecto1
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = " "; 
            XmlTextReader xmlTextReader = new XmlTextReader("C:\\Users\\gmg\\Desktop\\SalidaDeTokens.xml");
            string ultimaEtiqueta = "";
            while (xmlTextReader.Read())
            {
                if (xmlTextReader.NodeType==XmlNodeType.Element)
                {
                    richTextBox1.Text += (new String(' ', xmlTextReader.Depth*3)+"<"+xmlTextReader.Name+">");
                    ultimaEtiqueta = xmlTextReader.Name;
                    continue;
                }
                if (xmlTextReader.NodeType == XmlNodeType.Text)
                {
                    richTextBox1.Text += xmlTextReader.ReadContentAsString() + "</" + ultimaEtiqueta + ">";
                }
                else
                {
                    richTextBox1.Text += "\r";
                }
                    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = " ";
            XmlTextReader xmlTextReader = new XmlTextReader("C:\\Users\\gmg\\Desktop\\SalidaDeErrores.xml");
            string ultimaEtiqueta = "";
            while (xmlTextReader.Read())
            {
                if (xmlTextReader.NodeType == XmlNodeType.Element)
                {
                    richTextBox1.Text += (new String(' ', xmlTextReader.Depth * 3) + "<" + xmlTextReader.Name + ">");
                    ultimaEtiqueta = xmlTextReader.Name;
                    continue;
                }
                if (xmlTextReader.NodeType == XmlNodeType.Text)
                {
                    richTextBox1.Text += xmlTextReader.ReadContentAsString() + "</" + ultimaEtiqueta + ">";
                }
                else
                {
                    richTextBox1.Text += "\r";
                }

            }
        }
    }
}
