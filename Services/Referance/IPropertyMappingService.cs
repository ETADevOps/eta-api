namespace ETA.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExistsFor<TSource, TDestination>(string fields);
        string UsersPropertyMapping(string fields);
        string PatientsPropertyMapping(string fields);
        string ProvidersPropertyMapping(string fields);
    }
}