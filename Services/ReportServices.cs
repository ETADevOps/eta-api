using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using FastReport;
using FastReport.Export.Pdf;
using FastReport.Fonts;
using FastReport.Table;
using FastReport.Utils;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Drawing;

namespace ETA_API.Services
{
    public class ReportServices : IReportServices
    {
        private IConfiguration _configuration;
        public ReportServices(IConfiguration Configuration)
        {
            _configuration = Configuration ?? throw new ArgumentNullException(nameof(Configuration));
        }

        #region Start

        public async Task<string> GenerateGeneReport(int count, PatientsReportDataModel patientsReportData, List<GeneReportDetails> reportDetails, int reportMasterId, DateTime? generatedDate, int categoryCount, string templateDesign)
        {
            Report report = null;
            PDFExport pdfExport = null;
            string pdfbase64 = "";
            try
            {
                report = await PrepareReportLogic(count, patientsReportData, reportDetails, reportMasterId, generatedDate, categoryCount, templateDesign);

                pdfExport = new PDFExport
                {
                    EmbeddingFonts = true
                };

                byte[] buffer;
                using (var memoryStream = new MemoryStream())
                {
                    //report.Export(pdfExport, File.Create(@"Fonts/test.pdf"));

                    pdfExport.Export(report, memoryStream);
                    buffer = memoryStream.ToArray();
                }
                pdfbase64 = Convert.ToBase64String(buffer);

                return pdfbase64;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                report?.Dispose();
                pdfExport?.Dispose();
            }
        }
        private Task<Report> PrepareReportLogic(int count, PatientsReportDataModel patientsReportData, List<GeneReportDetails> geneReportDetails, int reportMasterId, DateTime? generatedDate, int categoryCount, string templateDesign)
        {
            try
            {
                Report report = new Report();

                List<PatientReportDetails> reportDetails = new List<PatientReportDetails>
                {
                    new PatientReportDetails
                    {
                        PatientName = patientsReportData.ReportBasicInfo.patient_name,
                        Gender = patientsReportData.ReportBasicInfo.gender,
                        DateOfBirth = patientsReportData.ReportBasicInfo.date_of_birth?.ToString("MM/dd/yyyy"),
                        SampleId = patientsReportData.ReportBasicInfo.sample_id,
                        Specimen = patientsReportData.ReportBasicInfo.specimen_type,
                        CollectionDate = patientsReportData.ReportBasicInfo.collection_date?.ToString("MM/dd/yyyy"),
                        ReceivedDate = patientsReportData.ReportBasicInfo.collection_date?.ToString("MM/dd/yyyy"),
                        ReportedDate = generatedDate?.ToString("MM/dd/yyyy"),
                        ProviderName = patientsReportData.ReportBasicInfo.provider_name,
                        ProviderAddress = patientsReportData.ReportBasicInfo.provider_address,
                        ReportLogo = patientsReportData.ReportBasicInfo.client_logo,
                        ReportBgColor = patientsReportData.ReportBasicInfo.report_bg_color,
                        ReportHeadingBgColor = patientsReportData.ReportBasicInfo.report_heading_color,
                        ReportSubHeadingBgColor = patientsReportData.ReportBasicInfo.report_sub_heading_color,
                        ReportBgFontColor = patientsReportData.ReportBasicInfo.report_bg_font_color,
                        ReportHeadingFontColor = patientsReportData.ReportBasicInfo.report_heading_font_color,
                        ReportSubHeadingFontColor = patientsReportData.ReportBasicInfo.report_sub_heading_font_color,
                    }
                };

                List<ReportDetails> reportDetails1 = new List<ReportDetails>
                {
                    new ReportDetails
                    {
                        ReportGenePredisPosition = patientsReportData.ReportDynamicInfo.report_gene_predisposition,
                        ReportImportantTakeways = patientsReportData.ReportDynamicInfo.report_important_takeways
                    }
                };

              

                if (count == 0)
                {
                    if (reportMasterId == 1)
                    {
                        if (templateDesign == "Menopause_design_1")
                        {
                            report.Load($@"Reports/GeneReports/Menopause.frx");
                            
                        }
                    }
                    else
                    {
                        if (templateDesign == "EndoDNA_design_1")
                        {
                            report.Load($@"Reports/GeneReports/EndoDNA_Design1.frx");
                        }
                        else
                        {
                            report.Load($@"Reports/GeneReports/EndoDNA_Design2.frx");
                        }
                    }
                }
                else
                {
                    if (reportMasterId == 1)
                    {
                        report.Load($@"Reports/GeneReports/DynamicGeneReport.frx");
                    }
                    else
                    {
                        report.Load($@"Reports/GeneReports/DynamicEndoDNAGeneReport.frx");
                    }
                }

                List<GeneReportDetails> genotypeDetails = new List<GeneReportDetails>();
               
                if (count != 0)
                {
                    report.RegisterData(geneReportDetails, "genes", 4);

                    report.RegisterData(geneReportDetails, "genotype", 4);

                    if (reportMasterId == 1)
                    {
                        TableCell sfo = report.FindObject("Cell94") as TableCell;
                        sfo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);
                        sfo.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_font_color);

                        TableCell gro = report.FindObject("Cell89") as TableCell;
                        gro.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);
                        gro.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_font_color);

