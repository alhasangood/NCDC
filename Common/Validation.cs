using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class Validation
    {
        public static bool IsPassword(string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password)) return true;

                Regex regexObj = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

                return regexObj.IsMatch(password);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsNumber(string number)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(number)) return true;

                Regex regexObj = new Regex(@"^[0-9]+$");

                return regexObj.IsMatch(number);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsLibyanMobile(string phoneNo)
        {
            if (string.IsNullOrWhiteSpace(phoneNo)) return true;

            string[] operatorCodes = new[] { "92", "94", "91", "93", "95", "96" };
            string[] operatorCodesWithZero = new[] { "092", "094", "091", "093", "095", "096" };


            return (phoneNo.All(char.IsDigit), phoneNo.Length, operatorCodes.Any(phoneNo.StartsWith), operatorCodesWithZero.Any(phoneNo.StartsWith)) switch
            {
                (true, 9, true, _) => true,
                (true, 10, _, true) => true,
                _ => false
            };
        }

        public static bool IsAlphaAndSpaces(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return true;

                Regex regexObj = new Regex(@"^[\u0621-\u064Aa-zA-Z ]+$");

                return regexObj.IsMatch(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsArabicAndSpaces(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return true;

                Regex regexObj = new Regex(@"^[\u0621-\u064A ]+$");

                return regexObj.IsMatch(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsEnglishAndSpaces(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return true;

                Regex regexObj = new Regex(@"^[a-zA-Z ]+$");

                return regexObj.IsMatch(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool IsLoginName(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return true;

                Regex regexObj = new Regex(@"^[a-zA-Z]+$");

                return regexObj.IsMatch(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool IsArabicAndNumbers(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return true;

                Regex regexObj = new Regex(@"^[\u0621-\u064A!@#\$%\^\&*\)\(+=._-]+$");

                return regexObj.IsMatch(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsNationalNo(string nationalNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nationalNo)) return true;

                if (nationalNo.Length < 12)
                {
                    return false;
                }

                int Gender = int.Parse(nationalNo.Substring(0, 1));
                if (Gender != 1 && Gender != 2)
                {
                    return false;
                }

                int NIDYear = int.Parse(nationalNo.Substring(1, 5));
                if (NIDYear <= 1800)
                {
                    return false;
                }

                int Checksum = 0;
                int WeightSum = 0;
                int CalculatedChecksum = 0;

                WeightSum += int.Parse(nationalNo.Substring(0, 1)) * 1;
                WeightSum += int.Parse(nationalNo.Substring(1, 1)) * 3;
                WeightSum += int.Parse(nationalNo.Substring(2, 1)) * 7;
                WeightSum += int.Parse(nationalNo.Substring(3, 1)) * 1;
                WeightSum += int.Parse(nationalNo.Substring(4, 1)) * 3;
                WeightSum += int.Parse(nationalNo.Substring(5, 1)) * 7;
                WeightSum += int.Parse(nationalNo.Substring(6, 1)) * 1;
                WeightSum += int.Parse(nationalNo.Substring(7, 1)) * 3;
                WeightSum += int.Parse(nationalNo.Substring(8, 1)) * 7;
                WeightSum += int.Parse(nationalNo.Substring(9, 1)) * 1;
                WeightSum += int.Parse(nationalNo.Substring(10, 1)) * 3;

                Checksum = int.Parse(nationalNo.Substring(11, 1));

                CalculatedChecksum = (10 - (WeightSum % 10)) % 10;

                if (CalculatedChecksum == Checksum)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool IsEnglishAndNumbers(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str)) return true;

                Regex regexObj = new Regex(@"^[a-zA-Z0-9]+$");

                return regexObj.IsMatch(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool IsBase64String(string str)
        {
            try
            {
                str = str.Trim();
                if (string.IsNullOrWhiteSpace(str)) return false;

                Regex regexObj = new Regex(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

                return !((str.Length % 4 == 0) && regexObj.IsMatch(str));
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IsValidAdministrativeNo(string AdministrativeNo)
        {
            try
            {
                if (AdministrativeNo.Length != 10)
                {
                    return false;
                }

                if (!AdministrativeNo.StartsWith("5") && !AdministrativeNo.StartsWith("6"))
                {
                    return false;
                }

                int gender = Convert.ToInt32(AdministrativeNo.Substring(0, 1));
                int year = Convert.ToInt32(AdministrativeNo.Substring(1, 4));
                int serial = Convert.ToInt32(AdministrativeNo.Substring(5, 4));
                int checksum = Convert.ToInt32(AdministrativeNo.Substring(9, 1));

                if (year < 1850 || year > DateTime.Now.Year)
                {
                    return false;
                }

                int? checkDigit = AdministrativeNoCheckDigit(AdministrativeNo.Substring(0, 9));

                if (!checkDigit.HasValue)
                {
                    return false;
                }

                if (checkDigit == checksum)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static int? AdministrativeNoCheckDigit(string AdministrativeNo)
        {
            try
            {
                if (AdministrativeNo.Length != 9)
                {
                    return null;
                }

                int gender = Convert.ToInt32(AdministrativeNo.Substring(0, 1));
                int year = Convert.ToInt32(AdministrativeNo.Substring(1, 4));
                int serial = Convert.ToInt32(AdministrativeNo.Substring(5, 4));

                int presum = 0;
                int tempYear = year;
                int checkDigit;

                while (tempYear != 0)
                {
                    presum = presum + tempYear % 10;
                    tempYear = Convert.ToInt32(Math.Truncate((double)tempYear / 10));
                }

                presum = presum + serial + gender;


                if (presum % 10 > 0)
                {
                    checkDigit = 10 - (presum % 10);
                }
                else
                {
                    checkDigit = 0;
                }

                return checkDigit;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
