using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Indspire.Soaring.Engagement.Data;
using Indspire.Soaring.Engagement.Database;
using Indspire.Soaring.Engagement.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

namespace Indspire.Soaring.Engagement.Controllers
{
    public class PrintController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrintController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string userNumber = null)
        {
            List<Attendee> attendees = new List<Attendee>();

            if (userNumber != null)
            {
                //Get the user
                var attendee = _context.Attendee.FirstOrDefault(i => i.UserNumber == userNumber);

                if (attendee == null)
                {
                    throw new ApplicationException("Attendee not found.");
                } else
                {
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);
                    attendees.Add(attendee);

                }
            } else
            {
                //assume we are printing ALL attendees
                attendees = _context.Attendee.ToList();
            }



            // Open a new PDF document

            float topPageMargin = Utilities.InchesToPoints(0.3175f);
            float bottomPageMargin = 0;

            float leftPageMargin = Utilities.InchesToPoints(0.24f);
            float rightPageMarign = leftPageMargin;

            float pageMargin = topPageMargin;

            int pageCols = 3;

            var doc = new Document();
            doc.SetPageSize(PageSize.LETTER);
            
            doc.SetMargins(leftPageMargin, rightPageMarign, topPageMargin, bottomPageMargin);
            var memoryStream = new MemoryStream();

            var pdfWriter = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            // Create the Label table

            PdfPTable table = new PdfPTable(pageCols);
            table.WidthPercentage = 100f;
            

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            foreach (var attendee in attendees)
            {
                

                #region Label Construction

                PdfPCell cell = new PdfPCell();

                //cell.VerticalAlignment = Element.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.BLACK;

                cell.BorderColor = BaseColor.WHITE;
                cell.BorderWidthTop = Utilities.InchesToPoints(0.3125f);
                cell.BorderWidthBottom = cell.BorderWidthTop;

                cell.BorderWidthLeft = Utilities.InchesToPoints(0.34f);
                cell.BorderWidthRight = cell.BorderWidthLeft;

                cell.FixedHeight = Utilities.InchesToPoints(2 + (0.3125f * 2));

                var imgContents = new Paragraph();
                var img = Image.GetInstance(QRCodeUtils.GenerateQRCodeAsBytes(attendee.UserNumber));
                imgContents.Add(new Chunk(img, Utilities.InchesToPoints(0.5f), Utilities.InchesToPoints(-1.9f)));
                imgContents.Alignment = Element.ALIGN_BOTTOM;
                cell.AddElement(imgContents);


                var txtContents = new Paragraph();
                txtContents.Add(new Chunk(string.Format("{0}", attendee.UserNumber), new Font(baseFont, 11f, Font.BOLD, BaseColor.WHITE)));
                txtContents.Alignment = Element.ALIGN_CENTER;
                cell.AddElement(txtContents);
               
                table.AddCell(cell);
                #endregion
            }

            table.CompleteRow();
            doc.Add(table);

            // Close PDF document and send

            pdfWriter.CloseStream = false;
            doc.Close();
            memoryStream.Position = 0;

            return File(memoryStream, "application/pdf");
        }
    }
}