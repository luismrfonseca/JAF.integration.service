using System;

namespace JAF.integration.service
{
    class Program
    {
        private static readonly LogManager logManager = new LogManager();
        private static MailManager mailManager = null;
        static int Main()
        {
            string sqlinstance = "";
            string sqluser = "";
            string sqlpass = "";

            logManager.LogWrite("---\tInicio da execução do serviço Gestão de Obras");

            //read config db SQL SERVER for INI FILE
            var MyIni = new IniFile("Settings.ini");
            logManager.LogWrite("- Verificar se as configurações existem no ficheiro de configuração Settings.ini ");
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
                MyIni.Write("password", "Jaf%0017#", "ServerMSSQL");
            }

            if (!MyIni.KeyExists("server", "SMTP"))
            {
                MyIni.Write("server", "smtp.gmail.com", "SMTP");
            }
            if (!MyIni.KeyExists("server", "SMTP"))
            {
                MyIni.Write("server", "smtp.gmail.com", "SMTP");
            }
            if (!MyIni.KeyExists("port", "SMTP"))
            {
                MyIni.Write("port", "smtp.gmail.com", "SMTP");
            }
            if (!MyIni.KeyExists("deliveryMethod", "SMTP"))
            {
                MyIni.Write("deliveryMethod", "false", "SMTP");
            }
            if (!MyIni.KeyExists("username", "SMTP"))
            {
                MyIni.Write("username", "ynb.development@gmail.com", "SMTP");
            }
            if (!MyIni.KeyExists("password", "SMTP"))
            {
                MyIni.Write("password", "within2020", "SMTP");
            }

            logManager.LogWrite("- Leitura das configurações de acesso à base dados do SQL SERVER ");
            sqlinstance = MyIni.Read("instance", "ServerMSSQL");
            sqluser = MyIni.Read("username", "ServerMSSQL");
            sqlpass = MyIni.Read("password", "ServerMSSQL");

            if (sqlinstance.Length <= 0)
            {
                logManager.LogWrite("ERROR - Não foi possivel encontrar a instancia do servidor MSSQL ");
                return -1;
            }
            if (sqluser.Length <= 0)
            {
                logManager.LogWrite("ERROR - Não foi possivel encontrar o username do servidor MSSQL ");
                return -1;
            }
            if (sqlpass.Length <= 0)
            {
                logManager.LogWrite("ERROR - Não foi possivel encontrar a password do servidor MSSQL ");
                return -1;
            }

            logManager.LogWrite("- Leitura das configurações de SMTP ");
            sqlinstance = MyIni.Read("instance", "ServerMSSQL");
            sqluser = MyIni.Read("username", "ServerMSSQL");
            sqlpass = MyIni.Read("password", "ServerMSSQL");


            
            MyIni.Read("server", "SMTP");
            MyIni.Read("port", "SMTP");
            MyIni.Read("deliveryMethod", "SMTP");
            MyIni.Read("username", "SMTP");
            MyIni.Read("password", "SMTP");
            
            //connect to db SQL SERVER

            //Get All Projects

            //Get All Resources



            logManager.LogWrite("---\tFim da execução do serviço Gestão de Obras");

            //Send Logfile por email



            return 0;
        }
    }
}
