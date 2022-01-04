using drualcman.Converters.Extensions;
using System;
using System.Threading.Tasks;

namespace drualcman
{
    /// <summary>
    /// Management of MS-SQL DataBases
    /// </summary>
    public partial class dataBases 
    {
        internal class defLog 
        {
            public string date { get; set; }
            public string starttime { get; set; }
            public string function { get; set; }
            public string sql { get; set; }
            public string vars { get; set; }
            public string endtime { get; set; }
            public object result { get; set; }
            public string error { get; set; }
            public string folder { get; set; }

            private void writeLog()
            {
                try
                {
                    Task writeLog = Task.Run(() =>
                    {
                        loggin Log = new loggin(this.folder, DateTime.Today.ToShortDateString().Replace("/", "") + ".log");
                        Log.date = this.date;
                        Log.starttime = this.starttime;
                        Log.user = "drualcman.databases";
                        Log.function = this.function;
                        Log.sql = this.sql;
                        Log.vars = this.vars;
                        Log.end(this.result, this.error);
                    });
                }
                catch
                {
                    return;
                }
            }

            public defLog(string Folder)
            {
                this.date = DateTime.Today.ToShortDateString();
                this.starttime = DateTime.Now.ToShortTimeString();
                this.endtime = DateTime.Now.ToShortTimeString();
                this.sql = string.Empty;
                this.function = string.Empty;
                this.vars = string.Empty;
                this.result = false;
                this.error = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(Folder))
                    {
                        archivos a = new archivos();
                        this.folder = a.checkCarpeta(Folder);
                        a = null;
                    }
                    else
                        this.folder = string.Empty;
                }
                catch
                {
                    this.folder = string.Empty;
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
                end(Result, string.Empty);
            }

            public void end(object Result, object Err)
            {
                string _err;
                try
                {
                    _err = Err.ToString();
                }
                catch
                {
                    _err = string.Empty;
                }
                end(Result, _err);
            }


            public void end(object Result, string Err)
            {
                this.endtime = DateTime.Now.ToShortTimeString();
                this.result = Result.ToJson();
                this.error = Err;
                writeLog();
            }
        }

    }
}