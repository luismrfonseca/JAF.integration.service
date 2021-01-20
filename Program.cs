using System;

namespace JAF.integration.service
{
    class Program
    {
        private static readonly LogManager logManager = new LogManager();
        private static MailManager mailManager = null;
        private static string key = "VoI2SycM4QtwFheILl4NZDmXRjZWMAmX";
        static int Main()
        {
            string sqlinstance = "";
            string sqluser = "";
            string sqlpass = "";

            string smtpServer = "";
            bool smtpSSL = false;
            int smtpPort = 0;
            bool smtpDefaultCredencials = true;
            string smtpUser = "";
            string smtpPass = "";

            string mailSendTo = "";

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
                MyIni.Write("password", "within2020", "SMTP");
            }

            if (!MyIni.KeyExists("sendTo", "MAIL"))
            {
                MyIni.Write("sendTo", "ynb.development24@gmail.com", "MAIL");
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
            smtpServer = MyIni.Read("server", "SMTP");
            smtpSSL = MyIni.Read("ssl", "SMTP") == "true";
            smtpPort = Convert.ToInt32(MyIni.Read("port", "SMTP"));
            smtpDefaultCredencials = MyIni.Read("DefaultCredencials", "SMTP") == "true";
            smtpUser = MyIni.Read("username", "SMTP");
            smtpPass = MyIni.Read("password", "SMTP");
            smtpPass = AesOperation.DecryptString(key, smtpPass);

            mailSendTo = MyIni.Read("sendTo", "MAIL");
            //connect to db SQL SERVER

            //Get All Projects

            //Get All Resources



            logManager.LogWrite("---\tFim da execução do serviço Gestão de Obras");

            //Send Logfile por email
            MailManager mail = new MailManager(smtpServer, smtpSSL, smtpPort, smtpDefaultCredencials, smtpUser, smtpPass);

            string mailSubject = "Serviço de Importação - Log de Importação - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string mailBody = "Envio em anexo do ficheiro de logs diário.";

            mail.sendLogByMail(smtpUser, mailSendTo, mailSubject, mailBody, logManager.GetLastFileLog());

            return 0;
        }
    }
}
