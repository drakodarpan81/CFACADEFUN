using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CFACADECONN;
using CFACADESTRUC;
using Npgsql;

namespace CFACADEFUN
{
    public class CFuncionesGral
    {
        public static string GeneraPassWord(string sUsuario, string sBaseDeDatos)
        {
            string sCadena = "";
            sCadena = sUsuario + "-" + sBaseDeDatos;
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(sCadena));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string consultarsIp()
        {
            IPHostEntry host;
            string sIpLocal = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    sIpLocal = ip.ToString().Replace(".", "").Trim();
                }
            }

            return sIpLocal;
        }

        public static string fConsultarFechaServerPostgres(CEstructura ep, ref string sError)
        {
            string sConsulta = "SELECT DATE(CURRENT_DATE)", sFecha, sFechaFormateada = "";

            NpgsqlConnection conn = new NpgsqlConnection();
            if (CConeccion.conexionPostgre(ep, ref conn, ref sError))
            {
                NpgsqlCommand com = new NpgsqlCommand(sConsulta, conn);
                NpgsqlDataReader reader = com.ExecuteReader();

                if(reader.Read())
                {
                    sFecha = reader[0].ToString();
                    sFechaFormateada = String.Format("{0}-{1}-{2}", sFecha.Substring(0, 2), sFecha.Substring(3, 2), sFecha.Substring(6, 4));
                }
            }
            else
            {
                sError =  string.Format("Se presento un problema al guardar la información: {0}", sError.ToString());
            }


            return sFechaFormateada;
        }

        public static bool fValidarFechasPostgres(CEstructura ep, string sFechaInicial, string sFechaFinal)
        {
            string sConsulta = "", sError = "";
            var nDiasDiferencia = 0;
            bool valorRegresa = true;

            NpgsqlConnection conn = new NpgsqlConnection();
            if (CConeccion.conexionPostgre(ep, ref conn, ref sError))
            {
                sConsulta = String.Format("SELECT fValidar_fechas('{0}'::DATE, '{1}'::DATE)", sFechaFinal, sFechaInicial);
                NpgsqlCommand com = new NpgsqlCommand(sConsulta, conn);
                NpgsqlDataReader reader;

                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    nDiasDiferencia = reader.GetInt32(0);

                    if (nDiasDiferencia < 0)
                    {
                        valorRegresa = false;
                    }
                }
            }

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

            return valorRegresa;
        }
    }
}
