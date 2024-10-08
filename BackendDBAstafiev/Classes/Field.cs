using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BackendDBAstafiev.Classes
{
    public class Field
    {
        public Column Column { get; set; }
        public string Value { get; set; }

        public Field(Column column, string value)
        {
            Column = column;
            Value = value;
        }

        // Метод для валидации значения в зависимости от типа данных столбца
        public bool Validate()
        {
            switch (Column.DataType.ToLower())
            {
                case "int":
                    return int.TryParse(Value, out _);

                case "real":
                    return double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out _);

                case "char":
                    return Value.Length == 1;

                case "string":
                    return true; // Для строки нет необходимости в дополнительной валидации

                case "color":
                    return ValidateColor(Value);

                case "colorinvl":
                    return ValidateColorInterval(Value);

                default:
                    return false;
            }
        }

        // color "(0;0;0)"
        private bool ValidateColor(string value)
        {
            string pattern = @"^\(\d{1,3};\s?\d{1,3};\s?\d{1,3}\)$";
            if (Regex.IsMatch(value, pattern))
            {
                return CheckColorRange(value);
            }
            return false;
        }

        // colorInvl "(0;0;0)-(255;255;255)"
        private bool ValidateColorInterval(string value)
        {
            string pattern = @"^\(\d{1,3};\d{1,3};\d{1,3}\)-\(\d{1,3};\d{1,3};\d{1,3}\)$";
            if (Regex.IsMatch(value, pattern))
            {
                var parts = value.Split('-');
                return CheckColorRange(parts[0]) && CheckColorRange(parts[1]);
            }
            return false;
        }

        private bool CheckColorRange(string color)
        {
            color = color.Trim('(', ')', ' ');
            var values = color.Split(';');
            foreach (var val in values)
            {
                if (!int.TryParse(val, out int num) || num < 0 || num > 255)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
