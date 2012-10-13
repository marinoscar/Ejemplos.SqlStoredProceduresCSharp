using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using CSharpAndStoredProcedures.Datos;

namespace CSharpAndStoredProcedures.Negocios
{
    public interface IRepositorioPersonas
    {
        IEnumerable<Persona> TraerPersonas();
        Persona TraerPersona(int id);
        Persona AgregarPersona(Persona persona);
        void EditarPersona(Persona persona);
        void EliminarPersona(int id);
    }

    public class RepositorioPersonas : IRepositorioPersonas
    {
        #region Constructores

        public RepositorioPersonas()
        {
            Contexto = new ContextoDeDatos();
        }

        public RepositorioPersonas(IContextoDeDatos contextoDeDatos)
        {
            Contexto = contextoDeDatos;
        } 

        #endregion

        #region Propiedades
        
        public IContextoDeDatos Contexto { get; protected set; } 

        #endregion

        #region Metodos
        
        public IEnumerable<Persona> TraerPersonas()
        {
            var personas = new List<Persona>();
            var cmd = Contexto.CrearCommando("[dbo].[usp_ListarPersonas]");
            cmd.Connection.Open();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    personas.Add(Contexto.CargarPersonaDeReader(reader));
                }
                reader.Close();
            }
            cmd.Connection.Close();
            return personas;
        }

        public Persona TraerPersona(int id)
        {
            var persona = new Persona();
            var cmd = Contexto.CrearCommando("[dbo].[usp_TraerPersona]");
            cmd.Parameters.Add(Contexto.CrearParametro("Id", new Persona() {Id = id}, cmd, false));
            cmd.Connection.Open();
            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (reader.Read())
                {
                    persona = Contexto.CargarPersonaDeReader(reader);
                }
                reader.Close();
            }
            cmd.Connection.Close();
            return persona;
        }

        public Persona AgregarPersona(Persona persona)
        {
            var cmd = Contexto.CrearComandoConParametros("[dbo].[usp_AgregarPersona]", persona);
            cmd.Connection.Open();
            cmd.Transaction = cmd.Connection.BeginTransaction();
            var id = cmd.ExecuteScalar();
            persona.Id = Convert.ToInt32(id);
            cmd.Transaction.Commit();
            cmd.Connection.Close();
            return persona;
        }

        public void EditarPersona(Persona persona)
        {
            var cmd = Contexto.CrearComandoConParametros("[dbo].[usp_ModificarPersona]", persona);
            cmd.Connection.Open();
            cmd.Transaction = cmd.Connection.BeginTransaction();
            cmd.ExecuteNonQuery();
            cmd.Transaction.Commit();
            cmd.Connection.Close();
        }

        public void EliminarPersona(int id)
        {
            var cmd = Contexto.CrearCommando("[dbo].[usp_EliminarPersona]");
            cmd.Parameters.Add(Contexto.CrearParametro("Id", new Persona() { Id = id }, cmd, false));
            cmd.Connection.Open();
            cmd.Transaction = cmd.Connection.BeginTransaction();
            cmd.ExecuteNonQuery();
            cmd.Transaction.Commit();
            cmd.Connection.Close();
        }

        #endregion
    }
}
