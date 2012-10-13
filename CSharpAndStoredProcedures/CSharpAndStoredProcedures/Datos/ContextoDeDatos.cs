using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CSharpAndStoredProcedures.Datos
{
    public interface IContextoDeDatos
    {
        DbCommand CrearCommando(string nombreProcedimiento);
        Persona CargarPersonaDeReader(IDataRecord reader);
        DbParameter CrearParametro(string nombreColumna, Persona persona, DbCommand comando, bool esParametroDeSalida);
        DbCommand CrearComandoConParametros(string nombreProcedimiento, Persona persona);
    }

    /// <summary>
    /// Tiene los metodos para extraer la informacion de la base de datos
    /// Este clase cuenta con una implementacion generica, lo que permitiria
    /// eventualmente extraer datos tanto de Sql Server como de otras fuentes
    /// de datos como MySql, Postgres u otras
    /// </summary>
    public class ContextoDeDatos : IContextoDeDatos
    {

        #region Variables

        private DbConnection _conexion; 

        #endregion
        
        #region Constructores

        public ContextoDeDatos()
        {
            InicializarClase("Personas");
        }

        public ContextoDeDatos(string nombreOStringDeConexion)
        {
            InicializarClase(nombreOStringDeConexion);
        }

        private void InicializarClase(string nombreOStringDeConexion)
        {
            var defaultFactory = ConfigurationManager.AppSettings["defaultFactory"];
            if (string.IsNullOrWhiteSpace(defaultFactory)) defaultFactory = "System.Data.SqlClient";
            var factory = DbProviderFactories.GetFactory(defaultFactory);
            _conexion = factory.CreateConnection();

            var connectionStringSettings = ConfigurationManager.ConnectionStrings[nombreOStringDeConexion];
            _conexion.ConnectionString = connectionStringSettings == null ? nombreOStringDeConexion : connectionStringSettings.ConnectionString;


        } 

        #endregion
        
        #region Metodos

        public DbCommand CrearCommando(string nombreProcedimiento)
        {
            var cmd = _conexion.CreateCommand();
            cmd.CommandText = nombreProcedimiento;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        } 

        public DbCommand CrearComandoConParametros(string nombreProcedimiento, Persona persona)
        {
            var cmd = CrearCommando(nombreProcedimiento);
            cmd.Parameters.Add(CrearParametro("Id", persona, cmd, false));
            cmd.Parameters.Add(CrearParametro("Nombre", persona, cmd, false));
            cmd.Parameters.Add(CrearParametro("Apellido1", persona, cmd, false));
            cmd.Parameters.Add(CrearParametro("Apellido2", persona, cmd, false));
            cmd.Parameters.Add(CrearParametro("CorreoElectronico", persona, cmd, false));
            return cmd;
        }

        public DbParameter CrearParametro(string nombreColumna, Persona persona, DbCommand comando, bool esParametroDeSalida)
        {
            var parametro = comando.CreateParameter();
            parametro.ParameterName = string.Format("@{0}", nombreColumna);
            parametro.Value = TraerValorDePropiedad(persona, nombreColumna);
            parametro.Direction = esParametroDeSalida ? ParameterDirection.InputOutput : ParameterDirection.Input;
            return parametro;
        }

        private static object TraerValorDePropiedad(Persona persona, string nombreDePropiedad)
        {
            return persona.GetType().GetProperty(nombreDePropiedad).GetValue(persona, null);
        }

        public Persona CargarPersonaDeReader(IDataRecord reader)
        {
            return new Persona()
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Apellido1 = reader.GetString(2),
                Apellido2 = reader.GetString(3),
                CorreoElectronico = reader.GetString(4)
            };
        }

        #endregion
    }
}

