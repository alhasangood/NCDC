using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class AppMessages
    {
        public const string ServerError = "الخادم لم يستطع معالجة الطلب، الرجاء المحاولة لاحقا او الاتصال بالدعم الفني";
        public const string noPermission = "ليس لديك الصلاحيات اللازمة لتنفيذ الامر";
    
        public const  string verifyData = "Please verify the data";
        public const  string enteredData = "Please check the entered data";
        public const  string itemSelection = "Please check item selection";
        public const  string notFound = "No data was found for the selected item";


        public const  string alreadyLocked = "This item is already frozen";
        public const  string alreadyUnLocked = "This item is already been Actived";
        public const  string alreadyDeleteded = "This item is already been deleted";


        public const  string Added = "Added was successful";
        public const  string edited = "Edited was successful";
        public const  string locked = "Freeze was successful";
        public const  string unLocked = "Actived was successful";
        public const  string deleteded = "Deleteded was successful";


        public const  string errorOccurred = "An error occurred during storage";

    
    }
}
