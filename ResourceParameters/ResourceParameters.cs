namespace ETA.API.ResourceParameter
{
    public class ResourceParameters
    {
        const int maxPageSize = Int32.MaxValue; //32 bit signed integer which represent the maximum length of a string
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1; //This property is used to represent the current page number in pagination system
        private int _pageSize = 10; // This field is used to store the page size often used in scenarios like pagineation which displays certain number of items per page
        public int PageSize
        {
            get => _pageSize; //It will get the value of the page size
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value; //This code ensures that the 'PageSize' property cannot be set to the values greater than 'maxpageSize'
        }
        public string? OrderBy { get; set; }
        public string? Fields { get; set; }
        public string? SearchFields { get; set; }
    }
}
