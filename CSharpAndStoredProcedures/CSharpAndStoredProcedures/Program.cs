using System;
using CSharpAndStoredProcedures.Datos;
using CSharpAndStoredProcedures.Negocios;
using CSharpAndStoredProcedures.Vistas;

namespace CSharpAndStoredProcedures
{
    class Program
    {
        static void Main(string[] args)
        {
            var opcion = 99;
            IVistaPersonas vista = new VistaPersonasConsola(new RepositorioPersonas(new ContextoDeDatos()));
            while (opcion != 0)
            {
                opcion = MostrarMenu();
                Console.Clear();
                try
                {
                    switch (opcion)
                    {
                        case 1:
                            vista.ListarPersonas();
                            break;
                        case 2:
                            vista.TraerPersona();
                            break;
                        case 3:
                            vista.AgregarPersona();
                            break;
                        case 4:
                            vista.EditarPersona();
                            break;
                        case 5:
                            vista.EliminarPersona();
                            break;
                    }
                }
                catch (Exception exception)
                {
                    ManejarError(exception);
                }
            }
        }

        private static void ManejarError(Exception exception)
        {
            Console.Clear();
            Console.WriteLine("A occurido un error");
            Console.WriteLine(exception.Message);
            Console.Write("Precione cualquier teclar para contiunar...");
            Console.ReadKey();
        }

        static int MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("Modulo de Personas");
            Console.WriteLine();
            Console.WriteLine("1. Listar");
            Console.WriteLine("2. Buscar por Id");
            Console.WriteLine("3. Agregar");
            Console.WriteLine("4. Editar");
            Console.WriteLine("5. Elimiar");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            var key = Console.ReadKey();
            Console.Clear();
            return Convert.ToInt32(key.KeyChar.ToString());
        }
    }
}

