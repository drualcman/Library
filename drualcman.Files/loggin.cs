#nullable disable
namespace drualcman
{
    /// <summary>
    /// Summary description for loggin
    /// </summary>
    public class loggin
    {
        public string date { get; set; }
        public string starttime { get; set; }
        public string function { get; set; }
        public string sql { get; set; }
        public string vars { get; set; }
        public string endtime { get; set; }
        public string user { get; set; }
        public string error { get; set; }
        public string info { get; set; }
        public string LogFile { get; set; }
        public string LogFolder { get; set; }

        public loggin()
        {
            this.LogFolder = "";
            this.LogFile = DateTime.Today.ToShortDateString().Replace("/", "");
            ConfigLog();
        }

        public loggin(string file)
        {
            this.LogFolder = "";
            this.LogFile = file;
            ConfigLog();
        }

        public loggin(string folder, string file)
        {
            this.LogFolder = folder;
            this.LogFile = file;
            ConfigLog();
        }

        public void ConfigLog()
        {
            this.date = DateTime.Today.ToShortDateString();
            this.starttime = DateTime.Now.ToShortTimeString();
            this.endtime = DateTime.Now.ToShortTimeString();
            this.sql = string.Empty;
            this.function = string.Empty;
            this.vars = string.Empty;
            this.error = string.Empty;
            //comprobar que el nombre de archivo tiene extension
            if(string.IsNullOrEmpty(archivos.GetFileExtension(this.LogFile))) this.LogFile += ".log";
        }

        private void writeLog()
        {
            try
            {
                const string tag = "|";
                string log = Environment.NewLine + this.date + tag + this.starttime + tag + this.function +
                                tag + (string.IsNullOrEmpty(this.sql) ? "" : this.sql.Replace(Environment.NewLine, " ")) +
                                tag + this.vars + tag + this.endtime + tag + this.user +
                                tag + (string.IsNullOrEmpty(this.error) ? "" : this.error.Replace(Environment.NewLine, " ")) +
                                tag + this.info;

                archivos f = new archivos();
                string file = f.checkCarpeta(this.LogFolder) + this.LogFile;
                if(f.existeFichero(file))
                {
                    //append to actual log
                    using System.IO.StreamWriter z_varocioStreamWriter = new System.IO.StreamWriter(file, true, System.Text.Encoding.UTF8);
                    z_varocioStreamWriter.Write(log);
                    z_varocioStreamWriter.Close();
                }
                else
                {
                    log = "DATE" + tag + "Start Time" + tag + "Function" + tag + "SQL" +
                            tag + "Variables" + tag + "End Time" + tag + "USER" +
                            tag + "Error Trace" + tag + "Info" + tag + log;
                    f.guardaDato(this.LogFile, log, this.LogFolder);
                }
                f = null;
            }
            catch
            {
                return;
            }
        }

        public void start(string Function, string SQL, string Vars)
        {
            this.function = Function;
            this.sql = SQL;
            this.vars = Vars;
        }

        public void register(string Function)
        {
            register(Function, "", "", "");
        }

        public void register(string Function, string SQL)
        {
            register(Function, SQL, "", "");
        }

        public void register(string Function, string SQL, string Vars)
        {
            register(Function, SQL, Vars, "");
        }

        public void register(string Function, string SQL, string Vars, string info)
        {
            this.starttime = DateTime.Now.ToString();
            this.function = Function;
            this.sql = SQL;
            this.vars = Vars;
            end(info);
        }

        public void end(string Result)
        {
            end(Result, string.Empty);
        }

        public void end(object Result)
        {
            string _result;
            try
            {
                _result = Result.ToString();
            }
            catch
            {
                _result = string.Empty;
            }
            end(_result, string.Empty);
        }

        public void end(object Result, object Err)
        {
            string _result;
            string _err;
            try
            {
                _result = Result.ToString();
            }
            catch
            {
                _result = string.Empty;
            }
            try
            {
                _err = Err.ToString();
            }
            catch
            {
                _err = string.Empty;
            }
            end(_result, _err);
        }

        public void end(string Result, string Err)
        {
            this.endtime = DateTime.Now.ToShortTimeString();
            this.error = Err;
            this.info = Result;
            writeLog();
        }
    }
}