//using System;
//using System.Net.Mail;
//using System.Text.RegularExpressions;

//namespace Common
//{
//    public class Validation
//    {
//        public static bool IsValidArabicName(string name)
//        {
//            try
//            {
//                Regex regexObj = new Regex(@"^[\u0621-\u064A ]+$");

//                if (regexObj.IsMatch(name.Trim()))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (FormatException)
//            {
//                return false;
//            }
//        }

//        public static bool IsValidEnglishName(string name)
//        {
//            try
//            {
//                Regex regexObj = new Regex(@"^[a-zA-Z ]+$");

//                if (regexObj.IsMatch(name.Trim()))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (FormatException)
//            {
//                return false;
//            }
//        }

//        public static bool IsValidPhoneNo(string phoneNo)
//        {

//            string[] operatorCodes = new[] { "92", "94", "91", "93", "95", "96" };
//            string[] operatorCodesWithZero = new[] { "092", "094", "091", "093", "095", "096" };

//            if (phoneNo.Length == 9 && phoneNo.All(char.IsDigit) && operatorCodes.Any(phoneNo.StartsWith))
//            {
//                return true;
//            }
//            else if (phoneNo.Length == 10 && phoneNo.All(char.IsDigit) && operatorCodesWithZero.Any(phoneNo.StartsWith))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static bool IsAlmadarPhoneNo(string phoneNo)
//        {

//            string[] operatorCodes = new[] { "91", "93" };
//            string[] operatorCodesWithZero = new[] { "091", "093" };

//            if (phoneNo.Length == 9 && phoneNo.All(char.IsDigit) && operatorCodes.Any(phoneNo.StartsWith))
//            {
//                return true;
//            }
//            else if (phoneNo.Length == 10 && phoneNo.All(char.IsDigit) && operatorCodesWithZero.Any(phoneNo.StartsWith))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public static bool IsValidEmail(string emailaddress)
//        {
//            try
//            {
//                MailAddress m = new MailAddress(emailaddress);

//                return true;
//            }
//            catch (FormatException)
//            {
//                return false;
//            }
//        }

//        public static bool IsValidArabicNameWithNumbers(string name)
//        {
//            try
//            {
//                Regex regexObj = new Regex(@"^[\u0621-\u064A\u0660-\u0669\0-9 ]+$");

//                if (regexObj.IsMatch(name.Trim()))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (FormatException)
//            {
//                return false;
//            }
//        }
//        public static bool IsValidEnglishNameWithNumbers(string name)
//        {
//            try
//            {
//                Regex regexObj = new Regex(@"^[a-zA-Z0-9 ]+$");

//                if (regexObj.IsMatch(name.Trim()))
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (FormatException)
//            {
//                return false;
//            }
//        }

//        public static bool IsValidSimNo(string simNo)
//        {
//            try
//            {
//                if (simNo.Length != 18 && simNo.Length != 19)
//                {
//                    return false;
//                }
//                else
//                {
//                    if (!simNo.StartsWith("8921801"))
//                    {
//                        return false;
//                    }
//                    else
//                    {
//                        return true;
//                    }
//                }
//            }
//            catch (FormatException)
//            {
//                return false;
//            }
//        }

//        public static bool IsValidNationalNo(string NationalNo)
//        {
//            try
//            {
//                if (NationalNo.Length < 12)
//                {
//                    return false;
//                }

//                int Gender = int.Parse(NationalNo.Substring(0, 1));
//                if (Gender != 1 && Gender != 2)
//                {
//                    return false;
//                }

//                int NIDYear = int.Parse(NationalNo.Substring(1, 5));
//                if (NIDYear <= 1800)
//                {
//                    return false;
//                }

//                int Checksum = 0;
//                int WeightSum = 0;
//                int CalculatedChecksum = 0;

//                WeightSum += int.Parse(NationalNo.Substring(0, 1)) * 1;
//                WeightSum += int.Parse(NationalNo.Substring(1, 1)) * 3;
//                WeightSum += int.Parse(NationalNo.Substring(2, 1)) * 7;
//                WeightSum += int.Parse(NationalNo.Substring(3, 1)) * 1;
//                WeightSum += int.Parse(NationalNo.Substring(4, 1)) * 3;
//                WeightSum += int.Parse(NationalNo.Substring(5, 1)) * 7;
//                WeightSum += int.Parse(NationalNo.Substring(6, 1)) * 1;
//                WeightSum += int.Parse(NationalNo.Substring(7, 1)) * 3;
//                WeightSum += int.Parse(NationalNo.Substring(8, 1)) * 7;
//                WeightSum += int.Parse(NationalNo.Substring(9, 1)) * 1;
//                WeightSum += int.Parse(NationalNo.Substring(10, 1)) * 3;

//                Checksum = int.Parse(NationalNo.Substring(11, 1));

//                CalculatedChecksum = (10 - (WeightSum % 10)) % 10;

//                if (CalculatedChecksum == Checksum)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }


//    }
//}
