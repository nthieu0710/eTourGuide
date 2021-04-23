using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTourGuide.Service.Helpers
{

    public static class WorksheetExtension
    {
        public static List<T> ToObject<T>(this ExcelWorksheet worksheet) where T : class
        {
            // List of header mapping with index
            var headers = new Dictionary<string, int>();

            // Get all names of mapping type
            var props = typeof(T).GetProperties();

            // Loop from first colum to last column at the first row
            for (int i = worksheet.Dimension.Start.Column; i <= worksheet.Dimension.End.Column; i++)
            {
                // loop through all properties until find matching property name with current header
                foreach (var prop in props)
                {
                    // if cell value at first row and column number i (header) match property name
                    if (worksheet.Cells[worksheet.Dimension.Start.Row, i].Value.ToString().Equals(prop.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        headers.Add(prop.Name, i);
                        break;
                    }
                }
            }

            // if number of headers is not equals to number of properties, return null
            if (headers.Count != props.Length)
            {
                return null;
            }

            // Init result list for generic type
            var specificListType = typeof(List<>).MakeGenericType(typeof(T));
            var list = Activator.CreateInstance(specificListType) as List<T>;

            //loop all rows except header
            for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                var tmp = Activator.CreateInstance(typeof(T)) as T;

                // loop through all properties and assign value for each property based on header-column mapping
                foreach (var prop in props)
                {
                    var cell = worksheet.Cells[i, headers[prop.Name]];

                    if (cell.Value == null)
                    {
                        prop.SetValue(tmp, null);
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
                    {
                        if (int.TryParse(cell.Value.ToString(), out _))
                        {
                            prop.SetValue(tmp, cell.GetValue<int>());
                        }
                        else
                        {
                            prop.SetValue(tmp, null);
                        }
                    }
                    else if (prop.PropertyType == typeof(decimal))
                    {
                        prop.SetValue(tmp, cell.GetValue<decimal>());
                    }
                    /*else if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(tmp, cell.GetValue<string>());
                    }*/
                    else if (prop.PropertyType == typeof(double))
                    {
                        prop.SetValue(tmp, cell.GetValue<double>());
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        prop.SetValue(tmp, cell.GetValue<DateTime>());
                    }
                    else
                    {
                        prop.SetValue(tmp, cell.Value);
                    }
                }

                list.Add(tmp);
            }

            return list;
        }
    }
}
