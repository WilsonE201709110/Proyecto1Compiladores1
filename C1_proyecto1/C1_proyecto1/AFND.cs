using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C1_proyecto1
{

    class AFND
    {
        //variable string
        private String ExpresionRegular;
        private String ExpresionR = ""; //EXpresionregularApolacaSimplificada
        private String ExpresionAux = "";
        private String ExpresionNueva = "";
        private String valorFinal = "";
        private String valorInicial = "";

        private string[,] TablaAFND= new string[100,4];
        private static string[,] TabladeS = new string[20, 2];

        //
        private int y = 1;
        private int s = 0;
        private int r = 0; //para tabla de S
        int numeroDeCadenayConjunto = 97;

        public AFND(String ExpresionRegular)
        {
            this.ExpresionRegular = ExpresionRegular;
        }

        public void setEXpresionNueva()
        {

        }

        private void RellenarMatriz()
        {
            var filas = TablaAFND.GetLength(0);
            var columnas = TablaAFND.GetLength(1);
            for (int i=0; i < filas; i++)
            {
                for(int j=0; j < columnas; j++)
                {
                    TablaAFND[i, j] = "";
                    
                }

            }
        }

        public void convertidorPolacaSimplificada()
        {
            int estadoInicio = 0; //Iniciamos en el estado 0
            int estadoPrincipal = 0;  //Estado en elque nos encontramos 
            
           

            char caracter; //Caracter de concatenacion
            
            for (estadoInicio = 0; estadoInicio < ExpresionRegular.Length; estadoInicio++)
            {
                caracter = ExpresionRegular[estadoInicio];

                switch (estadoPrincipal)
                {
                    case 0:
                        switch (caracter)
                        {
                            case '.':
                                ExpresionR += ".";
                                break;
                            case '+':
                                ExpresionR += "+";
                                break;
                            case '?':
                                ExpresionR += "?";
                                break;
                            case '|':
                                ExpresionR += "|";
                                break;
                            case '*':
                                ExpresionR += "*";
                                break;
                            case '"':
                                estadoPrincipal = 1;
                                break;
                            case '{':
                                estadoPrincipal = 2;
                                break;
                        }
                        break;
                    case 1:
                        if (caracter != '"')
                        {
                            ExpresionAux += caracter;
                        }
                        else
                        {
                            //estadoInicio = estadoInicio - 1;
                            GuardarCadena(Convert.ToChar(numeroDeCadenayConjunto).ToString(), ExpresionAux); //Gaurdar en tabla
                            ExpresionAux = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                        }
                        break;
                    case 2:
                        if (caracter != '}')
                        {
                            ExpresionAux += caracter;
                        }
                        else
                        {
                            //estadoInicio = estadoInicio - 1;
                            GuardarCadena(Convert.ToChar(numeroDeCadenayConjunto).ToString(), ExpresionAux); //Guardar en tabla
                            ExpresionAux = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                        }
                        break;

                }
            }
            RellenarMatriz();
            Console.WriteLine(ExpresionR);
            do
            {
                CrearTablaAFND(ExpresionR);
                Console.WriteLine(ExpresionR);
            } while (ExpresionR.Length>2);

            Console.WriteLine("--------------------------");
            RecorrarArreglo<string>(TablaAFND);
            Console.WriteLine("----------ESSS-----");
            RecorrarArreglo<string>(TabladeS);
            Transformador tranformarAFND = new Transformador(TablaAFND);
            tranformarAFND.CreacionDeTablas();

            //Limpiador(); //Limpiamos todo

        }
        private void GuardarCadena(String id,String cadena)
        {
            bool existe = false;
            if (!EstaVacia(TabladeS))
            {
                for (int i=0;i<TabladeS.GetLength(0);i++)
                {
                    if (TabladeS[i, 1] == cadena)
                    {
                        ExpresionR += TabladeS[i,0];
                        existe = true;
                        break;
                    }
                }
                if (!existe)
                {
                    TabladeS[r, 0] = id;
                    TabladeS[r, 1] = cadena;
                    ExpresionR += Convert.ToChar(numeroDeCadenayConjunto).ToString();
                    numeroDeCadenayConjunto++;
                    r++;
                }
            }
        }

        public bool EstaVacia(string[,] matriz)
        {
            foreach (string s in matriz)
            {
                if (string.IsNullOrWhiteSpace(s)) return false;
            }
            return true;
        }

        private void  CrearAFND()
        {

        }

        /////////// CREACION DE TABLA
        private void CrearTablaAFND(String er) //er = EXpresionRegular
        {
            for (int i = 0; i < er.Length; i++)
            {
                
                if (er[i].Equals('.')) //-------------"."--------------------------------------------
                {
                    if (Char.IsLetter(er[i + 1]) && Char.IsLetter(er[i + 2])) //.ab
                    { 
                        Concatenacion_ab(er[i + 1].ToString(), er[i + 2].ToString());
                        i++; i++;
                    }
                    else if (Char.IsLetter(er[i + 1]) && er[i + 2].Equals('$')) //.a$
                    {
                        Concatenacion_aS(er[i + 1].ToString(), er[i + 3].ToString());
                        i++; i++; i++;
                    }
                    else if (er[i + 1].Equals('$') && Char.IsLetter(er[i + 3])) //.$a
                    {
                        Concatenacion_Sa(er[i + 3].ToString(),er[i+2].ToString());
                        i += 3;
                    }
                    else if (er[i+1].Equals('$') && er[i+3].Equals('$')) //.$1$2
                    {
                        Concatenacion_SS(er[i+2].ToString(), er[i+4].ToString());
                        i += 4;
                    }
                    else
                    {
                        ExpresionNueva += er[i];
                    }
                }
                else if (er[i].Equals('|')) //-------------"|"------------------------------------------
                {
                    if (Char.IsLetter(er[i + 1]) && Char.IsLetter(er[i + 2])) //|ab
                    {
                        Disyuncion_ab(er[i + 1].ToString(), er[i + 2].ToString());
                        i += 2;
                    }
                    else if (Char.IsLetter(er[i + 1]) && er[i + 2].Equals('$'))//|a$
                    {
                        Disyuncion_aS(er[i + 1].ToString(), er[i + 3].ToString());
                        i += 3;
                    }
                    else if (er[i + 1].Equals('$') && Char.IsLetter(er[i + 3])) //|$a
                    {
                        Disyuncion_Sa(er[i+3].ToString(), er[i+2].ToString());
                        i += 3;
                    }
                    else if (er[i + 1].Equals('$') && er[i + 3].Equals('$')) //|$1$2
                    {
                        Disyuncion_SS(er[i+2].ToString(), er[i+4].ToString());
                        i += 4;
                    }
                    else
                    {
                        ExpresionNueva += er[i];
                    }
                }
                else if (er[i].Equals('*')) //-------------"*"---------------------------------------
                {
                    if (Char.IsLetter(er[i + 1]))//*a
                    {
                        ceroOmasvecez_a(er[i + 1].ToString());
                        i+=1;
                    }
                    else if (er[i + 1].Equals('$')) //*$
                    {
                        ceroOmasvecez_S(er[i + 2].ToString());
                        i+=2;
                    }
                    else
                    {
                        ExpresionNueva += er[i];
                    }


                }
                else if (er[i].Equals('?')) //-------------"?"-----------------------------------------
                {
                    if (Char.IsLetter(er[i + 1])) //?a
                    {
                        ceroOunavez_a(er[i + 1].ToString());
                        i++;
                    }
                    else if (er[i + 1].Equals('$'))  //?$
                    {
                        ceroOunavez_S(er[i + 2].ToString());
                        i += 2;
                    }
                    else
                    {
                        ExpresionNueva += er[i];
                    }
                }
                else if (er[i].Equals('+'))     //-------------"+"------------------------------------
                {
                    if (Char.IsLetter(er[i + 1])) //+a
                    {
                        unaOmasvecez_a(er[i + 1].ToString());
                        i += 1;
                    }
                    else if (er[i + 1].Equals('$'))//+$
                    {
                        unaOmasvecez_S(er[i + 2].ToString());
                        i += 2;
                    }
                    else
                    {
                        ExpresionNueva += er[i];
                    }
                }
                else
                {
                    ExpresionNueva += er[i];
                }

                
            }

            ExpresionR = ExpresionNueva;
            ExpresionNueva = "";
        }

        /*--------------------------------------------------------------------------------------------------
     * ------------------------CONCATENACION .  --------------------------------------------------------------------
     * -----------------------------------------------------------------------------------------------*/
        private void Concatenacion_ab(String valorA,String valorB)  // .ab, valorA = a, valorB =b
        {
            s++; //s+1;
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = valorA;
            TablaAFND[y, 2] = (y+1).ToString();
            TablaAFND[y, 3] = "$"+s.ToString(); y++; //$(s+1)
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = valorB;
            TablaAFND[y, 2] = (y+1).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ"+s.ToString(); //ƒ(s+1)
            y++;
            ExpresionNueva += "$"+s.ToString(); //$(s+1)
        }

        private void Concatenacion_aS(String valor,String n)  // .a$,  valor = a n = $1
        {
            String nodoAux = "";
            //valorInicial = n;
            for (int t=0; t < TablaAFND.Length; t++) 
            {
                Console.WriteLine(TablaAFND[t,3].ToString());
                if (TablaAFND[t, 3].ToString()==("$"+n).ToString())
                {
                    nodoAux = TablaAFND[t, 0].ToString();
                    TablaAFND[t, 3] = "-";
                    break;
                }
            }
            TablaAFND[y, 0] = y.ToString(); //[ny][a][nAux][$1]
            TablaAFND[y, 1] = valor;
            TablaAFND[y, 2] = nodoAux;
            TablaAFND[y, 3] = "$"+n;
            y++; //Nueva Fila
            ExpresionNueva += "$" + n;
        }

        private void Concatenacion_Sa(String valor,String n)  // .$a   valor = a
        {
            for (int t = 0; t < TablaAFND.Length; t++)
            {
                if (TablaAFND[t, 3].ToString().Equals("ƒ"+n))
                {
                    TablaAFND[t, 1] = valor;
                    TablaAFND[t, 2] = y.ToString();
                    TablaAFND[t, 3] = "-";
                    break;
                }
            }

            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ"+n;
            y++;
            ExpresionNueva += "$" + n;
        }
        
        private void Concatenacion_SS(String n1,String n2)  // .$1$2
        {
            
            String buscar = "$" + n2.ToString();

            String aux1 = "", aux2 = "";

            var filas = TablaAFND.GetLength(0);

            for (int i = 0; i < filas; i++) //buscar primero el inicio de 2
            {
                if (TablaAFND[i,3].ToString()==buscar)
                {
                    aux1 = TablaAFND[i, 1].ToString();
                    aux2 = TablaAFND[i, 2].ToString();
                    TablaAFND[i, 0] = "-";
                    TablaAFND[i, 1] = "-";
                    TablaAFND[i, 2] = "-";
                    TablaAFND[i, 3] = "-";
                    break;
                }
            }

            buscar = "ƒ" + n1.ToString();
            for (int j = 0; j < filas; j++) //buscar final de inicio de 1
            {
                if (TablaAFND[j, 3].ToString() == buscar)
                {
                    TablaAFND[j, 1] = aux1;
                    TablaAFND[j, 2] = aux2;
                    TablaAFND[j, 3] = "-";
                    break;
                }
            }

            buscar = "ƒ" + n2.ToString();
            for (int k = 0; k < filas; k++) //buscar el final del segundo para colocarlo como nuevo
            {
                if (TablaAFND[k, 3].ToString() == buscar) TablaAFND[k,3]= "ƒ" + n1.ToString();
            }


            ExpresionNueva += "$"+n1;
        }

        
    /*--------------------------------------------------------------------------------------------------
     * ------------------------DINYUNCION | --------------------------------------------------------------------
     * -----------------------------------------------------------------------------------------------*/
        private void Disyuncion_ab(String valor_a, String valor_b)  // |ab
        {
            s++;
            TablaAFND[y, 0] = y.ToString();//[n1][£][n2,n4][P]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString() + "," + (y + 3).ToString();
            TablaAFND[y, 3] = "$" + s.ToString(); y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n2][a][n3][-]
            TablaAFND[y, 1] = valor_a;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n3][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 3).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n4][b][n5][-]
            TablaAFND[y, 1] = valor_b;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n5][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n6][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + s.ToString();
            y++; //pasar ala siguente fila

            ExpresionNueva += "$" + s.ToString(); //agragar a nueva expresion 
        }
        
        private void Disyuncion_aS(String valor_a, String n)  // |a$   
        {
            var fila = TablaAFND.GetLength(0);
            String primero = "", ultimo = ""; //variables de ultimo y primero de ->>$
            
            for (int i = 0; i < fila; i++)
            {
                if (TablaAFND[i, 3].Equals("$" + n))
                {
                    primero = TablaAFND[i, 0];
                    TablaAFND[i, 3] = "-";
                }
                if (TablaAFND[i, 3].Equals("ƒ" + n))
                {
                    TablaAFND[i, 1] = "£";
                    TablaAFND[i, 2] = (y + 3).ToString();  //-> ((nK))
                    TablaAFND[i, 3] = "-";
                }
            }
            TablaAFND[y, 0] = y.ToString();//[n1][£][n2,n4][P]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString()+ "," + primero;
            TablaAFND[y, 3] = "$" + n; y++; //pasar ala siguente fila
            
            TablaAFND[y, 0] = y.ToString(); //[n4][a][n5][-]
            TablaAFND[y, 1] = valor_a;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n5][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n6][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + n;
            y++; //pasar ala siguente fila

            ExpresionNueva += "$" + n;
        }
        
        private void Disyuncion_Sa(String valor_a, String n)  // |$a 
        {
            var fila = TablaAFND.GetLength(0);
            String primero = "", ultimo = ""; //variables de ultimo y primero de ->>$

            for (int i = 0; i < fila; i++)
            {
                if (TablaAFND[i, 3].Equals("$" + n))
                {
                    primero = TablaAFND[i, 0];
                    TablaAFND[i, 3] = "-";
                }
                if (TablaAFND[i, 3].Equals("ƒ" + n))
                {
                    TablaAFND[i, 1] = "£";
                    TablaAFND[i, 2] = (y + 4).ToString();  //-> ((nK))
                    TablaAFND[i, 3] = "-";
                }
            }

            //y++;
            TablaAFND[y, 0] = y.ToString();//[n1][£][n2,n4][P]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = primero + "," + (y + 1).ToString();
            TablaAFND[y, 3] = "$" + n; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n2][a][n3][-]
            TablaAFND[y, 1] = valor_a;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n3][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila

            TablaAFND[y, 0] = y.ToString(); //[n6][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + n;


            y++;
            ExpresionNueva += "$" + n;
        }
        
        private void Disyuncion_SS(String n1, String n2)  // |$1$2
        {
            String buscar = "$" + n1.ToString();

            String aux1 = "", aux2=""; //variables para guardar los valores primeros de $1y$2

            var filas = TablaAFND.GetLength(0);

            for (int i = 0; i < filas; i++) //buscar primero el inicio de 1
            {
                if (TablaAFND[i, 3].ToString() == buscar)
                {
                    aux1 = TablaAFND[i, 0].ToString();
                    TablaAFND[i, 3] = "-";
                   
                }
                if (TablaAFND[i, 3] == ("ƒ" + n1))
                {
                    TablaAFND[i, 1] = "£";
                    TablaAFND[i, 2] = (y+1).ToString();
                    TablaAFND[i, 3] = "-";
                }
            }
            
            buscar = "$" + n2.ToString();
            for (int k = 0; k < filas; k++) //buscar inicio para $2
            {
                if (TablaAFND[k, 3].ToString() == buscar)
                {
                    aux2= TablaAFND[k, 0].ToString();
                    TablaAFND[k, 3] = "-";

                }
                if (TablaAFND[k, 3] == ("ƒ" + n2))
                {
                    TablaAFND[k, 1] = "£";
                    TablaAFND[k, 2] = (y + 1).ToString();
                    TablaAFND[k, 3] = "-";
                }
            }

            //Guardamos los valores en la tabla
            //y++;
            TablaAFND[y, 0] = y.ToString();//[n1][£][n2,n4][P] -> nodo inicial
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = aux1 + "," + aux2;
            TablaAFND[y, 3] = "$" + n1; y++; //pasar ala siguente fila

            TablaAFND[y, 0] = y.ToString(); //[n6][ƒ][ƒ][ƒ] -> nodo final
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + n1;
            y++;

            ExpresionNueva += "$" + n1;
        }
        
        private void ceroOunavez_a(String valor_a)  //?a   //valor_a = a;
        {
            s++;
            TablaAFND[y, 0] = y.ToString();//[n1][£][n2,n4][P]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y+1).ToString()+","+(y+3).ToString();
            TablaAFND[y, 3]= "$"+s.ToString(); y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n2][a][n3][-]
            TablaAFND[y, 1] =  valor_a;
            TablaAFND[y, 2] = (y+1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n3][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y+3).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n4][£][n5][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y+1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n5][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y+1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n6][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] ="ƒ";
            TablaAFND[y, 3] ="ƒ"+s.ToString();
            y++; //pasar ala siguente fila

            ExpresionNueva += "$" + s.ToString(); //agragar a nueva expresion 
        }

        private void ceroOunavez_S(String n)  // ?$ 
        {
            var fila = TablaAFND.GetLength(0);
            String primero = "", ultimo = "";
            for (int i=0; i<fila;i++)
            {
                if (TablaAFND[i,3].Equals("$"+n))
                {
                    primero = TablaAFND[i, 0];
                    TablaAFND[i, 3] = "-";
                }
                if (TablaAFND[i, 3].Equals("ƒ" + n))
                {
                    TablaAFND[i, 1] =  "£";
                    TablaAFND[i, 2] = (y+3).ToString();  //-> ((nK))
                    TablaAFND[i, 3] = "-";
                }
            }
            //y++;
            TablaAFND[y, 0] = y.ToString();//[n1][£][n2,n4][P]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = primero + "," + (y + 1).ToString();
            TablaAFND[y, 3] = "$" + n; y++; //pasar ala siguente fila
            
            TablaAFND[y, 0] = y.ToString(); //[n4][£][n5][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n5][£][n6][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++; //pasar ala siguente fila
            TablaAFND[y, 0] = y.ToString(); //[n6][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + n;
            y++;
            ExpresionNueva += "$" + n;

        }

        private void ceroOmasvecez_a(String valor)  // *a , valor = a
        {
            int yTemp = 0, yTemp2 = 0; s++; 
            TablaAFND[y, 0] = y.ToString(); //[n1][£][n2,n4][P]
            TablaAFND[y, 1] = "£"; yTemp = y+1; yTemp2 = 3 + y;
            TablaAFND[y, 2] = yTemp.ToString()+","+yTemp2.ToString();
            TablaAFND[y, 3] = "$"+s.ToString(); y++;
            TablaAFND[y, 0] = y.ToString(); //[n2][a][n3][-]
            TablaAFND[y, 1] = valor; 
            TablaAFND[y, 2] = (y+1).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n3][£][n2,n4][-]
            TablaAFND[y, 1] = "£";  yTemp = y+1; yTemp2 = y-1;
            TablaAFND[y, 2] = yTemp2.ToString() + "," + yTemp.ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n4][A][A][A]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ"+s.ToString();

            ExpresionNueva += "$" + s.ToString(); //$(s+1)
            y++; //aunmetar y 
        }

        private void ceroOmasvecez_S(String n)  // *$
        {
            String nodoAux = ""; int AuxdeY = 0;
            var fila = TablaAFND.GetLength(0);
            for (int t = 0; t < fila; t++)
            {
                if (TablaAFND[t, 3].ToString().Equals("$"+n))
                {
                    
                    nodoAux=TablaAFND[t, 0].ToString();
                    TablaAFND[t, 3] = "-";
                    break;
                }
            }
             AuxdeY = y; //y++;
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = nodoAux+","+(y+1).ToString();
            TablaAFND[y, 3] = "$"+n;
            y++;
            for (int t = 0; t < fila; t++)
            {
                if (TablaAFND[t, 3].ToString().Equals("ƒ"+n))
                {
                    TablaAFND[t, 1] = "£"; //y++; 
                    TablaAFND[t, 2] = nodoAux+","+y.ToString();
                    TablaAFND[t, 3] = "-";
                    break;
                }
            }
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ"+n;
            y++;
            //TablaAFND[AuxdeY, 2] = nodoAux + "," + y.ToString(); //En el primero
            ExpresionNueva += "$" + n; //$(s+1)
            //y++;
        }

        private void unaOmasvecez_a(String valor)  // +a
        {
            s++;
            TablaAFND[y, 0] = y.ToString(); //[n1][£][n2][P]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "$" + s.ToString(); y++;
            TablaAFND[y, 0] = y.ToString();//[n2][a][n3][-]
            TablaAFND[y, 1] = valor;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n3][£][n2,n4][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y -1).ToString()+","+(y+1).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n4][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + s.ToString();
            y++;
            ExpresionNueva += "$" + s.ToString();
            /*
            s++;
            TablaAFND[y, 0] = y.ToString(); //[n1][a][n2][P]
            TablaAFND[y, 1] = valor;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "$" + s.ToString(); y++;
            TablaAFND[y, 0] = y.ToString();//[n2][£][n3,n5][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y + 1).ToString()+","+(y+3).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n3][a][n4][-]
            TablaAFND[y, 1] = valor;
            TablaAFND[y, 2] = (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n4][£][n3,n5][-]
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = (y -1).ToString() + "," + (y + 1).ToString();
            TablaAFND[y, 3] = "-"; y++;
            TablaAFND[y, 0] = y.ToString(); //[n5][ƒ][ƒ][ƒ]
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ"+s.ToString();
            y++;
            ExpresionNueva += "$" + s.ToString();*/

        }

        private void unaOmasvecez_S(String n) //+$ ,    
        {
            String nodoAux = ""; int AuxdeY = 0;
            var fila = TablaAFND.GetLength(0);
            for (int t = 0; t < fila; t++)
            {
                if (TablaAFND[t, 3].ToString().Equals("$" + n))
                {

                    nodoAux = TablaAFND[t, 0].ToString();
                    TablaAFND[t, 3] = "-";
                    break;
                }
            }
            AuxdeY = y; //y++;
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = "£";
            TablaAFND[y, 2] = nodoAux;
            TablaAFND[y, 3] = "$" + n;
            y++;
            for (int t = 0; t < fila; t++)
            {
                if (TablaAFND[t, 3].ToString().Equals("ƒ" + n))
                {
                    TablaAFND[t, 1] = "£"; //y++; 
                    TablaAFND[t, 2] = nodoAux + "," + y.ToString();
                    TablaAFND[t, 3] = "-";
                    break;
                }
            }
            TablaAFND[y, 0] = y.ToString();
            TablaAFND[y, 1] = "ƒ";
            TablaAFND[y, 2] = "ƒ";
            TablaAFND[y, 3] = "ƒ" + n;
            y++;
            //TablaAFND[AuxdeY, 2] = nodoAux + "," + y.ToString(); //En el primero
            ExpresionNueva += "$" + n; //$(s+1)
            //y++;


        }

        private void insertarEnTablaMatrizAuxiliar(String[,] tablaAux, String n)
        {
            var fila = tablaAux.GetLength(0);
            for (int i = 0; i < fila; i++)
            {
                String[] m = tablaAux[i, 2].Split(',');

                TablaAFND[y, 0] = int.Parse(y+tablaAux[i, 0]).ToString();
                TablaAFND[y, 1] = tablaAux[i, 1];


                int numAux = int.Parse(tablaAux[i, 0].ToString());
                if (m.Length > 1)
                {
                    int numero1 = int.Parse(m[0].Trim().ToString()) - numAux;
                    int numero2 = int.Parse(m[1].Trim().ToString()) - numAux;
                    TablaAFND[y, 2] = (y + numero1).ToString() + "," + (y + numero2).ToString();
                }
                else
                {
                    int numero1 = int.Parse(m[0].Trim().ToString()) - numAux;
                    TablaAFND[y, 2] = (y + numero1).ToString();
                }

                //TablaAFND[y, 2] = int.Parse(y+tablaAux[i, 2]).ToString();
                TablaAFND[y, 3] = tablaAux[i, 3];
            }

            s++;
            ExpresionNueva += "$" + s.ToString(); //$(s+1)

            ceroOmasvecez_S(n);//funcion *$
        }

        private static void RecorrarArreglo<T>(T[,] matriz)
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

        private void Limpiador() //metodo para limpiar 
        {
            var filas = TablaAFND.GetLength(0);
            var columnas = TablaAFND.GetLength(1);
            var fila2 = TabladeS.GetLength(0);
            var columnas2 = TabladeS.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    TablaAFND[i, j] = "";
                }
            }
            for (int i = 0; i < fila2; i++)
            {
                for (int j = 0; j < columnas2; j++)
                {
                    TabladeS[i, j] = "";
                }
            }

           ExpresionRegular="";
            ExpresionR = ""; //EXpresionregularApolacaSimplificada
            ExpresionAux = "";
            ExpresionNueva = "";
            valorFinal = "";
            valorInicial = "";
            //
            y = 1;
            s = 0;
            r = 0; //para tabla de S
            numeroDeCadenayConjunto = 97;
        }

        public static string[,] getTablaDeS()
        {
            return TabladeS;
        }

        public static void setTablaDeS_Resetear() //borrar todo despues
        {
            var filas = TabladeS.GetLength(0);
            var columnas = TabladeS.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    TabladeS[i, j] = null;
                }
            }
        }
    }
}
