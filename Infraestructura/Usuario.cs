using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int RolId { get; set; }
        public Rol Rol { get; set; }
        public ICollection<Actividad> Actividades { get; set; }
    }
}
