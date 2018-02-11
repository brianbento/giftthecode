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
        private static QRCodeData GenerateQRCode(string content)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);

            return qrCodeData;
        }

        public static string GenerateQRCodeAsBase64(string content)
        {
            QRCodeData qrCodeData = GenerateQRCode(content);

            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20);

            return qrCodeImageAsBase64;
        }

        public static byte[] GenerateQRCodeAsBytes(string content, int pixelsPerModule = 4)
        {
            QRCodeData qrCodeData = GenerateQRCode(content);

            var qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(pixelsPerModule);
        }

        public static MemoryStream GenerateLabelsAsPDF(List<AttendeeLabel> labels)
        {
            // Open a new PDF document
            float topPageMargin = Utilities.InchesToPoints(0.335f);
            float bottomPageMargin = topPageMargin;

            float leftPageMargin = Utilities.InchesToPoints(0.32f);
            float rightPageMarign = leftPageMargin;

            float pageMargin = topPageMargin;

            int pageCols = 3;

            var doc = new Document();
            doc.SetPageSize(PageSize.Letter);

            doc.SetMargins(leftPageMargin, rightPageMarign, topPageMargin, bottomPageMargin);
            var memoryStream = new MemoryStream();

            var pdfWriter = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            // Create the Label table

            PdfPTable table = new PdfPTable(pageCols);
            table.WidthPercentage = 100f;


            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            foreach (var attendee in labels)
            {


                #region Label Construction

                PdfPCell cell = new PdfPCell();

                //cell.VerticalAlignment = Element.ALIGN_BOTTOM;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BackgroundColor = BaseColor.Black;

                cell.BorderColor = BaseColor.White;
                cell.BorderWidthTop = Utilities.InchesToPoints(0.28f);
                cell.BorderWidthBottom = cell.BorderWidthTop;

                cell.BorderWidthLeft = Utilities.InchesToPoints(0.3f);
                cell.BorderWidthRight = cell.BorderWidthLeft;

                cell.FixedHeight = Utilities.InchesToPoints(2 + (0.29f * 2));

                var imgContents = new Paragraph();
                var img = Image.GetInstance(QRCodeUtils.GenerateQRCodeAsBytes(attendee.UserNumber));
                imgContents.Add(new Chunk(img, Utilities.InchesToPoints(0.475f), Utilities.InchesToPoints(-1.9f)));
                imgContents.Alignment = Element.ALIGN_BOTTOM;
                cell.AddElement(imgContents);


                var txtContents = new Paragraph();
                txtContents.Add(new Chunk(string.Format("{0}", attendee.UserNumber), new Font(baseFont, 11f, Font.BOLD, BaseColor.White)));
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

            return memoryStream;
        }
    }
}
