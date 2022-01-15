using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UsersControlConduentTest.Models;

namespace UsersControlConduentTest.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetList(List<UserModel> list)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            try
            {
                CreateCodeGeneric(list, 0, listUserModel);

            }
            catch (Exception)
            {

                throw;
            }

            return Json(listUserModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetData(List<UserModel> list)
        {
            List<GeneralChartModel.DataPointDefault> listAverage = new List<GeneralChartModel.DataPointDefault>();
            string highestRating = string.Empty;
            string lowestRating = string.Empty;
            string average = string.Empty;
            try
            {
                if(list != null && list.Count > 0)
                {
                    list.ForEach(a => {
                        listAverage.Add(new GeneralChartModel.DataPointDefault(a.Nombres, a.Calificacion));
                    });

                    var studentInfo = list.OrderByDescending(a => a.Calificacion).FirstOrDefault();
                    highestRating = $"{studentInfo.Nombres} {studentInfo.ApellidoPaterno} {studentInfo.ApellidoMaterno}";

                    var _lowestRating = list.OrderBy(a => a.Calificacion).FirstOrDefault();
                    lowestRating = $"{_lowestRating.Nombres} {_lowestRating.ApellidoPaterno} {_lowestRating.ApellidoMaterno}";

                    double total = list.Sum(a => a.Calificacion);
                    int countstudents = list.Count();
                    average = (total / countstudents).ToString("N2");
                }
               
            }
            catch (Exception)
            {

                throw;
            }
            return Json(new { chart = listAverage, highestRating, lowestRating, average});
        }


        [HttpPost]
        public async Task<ActionResult> ChangeCode(List<UserModel> list, int iteracion)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            try
            {
                CreateCodeGeneric(list, iteracion, listUserModel);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(listUserModel);
        }

        private void CreateCodeGeneric(List<UserModel> list, int iteracion, List<UserModel> listUserModel)
        {
            foreach (var item in list)
            {
                string name = item.Nombres.Substring(0, 2);
                string lastName = "";                

                if (iteracion.Equals(0)) 
                { 
                    item.FechaNacimientoStr = item.FechaNacimiento.ToString("dd/MM/yyyy");

                    string firstLetter = name.Substring(0, 1);
                    string secondLetter = name.Substring(name.Length - 1, 1);
                    name = OrderCode("", firstLetter, iteracion);
                    name = OrderCode(name, secondLetter, iteracion);

                    firstLetter = item.ApellidoMaterno.Substring(item.ApellidoMaterno.Length - 2, 1);
                    secondLetter = item.ApellidoMaterno.Substring(item.ApellidoMaterno.Length - 1, 1);
                    lastName = OrderCode("", firstLetter, iteracion);
                    lastName = OrderCode(lastName, secondLetter, iteracion);

                    
                }
                else 
                {
                    item.FechaNacimiento = DateTime.ParseExact(item.FechaNacimientoStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    
                    char[] chars = item.ClaveModificada.ToCharArray();
                    char[] newChar = new char[chars.ToList().Count];
                    for (int i = 0; i < iteracion; i++)
                    {
                        newChar = new char[chars.ToList().Count];
                        for (int j = 0; j < chars.Length; j++)
                        {
                            int position = j == 0 ? chars.Length - 1: j-1;
                            newChar[position] = chars[j];
                        }

                        chars = newChar;
                    }
                    item.ClaveModificada = new string(newChar);
                }
                    

                double years = DateTime.Now.Subtract(item.FechaNacimiento).Days / 365;

                item.Clave = $"{item.Nombres.Substring(0, 2).ToUpper()}{item.ApellidoMaterno.Substring(item.ApellidoMaterno.Length - 2, 2).ToUpper()}{years}";


                if (iteracion.Equals(0))
                {
                    item.ClaveModificada = $"{name}{lastName}{years}";
                    item.FechaNacimientoStr = item.FechaNacimiento.ToString("dd/MM/yyyy");
                }
                    

                listUserModel.Add(item);
            }
        }

        private string OrderCode(string code, string letter, int iteracion)
        {
            
            string newCode = string.Empty;

            char letra = char.Parse(letter.ToUpper());
            int _iteracion = 3 + iteracion;
            for (int i = 0; i < _iteracion; i++)
            {
                letra--;
                if (letra < 'A')
                    letra = 'Z';
            }

            newCode = code + letra.ToString();

            return newCode;
        }
    }
}