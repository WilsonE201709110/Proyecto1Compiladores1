using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C1_proyecto1
{
    class AFD
    {
        private static string[,] TablaAFD = new string[100, 10];
        private string[,] TablaCerraduras = new string[100,3];
        private string[,] TablaMovimientos = new string[100, 3];

        public AFD(string[,] TablaCerraduras, string[,] TablaMovimientos) //CONSTRUCTOR
        {
            this.TablaCerraduras = TablaCerraduras;
            this.TablaMovimientos = TablaMovimientos;
        }

        public void CreacionTablaAFD()
        {
            TablaAFD[0, 0] = "AFD";

            for (int i = 1; i < TablaCerraduras.GetLength(0); i++)  //For para colocar nodos
            {
                if (TablaCerraduras[i-1,1]!=null)
                {
                    TablaAFD[i, 0] = TablaCerraduras[i - 1, 1];
                }
            }

            int k = 1;
            for (int i=0;i<TablaMovimientos.GetLength(0);i++) //for para colocar las transiciones
            {
                if (TablaMovimientos[i, 1] != null)
                {
                    string valorA = "";
                    for (int h = 0; h < AFND.getTablaDeS().GetLength(0); h++)
                    {
                        if (AFND.getTablaDeS()[h, 0] != null && AFND.getTablaDeS()[h, 0] == TablaMovimientos[i, 1])
                        {
                            valorA = AFND.getTablaDeS()[h,1];
                        }

                    }
                    bool existe = false;
                    for (int j=0;j<TablaAFD.GetLength(1);j++)
                    {
                        if (TablaAFD[0, j] == valorA)existe = true; //TablaMovimientos[i, 1]

                    }
                    if (!existe) { TablaAFD[0, k] = valorA; k++; }
                   // break;

                }
            }

            for (int i=0;i<TablaMovimientos.GetLength(0);i++) //for buscar nodos y sus transiciones , 
            {
                if (TablaMovimientos[i, 0] != null)
                {
                    string nodoNombre = "";

                    string nodo = TablaMovimientos[i, 0];
                    string transicion = TablaMovimientos[i, 1];
                    string nodoApuntador = TablaMovimientos[i, 2];
                    
                    int y = 0, x = 0; //Variables donde guardare las direcciones y = a, x= b

                    for (int j = 0; j < TablaCerraduras.GetLength(0); j++)  //For pra buscar el nombre del del nodo transicion
                    {
                        if (TablaCerraduras[j, 0] != null)
                        {
                            if (TablaCerraduras[j, 0] == nodoApuntador)
                            {
                                nodoNombre = TablaCerraduras[j, 1];
                                break;
                            }
                        }
                    }

                    for (int a = 1; a < TablaAFD.GetLength(0); a++) //buscar direccion de nodo en TAbla AFD
                    {
                        if (TablaAFD[a, 0].Equals(nodo)) { y = a; break; } 
                    }

                   
                    for (int b = 1; b < TablaAFD.GetLength(1); b++) //buscar direccion de transicion en TAbla AFD
                    {
                        for (int h = 0; h < AFND.getTablaDeS().GetLength(0); h++)
                        {
                            if (AFND.getTablaDeS()[h, 0] != null && AFND.getTablaDeS()[h, 0] == transicion) {
                                transicion = AFND.getTablaDeS()[h, 1];
                            }
                        }

                        if (TablaAFD[0,b]!=null&&TablaAFD[0, b].Equals(transicion)) { x = b; TablaAFD[y, x] = nodoNombre; break; }
                    }
                    
                   
                }
                
            }
            
            RecorrerArreglo<string>(TablaAFD);
           //Limpiador(); //borrar los datos
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

        public static string[,] getTablaAFD()
        {
            return TablaAFD;
        }

        public static void setTablaAFD_Resetear()
        {
            var filas = TablaAFD.GetLength(0);
            var columnas = TablaAFD.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j=0;j<columnas;j++)
                {
                    TablaAFD[i, j] = null;
                }
            }
        }

        public  void Limpiador() //metodo para limpiar 
        {
            var filas = TablaAFD.GetLength(0);
            var columnas = TablaAFD.GetLength(1);
            var columnas2 = TablaCerraduras.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    TablaAFD[i, j] = "";
                }
                for (int j = 0; j < columnas2; j++)
                {
                    TablaCerraduras[i, j] = "";
                    TablaMovimientos[i,j] = "";
                }
            }
        }
    }
}
