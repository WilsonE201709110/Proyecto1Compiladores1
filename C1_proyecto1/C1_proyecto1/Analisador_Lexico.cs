using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace C1_proyecto1
{
    enum Ntoken {Simbolo,Comentario,ComentarioMultiple,Conj,ConjNombre,conjunto,ExpresionRegular,Lexema,NombreEXpresionRegular,NombreLexema,Porcentaje,Desconocido}; //El nombre del token
    class Analisador_Lexico
    {
        //private RichTextBox rtb; //creamos  nuestro reich text

        //variables int
        int fila = 1;
        int Columna = 1;
        int a= 0, b=0,c=0;

        //variables bool
        bool cadenaConjunto = false;
        bool cadenaExpresionRegular = true;
        bool cadenaLexema = true;

        private bool hayErrores = false;

        public static string direccion;

        //Matricez
        private static string[,] conjuntos = new string[15,2];
        private static string[,] exRegulares = new string[15, 2];
        private static string[,] Lexemas = new string[15, 2];

        private string[,] tokens = new string[300, 4]; int x = 0;
        private string[,] errores = new string[100, 3]; int y = 0;

        public Analisador_Lexico() //Constructor
        {
            //this.rtb = rtb;
        }

        

        public void Analizador(String cadena)
        {
            int estadoInicio = 0; //Iniciamos en el estado 0
            int estadoPrincipal = 0;  //Estado en elque nos encontramos 
            char caracter; //Caracter de concatenacion
            String token = ""; //Aqui se almacenara la cadena para convertirla a token

            for (estadoInicio = 0; estadoInicio < cadena.Length; estadoInicio++)
            {
                caracter = cadena[estadoInicio];

                switch (estadoPrincipal)
                {
                    case 0:
                        if (caracter == '\n') { fila++; Columna=1; }
                        if (caracter == 'C')
                        {
                            token += caracter; //Concatena
                            estadoPrincipal = 13;
                            Columna++;
                        }
                        else if (Char.IsLetter(caracter))
                        {
                            token += caracter; //Concatena
                            estadoPrincipal = 6;
                            Console.WriteLine("caracter");
                            Columna++;
                        }
                        else
                        {
                            switch (caracter)
                            {

                                case ' ':
                                case '\r':
                                case '\t':
                                case '\n':
                                case '\b':
                                case '\f':
                                    estadoPrincipal = 0; //si es espacio o salto de liniea o tab su¿igue en el estado 0
                                    break;
                                case '{':
                                    estadoPrincipal = 1;
                                    estadoInicio = estadoInicio - 1;
                                    Columna++;
                                    break;
                                case '}':
                                    estadoPrincipal = 1;
                                    estadoInicio = estadoInicio - 1;
                                    Columna++;
                                    break;
                                case ';':
                                    estadoPrincipal = 1;
                                    estadoInicio = estadoInicio - 1;
                                    Columna++;
                                    break;
                                case '/':
                                    token += caracter; //Concatena
                                    estadoPrincipal = 2;
                                    Columna++;
                                    break;

                                case '<':
                                    token += caracter; //Concatena
                                    estadoPrincipal = 4;
                                    Columna++;
                                    break;
                                case '>':
                                    //token += CadenaConcatenar; //Concatena
                                    estadoPrincipal = 7;
                                    Columna++;
                                    break;
                                case ':':
                                    //token += CadenaConcatenar; //Concatena
                                    Console.WriteLine("Don puntos");
                                    Columna++;
                                    estadoPrincipal = 9;
                                    break;
                                case '%':
                                    token += caracter; //Concatena
                                    estadoPrincipal = 10;
                                    Columna++;
                                    break;
                                case '"':
                                    Console.WriteLine("comillas");
                                    estadoPrincipal = 11;
                                    Columna++;
                                    break;
                               
                                default:
                                    token += caracter;
                                    hayErrores = true;
                                    AnalisadorTokens(token, Ntoken.Desconocido); //Enviar al data del token
                                    token = "";             //Vaciar cadena
                                    estadoPrincipal = 0;    //regresa al estado 0
                                    Columna++;
                                    break;
                            }



                        }  //Cierre del else
                        break;
                    case 1:
                        token += caracter;
                        AnalisadorTokens(token,Ntoken.Simbolo); //Enviar al data del token
                        token = "";             //Vaciar cadena
                        estadoPrincipal = 0;    //regresa al estado 0
                        Columna++;
                        break;

                    case 2: //COMENTARIO NORMAL
                        if (caracter != '\n')
                        {
                            token += caracter;
                            estadoPrincipal = 2;
                            Columna++;
                        }
                        else //if (CadenaConcatenar=='\n')
                        {
                            token += caracter;
                            estadoInicio = estadoInicio - 1; //regresa al estado 0, para volver a leer
                            AnalisadorTokens(token,Ntoken.Comentario);
                            token = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                            Columna++;
                        }
                        break;



                    case 4: //////////COMENTARIO MULTIPLE

                        if (caracter != ('>'))
                        {
                            token += caracter;
                            estadoPrincipal = 4;
                            Columna++;
                        }
                        else
                        {
                            token += caracter;
                            estadoPrincipal = 5;
                            estadoInicio = estadoInicio - 1; //regresa al estado 0, para volver a leer
                            Columna++;
                        }

                        break;
                    case 5:
                        AnalisadorTokens(token, Ntoken.ComentarioMultiple); //enviar el token para validarlo
                        token = "";             //Vaciar cadena
                        estadoPrincipal = 0;    //regresa al estado 0
                        Columna++;
                        break;

                    case 6: //ExpresionesRegulares
                        if (caracter == (':'))
                        {
                            //token += caracter;
                            estadoPrincipal = 14;
                            Columna++;
                        }
                        else if (caracter == ('-'))
                        {
                            //token += caracter;
                            estadoPrincipal = 12;
                            Columna++;
                        }
                        else
                        {
                            token += caracter;
                            estadoPrincipal = 6;
                            Columna++;
                        }
                        break;
                    
                    case 12:
                        estadoInicio = estadoInicio - 1; //regresa al estado 0, para volver a leer
                        AnalisadorTokens(token, Ntoken.NombreEXpresionRegular); //Enviar para validar token

                        token = "";             //Vaciar cadena
                        estadoPrincipal = 0;    //regresa al estado 0
                        Columna++;
                        break;
                    case 14:
                        estadoInicio = estadoInicio - 1; //regresa al estado 0, para volver a leer
                        AnalisadorTokens(token, Ntoken.NombreLexema); //Enviar para validar token

                        token = "";             //Vaciar cadena
                        estadoPrincipal = 0;    //regresa al estado 0
                        Columna++;
                        break;
                    case 7: //CADENA DE EXPRESION REGULAR
                        if (caracter != ';')
                        {
                            token += caracter;
                            estadoPrincipal = 7;
                            Columna++;
                        }
                        else
                        {

                            //estadoInicio = estadoInicio - 1;  //regresa al estado 0, para volver a leer
                            if (cadenaExpresionRegular)
                            {
                                Columna++;
                                AnalisadorTokens(token, Ntoken.ExpresionRegular); //Enviar al data del token
                            }
                            else if (cadenaConjunto)
                            {
                                Columna++;
                                AnalisadorTokens(token, Ntoken.conjunto); //Enviar al data del token
                                cadenaExpresionRegular = true; cadenaLexema = true;
                            }
                            token = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                        }

                        break;

                    case 9: //CONCATENACION DENTRO DE :~~~~-
                        if (cadenaLexema)
                        {
                            if (caracter != ';')
                            {
                                Columna++;
                                token += caracter;
                                estadoPrincipal = 9;
                            }
                            else
                            {
                                Columna++;
                                //Console.WriteLine(token);
                               estadoInicio = estadoInicio - 1; //regresa al estado 0, para volver a leer
                                AnalisadorTokens(token, Ntoken.Lexema); //Enviar al data del token// TokenValidos(token);   //Tokne validos en el data
                                token = "";             //Vaciar cadena
                                estadoPrincipal = 0;    //regresa al estado 0
                            }
                        }
                        else
                        {
                            if (caracter != '-')
                            {
                                Columna++;
                                token += caracter;
                                estadoPrincipal = 9;
                            }
                            else
                            {
                                Columna++;
                                //estadoInicio = estadoInicio - 1;
                                AnalisadorTokens(token, Ntoken.ConjNombre);
                                token = "";             //Vaciar cadena
                                estadoPrincipal = 0;    //regresa al estado 0
                            }
                            if (caracter == '\n')
                            {
                                Columna++;
                                estadoInicio = estadoInicio - 1;
                                //DescripciondeLosToken(token); //Enviar al data del token
                                token = "";             //Vaciar cadena
                                estadoPrincipal = 0;    //regresa al estado 0
                            }
                        }
                       
                        break;

                    case 10:
                        if (caracter == ('%'))
                        {
                            Columna++;
                            token += caracter;
                            estadoPrincipal = 10;
                        }
                        else
                        {
                            Columna++;
                            cadenaExpresionRegular = false; cadenaLexema = true;
                            estadoInicio = estadoInicio - 1; //regresa al estado 0, para volver a leer
                            AnalisadorTokens(token,Ntoken.Porcentaje); //Enviar al data del token// TokenValidos(token);   //Tokne validos en el data
                            token = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                        }

                        break;

                    case 11: //CADENA
                        if (caracter!= '"')
                        {
                            Columna++;
                            token += caracter; //Concatena
                            estadoPrincipal = 11;
                        }
                        else
                        {
                            Columna++;
                            //estadoInicio= estadoInicio - 1; //regresa al estado 0, para volver a leer
                            AnalisadorTokens(token,Ntoken.Lexema); //Enviar al data del token//TokenValidos(token);   //Tokne validos en el data
                            token = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                        }
                        break;


                    case 13: ///PAÑABRA RESERVADA CONJ
                        if (caracter == ('O'))
                        {
                            Columna++;
                            token += caracter; //Concatena
                            estadoPrincipal = 13;
                        }
                        else if (caracter== 'N')
                        {
                            Columna++;
                            token += caracter; //Concatena
                            estadoPrincipal = 13;
                        }
                        else if (caracter== ('J'))
                        {
                            Columna++;
                            token +=caracter; //Concatena
                            estadoPrincipal = 13;
                        }
                        else
                        {
                            Columna++;
                            cadenaExpresionRegular = false; cadenaConjunto = true; cadenaLexema = false;
                            estadoInicio = estadoInicio - 1;
                            AnalisadorTokens(token,Ntoken.Conj); //Enviar al data del token//TokenValidos(token);   //Tokne validos en el data
                            token = "";             //Vaciar cadena
                            estadoPrincipal = 0;    //regresa al estado 0
                        }
                        break;
                   

                }

                

            }

            if (!hayErrores)
            {
                RecorrarArreglo<string>(conjuntos);
                RecorrarArreglo<string>(exRegulares);
                RecorrarArreglo<string>(Lexemas);

                ReporteXML(tokens);

            }
            else
            {
                ReporteXMLErorres(errores);
            }
            
        }

        private void AnalisadorTokens(String token,Ntoken valor)
        {
            switch (valor)
            {
                case Ntoken.Comentario:
                    asignacionTablaTokens(Ntoken.Comentario.ToString(),token,fila,Columna);
                    break;
                case Ntoken.Simbolo:
                    asignacionTablaTokens(Ntoken.Simbolo.ToString(), token, fila, Columna);
                    break;
                case Ntoken.ComentarioMultiple:
                    asignacionTablaTokens(Ntoken.ComentarioMultiple.ToString(), token, fila, Columna);
                    break;
                case Ntoken.NombreEXpresionRegular:
                    asignacionTablaTokens(Ntoken.NombreEXpresionRegular.ToString(), token, fila, Columna);
                    nameExR = token;
                    break;
                case Ntoken.ExpresionRegular:
                    setArregloExR(nameExR, token);
                    asignacionTablaTokens(Ntoken.ExpresionRegular.ToString(), token, fila, Columna);
                    break;
                case Ntoken.Conj:
                    asignacionTablaTokens(Ntoken.Conj.ToString(), token, fila, Columna);
                    break;
                case Ntoken.ConjNombre:
                    asignacionTablaTokens(Ntoken.ConjNombre.ToString(), token, fila, Columna);
                    nameConjunto = token;
                    break;
                case Ntoken.Porcentaje:
                    asignacionTablaTokens(Ntoken.Porcentaje.ToString(), token, fila, Columna);
                    break;
                case Ntoken.NombreLexema:
                    asignacionTablaTokens(Ntoken.NombreLexema.ToString(), token, fila, Columna);
                    nameLexema = token;
                    break;
                case Ntoken.Lexema:
                    setArregloLexema(nameLexema, token);
                    asignacionTablaTokens(Ntoken.Lexema.ToString(), token, fila, Columna);
                    break;
                case Ntoken.conjunto:
                    asignacionTablaTokens(Ntoken.conjunto.ToString(), token, fila, Columna);
                    setArregloConjunto(nameConjunto, token);
                    break;
                case Ntoken.Desconocido:
                    hayErrores = true;
                    asignacionTablaerrores(token, fila, Columna);
                    break;
                default:
                    hayErrores = true;
                    asignacionTablaerrores(token, fila, Columna);
                    break;
            }
            
        }

        //STring
        String nameConjunto = "";
        String nameExR = "";
        String nameLexema = "";
        private void setArregloConjunto(String nombre,String conjunto)
        {
            conjuntos[a,0] = nombre;
            conjuntos[a, 1] = conjunto;
            a++;
        }

        private void setArregloExR(String nombre, String conjunto)
        {
            exRegulares[b, 0] = nombre;
            exRegulares[b, 1] = conjunto;
            b++;
        }

        private void setArregloLexema(String nombre, String conjunto)
        {
            Lexemas[c, 0] = nombre;
            Lexemas[c, 1] = conjunto;
            c++;
        }

        private static void RecorrarArreglo<T>(T[,] matriz)
        {
            var filas = matriz.GetLength(0);
            var columnas = matriz.GetLength(1);
            var sb = new StringBuilder();
            var tmpFila = new T[matriz.GetLength(0)];
            for (int i = 0; i <filas; i++)
            {
                for(int j = 0; j < columnas; j++)
                {
                    tmpFila[j] = matriz[i, j];
                }

                sb.AppendLine(string.Join("\t", tmpFila));
                //sb.AppendLine("");
            }

            Console.WriteLine(sb.ToString());
        }

        public static string[,] getExpresionesRegulares()
        {
            return exRegulares;
        }

        public static string[,] getConjuntos()
        {
            return conjuntos;
        }

        public static string[,] getLexemas()
        {
            return Lexemas;
        }

        public bool getHayErroes()
        {
            return hayErrores;
        }

        public static void setTabla_Resetear(string[,] Tabla)
        {
            var filas = Tabla.GetLength(0);
            var columnas = Tabla.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Tabla[i, j] = null;
                }
            }
        }

        private void Limpiador() //metodo para limpiar 
        {
            var filas = conjuntos.GetLength(0);
            var columnas = conjuntos.GetLength(1);
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    conjuntos[i, j] = "";
                    exRegulares[i, j] = "";
                    Lexemas[i, j] = "";
                }
            }
            //variables int
            fila = 0;
            Columna = 0;
            a = 0; b = 0; c = 0;
            //variables bool
            cadenaConjunto = false;
            cadenaExpresionRegular = true;
            cadenaLexema = false;
        }

        private void asignacionTablaTokens(String Nombre, String Valor, int fila, int columna)
        {
            tokens[x,0] = Nombre;
            tokens[x, 1] = Valor;
            tokens[x, 2] = fila.ToString();
            tokens[x, 3] = columna.ToString();
            x++;
        }

        private void asignacionTablaerrores(String Valor, int fila, int columna)
        {
            errores[y, 0] = Valor;
            errores[y, 1] = fila.ToString();
            errores[y, 2] = columna.ToString();
            y++;
        }

        private void ReporteXML(string[,] TablaDeTokens)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement lista = doc.CreateElement("ListaTokens");
            doc.AppendChild(lista);

            XmlElement xmlToken;

            XmlElement xmlelementos;

            var filas = TablaDeTokens.GetLength(0);

            for (int i=0;i<filas;i++)
            {
                if (TablaDeTokens[i,0]!=null)
                {
                    xmlToken = doc.CreateElement("Token");
                    lista.AppendChild(xmlToken);

                    xmlelementos = doc.CreateElement("Nombre");
                    xmlelementos.AppendChild(doc.CreateTextNode(TablaDeTokens[i, 0]));
                    xmlToken.AppendChild(xmlelementos);

                    xmlelementos = doc.CreateElement("Valor");
                    xmlelementos.AppendChild(doc.CreateTextNode(TablaDeTokens[i, 1]));
                    xmlToken.AppendChild(xmlelementos);

                    xmlelementos = doc.CreateElement("Fila");
                    xmlelementos.AppendChild(doc.CreateTextNode(TablaDeTokens[i, 2]));
                    xmlToken.AppendChild(xmlelementos);

                    xmlelementos = doc.CreateElement("Columna");
                    xmlelementos.AppendChild(doc.CreateTextNode(TablaDeTokens[i, 3]));
                    xmlToken.AppendChild(xmlelementos);
                }
                
            }

            doc.Save("C:\\Users\\gmg\\Desktop\\SalidaDeTokens.xml");
        }

        private void ReporteXMLErorres(string[,] TablaDeTokens)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement lista = doc.CreateElement("ListaErores");
            doc.AppendChild(lista);

            XmlElement xmlErrores;

            XmlElement xmlelementos;

            var filas = errores.GetLength(0);

            for (int i = 0; i < filas; i++)
            {
                if (errores[i, 0] != null)
                {
                    xmlErrores = doc.CreateElement("Error");
                    lista.AppendChild(xmlErrores);

                    xmlelementos = doc.CreateElement("Valor");
                    xmlelementos.AppendChild(doc.CreateTextNode(errores[i, 0]));
                    xmlErrores.AppendChild(xmlelementos);

                    xmlelementos = doc.CreateElement("Fila");
                    xmlelementos.AppendChild(doc.CreateTextNode(errores[i, 1]));
                    xmlErrores.AppendChild(xmlelementos);

                    xmlelementos = doc.CreateElement("Columna");
                    xmlelementos.AppendChild(doc.CreateTextNode(errores[i, 2]));
                    xmlErrores.AppendChild(xmlelementos);
                }

            }

            doc.Save("C:\\Users\\gmg\\Desktop\\SalidaDeErrores.xml");
        }
    }
}
