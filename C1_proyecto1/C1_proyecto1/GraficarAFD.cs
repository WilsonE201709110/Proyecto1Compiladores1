using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace C1_proyecto1
{
    class GraficarAFD
    {
        String ruta;
        StringBuilder grafo;

        private string[,] tablaAFD = new string[100, 10];
        private string[] tablaAceptacion = new string[10];
       /// private string[,] tablaDeConjuntosyCadenas = new string[20, 2];

        public GraficarAFD(string[,] tablaAFD,string[] tablaAceptacion)
        {
            //Limpiador(); //Limpiar todo
            this.tablaAFD = tablaAFD;
            this.tablaAceptacion = tablaAceptacion;
            Console.WriteLine("Inicio");
            RecorrerArreglo<string>(tablaAFD);
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); //Debuelve la ruta del escritorio
            //this.tablaDeConjuntosyCadenas = AFND.getTablaDeS(); //pasar la tabla que esta en AFND 
        }

        public void Graficar()
        {
            grafo = new StringBuilder();
            String rdot = ruta + "\\Automatas\\Imagen_AFD.dot";
            String rpng = ruta + "\\Automatas\\Imagen_AFD.png";

            string texto = archivo();
            Console.WriteLine(texto);

            grafo.Append("digraph G {\n");
            grafo.Append(texto);
            this.generardot(rdot, rpng);

            Console.WriteLine("Final");
            RecorrerArreglo<string>(tablaAFD);
            //Limpiador(); //Limpiar todo

        }

        private string archivo()
        {
            string textoGrafica = "";
            var filas = tablaAFD.GetLength(0);
            var columnas = tablaAFD.GetLength(1);
            for (int i=1; i<filas;i++)
            {
                bool siEsAceptacion = false;
                if (tablaAFD[i, 0] != null)
                {
                    for (int j=0; j<tablaAceptacion.Length;j++) //buscar los estados de aceptacion
                    {
                        if (tablaAceptacion[j] != null) if (tablaAFD[i,0]==tablaAceptacion[j]) siEsAceptacion = true; ; //si esta tonce s siEsAceptacion ygual a verdadero
                    }
                    if (!siEsAceptacion)
                    {
                        textoGrafica += tablaAFD[i, 0] + "[shape=circle,label=\"" + tablaAFD[i, 0] + "\"];\n";
                    }
                    else
                    {
                        textoGrafica += tablaAFD[i, 0] + "[shape=doublecircle,label=\"" + tablaAFD[i, 0] + "\"];\n";
                    }
                }
            }
            for (int i=1;i<filas;i++)
            {
                for (int j=1;j<columnas;j++)
                {
                    if (tablaAFD[i, j] != null)
                    {
                        if (tablaAFD[0, j] == "\n")
                        {
                            textoGrafica += tablaAFD[i, 0] + "->" + tablaAFD[i, j] + "[label=\"n\"];\n";
                        }
                        else if (tablaAFD[0, j] == "\r")
                        {
                            textoGrafica += tablaAFD[i, 0] + "->" + tablaAFD[i, j] + "[label=\"r\"];\n";
                        }
                        else
                        {
                            textoGrafica += tablaAFD[i, 0] + "->" + tablaAFD[i, j] + "[label=\"" + tablaAFD[0, j] + "\"];\n";
                        }

                            
                        
                        
                    }
                }
            }
            textoGrafica += "}\n";

            return textoGrafica;

        }

        private void generardot(String rdot, String rpng)
        {
            System.IO.File.WriteAllText(rdot, grafo.ToString());
            String comandot = "dot.exe -Tpng " + rdot + " -o " + rpng + " ";
            var comando = string.Format(comandot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C" + comando);
            var proc = new System.Diagnostics.Process(); //Esta variable ejecuta procesos
            proc.StartInfo = procStart;
            proc.Start(); //Le desimos que empize
            proc.WaitForExit(); //le desimos que finlaize
            
        } 

        public void abrirgrafo()
        {
            if (File.Exists(ruta)) //verifica si el archivo existe
            {
                try
                {
                    System.Diagnostics.Process.Start(ruta+"\\Automatas");
                }catch(Exception ex)
                {
                    Console.WriteLine("ERROR " + ex);
                }
            }
            else
            {
                Console.WriteLine("ERROR iniexistente");
            }
        }

        //-----------MOSTRAR ARREGLO---------------------------
        private static void RecorrerArreglo<T>(T[,] matriz)
        {
            var filas = matriz.GetLength(0);
            var columnas = matriz.GetLength(1);
            var sb = new StringBuilder();
            var tmpFila = new T[matriz.GetLength(0)];
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    tmpFila[j] = matriz[i, j];
                }

                sb.AppendLine(string.Join("\t", tmpFila));
                //sb.AppendLine("");
            }

            Console.WriteLine(sb.ToString());
        }

        private void Limpiador()
        {
            var filas = tablaAFD.GetLength(0);
            var columnas = tablaAFD.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (tablaAFD[i, j] != null)
                    {
                        tablaAFD[i, j] = "";
                        tablaAceptacion[j] = "";
                    }
                    
                }
            
            }
            
        }
    }
}
