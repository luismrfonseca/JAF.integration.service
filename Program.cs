using JAF.integration.service.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JAF.integration.service
{
    class Program
    {
        private static readonly LogManager logManager = new LogManager();
        private static MailManager mailManager = null;
        private static string key = "VoI2SycM4QtwFheILl4NZDmXRjZWMAmX";

        [STAThread]
        public static async Task Main()
        {
            #region Setup Variaveis
            string sqlinstance = "";
            string sqluser = "";
            string sqlpass = "";
            string sqldatabase = "";

            string smtpServer = "";
            bool smtpSSL = false;
            int smtpPort = 0;
            bool smtpDefaultCredencials = true;
            string smtpUser = "";
            string smtpPass = "";

            string mailSendTo = "";

            string apiServer = "";

            string sql = "";
            SqlCommand command = null;
            SqlDataReader reader = null;
            #endregion

            logManager.LogWrite("---\tInicio da execução do serviço Gestão de Obras");

            //read config db SQL SERVER for INI FILE
            var MyIni = new IniFile("Settings.ini");
            logManager.LogWrite("- Verificar se as configurações existem no ficheiro de configuração Settings.ini ");

            #region Setup INI File
            if (!MyIni.KeyExists("instance", "ServerMSSQL"))
            {
                MyIni.Write("instance", "192.168.1.250\\thebox", "ServerMSSQL");
            }
            if (!MyIni.KeyExists("username", "ServerMSSQL"))
            {
                MyIni.Write("username", "sa", "ServerMSSQL");
            }
            if (!MyIni.KeyExists("password", "ServerMSSQL"))
            {
                MyIni.Write("password", "wxXHg3XRfqv7cTXnDariZQ==", "ServerMSSQL");
            }
            if (!MyIni.KeyExists("database", "ServerMSSQL"))
            {
                MyIni.Write("database", "JAF_BC_PRD", "ServerMSSQL");
            }

            if (!MyIni.KeyExists("server", "SMTP"))
            {
                MyIni.Write("server", "smtp.gmail.com", "SMTP");
            }
            if (!MyIni.KeyExists("ssl", "SMTP"))
            {
                MyIni.Write("ssl", "true", "SMTP");
            }
            if (!MyIni.KeyExists("port", "SMTP"))
            {
                MyIni.Write("port", "587", "SMTP");
            }
            if (!MyIni.KeyExists("DefaultCredencials", "SMTP"))
            {
                MyIni.Write("DefaultCredencials", "false", "SMTP");
            }
            if (!MyIni.KeyExists("username", "SMTP"))
            {
                MyIni.Write("username", "ynb.development@gmail.com", "SMTP");
            }
            if (!MyIni.KeyExists("password", "SMTP"))
            {
                MyIni.Write("password", "m/xGs42t1Aons8oe24jtAw==", "SMTP");
            }

            if (!MyIni.KeyExists("sendTo", "MAIL"))
            {
                MyIni.Write("sendTo", "ynb.development24@gmail.com", "MAIL");
            }
            if (!MyIni.KeyExists("server", "API"))
            {
                MyIni.Write("server", "http://localhost:3000", "API");
            }
            #endregion

            #region Leitura das configurações da base de dados
            logManager.LogWrite("- Leitura das configurações de acesso à base dados do SQL SERVER ");
            sqlinstance = MyIni.Read("instance", "ServerMSSQL");
            sqluser = MyIni.Read("username", "ServerMSSQL");
            sqlpass = MyIni.Read("password", "ServerMSSQL");
            sqlpass = AesOperation.DecryptString(key, sqlpass);
            sqldatabase = MyIni.Read("database", "ServerMSSQL");

            if (sqlinstance.Length <= 0)
            {
                logManager.LogWrite("ERROR - Não foi possivel encontrar a instancia do servidor MSSQL ");
                return;
            }
            if (sqluser.Length <= 0)
            {
                logManager.LogWrite("ERROR - Não foi possivel encontrar o username do servidor MSSQL ");
                return;
            }
            if (sqlpass.Length <= 0)
            {
                logManager.LogWrite("ERROR - Não foi possivel encontrar a password do servidor MSSQL ");
                return;
            }
            #endregion

            #region Leitura das configurações de SMTP
            logManager.LogWrite("- Leitura das configurações de SMTP ");
            smtpServer = MyIni.Read("server", "SMTP");
            smtpSSL = MyIni.Read("ssl", "SMTP") == "true";
            smtpPort = Convert.ToInt32(MyIni.Read("port", "SMTP"));
            smtpDefaultCredencials = MyIni.Read("DefaultCredencials", "SMTP") == "true";
            smtpUser = MyIni.Read("username", "SMTP");
            smtpPass = MyIni.Read("password", "SMTP");
            smtpPass = AesOperation.DecryptString(key, smtpPass);

            mailSendTo = MyIni.Read("sendTo", "MAIL");
            #endregion

            #region Leitura das configurações do servidor da API
            logManager.LogWrite("- Leitura das configurações de SMTP ");
            apiServer = MyIni.Read("server", "API");
            #endregion

            //connect to db SQL SERVER

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = sqlinstance;
            builder.UserID = sqluser;
            builder.Password = sqlpass;
            builder.InitialCatalog = sqldatabase;

            using(SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open();
                logManager.LogWrite("- Ligação à base de dados SQL SERVER");

                //Get All Resources
                sql = @"SELECT [No_] as id,[Type] as tipo,[Name] as nome,[Name 2] as nome2
                        FROM[JAF_BC_PRD].[dbo].[JAF$Resource]
                        WHERE Blocked = 0
                        --AND[Last Date Modified] > dateadd(dd, -31, cast(getdate() as date))
                        ORDER BY id";

                List<Recursos> listRecursos = new List<Recursos>();

                command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();
                    
                listRecursos = DataReaderMapToList<Recursos>(reader);

                reader.Close();

                logManager.LogWrite("- Recursos lidos: " + listRecursos.Count +" ");

                //Get All Projects
                List<Projetos> listProjectos = new List<Projetos>();
                sql = @"SELECT [No_] as id,
                    [Description] as descricao,[Description 2] as descricao2,
                    [Creation Date] as datacriacao,[Last Date Modified] as dataalteracao,
                    [Starting Date] as datainicio,[Ending Date] as datafim,[Status] as estado,
                    [Person Responsible] as responsavelObra,
                    [Bill-to Customer No_] as clienteNumero, [Bill-to Name] as clienteNome, 
                    [Bill-to Address] as clienteMorada, [Bill-to Address 2] as clienteMorada2,
                    [Bill-to City] as clienteCidade,[Bill-to County] as clientePais,
                    [Bill-to Post Code] as clienteCodigoPostal, [Bill-to Country_Region Code] as clienteRegiaoCod,
                    [Bill-to Contact] as clienteContato, [Complete] as completa, [Budget Base Bid] as valorOrcamentoBase,
                    [Job Address] as obraMorada1,[Job Address 2] as obraMorada2,
                    [Job City] as obraCidade,[Job Post Code] as obraCodigoPostal,
                    [Job District] as obraDistrito,
                    [Status Date] as dataEstado,[Delivery Date] as dataEntrega
                    FROM [JAF_BC_PRD].[dbo].[JAF$Job]
                    WHERE [Status] IN (2)";

                command = new SqlCommand(sql, conn);
                reader = command.ExecuteReader();

                listProjectos = DataReaderMapToList<Projetos>(reader);

                logManager.LogWrite("- Projectos lidos: " + listRecursos.Count + " ");

                HttpResponseMessage response = null;

                logManager.LogWrite("- A enviar dados");
                response = await PostSendDataNavision(apiServer, listRecursos, listProjectos);
                if (response.IsSuccessStatusCode)
                {
                    logManager.LogWrite("- Resposta: Dados enviados com sucesso. ");
                }
                else
                {
                    logManager.LogWrite("- Resposta: Problemas na importação de dados. ");
                }

                reader.Close();

            }

            logManager.LogWrite("---\tFim da execução do serviço Gestão de Obras");

            //Send Logfile por email
            MailManager mail = new MailManager(smtpServer, smtpSSL, smtpPort, smtpDefaultCredencials, smtpUser, smtpPass);

            string mailSubject = "Serviço de Importação - Log de Importação - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string mailBody = "Envio em anexo do ficheiro de logs diário.";

            //mail.sendLogByMail(smtpUser, mailSendTo, mailSubject, mailBody, logManager.GetLastFileLog());

            return;
        }

        #region Metodos Privados

        private static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        private static async Task<HttpResponseMessage> PostSendDataNavision(String urlApi, List<Recursos> recursos, List<Projetos> projetos)
        {
            DataNavision ydata = new DataNavision();
            ydata.Recursos = recursos;
            ydata.Projetos = projetos;

            var sendData = new
            {
                ydata = new Object[] { ydata }
            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(sendData), Encoding.UTF8, "application/json");
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/json");

                HttpResponseMessage response = (await client.PostAsync(urlApi + "/api/serviceNavision", content));

                return response;
            }
        }

        #endregion
    }
}
