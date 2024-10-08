namespace BackendDBAstafiev.Classes.Dto
{
    public class AddColumnDto
    {
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string dataType { get; set; }
    }
}
