using drualcman.Abstractions.Interfaces;
using System.Data;

namespace drualcman
{
    /// <summary>
    /// Management of MS-SQL DataBases
    /// </summary>
    public partial class dataBases : IDisposable
    {
        #region disponse
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    this.DbConnection?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        protected virtual async Task DisposeAsync()
        {
            if(this.DbConnection is not null)
                await this.DbConnection.DisposeAsync();
            Dispose();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Porperties

        private string connectionString;
        public string rutaDDBB
        {
            get { return connectionString; }
            set
            {
                connectionString = value;
                //asegurarse de que
                if(string.IsNullOrEmpty(connectionString))
                {
                    // crear la cadena de conexion con la base de datos por defecto
                    string source = "localhost";
                    string catalog = "default";
                    string user = "sa";
                    string pass = "0123456789";

                    connectionString = cadenaConexion(source, catalog, user, pass);
                }
            }
        }
        public bool LogResults { get; set; }
        #endregion

        #region management variables
        private ddbbSource DbSource;
        private string FolderLog;
        private bool ChrControl;
        private bool dbControl;
        Dictionary<string, object> WhereRequired;
        private IDbLog log;
        #endregion

        #region Constructor
        public void SetLogger(IDbLog logger)
        {
            if(logger == null) log = new defLog(this.FolderLog);
            else log = logger;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(params KeyValuePair<string, object>[] args)
        {
            SetLogger(null);
            this.ChrControl = true;
            this.FolderLog = string.Empty;
            this.dbControl = true;
            this.rutaDDBB = string.Empty;
            this.LogResults = false;
            WhereRequired = args.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Contructor por defecto
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// </summary>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos por defecto
            this.ChrControl = ControlCHR;
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string cadenaConexion, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.ChrControl = ControlCHR;
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="folder">Folder for loggin</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string cadenaConexion, string folder, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = folder;
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string cadenaConexion, bool ControlCHR, bool dbSecure, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string cadenaConexion, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string cadenaConexion, bool dbSecure, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="poolSize">How many connections is allowed</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize);
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="DDBB">Structura con los parametros de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(ddbbSource DDBB, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion(DDBB);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="DDBB">Structura con los parametros de la base de datos</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(ddbbSource DDBB, bool dbSecure, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion(DDBB);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string folderLog, bool ControlCHR, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, bool dbSecure, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }


        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, bool dbSecure, string folderLog, bool ControlCHR, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }


        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string folderLog, bool ControlCHR, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, bool dbSecure, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }


        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, bool dbSecure, string folderLog, bool ControlCHR, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }


        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, string folderLog, bool ControlCHR, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="persist">Enable or not Persis Security Info</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, bool persist, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation, packet, persist);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }


        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="persist">Enable or not Persis Security Info</param>
        /// <param name="poolSize">How many connections</param>
        /// <param name="args">Add Where colmns names and default value to include in all the Select queries from the Models</param>
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, bool persist, int poolSize, params KeyValuePair<string, object>[] args) : this(args)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet, persist);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            SetLogger(null);
        }
        #endregion

        #region Cadenas de conexion
        /// <summary>
        /// Estructura de la cadena de conexion
        /// </summary>
        public struct ddbbSource
        {
            public string SOURCE { get; set; }
            public string CATALOG { get; set; }
            public string USER { get; set; }
            public string PASS { get; set; }
            public string WORKSTATION { get; set; }
            public string PACKET { get; set; }
            public bool SECURITY { get; set; }
            public int POOL_SIZE { get; set; }

            public override string ToString()
            {
                string strCadena = string.Empty;
                if(!string.IsNullOrEmpty(SOURCE))
                {
                    strCadena += "Data Source=" + SOURCE + ";" +
                        "Initial Catalog=" + CATALOG + ";" +
                        "Persist Security Info=";

                    if(SECURITY) strCadena += "true";
                    else strCadena += "false";

                    strCadena += ";User ID=" + USER +
                        ";Password=" + PASS + ";";
                    strCadena += ";Max Pool Size=" + POOL_SIZE.ToString() + ";";
                    if(!string.IsNullOrEmpty(WORKSTATION)) strCadena += "workstation id=" + WORKSTATION + ";";
                    if(!string.IsNullOrEmpty(PACKET)) strCadena += "packet size=" + PACKET + ";";
                }
                return strCadena;
            }
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada 
        /// </summary>
        /// <param name="sourceDDBB"></param>
        /// <returns></returns>
        public string cadenaConexion(ddbbSource sourceDDBB)
        {
            return cadenaConexion(sourceDDBB.SOURCE, sourceDDBB.CATALOG,
                                  sourceDDBB.USER, sourceDDBB.PASS,
                                  sourceDDBB.POOL_SIZE, sourceDDBB.WORKSTATION,
                                  sourceDDBB.PACKET, sourceDDBB.SECURITY);
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, int poolSize)
        {
            return cadenaConexion(source, catalog, user, pass, poolSize, "");
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass)
        {
            return cadenaConexion(source, catalog, user, pass, "");
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, int poolSize, string workstation)
        {
            return cadenaConexion(source, catalog, user, pass, poolSize, workstation, "");
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, string workstation)
        {
            return cadenaConexion(source, catalog, user, pass, workstation, "");
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, int poolSize, string workstation, string packet)
        {
            return cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet, true);
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, string workstation, string packet)
        {
            return cadenaConexion(source, catalog, user, pass, workstation, packet, true);
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="security">Persis Security Info</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, string workstation, string packet, bool security)
        {
            return cadenaConexion(source, catalog, user, pass, 100, workstation, packet, true);
        }

        /// <summary>
        /// Crea la cadena de conexion a la base de datos utilizando los parametros de entrada
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="workstation">Workstation ID</param>
        /// <param name="packet">Packet Size</param>
        /// <param name="security">Persis Security Info</param>
        /// <returns></returns>
        public string cadenaConexion(string source, string catalog, string user, string pass, int poolSize, string workstation, string packet, bool security)
        {
            this.DbSource.SOURCE = source;
            this.DbSource.CATALOG = catalog;
            this.DbSource.USER = user;
            this.DbSource.PASS = pass;
            this.DbSource.POOL_SIZE = poolSize;
            this.DbSource.WORKSTATION = workstation;
            this.DbSource.PACKET = packet;
            this.DbSource.SECURITY = security;

            // crear la cadena de conexión con la base de datos por defecto
            // conexión por defecto al servidor de Gym4u
            string strCadena = string.Empty;
            if(!string.IsNullOrEmpty(source))
            {
                strCadena += "Data Source=" + source + ";" +
                    "Initial Catalog=" + catalog + ";" +
                    "Persist Security Info=";

                if(security) strCadena += "true";
                else strCadena += "false";

                strCadena += ";User ID=" + user +
                    ";Password=" + pass + ";";
                strCadena += ";Max Pool Size=" + poolSize.ToString() + ";";
                if(!string.IsNullOrEmpty(workstation)) strCadena += "workstation id=" + workstation + ";";
                if(!string.IsNullOrEmpty(packet)) strCadena += "packet size=" + packet + ";";
            }
            return strCadena;
        }
        #endregion

        #region methods
        /// <summary>
        /// Setup the parameter for the required where without the constructor
        /// </summary>
        /// <param name="where"></param>
        public void SetWhere(Dictionary<string, object> where)
        {
            this.WhereRequired = where;
        }

        /// <summary>
        /// Setup the parameter for the required where without the constructor
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void SetWhere(string column, object value)
        {
            if(this.WhereRequired.ContainsKey(column))
            {
                this.WhereRequired[column] = value;
            }
            else
            {
                this.WhereRequired.Add(column, value);
            }
        }

        public object GetWhereValue(string key)
        {
            return this.WhereRequired.Where(k => k.Key == key).FirstOrDefault().Value;
        }
        #endregion

        #region DbSource methods
        /// <summary>
        /// Change the catalog name
        /// </summary>
        /// <param name="catalogName"></param>
        public void SetCatalog(string catalogName) => this.DbSource.CATALOG = catalogName;

        public string GetDbSource() => this.DbSource.ToString();
        #endregion
    }


}