using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C1_proyecto1
{
    class VerificadorDeLexemas
    {
        


        public VerificadorDeLexemas()
        {
            
        }

        public string validarLexema(string[,] Lexemas, string[,] afd, string[,] conjuntos,string[] aceptaciones, string nomLex)
        {
            String lexema = null;
            String nombreLexema = "";
            String conjunto = null;

            String lista = "";

            string x = "A";

            char caracter;
            bool caracterAceptado = false;
            

            for (int i=0;i<Lexemas.GetLength(0); i++) //Rrecorrer la tabla donde estan los lexemas
            {
                if (Lexemas[i,0]!=null && Lexemas[i,0]==nomLex) //si la celda es diferente de null
                {
                    Console.WriteLine(Lexemas[i, 1]+"->"+i);
                    nombreLexema = Lexemas[i,0];
                    lexema = Lexemas[i, 1];
                    x = "A";
                    for (int j=0;j < lexema.Length;j++) //for para recorrer el lexema carecter a carecter
                    {
                        caracter = lexema[j];
                        
                        for(int k = 1; k < afd.GetLength(0); k++) //buscar por filas en afd
                        {

                            if (afd[k, 0] != null && afd[k,0]==x)
                            {
                                for(int l = 1; l < afd.GetLength(1); l++)//buscqar por columnas afd
                                {
                                    if (afd[k, l] != null)
                                    {
                                        if (buascadorEnConjuntos(conjuntos, afd[0, l]))
                                        {
                                            
                                            conjunto = obteneValorEnConjuntos(conjuntos, afd[0,l]);
                                            caracterAceptado =  TipoDeConjunto(caracter, conjunto);
                                            
                                            x = afd[k,l];

                                            if (caracterAceptado) { k++; break; }
                                        }
                                        else if(VerificarCadena(afd[0,l], lexema, j))
                                        {
                                            j = j+afd[0, l].Length;
                                            caracterAceptado = true;
                                            x = afd[k, l];
                                            k++;
                                            break;
                                        }
                                        else
                                        {
                                            caracterAceptado = false;
                                            break;
                                        }
                                    }
                                }
                                if (!caracterAceptado)
                                {
                                    break;
                                }
                            }

                        }
                        if (!caracterAceptado)
                        {
                            break;
                        }

                    }
                    if (caracterAceptado && verificarEstadoDeAceptacion(aceptaciones,x))
                    {
                        lista += nombreLexema + " ->: //LEXEMA Valor CORRECTO\n";
                    }
                    else
                    {
                        lista += nombreLexema + " ->: //LEXEMA Valor INCORRECTO\n";
                    }
                }
                
            }

            return lista;
        }

        private bool buascadorEnConjuntos(string[,] conjuntos, string Elemento)
        {
            for (int i=0;i<conjuntos.GetLength(0);i++)
            {
                if (conjuntos[i,0]!=null)
                {
                    if (conjuntos[i, 0]==Elemento)
                    {
                        return true;
                    }
                     
                }
                
            }
            return false;
        } 

        private string obteneValorEnConjuntos(string[,] conjuntos,String Elemento)
        {
            string conjuntoElemento = "";
            for (int i = 0; i < conjuntos.GetLength(0); i++)
            {
                if (conjuntos[i, 0] != null && conjuntos[i, 0] == Elemento)
                {
                    conjuntoElemento = conjuntos[i, 1];
                    
                    return conjuntoElemento;
                }
            }

            return Elemento;
        }

        private bool TipoDeConjunto(char conjuntoElemento, String conjunto)
        {
            int primero;
            int ultimo;

            int asciiConjunto = conjuntoElemento;

            string[] arregloDeconjuntoString;
            int[] arregloDeconjuntoInt= new int[conjunto.Length];

            if (conjunto == "[:TODO:]")
            {
                primero=65;
                ultimo=122;
                return buscadorPorRango(primero, ultimo, asciiConjunto);
            }
            else if(conjunto =="[:ABCDEFGHIJKLMNÑOPQRSTUVWXYZ:]")
            {
                return buscadorPorRango(65, 90, asciiConjunto);
            }
            else if (conjunto[1]=='~')
            {
                primero = conjunto[0];
                ultimo = conjunto[2];
                return buscadorPorRango(primero,ultimo,asciiConjunto);

            }
            else if (conjunto[1] == ',')
            {
                arregloDeconjuntoString = conjunto.Split(',');
                int i = 0;
                foreach(string c in arregloDeconjuntoString)
                {
                    arregloDeconjuntoInt[i] = Convert.ToChar(c);
                    i++;
                }
                return buscarEnArreglo(arregloDeconjuntoInt, asciiConjunto);
            }

            return false;
        }

        private bool buscadorPorRango(int primero,int ultimo, int elemento)
        {
            if (primero<=elemento&&elemento<=ultimo)
            {
                return true;
            }
            return false;
        }

        private bool buscarEnArreglo(int[] caracteres,int elemento)
        {
            foreach(int i in caracteres)
            {
                if (elemento==i) return true ;
            }
            return false;
        }

        //Verificar si existe la cadena
        private bool VerificarCadena(String cadena, String minLexema, int indice)
        {
            String cadenaVerificadora="";
            Console.WriteLine(cadena.Length + "->" + minLexema.Length + "##->"+indice);
            int contador = 0;

            for (int j = indice; j < minLexema.Length; j++)
            {
                contador++;
            }
            for (int i = 0; i < cadena.Length; i++)
            {
                
                if (i < contador)
                {
                    Console.WriteLine(contador+"$");
                    cadenaVerificadora += minLexema[indice+i].ToString();
                }
            }

            if (cadena == cadenaVerificadora) return true;
            return false;
        }


        private bool verificarEstadoDeAceptacion(string[] aceptaciones,string estado)
        {
           //onsole.WriteLine("Estado->"+estado);
            foreach(string s in aceptaciones)
            {

             // Console.WriteLine("aqui adentro->"+estado+"->"+ s+"|");
                if (estado == s) return true;
            }
            return false;
        }
        
    }
}
