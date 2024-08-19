using ETA.API.Models;
using ETA.API.ResourceParameter;
using Microsoft.AspNetCore.Mvc;

namespace ETA.API.Helpers
{
    public class ShapeData : IShapeData
    {
        public IEnumerable<LinkDto> CreateLinks(ResourceParameters userResourceParameters, bool hasNext, bool hasPrevious, string apiName, IUrlHelper url)
        {
            var links = new List<LinkDto>();

            // self 
            links.Add(
               new LinkDto(CreateResourceUri(
                   userResourceParameters, ResourceUriType.Current, apiName, url)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new LinkDto(CreateResourceUri(
                      userResourceParameters, ResourceUriType.NextPage, apiName, url),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateResourceUri(
                        userResourceParameters, ResourceUriType.PreviousPage, apiName, url),
                    "previousPage", "GET"));
            }

            return links;
        }


        public string CreateResourceUri(
ResourceParameters userResourceParameters,
ResourceUriType type, string apiName, IUrlHelper url)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return url.Link(apiName,
                      new
                      {
                          fields = userResourceParameters.Fields,
                          orderBy = userResourceParameters.OrderBy,
                          pageNumber = userResourceParameters.PageNumber - 1,
                          pageSize = userResourceParameters.PageSize,
                          searchQuery = userResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return url.Link(apiName,
                      new
                      {
                          fields = userResourceParameters.Fields,
                          orderBy = userResourceParameters.OrderBy,
                          pageNumber = userResourceParameters.PageNumber + 1,
                          pageSize = userResourceParameters.PageSize,
                          searchQuery = userResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return url.Link(apiName,
                    new
                    {
                        fields = userResourceParameters.Fields,
                        orderBy = userResourceParameters.OrderBy,
                        pageNumber = userResourceParameters.PageNumber,
                        pageSize = userResourceParameters.PageSize,
                        searchQuery = userResourceParameters.SearchQuery
                    });
            }

        }
    }
}
