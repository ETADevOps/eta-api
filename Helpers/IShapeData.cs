using ETA.API.Models;
using ETA.API.ResourceParameter;
using Microsoft.AspNetCore.Mvc;

namespace ETA.API.Helpers
{
    public interface IShapeData
    {
        IEnumerable<LinkDto> CreateLinks(ResourceParameters userResourceParameters, bool hasNext, bool hasPrevious, string apiName, IUrlHelper url);
        string CreateResourceUri(ResourceParameters userResourceParameters, ResourceUriType type, string apiName, IUrlHelper url);
    }
}