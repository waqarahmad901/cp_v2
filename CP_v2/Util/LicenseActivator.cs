using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using Microsoft.Win32;

namespace CP_v2.Util
{
    public static class LicenseActivator
    {

        #region  Redegit Key

        public static void MakeRegeditActivationCode()
        {
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Activation");
        }

        public static string ReadRegeditKey()
        {
            try
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Activation", true);
                return myKey.GetValue("ActivationCode").ToString();
            }
            catch
            {
                SetRegeditKeyValue("");
                return null;
            }
        }

        public static void SetRegeditKeyValue(string value)
        {
            MakeRegeditActivationCode();
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Activation", true);
            myKey.SetValue("ActivationCode", value, RegistryValueKind.String);
        }

        #endregion

        #region Get Hardware Information

        public static string GetMacAddress()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                macAddresses = nic.GetPhysicalAddress().ToString();
                break;
            }
            return macAddresses;
        }

        public static string GetCpuId()
        {
            string cpuid = null;
            try
            {
                ManagementObjectSearcher mo = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (var item in mo.Get())
                {
                    cpuid = item["ProcessorId"].ToString();
                }
                return cpuid;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Hash Function

        public static string MyCustomHash(string input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF32.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            int select = 1;
            foreach (byte b in bs)
            {
                switch (select)
                {
                    case 1:
                        s.Append(b.ToString("x4").ToLower());
                        break;
                    case 2:
                        s.Append(b.ToString("x3").ToLower());
                        break;
                    case 3:
                        s.Append(b.ToString("x2").ToLower());
                        break;
                    case 4:
                        s.Append(b.ToString("x1").ToLower());
                        break;
                    default:
                        break;
                }
                select++;
                if (select > 4) select = 1;
            }
            string password = s.ToString();
            return password;
        }

        #endregion

    }
}