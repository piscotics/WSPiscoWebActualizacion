using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PiscoWebActualizacion.App_Code
{
    public class AdasysConnection
    {
        private FbCommand _Comand;
        private FbDataReader _DataReader;
        private FbConnection _Conect;
        private FbConnection _ConectPrincipal;
        private string StrCadena;
        // private FbCommand comando;

        public string StrCadena1
        {
            get { return StrCadena; }
            set { StrCadena = value; }
        }
        public FbCommand Comand
        {
            get { return _Comand; }
            set { _Comand = value; }
        }
        public FbConnection Conect
        {
            get { return _Conect; }
            set { _Conect = value; }
        }
        public FbDataReader DataReader
        {
            get { return _DataReader; }
            set { _DataReader = value; }
        }
        public void FbConeccion()
        {
            _Conect = new FbConnection();
          //  _Conect.ConnectionString = "User=SYSDBA;password=masterkey;DataSource=localhost;Database=BDPISCO;Charset=NONE;Dialect=3;Max Pool Size=1024;";
          //_Conect.ConnectionString = "User=SYSDBA;password=masterkey;DataSource=localhost;Database=190.85.119.106:D:\\AdaSysDatos\\FUNERARIAS.FDB;Charset=NONE;Dialect=3;Max Pool Size=1024;";
           _Conect.ConnectionString = "User=SYSDBA;password=masterkey;DataSource=localhost;port=3050;Database=190.85.119.106:D:\\AdaSysDatos\\FUNERARIAS.FDB;Charset=NONE;Dialect=3;Max Pool Size=1024;";
        }

        public void FbConeccionPrincipal()
        {
            _Conect = new FbConnection();
             _Conect.ConnectionString = "User=SYSDBA;password=masterkey;DataSource=localhost;Database=BDPISCO;Charset=NONE;Dialect=3;Max Pool Size=1024;";
            //_Conect.ConnectionString = "User=SYSDBA;password=masterkey;DataSource=localhost;Database=190.85.119.106:D:\\AdaSysDatos\\FUNERARIAS.FDB;Charset=NONE;Dialect=3;Max Pool Size=1024;";
           // _Conect.ConnectionString = "User=SYSDBA;password=masterkey;DataSource=localhost;port=3050;Database=190.85.119.106:D:\\AdaSysDatos\\FUNERARIAS.FDB;Charset=NONE;Dialect=3;Max Pool Size=1024;";
        }

        public void FbCierraConectUpdate()
        {
            _Conect.Close();

        }


        public DataSet FbGetData(string strProcedure)
        {

            DataSet dsData = new DataSet();

            _Comand = new FbCommand();
            _Comand.Connection = _Conect;
            _Comand.CommandType = CommandType.StoredProcedure;
            _Comand.CommandText = strProcedure;

            FbDataAdapter fbdtAdapter = new FbDataAdapter(_Comand);
            fbdtAdapter.Fill(dsData);
            return dsData;
        }
        //cierra una coneccion que tiene activo un data reader
        public void FbCierraConect()
        {
            _Conect.Close();
            _DataReader.Close();
        }

        public void StParameters(string parameter, string values)
        {
            _Comand.Parameters.Add(new FbParameter("@" + parameter, values));
        }

        //crea el store procedure
        #region Permite iniciar un store procedure
        public void StProcedure(string Procedure)
        {
            try
            {
                _Conect.Open();
                _Comand = new FbCommand();
                _Comand.Connection = _Conect;
                _Comand.CommandType = CommandType.StoredProcedure;
                _Comand.CommandText = Procedure;
            }
            catch
            {
            }
        }
        #endregion

        public void ExecuteProcedure()
        {
            _DataReader = _Comand.ExecuteReader();
        }

        //realiza una Insercion sobre la base de datos
        public void FbInsert()
        {
            _Comand = new FbCommand(this.StrCadena, _Conect);
            _Conect.Open();
            _Comand.ExecuteNonQuery();
        }

        //Realiza un Update sobre la base de datos
        public void FbUpdate()
        {
            _Comand = new FbCommand(this.StrCadena, _Conect);
            _Conect.Open();
            Comand.ExecuteNonQuery();

        }

        public void ExecuteProcedureBusq()
        {
            _Comand.ExecuteNonQuery();
            _Conect.Close();

        }
    }

}