using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface ICategoryRepository
    {
        Task<List<CategoryProcModel>> GetCategoryList();
        Task<int> CreateCategory(CreateCategoryDto createCategoryDto, ServiceResponse Obj);
        Task<int> DeleteCategory(DeleteByIdModel deleteCategory);
        Task<CategoryProcModel> GetCategoryById(int categoryId);
        Task<int> UpdateCategory(UpdateCategoryDto updateCategoryDto, ServiceResponse obj);
        Task<List<CategoryOpeningDataProcModel>> GetCategoryOpeningData();
        Task<int> CreateReportCategory(CreateReportCategoryDto createReportCategoryDto, ServiceResponse Obj);
        Task<List<ReportCategoryListProcModel>> GetReportCategoryList();
        Task<ReportCategoryByIdProcModel> GetReportCategoryById(int categoryId);
        Task<int> DeleteReportCategory(DeleteByIdModel deleteReportCategory);
        Task<int> UpdateReportCategory(UpdateReportCategoryDto updateReportCategoryDto, ServiceResponse obj);
        Task<List<SubCategoryOpeningDataProcModel>> GetSubCategoryOpeningData();
        Task<int> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto, ServiceResponse Obj);
        Task<int> UpdateSubCategory(UpdateSubCategoryDto updateSubCategoryDto, ServiceResponse obj);
        Task<SubCategoryByIdProcModel> GetSubCategoryById(int subCategoryId);
        Task<List<SubCategoryListProcModel>> GetSubCategoryList();
        Task<int> DeleteSubCategory(DeleteByIdModel deleteSubCategory);
        Task<List<CategoryGeneOpeningDataProcModel>> GetCategoryGeneOpeningData();
        Task<List<SubCategoryByCategoryIdProcModel>> GetSubCategoryByCategoryId(int categoryId);
        Task<int> DeleteCategoryGeneMapping(DeleteByIdModel deleteGeneSnp);
        Task<List<CategoryGeneMappingListProcModel>> GetCategoryGeneMappingList();
        Task<CategoryGeneMappingByIdProcModel> GetCategoryGeneMappingById(int categoryGeneMappingId);
        Task<int> CreateCategoryGeneMapping(CreateCategoryGeneMappingDto createCategoryGeneMappingDto, ServiceResponse Obj);
        Task<int> UpdateCategoryGeneMappingById(UpdateCategoryGeneMappingByIdDto updateCategoryGeneMappingByIdDto, ServiceResponse obj);
        Task<List<CategoryGeneSnpOpeningDataProcModel>> GetCategoryGeneSnpOpeningData();
        Task<List<CategoryGeneSnpMappingListProcModel>> GetCategoryGeneSnpMappingList();
        Task<CategoryGeneSnpMappingByIdProcModel> GetCategoryGeneSnpMappingById(int categoryGeneSnpMappingId);
        Task<int> CreateCategoryGeneSnpMapping(CreateCategoryGeneSnpMappingDto createCategoryGeneSnpMappingDto, ServiceResponse Obj);
        Task<int> UpdateCategoryGeneSnpMapping(UpdateCategoryGeneSnpMappingDto updateCategoryGeneSnpMappingDto, ServiceResponse obj);
        Task<int> DeleteCategoryGeneSnpMapping(DeleteByIdModel deleteCategoryGeneSnp);
        Task<List<CategoryGeneSnpResultOpeningDataProcModel>> GetCategoryGeneSnpResultOpeningData();
        Task<List<CategoryGeneSnpResultMappingListProcModel>> GetCategoryGeneSnpResultMappingList();
        Task<CategoryGeneSnpResultMappingByIdProcModel> GetCategoryGeneSnpResultMappingById(int categoryGeneSnpResultMappingId);
        Task<int> DeleteCategoryGeneSnpResultMapping(DeleteByIdModel deleteCategoryGeneSnpResult);
        Task<int> CreateCategoryGeneSnpResultMapping(CreateCategoryGeneSnpResult createCategoryGeneSnpResult, ServiceResponse Obj);
        Task<int> UpdateCategoryGeneSnpResultMapping(UpdateCategoryGeneSnpResultDto updateCategoryGeneSnpResultDto, ServiceResponse obj);

    }
}
