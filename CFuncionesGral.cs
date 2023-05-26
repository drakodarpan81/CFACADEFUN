using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
    }
}
