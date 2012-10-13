using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpAndStoredProcedures.Datos;
using CSharpAndStoredProcedures.Negocios;

namespace CSharpAndStoredProcedures.Vistas
{
    public interface IVistaPersonas
    {
        void ListarPersonas();
        void AgregarPersona();
        void EditarPersona();
        void EliminarPersona();
        void TraerPersona();
    }

    public class VistaPersonasConsola : IVistaPersonas
    {

        #region Constructores
        
        public VistaPersonasConsola(IRepositorioPersonas repositorio)
        {
            Repositorio = repositorio;
        } 

        #endregion

        #region Propiedades
        
        public IRepositorioPersonas Repositorio { get; protected set; } 

        #endregion

        #region Metodos

        public void ListarPersonas()
        {
            foreach (var persona in Repositorio.TraerPersonas())
            {
                Console.WriteLine();
                MostrarPersona(persona);
            }
            EsperarUsuario();
        }

        public void MostrarPersona(Persona persona)
        {
            Console.WriteLine("Id....: {0}", persona.Id);
            Console.WriteLine("Nombre: {0} {1} {2}", persona.Nombre, persona.Apellido1, persona.Apellido2);
            Console.WriteLine("Email.: {0}", persona.CorreoElectronico);
        } 

        public void AgregarPersona()
        {
            var persona = DatosPersona();
            Repositorio.AgregarPersona(persona);
        }

        public void EditarPersona()
        {
            var id = ObtenerId();
            var persona = DatosPersona();
            persona.Id = id;
            Repositorio.EditarPersona(persona);
        }

        public void EliminarPersona()
        {
            var persona = TraerPersonaDeBd();
            if(persona == null)
            {
                EsperarUsuario();
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Esta deacuerdo con eliminar este registro (s/n)?");
            var respuesta = Console.ReadKey();
            if(respuesta.Key == ConsoleKey.S)
                Repositorio.EliminarPersona(persona.Id);
        }

        public void TraerPersona()
        {
            TraerPersonaDeBd();
            EsperarUsuario();
        }

        private Persona TraerPersonaDeBd()
        {
            var id = ObtenerId();
            var persona = Repositorio.TraerPersona(id);
            if(persona.Id == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Id invalido...");
                return null;
            }
            MostrarPersona(persona);
            return persona;
        }

        public int ObtenerId()
        {
            Console.Write("Digite Id de la persona: ");
            var id = Console.ReadLine();
            return Convert.ToInt32(id);
        }

        public Persona DatosPersona()
        {
            var persona = new Persona();
            Console.Write("Nombre............: ");
            persona.Nombre = Console.ReadLine();
            Console.Write("Primer Apellido...: ");
            persona.Apellido1 = Console.ReadLine();
            Console.Write("Segundo Apellido..: ");
            persona.Apellido2 = Console.ReadLine();
            Console.Write("Correo Electronico: ");
            persona.CorreoElectronico = Console.ReadLine();
            return persona;
        }

        private static void EsperarUsuario()
        {
            Console.WriteLine();
            Console.Write("Precione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        #endregion
    }
}


