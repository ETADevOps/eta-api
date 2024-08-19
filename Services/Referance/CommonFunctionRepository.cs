using AutoMapper;
using ETA.API.ExtensionMethod;
using System.Text;

namespace ETA.API.Services.Referance
{
    public class CommonFunctionRepository : ICommonFunctionRepository
    {
        private readonly IMapper _mapper;
        public StringBuilder GetDynamicQuery(string[] searchField, string searchQuery, Type p)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i <= searchField.Length - 1; i++)
            {
                //bool isMultiplecase = true;
                bool isDateTime = DataReaderExtensionMethod.GetPropertyValue(searchField[i], p) == "DateTime" ? true : false;
                string variableType = DataReaderExtensionMethod.GetPropertyValue(searchField[i], p);
                if (!isDateTime)
                {
                    if (variableType == "Int16")
                    {
                        int value;
                        if (int.TryParse(searchQuery, out value))//added for checking the integer value
                        {
                            if (stringBuilder.Length > 0)
                                stringBuilder.Append("  or ");
                            stringBuilder.Append(String.Format("({0}==(Convert.ToInt16(@0)))", searchField[i]));
                        }
                    }
                    else if (variableType == "Int32")
                    {
                        int value;
                        if (int.TryParse(searchQuery, out value))//added for checking the integer value
                        {
                            if (stringBuilder.Length > 0)
                                stringBuilder.Append("  or ");
                            stringBuilder.Append(String.Format("({0}==(Convert.ToInt32(@0)))", searchField[i]));
                        }
                    }
                    else if (variableType == "Double")
                    {
                        double value;
                        if (double.TryParse(searchQuery, out value))//added for checking the integer value
                        {
                            if (stringBuilder.Length > 0)
                                stringBuilder.Append("  or ");
                            stringBuilder.Append(String.Format("({0}==(Convert.ToDouble(@0)))", searchField[i]));
                        }
                    }
                    else
                    {
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append("  or ");
                        stringBuilder.Append(String.Format("({0}==null ? false:{0}.ToLower().Contains(@0.ToLower()))", searchField[i]));
                    }
                }
                else
                {
                    DateTime date;
                    if (DateTime.TryParseExact(searchQuery, "mm/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
                    {
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append("  or ");
                        stringBuilder.Append(String.Format("({0}==null ? false:{0}==(Convert.ToDateTime(@0)))", searchField[i]));
                    }
                }
            }
            return stringBuilder;
        }
    }
}
