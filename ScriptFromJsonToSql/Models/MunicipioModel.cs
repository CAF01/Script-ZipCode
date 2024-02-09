using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptFromJsonToSql.Models
{
    public class MunicipioModel
    {
        public int d_codigo { get; set; } //C.P
        public string D_mnpio { get; set; }
        public string d_estado { get; set; }
        public string d_ciudad { get; set; }
    }

	public class CiudadContainer
	{
		public List<MunicipioModel> table { get; set; }
	}
}
