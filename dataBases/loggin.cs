using drualcman.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace drualcman
{
    /// <summary>
    /// Management of MS-SQL DataBases
    /// </summary>
    public partial class dataBases 
    {
        internal class defLog : IDisposable
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

            public void Dispose()
            {
                this.date = null;
                this.starttime = null;
                this.function = null;
                this.sql = null;
                this.vars = null;
                this.endtime = null;
                this.error = null;
                this.result = null;
                this.folder = null;
            }

            private void writeLog()
            {
                try
                {
                    loggin Log = new loggin(this.folder, DateTime.Today.ToShortDateString().Replace("/", "") + ".log");
                    Log.date = this.date;
                    Log.starttime = this.starttime;
                    Log.user = "drualcman.databases";
                    Log.function = this.function;
                    Log.sql = this.sql;
                    Log.vars = this.vars;
                    Log.end(this.result, this.error);
                    Log.Dispose();

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
                string _result;
                try
                {
                    _result = Result.ToJson();
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
                    _result = Result.ToJson();
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