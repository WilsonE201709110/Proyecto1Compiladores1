using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace C1_proyecto1
{
    public partial class Form1 : Form
    {

        private int con_Page = 0; //contador global

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Analisador_Lexico al = new Analisador_Lexico();
            Analisador_Lexico.setTabla_Resetear(Analisador_Lexico.getConjuntos());
            Analisador_Lexico.setTabla_Resetear(Analisador_Lexico.getLexemas());
            Analisador_Lexico.setTabla_Resetear(Analisador_Lexico.getExpresionesRegulares());

            if (!al.getHayErroes())
            {
                Control box;

                if (tabControl1.SelectedTab.HasChildren)
                {

                    foreach (Control item in tabControl1.SelectedTab.Controls)
                    {
                        box = item;
                        if (box is RichTextBox)
                        {
                            al.Analizador(box.Text);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(":(");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) //Guardar
        {
            Control box;

            if (tabControl1.SelectedTab.HasChildren)
            {
                /*
                 * si la pestaña selecionade tiene hijos recorremos los 
                 * controles de la pestaña
                 * */
                foreach (Control item in tabControl1.SelectedTab.Controls)
                {
                    box = item;
                    if(box is RichTextBox)
                    {
                        SaveFileDialog sfd = new SaveFileDialog()
                        {
                            Title = "Seleccionar el destino",
                            Filter = "Archivo | *.er",
                            AddExtension = true
                        };
                        sfd.ShowDialog();

                        StreamWriter writer = new StreamWriter(sfd.FileName);
                        writer.Write(box.Text);
                        writer.Close();

                        tabControl1.SelectedTab.Text = sfd.FileName;
                    }
                }
            }
            else
            {
                MessageBox.Show(":(");
            }


        }

        private void button2_Click(object sender, EventArgs e) //NUEVO
        {
            TabPage page = new TabPage() //creamos nuestro tabComtrol
            {
                Text = "Archivo Nuevo",
                Name = "Page" + con_Page
            };

            //AddPage(page, "");

            RichTextBox caja = new RichTextBox() //creamos nuestro RichTExtBox
            {
                Name = "richTextBox" + con_Page,
                Text = "",
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Fill,
                AcceptsTab = true
            };

            page.Controls.Add(caja); //agregamos el richtextbox la apagina tab
             
            tabControl1.Controls.Add(page); //agragamos nuestro tab en el tabcontrol q tenemos
        }

        private void AddPage(TabPage page, string v)
        {
            throw new NotImplementedException();
        }

        

        private void button7_Click(object sender, EventArgs e) //Abrir
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Expresion",
                Filter = "Archivo Expresion Regulares | *.er",

            };
            ofd.ShowDialog();

            
            
            TabPage page = new TabPage()
            {
                Text = ofd.FileName,
                Name = "Page" + con_Page
            };

            StreamReader reader = new StreamReader(ofd.FileName); //clase io

            RichTextBox caja = new RichTextBox() //creamos nuestro RichTExtBox
            {
                Name = "richTextBox" + con_Page,
                Text = reader.ReadToEnd(),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Fill,
                AcceptsTab = true
            };

            page.Controls.Add(caja); //agregamos el richtextbox la apagina tab

            tabControl1.Controls.Add(page); //agragamos nuestro tab en el tabcontrol q tenemos
            reader.Close();
        }

        
        //botones de graficar
        private int numeroExpresion = -1;
        private static string[,] ExpresionRegular = Analisador_Lexico.getExpresionesRegulares();
        private void button5_Click(object sender, EventArgs e)
        {
            if (numeroExpresion >= 0)
            {
                if (ExpresionRegular[numeroExpresion, 1] != null)
                {
                    AFND automata = new AFND(ExpresionRegular[numeroExpresion, 1]);
                    automata.convertidorPolacaSimplificada();

                    //GRaficar el AFD
                    GraficarAFD gaficar = new GraficarAFD(AFD.getTablaAFD(), Transformador.getTablaAceptaciones());
                    gaficar.Graficar();
                    gaficar.abrirgrafo();
                    MostrarImagen();

                    mostrarEnTabla(AFD.getTablaAFD());

                    VerificadorDeLexemas vl = new VerificadorDeLexemas();
                    richTextBox1.Text = vl.validarLexema(Analisador_Lexico.getLexemas(), AFD.getTablaAFD(), Analisador_Lexico.getConjuntos(),Transformador.getTablaAceptaciones(), ExpresionRegular[numeroExpresion, 0]);
                    
                    AFD.setTablaAFD_Resetear();
                    AFND.setTablaDeS_Resetear();
                    Transformador.setTablaAceptacionBorrar();
                }
            }
            numeroExpresion--;


            
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (numeroExpresion >= 0)
            {
                if (ExpresionRegular[numeroExpresion, 1] != null)
                {
                    AFND automata = new AFND(ExpresionRegular[numeroExpresion, 1]);
                    automata.convertidorPolacaSimplificada();

                    //GRaficar el AFD
                    GraficarAFD gaficar = new GraficarAFD(AFD.getTablaAFD(), Transformador.getTablaAceptaciones());
                    gaficar.Graficar();
                    gaficar.abrirgrafo();
                    MostrarImagen();

                    mostrarEnTabla(AFD.getTablaAFD());

                    VerificadorDeLexemas vl = new VerificadorDeLexemas();
                    richTextBox1.Text = vl.validarLexema(Analisador_Lexico.getLexemas(),AFD.getTablaAFD(),Analisador_Lexico.getConjuntos(),Transformador.getTablaAceptaciones(),ExpresionRegular[numeroExpresion,0]);

                    AFD.setTablaAFD_Resetear();
                    AFND.setTablaDeS_Resetear();
                    
                    Transformador.setTablaAceptacionBorrar();
                }
            }
            numeroExpresion++;

        }

        private void MostrarImagen()
        {
                pictureBox1.ImageLocation = "C:\\Users\\gmg\\Desktop\\Automatas\\Imagen_AFD.png"; //Le damos la diracion ala imagen
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; //acomoda la imagen
        }

        private void mostrarEnTabla(string[,] matrizAFD)
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();
            var filas = matrizAFD.GetLength(1);
            listView1.View = View.Details;
            int columnas = 0;
            for (int i=0;i<filas; i++)
            {
                if (matrizAFD[0, i] != null)
                {
                    listView1.Columns.Add(matrizAFD[0,i], 60, HorizontalAlignment.Center);
                    columnas++;
                }
            }
            for (int i = 1; i < filas; i++)
            {
                if (matrizAFD[i,0]!=null)
                {
                    ListViewItem item1 = new ListViewItem(matrizAFD[i,0],i);
                    item1.Checked = true;
                    for (int j = 1; j < columnas; j++)
                    {
                        item1.SubItems.Add(matrizAFD[i,j]);
                    }

                    listView1.Items.AddRange(new ListViewItem[] { item1});
                }

                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();

            frm2.Show();
        }
    }
}
