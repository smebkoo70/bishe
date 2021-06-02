using CCWin.SkinClass;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace bishe1.Controllers
{
    public class RandomStr
    {
        public static char[] constant =
        {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };
        public static char[] Numconstant =
        {
            '0','1','2','3','4','5','6','7','8','9'
        };
        public static int[] generatenum = { 10, 10, 10, 10, 10, 10, 20, 20, 20, 20, 50, 50, 50, 100, 100, 200, 500 };
        public static string GenerateRandom(int Length)
        {
            StringBuilder newRandom = new StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {

                System.Threading.Thread.Sleep(10);
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }
        //生成随机密钥
        public static string RandomSecret()
        {
            string secstr = GenerateRandom(6);
            return secstr;
        }

        //生成随机卡号
        public static string RandomCardNumber()
        {
            Random rand = new Random();
            int randnum = rand.Next(100000000, 999999999);
            string numberstr = randnum.ToString();
            return numberstr;

        }

        //生成随机面额
        public static int RandomDenomination()
        {
            Random rand = new Random();
            int randnum = rand.Next(generatenum.Length);
            int miane = generatenum[randnum];
            return miane;
        }

        //生成随机卡号
        public static string RandomAuditID()
        {
            Random rand = new Random();
            int randnum = rand.Next(100000, 999999);
            string numberstr = randnum.ToString();
            string datay = DateTime.Now.Year.ToString();
            string datam = DateTime.Now.Month.ToString();
            string datad = DateTime.Now.Day.ToString();

            string datestr = datay + datam + datad + numberstr;
            return datestr;

        }
    }
}