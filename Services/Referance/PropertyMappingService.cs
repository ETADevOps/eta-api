using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<UsersMapDto, UsersStoreProcModel>(_usersListPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<PatientsDto, PatientsProcModel>(_patientListPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<ProvidersDto, ProvidersProcModel>(_providerListPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<PatientGeneDumpModel, PatientGeneDumpProcModel>(_geneDumpPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<PatientReportDetailsUrl, PatientReportDetailsModel>(_urlPropertyMapping));

        }

        private Dictionary<string, PropertyMappingValue> _usersListPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) // we are telling that strings key are case insensitive
        {
            {"puser_id", new PropertyMappingValue(new List<string>() {"puser_id"})},
            {"pfirst_name", new PropertyMappingValue(new List<string>() {"pfirst_name"})},
            {"plast_name", new PropertyMappingValue(new List<string>() {"plast_name"})},
            {"puser_name", new PropertyMappingValue(new List<string>() {"puser_name"})},
            {"paddress", new PropertyMappingValue(new List<string>() {"paddress"})},
            {"pcity", new PropertyMappingValue(new List<string>() {"pcity"})},
            {"pstate", new PropertyMappingValue(new List<string>() {"pstate"})},
            {"pzip", new PropertyMappingValue(new List<string>() {"pzip"})},
            {"pphone", new PropertyMappingValue(new List<string>() {"pphone"})},
            {"pemail", new PropertyMappingValue(new List<string>() {"pemail"})},
            {"pstatus", new PropertyMappingValue(new List<string>() {"pstatus"})}
        };
        public string UsersPropertyMapping(string SearchFields)
        {
            string mapplingValue = string.Empty;

            switch (SearchFields)
            {

                case "UserId":
                    mapplingValue = "puser_id";
                    break;
                case "FirstName":
                    mapplingValue = "pfirst_name";
                    break;
                case "LastName":
                    mapplingValue = "plast_name";
                    break;
                case "UserName":
                    mapplingValue = "puser_name";
                    break;
                case "Address":
                    mapplingValue = "paddress";
                    break;
                case "City":
                    mapplingValue = "pcity";
                    break;
                case "State":
                    mapplingValue = "pstate";
                    break;
                case "Zip":
                    mapplingValue = "pzip";
                    break;
                case "Phone":
                    mapplingValue = "pphone";
                    break;
                case "Email":
                    mapplingValue = "pemail";
                    break;
                case "Status":
                    mapplingValue = "pstatus";
                    break;

                default:
                    mapplingValue = "Not Mentioned";
                    break;
            }
            return mapplingValue;
        }

        private Dictionary<string, PropertyMappingValue> _patientListPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"pfirst_name", new PropertyMappingValue(new List<string>() {"pfirst_name"})},
                {"plast_name", new PropertyMappingValue(new List < string >() {"plast_name"}) },
                {"pgender", new PropertyMappingValue(new List < string >() {"pgender"}) },
                {"pdate_of_birth", new PropertyMappingValue(new List < string >() {"pdate_of_birth"}) },
                {"paddress", new PropertyMappingValue(new List < string >() {"paddress"}) },
                {"pcity", new PropertyMappingValue(new List < string >() {"pcity"}) },
                {"pstate", new PropertyMappingValue(new List < string >() {"pstate"}) },
                {"pzip", new PropertyMappingValue(new List < string >() {"pzip"}) },
                {"pspecimen_type", new PropertyMappingValue(new List < string >() {"pspecimen_type"}) },
                {"pcollection_method", new PropertyMappingValue(new List < string >() {"pcollection_method"}) },
                {"pcollection_date", new PropertyMappingValue(new List < string >() {"pcollection_date"}) },
                {"pprovider_id", new PropertyMappingValue(new List < string >() {"pprovider_id"}) },
            };
        public string PatientsPropertyMapping(string SearchFields)
        {
            string mapplingValue = string.Empty;

            switch (SearchFields)
            {
                case "FirstName":
                    mapplingValue = "pfirst_name";
                    break;
                case "LastName":
                    mapplingValue = "plast_name";
                    break;
                case "Gender":
                    mapplingValue = "pgender";
                    break;
                case "DateOfBirth":
                    mapplingValue = "pdate_of_birth";
                    break;
                case "Address":
                    mapplingValue = "paddress";
                    break;
                case "City":
                    mapplingValue = "pcity";
                    break;
                case "State":
                    mapplingValue = "pstate";
                    break;
                case "Zip":
                    mapplingValue = "pzip";
                    break;
                case "SpecimenType":
                    mapplingValue = "pspecimen_type";
                    break;
                case "CollectionMethod":
                    mapplingValue = "pcollection_method";
                    break;
                case "CollectionDate":
                    mapplingValue = "pcollection_date";
                    break;
                case "ProviderId":
                    mapplingValue = "pprovider_id";
                    break;

                default:
                    mapplingValue = "Not Mentioned";
                    break;
            }
            return mapplingValue;
        }
        private Dictionary<string, PropertyMappingValue> _providerListPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"pprovider_id", new PropertyMappingValue(new List < string >() {"pprovider_id"}) },
                {"pprovider_name", new PropertyMappingValue(new List < string >() {"pprovider_name"}) },
                {"paddress", new PropertyMappingValue(new List < string >() {"paddress"}) },
                {"pcity", new PropertyMappingValue(new List < string >() {"pcity"}) },
                {"pstate", new PropertyMappingValue(new List < string >() {"pstate"}) },
                {"pzip", new PropertyMappingValue(new List < string >() {"pzip"}) },
                {"pphone", new PropertyMappingValue(new List < string >() {"pphone"}) },
                {"pemail", new PropertyMappingValue(new List < string >() {"pemail"}) },
                {"pstatus", new PropertyMappingValue(new List < string >() {"pstatus"}) },
            };
        public string ProvidersPropertyMapping(string SearchFields)
        {
            string mapplingValue = string.Empty;

            switch (SearchFields)
            {
                case "Providername":
                    mapplingValue = "pprovider_name";
                    break;
                case "Address":
                    mapplingValue = "paddress";
                    break;
                case "City":
                    mapplingValue = "pcity";
                    break;
                case "State":
                    mapplingValue = "pstate";
                    break;
                case "Zip":
                    mapplingValue = "pzip";
                    break;
                case "Phone":
                    mapplingValue = "pphone";
                    break;
                case "Email":
                    mapplingValue = "pemail";
                    break;
                default:
                    mapplingValue = "Not Mentioned";
                    break;
            }
            return mapplingValue;
        }
        private Dictionary<string, PropertyMappingValue> _geneDumpPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) // we are telling that strings key are case insensitive
        {
            {"psample_id", new PropertyMappingValue(new List<string>() {"psample_id"})},
            {"psnp_name", new PropertyMappingValue(new List<string>() {"psnp_name"})},
            {"pallele1", new PropertyMappingValue(new List<string>() {"pallele1"})},
            {"pallele2", new PropertyMappingValue(new List<string>() {"pallele2"})}
        };

        public string GeneDumpPropertyMapping(string SearchFields)
        {
            string mapplingValue = string.Empty;

            switch (SearchFields)
            {
                case "SampleId":
                    mapplingValue = "psample_id";
                    break;
                case "SnpName":
                    mapplingValue = "psnp_name";
                    break;
                case "Allele1":
                    mapplingValue = "pallele1";
                    break;
                case "Aallele2":
                    mapplingValue = "pallele2";
                    break;
                default:
                    mapplingValue = "Not Mentioned";
                    break;
            }
            return mapplingValue;
        }
        private Dictionary<string, PropertyMappingValue> _urlPropertyMapping =
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase) // we are telling that strings key are case insensitive
        {

            {"pgene_file_url", new PropertyMappingValue(new List<string>() {"pgene_file_url"})},
            {"preport_file_url", new PropertyMappingValue(new List<string>() {"preport_file_url"})}
        };

        public string UrlPropertyMapping(string SearchFields)
        {
            string mapplingValue = string.Empty;

            switch (SearchFields)
            {
                case "PatientGeneFileUrl":
                    mapplingValue = "pgene_file_url";
                    break;
                case "PatientReportUrl":
                    mapplingValue = "preport_file_url";
                    break;
                default:
                    mapplingValue = "Not Mentioned";
                    break;
            }
            return mapplingValue;
        }
    }
}