                        TableCell rso = report.FindObject("Cell79") as TableCell;
                        rso.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);
                        rso.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_font_color);

                        TableObject sofbod = report.FindObject("Table6") as TableObject;
                        sofbod.Border.Color = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);

                        TableObject grtbod = report.FindObject("Table1") as TableObject;
                        grtbod.Border.Color = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);

                        TableObject scbbod = report.FindObject("Table5") as TableObject;
                        scbbod.Border.Color = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);

                        //subheadingcolors

                        TableCell blo = report.FindObject("Cell34") as TableCell;
                        blo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        //blo.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell gno = report.FindObject("Cell1") as TableCell;
                        gno.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        gno.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell snpo = report.FindObject("Cell2") as TableCell;
                        snpo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        snpo.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell ygo = report.FindObject("Cell3") as TableCell;
                        ygo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        ygo.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell yro = report.FindObject("Cell4") as TableCell;
                        yro.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        yro.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell yog = report.FindObject("Cell77") as TableCell;
                        yog.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        yog.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell yor = report.FindObject("Cell78") as TableCell;
                        yor.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        yor.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                    }

                    if (reportMasterId == 2)
                    {
                        TableCell sffo = report.FindObject("Cell94") as TableCell;
                        sffo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);
                        sffo.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_font_color);

                        TableCell grro = report.FindObject("Cell89") as TableCell;
                        grro.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);
                        grro.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_font_color);

                        TableCell rsso = report.FindObject("Cell79") as TableCell;
                        rsso.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);
                        rsso.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_heading_font_color);

                        TableObject soffbod = report.FindObject("Table6") as TableObject;
                        soffbod.Border.Color = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);

                        TableObject grrtbod = report.FindObject("Table1") as TableObject;
                        grrtbod.Border.Color = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);

                        TableObject sccbbod = report.FindObject("Table5") as TableObject;
                        sccbbod.Border.Color = StringToColor(patientsReportData.ReportBasicInfo.report_heading_color);

                        //subheadingcolors & SubHeadingFontColor

                        TableCell bllo = report.FindObject("Cell34") as TableCell;
                        bllo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        //bllo.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell gnno = report.FindObject("Cell1") as TableCell;
                        gnno.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        gnno.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell snpho = report.FindObject("Cell2") as TableCell;
                        snpho.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        snpho.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell ygho = report.FindObject("Cell3") as TableCell;
                        ygho.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        ygho.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell yrho = report.FindObject("Cell4") as TableCell;
                        yrho.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        yrho.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell yohg = report.FindObject("Cell77") as TableCell;
                        yohg.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        yohg.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell yohr = report.FindObject("Cell78") as TableCell;
                        yohr.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        yohr.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell rno = report.FindObject("Cell24") as TableCell;
                        rno.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        rno.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);

                        TableCell rio = report.FindObject("Cell25") as TableCell;
                        rio.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_color);
                        rio.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_sub_heading_font_color);


                    }

                    genotypeDetails.AddRange(geneReportDetails);

                    genotypeDetails.RemoveAll(x => x.StudyName == "Study name: “”" && String.IsNullOrEmpty(x.StudyDescription) && String.IsNullOrEmpty(x.ReferenceLink));

                    report.RegisterData(genotypeDetails, "result", 4);

                    if (genotypeDetails.Count == 0)
                    {
                        report.Pages[1].Visible = false;
                    }

                    if (reportMasterId == 2)
                    {
                        var subCategoryReportIntroductions = new List<SubCategoryReportIntroduction>();

                        var patientReportGenesLst = patientsReportData.PatientGenes.GroupBy(x => x.category).ToList();
                        var categoryLst = patientReportGenesLst[categoryCount].ToList();

                        foreach (var item in categoryLst)
                        {
                            SubCategoryReportIntroduction subCategoryReport = new SubCategoryReportIntroduction();
                            subCategoryReport.SubCategoryName = item.sub_category;
                            subCategoryReport.SubCategoryIntroduction = item.report_introduction;

                            subCategoryReportIntroductions.Add(subCategoryReport);
                        }

                        List<SubCategoryReportIntroduction> reportIntros = subCategoryReportIntroductions.DistinctBy(x => x.SubCategoryName).DistinctBy(x => x.SubCategoryName).ToList();

                        report.RegisterData(reportIntros, "intro", 4);
                    }
                }
                else
                {
                    if (reportMasterId == 1)
                    {
                        TextObject hotFlashTextObj = report.FindObject("Text3671") as TextObject;
                        hotFlashTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "2";

                        TextObject vgTextObj = report.FindObject("Text3672") as TextObject;
                        vgTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "6";

                        TextObject orTextObj = report.FindObject("Text3673") as TextObject;
                        orTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "9";

                        TextObject emTextObj = report.FindObject("Text3674") as TextObject;
                        emTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "12";

                        TextObject sdTextObj = report.FindObject("Text3675") as TextObject;
                        sdTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "17";

                        TextObject maTextObj = report.FindObject("Text3676") as TextObject;
                        maTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "20";

                        TextObject cdTextObj = report.FindObject("Text3677") as TextObject;
                        cdTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "23";

                        TextObject wgTextObj = report.FindObject("Text3678") as TextObject;
                        wgTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "26";

                    }
                    else
                    {

                        TextObject anixTextObj = report.FindObject("Text3671") as TextObject;
                        anixTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "2";

                        TextObject cogTextObj = report.FindObject("Text3678") as TextObject;
                        cogTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "5";

                        TextObject drugTextObj = report.FindObject("Text3672") as TextObject;
                        drugTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "8";

                        TextObject dmTextObj = report.FindObject("Text3673") as TextObject;
                        dmTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "12";

                        TextObject moodTextObj = report.FindObject("Text3674") as TextObject;
                        moodTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "14";

                        TextObject painTextObj = report.FindObject("Text3675") as TextObject;
                        painTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "23";

                        TextObject sleepTextObj = report.FindObject("Text3676") as TextObject;
                        sleepTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "27";

                        TextObject thcTextObj = report.FindObject("Text3677") as TextObject;
                        thcTextObj.Hyperlink.Value = patientsReportData.ReportBasicInfo.PageLinkUrl + "33";

                    }

                    if (reportMasterId == 1)
                    {
                        ShapeObject hotFlashshpObj = report.FindObject("Shape4") as ShapeObject;
                        hotFlashshpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject vaginalDrynessObj = report.FindObject("Shape5") as ShapeObject;
                        vaginalDrynessObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject osteoporosisRiskpObj = report.FindObject("Shape6") as ShapeObject;
                        osteoporosisRiskpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject earlyMenopausepObj = report.FindObject("Shape7") as ShapeObject;
                        earlyMenopausepObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject sleepDisturbancepObj = report.FindObject("Shape8") as ShapeObject;
                        sleepDisturbancepObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject moodAlterationspObj = report.FindObject("Shape9") as ShapeObject;
                        moodAlterationspObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject cognitiveDisfunctionpObj = report.FindObject("Shape10") as ShapeObject;
                        cognitiveDisfunctionpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        ShapeObject weightGainpObj = report.FindObject("Shape11") as ShapeObject;
                        weightGainpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                        //FontColor
                        TextObject hotFlashTexttObj = report.FindObject("Text3671") as TextObject;
                        hotFlashTexttObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject vggTextObj = report.FindObject("Text3672") as TextObject;
                        vggTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject orrTextObj = report.FindObject("Text3673") as TextObject;
                        orrTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject emmTextObj = report.FindObject("Text3674") as TextObject;
                        emmTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject sddTextObj = report.FindObject("Text3675") as TextObject;
                        sddTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject maaTextObj = report.FindObject("Text3676") as TextObject;
                        maaTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject cddTextObj = report.FindObject("Text3677") as TextObject;
                        cddTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        TextObject wggTextObj = report.FindObject("Text3678") as TextObject;
                        wggTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);


                    }

                    if (reportMasterId == 2)
                    {
                        if (templateDesign == "EndoDNA_design_1")
                        {
                            ShapeObject anxietypObj = report.FindObject("Shape4") as ShapeObject;
                            anxietypObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject cognitiveObj = report.FindObject("Shape11") as ShapeObject;
                            cognitiveObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject drugDependencepObj = report.FindObject("Shape5") as ShapeObject;
                            drugDependencepObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject drugMetabolismpObj = report.FindObject("Shape6") as ShapeObject;
                            drugMetabolismpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject moodpObj = report.FindObject("Shape7") as ShapeObject;
                            moodpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject pnnpObj = report.FindObject("Shape8") as ShapeObject;
                            pnnpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject sleeppObj = report.FindObject("Shape9") as ShapeObject;
                            sleeppObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject tsepObj = report.FindObject("Shape10") as ShapeObject;
                            tsepObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            //FontColor
                            TextObject anixpTextObj = report.FindObject("Text3671") as TextObject;
                            anixpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject cogpTextObj = report.FindObject("Text3678") as TextObject;
                            cogpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject drugpTextObj = report.FindObject("Text3672") as TextObject;
                            drugpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject dmpTextObj = report.FindObject("Text3673") as TextObject;
                            dmpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject moodpTextObj = report.FindObject("Text3674") as TextObject;
                            moodpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject painpTextObj = report.FindObject("Text3675") as TextObject;
                            painpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject sleeppTextObj = report.FindObject("Text3676") as TextObject;
                            sleeppTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject thcpTextObj = report.FindObject("Text3677") as TextObject;
                            thcpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                        }
                        else
                        {
                            ShapeObject anxietypObj = report.FindObject("Shape4") as ShapeObject;
                            anxietypObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject cognitiveObj = report.FindObject("Shape11") as ShapeObject;
                            cognitiveObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject drugDependencepObj = report.FindObject("Shape5") as ShapeObject;
                            drugDependencepObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject drugMetabolismpObj = report.FindObject("Shape6") as ShapeObject;
                            drugMetabolismpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject moodpObj = report.FindObject("Shape7") as ShapeObject;
                            moodpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject pnnpObj = report.FindObject("Shape8") as ShapeObject;
                            pnnpObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject sleeppObj = report.FindObject("Shape9") as ShapeObject;
                            sleeppObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            ShapeObject tsepObj = report.FindObject("Shape10") as ShapeObject;
                            tsepObj.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            PageHeaderBand pgo = report.FindObject("PageHeader1") as PageHeaderBand;
                            pgo.FillColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_color);

                            //FontColor
                            TextObject anixpTextObj = report.FindObject("Text3671") as TextObject;
                            anixpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject cogpTextObj = report.FindObject("Text3678") as TextObject;
                            cogpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject drugpTextObj = report.FindObject("Text3672") as TextObject;
                            drugpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject dmpTextObj = report.FindObject("Text3673") as TextObject;
                            dmpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject moodpTextObj = report.FindObject("Text3674") as TextObject;
                            moodpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject painpTextObj = report.FindObject("Text3675") as TextObject;
                            painpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject sleeppTextObj = report.FindObject("Text3676") as TextObject;
                            sleeppTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);

                            TextObject thcpTextObj = report.FindObject("Text3677") as TextObject;
                            thcpTextObj.TextColor = StringToColor(patientsReportData.ReportBasicInfo.report_bg_font_color);
                        }
                    }

                    report.RegisterData(reportDetails, "res", 4);
                    report.RegisterData(reportDetails1, "imp", 4);


                }

                report.Compressed = true;

                report.Prepare();

                return Task.FromResult(report);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Color StringToColor(string colorStr)
        {
            TypeConverter cc = TypeDescriptor.GetConverter(typeof(Color));
            var result = (Color)cc.ConvertFromString(colorStr);
            return result;
        }

        private void DisableLabels(object sender, EventArgs e, Report report)
        {
            var textObject = sender as TextObject;
            var val = (string)textObject.Text;

            textObject.Visible = false;
        }
        private void ResultAfterData(object sender, EventArgs e, Report report)
        {
            var textObject = sender as TextObject;
            var val = (string)textObject.Text;
            if (val.ToUpper() == "TT" || val.ToUpper() == "CG")
            {
                textObject.TextColor = Color.FromArgb(230, 0, 0); //System.Drawing.Color.IndianRed;
            }
            else
            {
                textObject.TextColor = System.Drawing.Color.Black;
            }
        }
        #endregion
    }
}
