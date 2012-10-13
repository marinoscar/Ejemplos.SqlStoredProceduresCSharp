using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpAndStoredProcedures.Datos
{
    /// <summary>
    /// Modelo que representa la tabla de la base de datos
    /// </summary>
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
