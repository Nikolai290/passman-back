namespace passman_back.Infrastructure.Business.Enums {
    public class Export {
        public const string Csv = "csv";
        public const string Xlsx = "xlsx";
        public const string Xls = "xls";
        public const string Json = "json";

        public static string GetContentType(string type) {
            switch (type.ToLower()) {
                case Csv:
                    return "application/csv";
                case Xlsx:
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case Xls:
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case Json:
                    return "application/json";
                default:
                    return type;
            }
        }
    }
}
