// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using Indspire.Soaring.Engagement.Models;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using QRCoder;

    public class QRCodeUtils
    {
        public static string GenerateQRCodeAsBase64(string content)
        {
            var qrCodeData = GenerateQRCode(content);

            var qrCodeImageAsBase64 = string.Empty;

            using (var qrCode = new Base64QRCode(qrCodeData))
            {
                qrCodeImageAsBase64 = qrCode.GetGraphic(20);
            }

            return qrCodeImageAsBase64;
        }

        public static byte[] GenerateQRCodeAsBytes(string content, int pixelsPerModule = 4)
        {
            var qrCodeData = GenerateQRCode(content);

            byte[] qrCodeImageAsByte = null;

            using (var qrCode = new PngByteQRCode(qrCodeData))
            {
                qrCodeImageAsByte = qrCode.GetGraphic(pixelsPerModule);
            }

            return qrCodeImageAsByte;
        }

        public static MemoryStream GenerateLabelsAsPDF(List<AttendeeLabel> labels)
        {
            // Open a new PDF document
            var topPageMargin = Utilities.InchesToPoints(0.335f);
            var bottomPageMargin = topPageMargin;

            var leftPageMargin = Utilities.InchesToPoints(0.32f);
            var rightPageMarign = leftPageMargin;

            var pageMargin = topPageMargin;

            var pageCols = 3;

            var doc = new Document();
            doc.SetPageSize(PageSize.Letter);

            doc.SetMargins(leftPageMargin, rightPageMarign, topPageMargin, bottomPageMargin);
            var memoryStream = new MemoryStream();

            var pdfWriter = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            // Create the Label table
            var table = new PdfPTable(pageCols)
            {
                WidthPercentage = 100f
            };

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            foreach (var attendee in labels)
            {
                var cell = new PdfPCell();

                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.Black;

                cell.BorderColor = BaseColor.White;
                cell.BorderWidthTop = Utilities.InchesToPoints(0.28f);
                cell.BorderWidthBottom = cell.BorderWidthTop;

                cell.BorderWidthLeft = Utilities.InchesToPoints(0.3f);
                cell.BorderWidthRight = cell.BorderWidthLeft;

                cell.FixedHeight = Utilities.InchesToPoints(2 + (0.29f * 2));

                var imgContents = new Paragraph();

                var img = Image.GetInstance(
                    QRCodeUtils.GenerateQRCodeAsBytes(attendee.UserNumber));

                imgContents.Add(new Chunk(
                    img,
                    Utilities.InchesToPoints(0.475f),
                    Utilities.InchesToPoints(-1.9f)));

                imgContents.Alignment = Element.ALIGN_BOTTOM;

                cell.AddElement(imgContents);

                var txtContents = new Paragraph();

                txtContents.Add(new Chunk(
                    string.Format("{0}", attendee.UserNumber),
                    new Font(baseFont, 11f, Font.BOLD, BaseColor.White)));

                txtContents.Alignment = Element.ALIGN_CENTER;
                cell.AddElement(txtContents);

                table.AddCell(cell);
            }

            table.CompleteRow();
            doc.Add(table);

            // Close PDF document and send
            pdfWriter.CloseStream = false;
            doc.Close();
            memoryStream.Position = 0;

            return memoryStream;
        }

        private static QRCodeData GenerateQRCode(string content)
        {
            QRCodeData qrCodeData = null;

            using (var qrGenerator = new QRCodeGenerator())
            {
                qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            }

            return qrCodeData;
        }
    }
}
