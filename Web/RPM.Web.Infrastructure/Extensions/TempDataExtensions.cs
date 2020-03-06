namespace RPM.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System;
    using System.Collections.Generic;
    using System.Text;

    using static RPM.Common.GlobalConstants;

    public static class TempDataExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[TempDataSuccessKey] = message;
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[TempDataErrorKey] = message;
        }
    }
}
