using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace camdochieuduong.Function
{
    public static class Constants
    {
        public const string CamDo = "Cầm Đồ";
        public const string ThayGiay = "Thay Giấy";
        public const string ChuocDo = "Chuộc Đồ";
        public const string BaoMat = "Báo Mất Giấy";
        public const string Thu = "Thu";
        public const string Chi = "Chi";
        public const string Server = "Server";
        public const string Client = "Client";

    }
    public class myFunction
    {
        public static void VerifyNumberInputOnly (object sender, KeyPressEventArgs e)
          {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
           }
    public static void CreateGiaoDich(string I_IDBienNhan,
                                   string I_NgayCam,
                                   string I_KhachHang,
                                   string I_DienThoai,
                                   string I_MoTa,
                                   string I_GiaTri,
                                   string I_TienCam,
                                   string I_TruHotCon,
                                   string I_ThayTheCho,
                                   string I_DonGoc,
                                   string I_LoaiGiaoDich,
                                   string I_TienLai)
                                   
        {
            //Save data
            Model.camdochieuduongEntities camdochieuduongEntity = new Model.camdochieuduongEntities();
            Model.GiaoDich giaodich = new Model.GiaoDich();
            giaodich.IDBienNhan = I_IDBienNhan;
            giaodich.NgayCam = DateTime.Parse(I_NgayCam);
            giaodich.KhachHang = I_KhachHang;
            giaodich.MoTa = I_MoTa;
            giaodich.DienThoai = I_DienThoai;
            giaodich.GiaTri = Convert.ToInt64(I_GiaTri.Replace(",", "")); //replace , with blank;
            giaodich.TienCam = Convert.ToInt64(I_TienCam.Replace(",", "")); //replace , with blank;
            giaodich.TruHotCon = I_TruHotCon;
            giaodich.ThayTheCho = I_ThayTheCho;
            giaodich.DonGoc = I_DonGoc;
            giaodich.LoaiGiaoDich = I_LoaiGiaoDich;
            giaodich.InBienNhan = 1;
            giaodich.TienLai = Convert.ToInt64(I_TienLai.Replace(",", ""));
            camdochieuduongEntity.GiaoDiches.Add(giaodich);
            camdochieuduongEntity.SaveChanges();
        }
        public static string CreateIDBienNhan() {
            Model.camdochieuduongEntities camdochieuduongEntity = new Model.camdochieuduongEntities();
            Model.NumberRange nr = camdochieuduongEntity.NumberRanges.Single();
            var currYear = DateTime.Now.Year.ToString();
            var currYear2 = currYear.Substring(currYear.Length - 2);
            var currMonth = DateTime.Now.Month.ToString();
            var nrID = Convert.ToInt32(nr.ID);
            if ((currYear2 == nr.Year.Trim()) && (currMonth == nr.Month.Trim()))
            {
                nrID++;
                nr.ID = nrID.ToString();
            }
            else
            {
                nr.Year = currYear2;
                nr.Month = currMonth;
                nr.ID = "1";
            }
            camdochieuduongEntity.SaveChanges();
            var IDBienNhan = nr.ID + '-' + currMonth + currYear2;
            return IDBienNhan;
        }
        public static void PrintToPrinterA4(string IDBienNhan, string PrinterName)
        {
            //PrintReport(System.Windows.Forms.Application.StartupPath + "\\CrystalReport1.rpt", "Send To OneNote 2010");
            //PrintReport(System.Windows.Forms.Application.StartupPath + "\\CrystalReport1.rpt", "Microsoft Print to PDF");
            var rptPath = "C:\\Users\\ManhTran\\Documents\\Visual Studio 2015\\Projects\\camdochieuduong\\camdochieuduong\\CrystalReport\\rptLapBienNhan.rpt";
            //PrintReport(IDBienNhan, rptPath, "Microsoft Print to PDF");
            PrintReport(IDBienNhan, rptPath, PrinterName);
        }
        public static void PrintReport(string IDBienNhan, string reportPath, string PrinterName)
        {
            Model.camdochieuduongEntities camdochieuduongEntity = new Model.camdochieuduongEntities();
            Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(IDBienNhan);
            var TienCamChu = "";
            if (GD.TienCam != 0)
            {
                TienCamChu = camdochieuduong.Function.ConvertMoneyToChar.NumberToText(GD.TienCam.ToString());
            }
            else
            {
                TienCamChu = "";
            }

            CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc =
                                new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rptDoc.Load(reportPath);

            rptDoc.SetParameterValue("ms", GD.IDBienNhan);
            rptDoc.SetParameterValue("P_KhachHang", GD.KhachHang);
            rptDoc.SetParameterValue("P_DienThoai", GD.DienThoai);
            rptDoc.SetParameterValue("P_MoTa", GD.MoTa);
            rptDoc.SetParameterValue("P_TruHotCon", GD.TruHotCon);
            rptDoc.SetParameterValue("P_TienCam", String.Format("{0:n0}", GD.TienCam));
            rptDoc.SetParameterValue("P_GiaTri", String.Format("{0:n0}", GD.GiaTri));
            rptDoc.SetParameterValue("P_NgayCam", GD.NgayCam.ToString());
            rptDoc.SetParameterValue("P_GioCam", GD.NgayCam.Value.TimeOfDay.ToString());
            rptDoc.SetParameterValue("P_TienCamChu", TienCamChu);
            rptDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            rptDoc.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;
            rptDoc.PrintOptions.PrinterName = PrinterName.Trim();
            rptDoc.PrintToPrinter(1, false, 1, 1);
        }
        public static void PrintToPrinterA8(string IDBienNhan,string CamThem, string ThayGiayCho)
        {
            //PrintReport(System.Windows.Forms.Application.StartupPath + "\\CrystalReport1.rpt", "Send To OneNote 2010");
            //PrintReport(System.Windows.Forms.Application.StartupPath + "\\CrystalReport1.rpt", "Microsoft Print to PDF");
            var rptPath = "C:\\Users\\ManhTran\\Documents\\Visual Studio 2015\\Projects\\camdochieuduong\\camdochieuduong\\CrystalReport\\rptLapBienNhanMini.rpt";
            PrintReportA8(IDBienNhan, rptPath, "Microsoft Print to PDF",CamThem, ThayGiayCho);
        }
        public static void PrintReportA8(string IDBienNhan, string reportPath, string PrinterName, string CamThem, string ThayGiayCho)
        {
            Model.camdochieuduongEntities camdochieuduongEntity = new Model.camdochieuduongEntities();
            Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(IDBienNhan);

            CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc =
                                new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rptDoc.Load(reportPath);
            rptDoc.SetParameterValue("P_IDBienNhan", GD.IDBienNhan);
            rptDoc.SetParameterValue("P_ThayGiayCho", ThayGiayCho);
            rptDoc.SetParameterValue("P_KhachHang", "Tên: " + GD.KhachHang);
            rptDoc.SetParameterValue("P_DienThoai", "SĐT: " + GD.DienThoai);
            rptDoc.SetParameterValue("P_TienCam", "Tiền Cầm: " + String.Format("{0:n0}", GD.TienCam) + " Đ");
            rptDoc.SetParameterValue("P_CamThem", CamThem);
            var MoTa = "Mô Tả: " + GD.MoTa;
            if (GD.TruHotCon != "")
            {
                MoTa = MoTa + "(Còn: " + GD.TruHotCon + ")";
            }
            rptDoc.SetParameterValue("P_MoTa", MoTa);
            rptDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            //PaperSize xCustomSize = new PaperSize("Custom", 6, 8);
            //xCustomSize.PaperName = PaperKind.Custom;
            //PrintDocument.DefaultPageSettings.PaperSize = xCustomSize;
            //rptDoc.PrintOptions.PaperSize = ;
            rptDoc.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            rptDoc.PrintOptions.PrinterName = PrinterName;
            rptDoc.PrintToPrinter(1, false, 1, 1);
        }
    }
}
