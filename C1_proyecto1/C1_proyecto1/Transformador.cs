using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C1_proyecto1
{
    class Transformador
    {
        private string[,] TablaAFND = new string[100, 4];
        private string[,] TablaCerraduras = new string[100, 3];
        private string[,] TablaMovimientos = new string[100, 3];

        private static string[] TablaNodosAceptacion = new string[10]; 

        private int f = 0; //fila de la tabla cerradura
        private int k = 0, h=0; //fila de la tabla movimientos
        private int r = 0; //recorredor de la tablamovimientos
        private int m = 0; //indice de la tabla nodos de aceptacion

        private int numeroDeNodo = 65; //A-B-C----

        
        private bool estadoAceptacion = false; //verifica si el nodo es de aceptacion

        private String conjuntoApuntadores = "";

        public Transformador(String[,] TablaAFND)  //CONSTRUCTOR
        {
            this.TablaAFND = TablaAFND;
        }

        public void CreacionDeTablas() {

            var filas = TablaAFND.GetLength(0);

            String nodoPrimero = "";
            String nodoTransicion = "";
            String nodoApuntador = "";

            for (int i = 0; i < filas; i++)  //Ciclo para encontrar el primero
            {
                if (TablaAFND[i, 3] == "$1")
                {
                    nodoPrimero = TablaAFND[i, 0];
                    nodoTransicion = TablaAFND[i, 1];
                    nodoApuntador = TablaAFND[i, 2];

                    Cerraduras(nodoPrimero, nodoTransicion, nodoApuntador); //llamar metodo cerradura por primera vez

                    break;
                }
            }

            

            bool final = false;
            bool primero = false;
            int c = 0;
            
            conjuntoApuntadores += nodoPrimero;

            string[] matrizApuntadores = conjuntoApuntadores.Split(','); //trasladar la variable conjunto de apuntadores a una matriz de apuntadores
            conjuntoApuntadores = ordenamientoInsercion(matrizApuntadores); //ordenar los apuntadores y guardarlos en la variable conJUNTO DE APUNTADORES YA ORDENADOS
            //conjuntoApuntadores += "null"; //asignamos un null

            TablaCerraduras[f, 0] = nodoPrimero;
            TablaCerraduras[f, 1] = Convert.ToChar(numeroDeNodo).ToString(); //asignamos el primer nodo A, y asi sucesivamente
            TablaCerraduras[f, 2] = conjuntoApuntadores; //como no transiciones con £ solo le colocamos 0
            if (estadoAceptacion) //si el en nodo es de aceptacion lo guado en mi arreglo 
            {
                TablaNodosAceptacion[m] = Convert.ToChar(numeroDeNodo).ToString(); estadoAceptacion = false; m++;
            }
            numeroDeNodo++; //aumentamos esta variable para el sigiente nodo
            
            conjuntoApuntadores = ""; //vaciamos la variable 
            
            Movimiento(TablaCerraduras[f, 1], TablaCerraduras[f, 2]); //LLAMAMOS AL METODO MOVIMIENTO PARA VEROFICAR LOS MOVIMIENTOS
            f++; //pasa ala siguente fila

            for (int t=0;t<TablaMovimientos.GetLength(0);t++)
            {
                if (TablaMovimientos[t, 2] != null)
                {
                    for (int i = 0; i < TablaCerraduras.GetLength(0); i++)
                    {
                        if (TablaMovimientos[t, 2] != null)
                        {
                            if (TablaCerraduras[i, 0]==TablaMovimientos[t, 2])
                            {
                                Console.WriteLine(TablaMovimientos[t, 2]);
                                primero = true;
                                break;
                            }
                        }
                        else
                        {
                            primero = true;
                        }
                    }

                }
                else
                {
                    primero = true;
                }

                if (!primero)
                {
                    for (int i = 0; i < filas; i++)  //Ciclo para encontrar el primero
                    {
                       // for (int j = r; j < TablaMovimientos.GetLength(0) - 1; j++)
                       // {
                            if (TablaAFND[i, 0] == TablaMovimientos[t, 2]) //verificar si [A] = tablamovimetnot [A]
                            {
                                nodoPrimero = TablaAFND[i, 0];
                                nodoTransicion = TablaAFND[i, 1];
                                nodoApuntador = TablaAFND[i, 2];
                                r++; //aumentamos para la siguente posicion
                                Cerraduras(nodoPrimero, nodoTransicion, nodoApuntador); //llamar metodo cerradura

                                conjuntoApuntadores += nodoPrimero;

                                matrizApuntadores = conjuntoApuntadores.Split(','); //trasladar la variable conjunto de apuntadores a una matriz de apuntadores
                                conjuntoApuntadores = ordenamientoInsercion(matrizApuntadores); //ordenar los apuntadores y guardarlos en la variable conJUNTO DE APUNTADORES YA ORDENADOS
                                                                                                //conjuntoApuntadores += "null"; //asignamos un null
                                /*bool tablayConjunto = false;
                                for (int l = 0; l < TablaCerraduras.GetLength(0); l++)
                                {
                                    if (TablaCerraduras[l, 0] == conjuntoApuntadores) tablayConjunto = true;
                                }
                                if (!tablayConjunto)
                                {*/
                                    TablaCerraduras[f, 0] = nodoPrimero;
                                    TablaCerraduras[f, 1] = Convert.ToChar(numeroDeNodo).ToString(); //asignamos el primer nodo A, y asi sucesivamente
                                    TablaCerraduras[f, 2] = conjuntoApuntadores; //como no transiciones con £ solo le colocamos 0

                                    if (estadoAceptacion) //si el en nodo es de aceptacion lo guado en mi arreglo 
                                    {
                                        TablaNodosAceptacion[m] = Convert.ToChar(numeroDeNodo).ToString(); estadoAceptacion = false; m++;
                                    }
                                    numeroDeNodo++; //aumentamos esta variable para el sigiente nodo
                                    
                                    conjuntoApuntadores = ""; //vaciamos la variable

                                    Movimiento(TablaCerraduras[f , 1], TablaCerraduras[f , 2]); //LLAMAMOS AL METODO MOVIMIENTO PARA VEROFICAR LOS MOVIMIENTOS
                                    f++; //pasa ala siguente fila
                                    break;
                                //}
                               // tablayConjunto = false;

                            }
                       // }

                    }
                }

                primero = false;
            }


           /* do
            {

                for (int i = 0; i < TablaCerraduras.GetLength(0); i++)
                {
                        if (TablaMovimientos[h, 2] != null)
                        {
                            if (TablaCerraduras[i, 0] == TablaMovimientos[h, 2])
                            {
                            Console.WriteLine(TablaMovimientos[h, 2]);
                                primero = true;
                                h++;
                                break;
                            }
                        }
                        else
                        {
                            primero = true;
                        }
                }

                if (!primero)
                {
                    for (int i = 0; i < filas; i++)  //Ciclo para encontrar el primero
                    {
                        for (int j=r;j<TablaMovimientos.GetLength(0)-1;j++)
                        {
                            if (TablaAFND[i, 0] == TablaMovimientos[j, 2]) //verificar si [A] = tablamovimetnot [A]
                            {
                                nodoPrimero = TablaAFND[i, 0];
                                nodoTransicion = TablaAFND[i, 1];
                                nodoApuntador = TablaAFND[i, 2];
                                r++; //aumentamos para la siguente posicion
                                Cerraduras(nodoPrimero, nodoTransicion, nodoApuntador); //llamar metodo cerradura
                                
                                conjuntoApuntadores += nodoPrimero;

                                matrizApuntadores = conjuntoApuntadores.Split(','); //trasladar la variable conjunto de apuntadores a una matriz de apuntadores
                                conjuntoApuntadores = ordenamientoInsercion(matrizApuntadores); //ordenar los apuntadores y guardarlos en la variable conJUNTO DE APUNTADORES YA ORDENADOS
                                                                                                //conjuntoApuntadores += "null"; //asignamos un null
                                bool tablayConjunto = false;
                                for (int l = 0; l < TablaCerraduras.GetLength(0); l++)
                                {
                                    if (TablaCerraduras[l, 2] == conjuntoApuntadores) tablayConjunto=true;
                                }
                                    if(!tablayConjunto)
                                    {
                                        TablaCerraduras[f, 0] = nodoPrimero;
                                        TablaCerraduras[f, 1] = Convert.ToChar(numeroDeNodo).ToString(); //asignamos el primer nodo A, y asi sucesivamente
                                        TablaCerraduras[f, 2] = conjuntoApuntadores; //como no transiciones con £ solo le colocamos 0
                                    
                                         if (estadoAceptacion) //si el en nodo es de aceptacion lo guado en mi arreglo 
                                        {
                                            TablaNodosAceptacion[m] = Convert.ToChar(numeroDeNodo).ToString(); estadoAceptacion = false; m++;
                                        }
                                        numeroDeNodo++; //aumentamos esta variable para el sigiente nodo
                                        f++; //pasa ala siguente fila
                                        conjuntoApuntadores = ""; //vaciamos la variable
                                    
                                        Movimiento(TablaCerraduras[f - 1, 1], TablaCerraduras[f - 1, 2]); //LLAMAMOS AL METODO MOVIMIENTO PARA VEROFICAR LOS MOVIMIENTOS
                                        break;
                                    }
                                tablayConjunto = false;
                                
                            }
                        }

                    }
                }
               

                primero = false;
                if (c>k&&c>f)final=true;
                c++;

            } while (!final);*/

            Console.WriteLine("-______________-____________________________-________________");
            RecorrerArreglo<string>(TablaCerraduras);
            Console.WriteLine("-______________-____________________________-________________");
            RecorrerArreglo<string>(TablaMovimientos);
            foreach (string s in TablaNodosAceptacion)
            {
                Console.WriteLine(s+",");
            }

            AFD automata = new AFD(TablaCerraduras,TablaMovimientos);
            automata.CreacionTablaAFD();
            //automata.Limpiador();
            ///Limpiador(); //Limpiar todo
        }


        /*---------------Ordenar los apuntadores-----------------------------
        --------------------------------------------------------------------
        --------------------------------------------------------------------*/
        private string ordenamientoInsercion(string[] matriz)
        {
            int i, pos, aux;
            var elementos = matriz.Length;


            try
            {
                //Algoritmo
                for (i = 0; i < elementos; i++)
                {
                    if (matriz[i].Equals(null))
                    {
                        i = elementos;
                    }
                    else
                    {
                        pos = i;
                        aux = int.Parse(matriz[i]);

                        while ((pos > 0) && (int.Parse(matriz[pos - 1]) > aux))
                        {
                            matriz[pos] = matriz[pos - 1];
                            pos--;
                        }

                        matriz[pos] = aux.ToString();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Problema-> " + e);
            }
            
            string matriz2 = "";
            bool SiEsta = false;

            string[] matrizAux = matriz2.Split(',');
            for (int z = 0; z < matriz.Length; z++)
            {
                for (int w = 0; w < matrizAux.Length; w++)
                {
                    if (matriz[z] == matrizAux[w]) SiEsta=true;
                }
                if (!SiEsta)
                {
                    matriz2 += matriz[z] + ",";
                    matrizAux = matriz2.Split(',');
                }
                SiEsta = false;
            } 
            
            return matriz2;
        } 


        /*______________________________________________________________________________________________________________-
        ______________________________________________________________________________________________________________--
        ----------------------------------VERIFICA SI TABLA ESTA VACIA--------------------------------------------------------
        __________________________________________________________________________________________________________________---
        _______________________________________________________________________________________________________*/
        public bool EstaVacia(string[,] matriz)
        {
            foreach (string s in matriz)
            {
                if (string.IsNullOrWhiteSpace(s)) return false;
            }
            return true;
        }
        //-----------------METODO CERRADURA-----------------------------------------
        private void Cerraduras(String nodo,String Transicion,String Apuntador)
        {
            var filaDeTrablaAFND = TablaAFND.GetLength(0);

            string[] apuntadores = Apuntador.Split(',');
            if (Transicion == "£")         //si la transicion es £
            {
                for (int i = 0; i < apuntadores.Length; i++) //recorre la lista de apuntadores
                {
                    for (int j = 0; j < filaDeTrablaAFND; j++)
                    {
                        if (apuntadores[i].Trim() == TablaAFND[j, 0]) //
                        {
                            if (TablaAFND[j, 1] == "£")
                            {
                                conjuntoApuntadores += TablaAFND[j, 0] + ",";
                                Cerraduras(TablaAFND[j, 0], TablaAFND[j, 1], TablaAFND[j, 2]); //Recursividad
                                break;
                            }
                            else if (TablaAFND[j, 1] == "ƒ")
                            {
                               //Cerraduras(TablaAFND[j, 0], TablaAFND[j, 1], TablaAFND[j, 2]); //Recursividad
                               conjuntoApuntadores += TablaAFND[j, 0] + ",";
                               estadoAceptacion = true;break;
                            }
                            else
                            {
                                conjuntoApuntadores += TablaAFND[j, 0] + ",";break;
                            }
                        }
                    }

                }
            }
            else if (Transicion == "ƒ")
            {
                
                conjuntoApuntadores += nodo + ",";
                estadoAceptacion = true;
            }
            else
            {
                conjuntoApuntadores += nodo+",";
            }
            

        }

        //-----------------------METODO MOVIMIENTO--------------------------------------
        private void Movimiento(String nodo, String apuntador) //PArametros de tablacerradura[][]
        {
            
            string[] arreglodeApuntadores = apuntador.Split(',');

            var filas = TablaAFND.GetLength(0);

            for (int i = 0; i < arreglodeApuntadores.Length-1;i++) //rreCORRER LOS ELEMENTOS DEL APUNTADOR
            {
                if (arreglodeApuntadores[i]!=null)
                {
                    for (int j = 0; j < filas; j++) //RECORRER LA TABLAAFND
                    {
                        if (arreglodeApuntadores[i] == TablaAFND[j, 0]) //CONDICION SI ENCIENTRA EL APUNTADOR EN LA TABLAAFND
                        {
                            if (TablaAFND[j, 1] != "£" && TablaAFND[j, 1] != "ƒ" && TablaAFND[j, 1] != "\0") //VERIFICA QUE NO TENGA EPSILON y final
                            {
                                TablaMovimientos[k, 0] = nodo;
                                TablaMovimientos[k, 1] = TablaAFND[j, 1];
                                TablaMovimientos[k, 2] = TablaAFND[j, 2];
                                k++;
                                break;
                            }
                        }
                    }
                }
                
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

        //----GEt----

        public static string[] getTablaAceptaciones()
        {
            return TablaNodosAceptacion;
        }

        public static void setTablaAceptacionBorrar()
        {
            for (int i=0;i<TablaNodosAceptacion.Length;i++)
            {
                TablaNodosAceptacion[i] = null;
            }
        }

        private void Limpiador() //metodo para limpiar 
        {
            var filas = TablaAFND.GetLength(0);
            var columnas = TablaAFND.GetLength(1);
            var columnas2= TablaCerraduras.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    TablaAFND[i, j] = "";
                }
                for (int j = 0; j < columnas2; j++)
                {
                    TablaCerraduras[i, j] = "";
                    TablaMovimientos[i, j] = "";
                }
            }
            for (int i = 0; i < TablaNodosAceptacion.Length; i++)
            {
                TablaNodosAceptacion[i] = "";
            }

            f = 0;
            k = 0; h = 0; //fila de la tabla movimientos
            r = 0; //recorredor de la tablamovimientos
            m = 0; //indice de la tabla nodos de aceptacion
            numeroDeNodo = 65; //A-B-C----
            estadoAceptacion = false; //verifica si el nodo es de aceptacion
            conjuntoApuntadores = "";

        }

    }
}
