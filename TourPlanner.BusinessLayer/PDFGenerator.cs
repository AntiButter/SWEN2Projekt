using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TourPlanner.Logging;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public static class PDFGenerator
    { 
        
        public static void summarizeReport()
        {
            TourGetter tourGetter = new TourGetter();
            string pdfPath = "../../../../PDF/Summary.pdf";

            PdfWriter writer = new PdfWriter(pdfPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph tourLogHeader = new Paragraph("Summary of each Tour:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold();
            document.Add(tourLogHeader);
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.AddHeaderCell(getHeaderCell("Tour"));
            table.AddHeaderCell(getHeaderCell("Average Time"));
            table.AddHeaderCell(getHeaderCell("Average Distance"));
            table.AddHeaderCell(getHeaderCell("Average Rating"));
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);

            IEnumerable<Tour> tours = tourGetter.GetItems();

            foreach (var tour in tours)
            {
                table.AddCell(tour.Name);

                double averageTime = 0;
                double averageDistance = (double)tour.TourDistance;
                double averageRating = 0;
                int logCount = 0;

                foreach(var log in tour.Logs)
                {
                    averageTime += log.TotalTime;
                    averageRating += log.Rating;
                    logCount++;
                }
                
                averageTime = averageTime / logCount;
                averageRating = averageRating / logCount;

                table.AddCell(averageTime.ToString()+" minutes");
                table.AddCell(averageDistance.ToString()+ " km");
                table.AddCell(averageRating.ToString());

                table.StartNewRow();
            }
            
            document.Add(table);

            document.Close();
        } 
        
        public static void tourReport(Tour tour)
        {
            string pdfPath = "../../../../PDF/TourID"+tour.ID+".pdf";
            string picture = "../../../../Pictures/TourID" + tour.ID + ".png";

            PdfWriter writer = new PdfWriter(pdfPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph tourInfoHeader = new Paragraph("Tour Info:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold();
            document.Add(tourInfoHeader);
            List list = new List()
                    .SetSymbolIndent(12)
                    .SetListSymbol("\u2022")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
            list.Add(new ListItem("Tour Name: "+tour.Name))
                    .Add(new ListItem("From: " + tour.From))
                    .Add(new ListItem("To: " + tour.To))
                    .Add(new ListItem("Description: " + tour.Description))
                    .Add(new ListItem("Type: " + tour.TransportType))
                    .Add(new ListItem("Time: " + tour.EstimatedTime + " Hour:Minute:Seconds"))
                    .Add(new ListItem("Distance: " + tour.TourDistance + " km"))
                    .Add(new ListItem("Child Friendliness: " + tour.ChildFriendliness))
                    .Add(new ListItem("Popularity: " + tour.Popularity));
            document.Add(list);

            document.Add(new Paragraph("\n\n"));

            Paragraph tourLogHeader = new Paragraph("Tour Logs:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold();
            document.Add(tourLogHeader);
            Table table = new Table(UnitValue.CreatePercentArray(6)).UseAllAvailableWidth();
            table.AddHeaderCell(getHeaderCell("Log created"));
            table.AddHeaderCell(getHeaderCell("Duration"));
            table.AddHeaderCell(getHeaderCell("Difficulty 1-5"));
            table.AddHeaderCell(getHeaderCell("Rating 1-5"));
            table.AddHeaderCell(getHeaderCell("Comment"));
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);

            if(tour.Logs.Count() != 0)
            {
                foreach(var log in tour.Logs)
                {
                    table.AddCell(log.LogTime);
                    table.AddCell(log.TotalTime.ToString());
                    table.AddCell(log.Difficulty.ToString());
                    table.AddCell(log.Rating.ToString());
                    if(log.Comment != null)
                    {
                        table.AddCell(log.Comment);
                        table.SetFontSize(12);
                    }
                    else
                    {
                        table.AddCell("");
                    }
                    table.StartNewRow();
                }
            }
            document.Add(table);
            
            document.Add(new AreaBreak());

            Paragraph imageHeader = new Paragraph("Route Image:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold();
            document.Add(imageHeader);

            ImageData imageData;

            try
            {
                imageData = ImageDataFactory.Create(picture);
            }
            catch (Exception ex)
            {
                Logger.Error("Picture for Tour report could not be found, using placeholder image instead");
                imageData = ImageDataFactory.Create("../../../../Pictures/filler.PNG");
            }
            document.Add(new Image(imageData));

            document.Close();
        }

        private static Cell getHeaderCell(String s)
        {
            return new Cell().Add(new Paragraph(s)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}
