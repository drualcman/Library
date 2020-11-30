using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml;

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
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.rutaDDBB = null;
                    this.FolderLog = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~dataBases()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Propiedades
        private string RutaDDBB;
        public string rutaDDBB
        {
            get { return RutaDDBB; }
            set
            {
                RutaDDBB = value;
                //asegurarse de que
                if (string.IsNullOrEmpty(RutaDDBB))
                {
                    // crear la cadena de conexion con la base de datos por defecto
                    string source = "sql6001.site4now.net";
                    string catalog = "DB_A33893_drualcman";
                    string user = "DB_A33893_drualcman_admin";
                    string pass = "kW6vT27z";

                    RutaDDBB = cadenaConexion(source, catalog, user, pass);
                }
            }
        }
        private string FolderLog { get; set; }
        private bool ChrControl { get; set; }
        private bool dbControl { get; set; }
        private bool LogError { get; set; }

        #endregion Propiedades

        #region Constructor
        /// <summary>
        /// Contructor por defecto
        /// </summary>
        public dataBases()
        {
            this.ChrControl = true;
            this.FolderLog = string.Empty;
            this.dbControl = true;
            this.rutaDDBB = string.Empty;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor por defecto
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// </summary>
        public dataBases(bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos por defecto
            this.ChrControl = ControlCHR;
            this.rutaDDBB = string.Empty; // cadenaConexion(source, catalog, user, pass);
            this.FolderLog = string.Empty;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        public dataBases(string cadenaConexion, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = string.Empty;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="folder">Folder for loggin</param>
        public dataBases(string cadenaConexion, string folder)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = folder;
            this.ChrControl = true;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        public dataBases(string cadenaConexion, bool ControlCHR, bool dbSecure)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = string.Empty;
            this.ChrControl = ControlCHR;
            this.dbControl = dbSecure;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        public dataBases(string cadenaConexion, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="cadenaConexion">Ruta completa de la base de datos</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        public dataBases(string cadenaConexion, bool dbSecure, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion;
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        public dataBases(string source, string catalog, string user, string pass)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass);
            this.FolderLog = string.Empty;
            this.ChrControl = true;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Constructor con parametros de conexcion
        /// </summary>
        /// <param name="source">Ruta del servidor</param>
        /// <param name="catalog">Base de datos a utilizar</param>
        /// <param name="user">Usuario</param>
        /// <param name="pass">Password</param>
        /// <param name="poolSize">How many connections is allowed</param>
        public dataBases(string source, string catalog, string user, string pass, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize);
            this.FolderLog = string.Empty;
            this.ChrControl = true;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="DDBB">Structura con los parametros de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        public dataBases(ddbbSource DDBB, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion(DDBB);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
        }

        /// <summary>
        /// Contructor con cadena de conexion completa
        /// </summary>
        /// <param name="DDBB">Structura con los parametros de la base de datos</param>
        /// <param name="dbSecure">Habilitar o desabilitar la seguridad de la base de datos</param>
        /// <param name="folderLog">Ruta para el fichero del log</param>
        /// <param name="ControlCHR">Controlar no poder pasar CHR en el string de la consulta</param>
        public dataBases(ddbbSource DDBB, bool dbSecure, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos con la cadena de conexion completa            
            this.rutaDDBB = cadenaConexion(DDBB);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string folderLog, bool ControlCHR, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, bool dbSecure, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, bool dbSecure, string folderLog, bool ControlCHR, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string folderLog, bool ControlCHR, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, bool dbSecure, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, bool dbSecure, string folderLog, bool ControlCHR, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, string folderLog, bool ControlCHR, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, bool persist)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, workstation, packet, persist);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
        public dataBases(string source, string catalog, string user, string pass, string workstation, string packet, bool dbSecure, string folderLog, bool ControlCHR, bool persist, int poolSize)
        {
            // crear la cadena de conexion con la base de datos en funcion los datos
            this.rutaDDBB = cadenaConexion(source, catalog, user, pass, poolSize, workstation, packet, persist);
            this.FolderLog = folderLog;
            this.ChrControl = ControlCHR;
            this.dbControl = true;
            this.dbControl = dbSecure;
            this.LogError = false;
            
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
            // crear la cadena de conexión con la base de datos por defecto
            // conexión por defecto al servidor de Gym4u
            string strCadena = string.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                strCadena += "Data Source=" + source + ";" +
                    "Initial Catalog=" + catalog + ";" +
                    "Persist Security Info=";

                if (security) strCadena += "true";
                else strCadena += "false";

                strCadena += ";User ID=" + user +
                    ";Password=" + pass + ";";
                strCadena += ";Max Pool Size=" + poolSize.ToString() + ";";
                if (!string.IsNullOrEmpty(workstation)) strCadena += "workstation id=" + workstation + ";";
                if (!string.IsNullOrEmpty(packet)) strCadena += "packet size=" + packet + ";";
            }
            return strCadena;
        }
        #endregion

    }


}