using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace CP_v2
{
    public class PrintTicket
    {
        public string TicketNumber { get; set; }
        public string CarNumber { get; set; }
        public string Price { get; set; }
        public string ImageTitle { get; set; }
        public string ImageBarCode { get; set; }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int SPACE = 145;
            string title = ImageTitle;
            string barcode = ImageBarCode;
            Graphics g = e.Graphics;
            g.DrawRectangle(Pens.Black, 5, 5, 420, 450);
             
            g.DrawImage(Image.FromFile(title), 50, 7);
            Font fBody = new Font("Lucida Console", 15, FontStyle.Bold);
            Font fBody1 = new Font("Lucida Console", 15, FontStyle.Regular);
            Font rs = new Font("Stencil", 25, FontStyle.Bold);
            Font fTType = new Font("", 150, FontStyle.Bold);
            SolidBrush sb = new SolidBrush(Color.Black); 
            g.DrawString("-------------------------------", fBody1, sb, 10, 120);

            g.DrawString("Date.", fBody, sb, 10, SPACE);
            g.DrawString(DateTime.Now.ToShortDateString(), fBody1, sb, 165, SPACE);

            g.DrawString("Time.", fBody, sb, 10, SPACE + 30);
            g.DrawString(DateTime.Now.ToShortTimeString(), fBody1, sb, 165, SPACE + 30);

            g.DrawString("Ticket No.", fBody, sb, 10, SPACE + 60);
            g.DrawString(TicketNumber, fBody1, sb, 165, SPACE + 60);

            g.DrawString("Car No.", fBody, sb, 10, SPACE + 90);
            g.DrawString(CarNumber, fBody1, sb, 165, SPACE + 90);

            g.DrawString("Rs. " + Price, rs, sb, 10, SPACE + 140); 
            g.DrawImage(Image.FromFile(barcode), 10, SPACE + 200);
            g.DrawString("Helpline No.: +00 00000000", fBody, sb, 10, 465);
        }

       public void DrawTicket()
        {
            PrintDocument pd = new PrintDocument();
            PaperSize ps = new PaperSize("", 475, 550);
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            pd.PrintController = new StandardPrintController();
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DefaultPageSettings.PaperSize = ps;
            pd.Print();
        }
    }
}