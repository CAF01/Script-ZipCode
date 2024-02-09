using ClosedXML.Excel;
using Newtonsoft.Json;
using ScriptFromJsonToSql.Models;
using System.Globalization;
using System.Text;

var archivosJson = Directory.GetFiles("../../../Files", "*.json");

static int LeerArchivos(string[] archivosJson)
{
	List<int> CPValidator= new List<int>();
	int contadorArchivos = 0;
	
	foreach (var archivoJson in archivosJson)
	{
		var json = File.ReadAllText(archivoJson);

		var municipios = JsonConvert.DeserializeObject<List<MunicipioModel>>(json);
		var ciudades = ConvertirExcelASQL("../../../CitiesExcel/ciudades.xlsx");

		CreateScriptBasedCities(municipios, CPValidator,ciudades);

		contadorArchivos++;
	}

	return contadorArchivos;
}

static List<CiudadModel> ConvertirExcelASQL(string rutaArchivoExcel)
{
	var listaCiudades = new List<CiudadModel>();
	try
	{
		using (var workbook = new XLWorkbook(rutaArchivoExcel))
		{
			var worksheet = workbook.Worksheets.Worksheet(1);
			bool primerRegistro = true;

			foreach (var row in worksheet.RowsUsed())
			{
				if (primerRegistro)
				{
					primerRegistro = false;
					continue;
				}

				int ciudadId = int.Parse(row.Cell(1).Value.ToString());
				string nombre = row.Cell(2).Value.ToString();

				listaCiudades.Add(new CiudadModel
				{
					CiudadId = ciudadId,
					Nombre = nombre.ToLower()
				});
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error al convertir el archivo Excel: {ex.Message}");
	}

	return listaCiudades;
}

static string RemoveAccents(string input)
{
	string normalizedString = input.Normalize(NormalizationForm.FormD);
	string withoutAccents = new string(normalizedString
		.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
		.ToArray());

	return withoutAccents;
}

static void CreateScriptBasedCities(List<MunicipioModel> municipios, List<int> CPValidator, List<CiudadModel> ciudadModels)
{
	try
	{
		using (StreamWriter sw = new StreamWriter($"../../../GeneratedScripts/scriptCiudad_{municipios[0].d_estado}.txt"))
		{
			foreach (var ciudad in municipios)
			{
				var cpAgregado = CPValidator.FirstOrDefault(x => x == ciudad.d_codigo);
				if (cpAgregado == 0)
				{
					CPValidator.Add(ciudad.d_codigo);
				}
				else
				{
					continue;
				}
				if(ciudad.D_mnpio == "Ahualulco del Sonido 13" || ciudad.D_mnpio == "Villa Hidalgo Yalálag" || ciudad.D_mnpio == "Ñuu Savi" || ciudad.D_mnpio == "Santa Cruz del Rincón")
				{
					continue;
				}

				ciudad.D_mnpio = RemoveAccents(ciudad.D_mnpio);

				switch (ciudad.D_mnpio)
				{
					case "Castanos":
						ciudad.D_mnpio = "castaños";
						break;
					case "Acambay de Ruiz Castaneda":
						ciudad.D_mnpio = "acambay de ruiz castañeda";
						break;
					case "Acuna":
						ciudad.D_mnpio = "acuña";
						break;
					case "Penon Blanco":
						ciudad.D_mnpio = "peñon blanco";
						break;
					case "Tlajomulco de Zuniga":
						ciudad.D_mnpio = "tlajomulco de zuñiga";
						break;
					case "Bolanos":
						ciudad.D_mnpio = "bolaños";
						break;
					case "San Martin de Bolanos":
						ciudad.D_mnpio = "san martin de bolaños";
						break;
					case "Canadas de Obregon":
						ciudad.D_mnpio = "cañadas de obregon";
						break;
					case "Brisenas":
						ciudad.D_mnpio = "briseñas";
						break;
					case "Guemez":
						ciudad.D_mnpio = "güemez";
						break;
					case "Tinguindin":
						ciudad.D_mnpio = "tingüindin";
						break;
					case "Amatlan de Canas":
						ciudad.D_mnpio = "amatlan de cañas";
						break;
					case "General Trevino":
						ciudad.D_mnpio = "general treviño";
						break;
					case "Santa Maria Penoles":
						ciudad.D_mnpio = "santa maria peñoles";
						break;
					case "San Vicente Nunu":
						ciudad.D_mnpio = "san vicente nuñu";
						break;
					case "San Juan Numi":
						ciudad.D_mnpio = "san juan ñumi";
						break;
					case "Magdalena Penasco":
						ciudad.D_mnpio = "magdalena peñasco";
						break;
					case "San Bartolome Yucuane":
						ciudad.D_mnpio = "san bartolome yucuañe";
						break;
					case "San Mateo Penasco":
						ciudad.D_mnpio = "san mateo peñasco";
						break;
					case "San Francisco Nuxano":
						ciudad.D_mnpio = "san francisco nuxaño";
						break;
					case "San Andres Nuxino":
						ciudad.D_mnpio = "san andres nuxiño";
						break;
					case "Matias Romero Avendano":
						ciudad.D_mnpio = "matias romero avendaño";
						break;
					case "San Jose del Penasco":
						ciudad.D_mnpio = "san jose del peñasco";
						break;
					case "San Mateo Pinas":
						ciudad.D_mnpio = "san mateo piñas";
						break;
					case "La Compania":
						ciudad.D_mnpio = "la compañía";
						break;
					case "Penamiller":
						ciudad.D_mnpio = "peñamiller";
						break;
					case "Puerto Penasco":
						ciudad.D_mnpio = "puerto peñasco";
						break;
					case "Espanita":
						ciudad.D_mnpio = "españita";
						break;
					case "Canitas de Felipe Pescador":
						ciudad.D_mnpio = "cañitas de felipe pescador";
						break;
					case "Canada Morelos":
						ciudad.D_mnpio = "cañada morelos";
						break;
					case "Munoz de Domingo Arenas":
						ciudad.D_mnpio = "muñoz de domingo arenas";
						break;
					case "Heroica Villa de San Blas Atempa":
						ciudad.D_mnpio = "san blas atempa";
						break;
					case "Ozuluama de Mascarenas":
						ciudad.D_mnpio = "ozuluama de mascareñas";
						break;
					case "San Antonio Canada":
						ciudad.D_mnpio = "san antonio cañada";
						break;
					default:
						break;
				}

				var ciudadEncontrada = ciudadModels.FirstOrDefault(x => x.Nombre == ciudad?.D_mnpio.ToLower()
					|| x.Nombre.Contains(ciudad?.D_mnpio.ToLower())
				);

				if (ciudadEncontrada != null)
				{
					ciudad.d_ciudad = $"{ciudadEncontrada.CiudadId}";
				}
				else
				{
					Console.WriteLine($"No se encontró la ciudad: {ciudad.D_mnpio}");
				}

				string scriptInsert = $"INSERT INTO SCH_CPostal (ciudadId,codigoPostal,estado) VALUES ({ciudad.d_ciudad},{ciudad.d_codigo},'{ciudad.d_estado}');";

				sw.WriteLine(scriptInsert);
			}
		}
	}
	catch (Exception s)
	{
		Console.WriteLine(s.Message);
		throw;
	}

	
}

Console.WriteLine($"Se procesaron {LeerArchivos(archivosJson)} archivos");

Console.ReadKey();

