using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowSQLTable.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ShowSQLTable.Controllers
{
    public class HomeController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        // miután elkészült az sql mezőkat tartalmazó osztály lehet a listát felvenni
        List<LogData> logDatas = new List<LogData>();
        List<UserData> userDatas = new List<UserData>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            con.ConnectionString = ShowSQLTable.Properties.Resources.ConnectionString;
            // Project/Project propertiesben resource file létrehozása, ott változónak fel lehet vennni a connection adatait, itt pedig hivatkozni
        }

        public IActionResult Index()
        {
            //FetchData();
            //return View(logDatas);
            FetchUserData();
            return View(userDatas);
        }
        private void FetchData()
        {
            if(logDatas.Count > 0)
            {
                logDatas.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [tp_SiteID],[tp_ID],[tp_Login],[tp_Title],[tp_Email],[tp_ExternalTokenLastUpdated],[tp_Mobile] FROM [WSS_DEV_Content_PIMS_01].[dbo].[UserInfo]";
                dr = com.ExecuteReader();
                // a lista elkészülte után lehet az adatokat beolvasni
                while (dr.Read())
                {
                    logDatas.Add(new LogData()
                    {
                        SiteID = dr["tp_SiteID"].ToString(),
                        ID = dr["tp_ID"].ToString(),
                        Login = dr["tp_Login"].ToString(),
                        Title = dr["tp_Title"].ToString(),
                        Email = dr["tp_Email"].ToString(),
                        ExternalTokenLastUpdated = dr["tp_ExternalTokenLastUpdated"].ToString(),
                        Mobile = dr["tp_Mobile"].ToString()
                    });
                }
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void FetchUserData()
        {
            if (userDatas.Count > 0)
            {
                userDatas.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT [id], [UserName], [FirstName], [LastName], [E-mail] FROM [Webster].[dbo].[Users]";
                dr = com.ExecuteReader();
                // a lista elkészülte után lehet az adatokat beolvasni
                while (dr.Read())
                {
                    userDatas.Add(new UserData()
                    {

                        ID = (int)dr["ID"],
                        UserName = dr["UserName"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Email = dr["E-mail"].ToString()

                    });
                }
                con.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
