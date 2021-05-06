using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using camdochieuduong.Function;
using System.Diagnostics;
using camdochieuduong.Model;

namespace camdochieuduong
{
    public partial class fMainCD : Form
    {
        
        private Model.camdochieuduongEntities camdochieuduongEntity = new Model.camdochieuduongEntities();

        
        public fMainCD()
        {
            InitializeComponent();
            setInit();
            getGrid1();
            
        }
        private void getGrid1()
        {
            dsCamDo.tbCamDo.Clear();
            gridCamDo.DataSource = null;
            var listGiaoDich = camdochieuduongEntity.GiaoDiches.ToList();
            foreach (Model.GiaoDich GD in listGiaoDich)
            {
                if (GD.Canceled != "X")
                {
                    DataRow dr = dsCamDo.tbCamDo.NewRow();
                    dr["IDBienNhan"] = GD.IDBienNhan;
                    dr["KhachHang"] = GD.KhachHang;
                    dr["NgayCam"] = GD.NgayCam;
                    dr["MoTa"] = GD.MoTa;
                    dr["NgayCam"] = GD.NgayCam;
                    dr["DienThoai"] = GD.DienThoai;
                    dr["TruHotCon"] = GD.TruHotCon;
                    dr["TienCam"] = GD.TienCam;
                    dr["GiaTri"] = GD.GiaTri;
                    dr["InBienNhan"] = GD.InBienNhan;
                    dsCamDo.tbCamDo.Rows.Add(dr);
                    gridCamDo.DataSource = dsCamDo.tbCamDo;
                }
            }
        }
        private void setInit()
        {
            var ServerClient = Constants.Server; //Sever/Client app config --important

            txtngaycam.Text = DateTime.Now.ToString();
            txtkhachhang.Text = "";
            txtdienthoai.Text = "";
            txtmota.Text = "";
            txtgiatrimonhang.Text = "0";
            txttiencam.Text = "0";
            txttruhotcon.Text = "";
            txtmadonthaygiay.Text = "";
            gridThayGiay.DataSource = null;
            gridGiaoDich._gridGiaoDich.Clear();
            gridtgHistory1.History.Clear();
            gridtgHistory.DataSource = null;
            btntgChuoc.Enabled = true;
            btntgThayGiay.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btntk1Cancel.Enabled = true;
            //
            txttk1loaigiaodich.Items.Clear();
            txttk1loaigiaodich.Items.Add(Constants.CamDo);
            txttk1loaigiaodich.Items.Add(Constants.ThayGiay);
            txttk1loaigiaodich.Items.Add(Constants.ChuocDo);
            txttk1loaigiaodich.Items.Add(Constants.BaoMat);

            //thuchi
            txttcngaynhap.Text = DateTime.Now.ToString();
            cmbtcthuchi.Items.Clear();
            cmbtcthuchi.Items.Add(Constants.Thu);
            cmbtcthuchi.Items.Add(Constants.Chi);

            if (ServerClient ==  Constants.Server) //Server
            {
                this.Text = this.Text + "(" + Constants.Server +")";
            }
            else //Client
            {
                this.Text = this.Text + "(" + Constants.Client + ")";
                xtraTabControl.TabPages[2].PageVisible = false;
                xtraTabControl.TabPages[3].PageVisible = false;
                xtraTabControl.TabPages[4].PageVisible = false;
                xtraTabControl.TabPages[5].PageVisible = false;
                xtraTabControl.TabPages[6].PageVisible = false;

            }

            //Cau Hinh
            Model.CauHinh CH = camdochieuduongEntity.CauHinhs.Find(Constants.CamDo);
            txtchlaitren10tr.Text = CH.LaiTren10Tr.ToString();
            txtchlaiduoi10tr.Text = CH.LaiDuoi10Tr.ToString();
            txtchsongayapdung.Text = CH.SoNgayApDung.ToString();
            txtchsongaytoihan.Text = CH.SoNgayToiHan.ToString();


        }
        private void setDisable()
        {
            btntgChuoc.Enabled = false;
            btntgThayGiay.Enabled = false;
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            btntk1Cancel.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txttiencam.Text == ""
                || txttiencam.Text == "0"
                || txtgiatrimonhang.Text == ""
                || txtgiatrimonhang.Text == "0"
                || txtkhachhang.Text == ""
                || txtmota.Text == ""
                )
            {
                MessageBox.Show("Điền thiếu thông tin");
            }else
            {
                setDisable();
                //Dang ki ID moi
                var IDBienNhan = camdochieuduong.Function.myFunction.CreateIDBienNhan();
                camdochieuduong.Function.myFunction.CreateGiaoDich(
                                                                    IDBienNhan,
                                                                    DateTime.Now.ToString(),
                                                                    txtkhachhang.Text,
                                                                    txtdienthoai.Text,
                                                                    txtmota.Text,
                                                                    txtgiatrimonhang.Text,
                                                                    txttiencam.Text,
                                                                    txttruhotcon.Text,
                                                                    "",
                                                                    IDBienNhan,
                                                                    Constants.CamDo,
                                                                    "0");
                //Print 
                string a4printer = ConfigurationManager.AppSettings.Get("a4printer");
                myFunction.PrintToPrinterA4(IDBienNhan, a4printer);

                //////In giay nho- Tam thoi disable in giay nho chuc nang cam do
                ////var CamThem = "";
                ////var ThayGiayCho = "";
                ////myFunction.PrintToPrinterA8(IDBienNhan, CamThem, ThayGiayCho);

                //Init screen
                setInit();
                ////Get new data to Grid
                getGrid1();
            }           
        }

        private void txttiencam_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txttiencam_TextChanged(object sender, EventArgs e)
        {
            if (txttiencam.Text != "")
            {
                lbltienchu.Text = camdochieuduong.Function.ConvertMoneyToChar.NumberToText(txttiencam.Text);
            }
            else
            {
                lbltienchu.Text = "";
            }
        }

        private void txttiencam_KeyPress(object sender, KeyPressEventArgs e)
        {
            myFunction.VerifyNumberInputOnly(sender, e);
        }

        private void txtgiatrimonhang_KeyPress(object sender, KeyPressEventArgs e)
        {
            myFunction.VerifyNumberInputOnly(sender, e);
        }

        private void txtdienthoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            myFunction.VerifyNumberInputOnly(sender, e);
        }
        private void txtmadonthaygiay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //Get ID Thay Giay
                Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(txtmadonthaygiay.Text);
                if (GD != null) {

                    bool exists = gridGiaoDich._gridGiaoDich.AsEnumerable().Where(c => c.Field<string>("IDBienNhan").Equals(GD.IDBienNhan)).Count() > 0;
                    if (!exists) //not exist in table display grid
                    {
                        DataRow dr = gridGiaoDich._gridGiaoDich.NewRow();
                        dr["IDBienNhan"] = GD.IDBienNhan;
                        dr["KhachHang"] = GD.KhachHang;
                        //dr["NgayCam"] = DateTime.Parse(GD.NgayCam).Date;
                        dr["NgayCam"] = GD.NgayCam;
                        dr["MoTa"] = GD.MoTa;
                        dr["TienCam"] = GD.TienCam;
                        dr["GiaTri"] = GD.GiaTri;
                        dr["DienThoai"] = GD.DienThoai;
                        dr["TruHotCon"] = GD.TruHotCon;
                        dr["DonGoc"] = GD.DonGoc;
                        DateTime currDate = DateTime.Now.Date;
                        DateTime toDate = GD.NgayCam.Value.Date;
                        double SoNgay = (currDate - toDate).TotalDays + 1;
                        dr["SoNgay"] = SoNgay;
                        double SoTienCam = Convert.ToInt64(GD.TienCam);
                        //Model.CauHinh CH = camdochieuduongEntity.GiaoDiches.FirstOrDefault();
                        double laisuat = 0;
                        if (SoTienCam >= 10000000)
                        { //Lon hon hoac = 10tr, 2%
                            laisuat = Convert.ToDouble(txtchlaitren10tr.Text);
                        }
                        else
                        {
                            laisuat = Convert.ToDouble(txtchlaiduoi10tr.Text);
                        }
                        var roundTienLai = Math.Round((SoNgay * SoTienCam * laisuat / 100 / 1000), 3);
                        dr["TienLai"] = roundTienLai * 1000;
                        gridGiaoDich._gridGiaoDich.Rows.Add(dr);
                        gridThayGiay.DataSource = gridGiaoDich._gridGiaoDich;
                        //clear ma don hang
                        txtmadonthaygiay.Text = "";
                        //Get History
                        GetHistory(GD.DonGoc);
                        //Sum 
                        long Sum = Convert.ToInt64(gridView2.Columns["TienCam"].SummaryItem.SummaryValue.ToString()) + Convert.ToInt64(gridView2.Columns["TienLai"].SummaryItem.SummaryValue.ToString());
                        lblSum.Text = String.Format("{0:n0}", Sum);

                    }

                }
            }

        }

        private void groupControl6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btntgThayGiay_Click(object sender, EventArgs e)
        {
            setDisable();
            //Loop through all row
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                //Check and Update Giao Dich 
                Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(gridView2.GetRowCellValue(i, "IDBienNhan").ToString());
                if (GD.ThayTheBang == null)
                {
                    //Dang ki ID moi
                    var IDBienNhanNew = camdochieuduong.Function.myFunction.CreateIDBienNhan();
                    //Tao Giao Dich Thay giay
                    camdochieuduong.Function.myFunction.CreateGiaoDich(
                    IDBienNhanNew,
                    DateTime.Now.ToString(),
                    gridView2.GetRowCellValue(i, "KhachHang").ToString(),
                    gridView2.GetRowCellValue(i, "DienThoai").ToString(),
                    gridView2.GetRowCellValue(i, "MoTa").ToString(),
                    gridView2.GetRowCellValue(i, "GiaTri").ToString(),
                    gridView2.GetRowCellValue(i, "TienCam").ToString(),
                    gridView2.GetRowCellValue(i, "TruHotCon").ToString(),
                    gridView2.GetRowCellValue(i, "IDBienNhan").ToString(),
                    GD.DonGoc,
                    Constants.ThayGiay,
                    "0");
                    //Update Giao Dich cu
                    GD.ThayTheBang = IDBienNhanNew;
                    GD.TienLai = Convert.ToInt64(gridView2.GetRowCellValue(i, "TienLai").ToString());
                    camdochieuduongEntity.SaveChanges();
                    //In bien nhan moi
                    string a4printer = ConfigurationManager.AppSettings.Get("a4printer");
                    myFunction.PrintToPrinterA4(IDBienNhanNew, a4printer);
                    //////In giay nho Tam thoi disable
                    ////long ChenhLech = Convert.ToInt64(gridView2.GetRowCellValue(i, "TienCam").ToString()) - Convert.ToInt64(GD.TienCam);
                    ////var CamThem = "";
                    ////if (ChenhLech > 0)
                    ////{
                    ////    CamThem = "Thêm " + String.Format("{0:n0}", ChenhLech) + " Đ";
                    ////}else if (ChenhLech < 0)
                    ////{
                    ////    CamThem = "Bớt " + String.Format("{0:n0}", ChenhLech) + " Đ";
                    ////}
                    ////var ThayGiayCho = "Thay cho: " + gridView2.GetRowCellValue(i, "IDBienNhan").ToString();
                    ////myFunction.PrintToPrinterA8(IDBienNhanNew, CamThem, ThayGiayCho);
                }
                else {
                    System.Windows.Forms.MessageBox.Show("Lỗi. Đơn này đã thay giấy rồi!!");
                }

            }
            setInit();

        }

        private void gridView2_RowClick(object sender, RowClickEventArgs e)
        {
            //Lay thong tin Don Goc
            GridView gridView = gridThayGiay.FocusedView as GridView;
            var DonGoc = gridView.GetFocusedRowCellValue("DonGoc").ToString();
            GetHistory(DonGoc);
        }

        private void GetHistory(string DonGoc)
        {
            gridtgHistory1.History.Clear();
            gridtgHistory.DataSource = null;
            var listGiaoDich = camdochieuduongEntity.GiaoDiches.Where(x => x.DonGoc == DonGoc).ToList();
            foreach (Model.GiaoDich GD in listGiaoDich)
            {
                if (GD.Canceled != "X")
                {
                    DataRow dr = gridtgHistory1.History.NewRow();
                    dr["IDBienNhan"] = GD.IDBienNhan;
                    dr["KhachHang"] = GD.KhachHang;
                    dr["NgayCam"] = GD.NgayCam;
                    dr["MoTa"] = GD.MoTa;
                    dr["TienLai"] = GD.TienLai;
                    dr["TienCam"] = GD.TienCam;
                    dr["LoaiGiaoDich"] = GD.LoaiGiaoDich;
                    dr["ThayTheCho"] = GD.ThayTheCho;
                    gridtgHistory1.History.Rows.Add(dr);
                    gridtgHistory.DataSource = gridtgHistory1.History;
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            setInit();
        }

        private void btntgChuoc_Click(object sender, EventArgs e)
        {
            setDisable();
            //Loop through all row
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                //Check and Update Giao Dich 
                Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(gridView2.GetRowCellValue(i, "IDBienNhan").ToString());
                if (GD.DaChuoc == null)
                {
                    //Update Giao Dich Chuoc Do dang lam
                    GD.DaChuoc = "X";
                    GD.LoaiGiaoDich = Constants.ChuocDo;
                    GD.TienLai = Convert.ToInt64(gridView2.GetRowCellValue(i, "TienLai").ToString());
                    camdochieuduongEntity.SaveChanges();

                    //Update cac don cu cua don nay
                    UpdateChuocHistory(gridView2.GetRowCellValue(i, "DonGoc").ToString());
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Lỗi. Đơn này đã chuộc rồi!!");
                }

            }
            setInit();
        }
        private void UpdateChuocHistory(string DonGoc)
        {
            var listGiaoDich = camdochieuduongEntity.GiaoDiches.Where(x => x.DonGoc == DonGoc).ToList();
            foreach (Model.GiaoDich GD in listGiaoDich)
            {
               GD.DaChuoc = "X";
               camdochieuduongEntity.SaveChanges();

            }
        }

        private void btnBaoMat_Click(object sender, EventArgs e)
        {
            setDisable();
            //Loop through all row
            for (int i = 0; i < gridView2.DataRowCount; i++)
            {
                //Check and Update Giao Dich 
                Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(gridView2.GetRowCellValue(i, "IDBienNhan").ToString());
                if (GD.BaoMat == null)
                {
                    //Update Giao Dich cu
                    GD.BaoMat = "X";
                    GD.LoaiGiaoDich = Constants.BaoMat;
                    camdochieuduongEntity.SaveChanges();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Lỗi. Đơn này đã báo mất rồi");
                }

            }
            setInit();
        }

        private void btntkSearch_Click(object sender, EventArgs e)
        {
            gridThongKe.DataSource = null;
            dsThongKe.tbGiaoDich.Clear();
            long tongtiencam = 0;
            long tongtienchuoc = 0;
            long tongtienlai = 0;
            long tongtienthu = 0;
            long tongtienchi = 0;
            long tienconlai = 0;
            var fromdate = txttktungay.Value.Date;
            var todate = txttkdenngay.Value.AddDays(1).Date;
            var listGiaoDich = camdochieuduongEntity.GiaoDiches.Where(x => x.NgayCam >= fromdate && x.NgayCam <= todate).ToList();
            foreach (Model.GiaoDich GD in listGiaoDich)
            {
                DataRow dr = dsThongKe.tbGiaoDich.NewRow();
                dr["IDBienNhan"] = GD.IDBienNhan;
                dr["KhachHang"] = GD.KhachHang;
                dr["NgayCam"] = GD.NgayCam;
                dr["GioCam"] = GD.NgayCam;
                dr["LoaiGiaoDich"] = GD.LoaiGiaoDich;
                dr["TienLai"] = GD.TienLai;
                dr["TienCam"] = GD.TienCam;
                dr["MoTa"] = GD.MoTa;
                dr["TruHotCon"] = GD.TruHotCon;
                dr["DienThoai"] = GD.DienThoai;
                dsThongKe.tbGiaoDich.Rows.Add(dr);

                //summary
                if (GD.LoaiGiaoDich == Constants.CamDo)
                {
                    dr["ThanhToan"] = GD.TienCam;
                    tongtiencam = tongtiencam + Convert.ToInt64(GD.TienCam);
                    tongtienlai = tongtienlai + Convert.ToInt64(GD.TienLai);
                } else if (GD.LoaiGiaoDich == Constants.ChuocDo)
                {
                    dr["ThanhToan"] = GD.TienCam + GD.TienLai;
                    tongtienchuoc = tongtienchuoc + Convert.ToInt64(GD.TienCam) + Convert.ToInt64(GD.TienLai);
                }
                else if (GD.LoaiGiaoDich == Constants.ThayGiay)
                {
                    dr["ThanhToan"] = GD.TienLai;
                    tongtienlai = tongtienlai + Convert.ToInt64(GD.TienLai);
                }
            }
            gridThongKe.DataSource = dsThongKe.tbGiaoDich;

            //ThuChi
            var listThuChi = camdochieuduongEntity.ThuChis.Where(x => x.NgayNhap >= fromdate && x.NgayNhap <= todate).ToList();
            foreach (Model.ThuChi TC in listThuChi)
            {
                if (TC.Cancel != "X" && TC.LoaiGiaoDich == Constants.Thu) {
                    tongtienthu = tongtienthu + Convert.ToInt64(TC.SoTien);
                }
                if (TC.Cancel != "X" && TC.LoaiGiaoDich == Constants.Chi) {
                    tongtienchi = tongtienchi + Convert.ToInt64(TC.SoTien);
                }

            }
            gridThuChi.DataSource = dsThuChi.tbThuChi;

            tienconlai = tongtienchuoc - tongtiencam + tongtienthu - tongtienchi;

            txttktongtienchuoc.Text = String.Format("{0:n0}", tongtienchuoc);
            txttktongtiencam.Text = String.Format("{0:n0}", tongtiencam);
            txttktongtienlai.Text = String.Format("{0:n0}", tongtienlai);
            txttktongtienthu.Text = String.Format("{0:n0}", tongtienthu);
            txttktongtienchi.Text = String.Format("{0:n0}", tongtienchi);
            txttktienconlai.Text = String.Format("{0:n0}", tienconlai);
        }

        private void btntkExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel 2021|*.xlsx";
            saveFileDialog1.Title = "Chọn nơi lưu file";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                gridThongKe.ExportToXlsx(saveFileDialog1.FileName);
                // Open the created XLSX file with the default application.
                Process.Start(saveFileDialog1.FileName);
            }


        }

        private void btntk1Search_Click(object sender, EventArgs e)
        {
            getTimKiem();


        }
        public void getTimKiem()
        {
            gridTimKiem.DataSource = null;
            dsTimKiem.tbTimKiem.Clear();
            var fromdate = txttk1tungay.Value.Date;
            var todate = txttk1denngay.Value.AddDays(1).Date;
            if (txttk1tiencamtu.Text == "")
            {
                txttk1tiencamtu.Text = "0";
            }
            if (txttk1tiencamden.Text == "")
            {
                txttk1tiencamden.Text = "0";
            }
            var tiencamtu = Convert.ToInt64(txttk1tiencamtu.Text);
            var tiencamden = Convert.ToInt64(txttk1tiencamden.Text);
            var listGiaoDich = camdochieuduongEntity.GiaoDiches.Where(x => x.NgayCam >= fromdate && x.NgayCam <= todate
                                                                        && (x.IDBienNhan.Contains(txttk1madonhang.Text) || (txttk1madonhang.Text == ""))
                                                                        && (x.KhachHang.Contains(txttk1khachhang.Text) || (txttk1khachhang.Text == ""))
                                                                        && (x.DienThoai.Contains(txttk1dienthoai.Text) || (txttk1dienthoai.Text == ""))
                                                                        && (x.MoTa.Contains(txttk1mota.Text) || (txttk1mota.Text == ""))
                                                                        && (x.LoaiGiaoDich.Contains(txttk1loaigiaodich.Text) || (txttk1loaigiaodich.Text == ""))
                                                                        && (x.TienCam >= tiencamtu || (txttk1tiencamtu.Text == "0"))
                                                                        && (x.TienCam <= tiencamden || (txttk1tiencamden.Text == "0"))
                                                                        ).ToList();
            foreach (Model.GiaoDich GD in listGiaoDich)
            {
                DataRow dr = dsTimKiem.tbTimKiem.NewRow();
                dr["IDBienNhan"] = GD.IDBienNhan;
                dr["KhachHang"] = GD.KhachHang;
                dr["NgayCam"] = GD.NgayCam;
                dr["LoaiGiaoDich"] = GD.LoaiGiaoDich;
                dr["TienLai"] = GD.TienLai;
                dr["TienCam"] = GD.TienCam;
                dr["MoTa"] = GD.MoTa;
                dr["DienThoai"] = GD.DienThoai;
                if (GD.DaChuoc != "X")
                {
                    dr["TonKho"] = "Còn Hàng";
                }else
                {
                    dr["TonKho"] = "Đã Chuộc";
                }
                //summary
                if (GD.LoaiGiaoDich == Constants.CamDo)
                {
                    dr["ThanhToan"] = GD.TienCam; ;
                }
                else if (GD.LoaiGiaoDich == Constants.ChuocDo)
                {
                    dr["ThanhToan"] = GD.TienCam + GD.TienLai;
                }
                else if (GD.LoaiGiaoDich == Constants.ThayGiay)
                {
                    dr["ThanhToan"] = GD.TienLai;
                }
                dsTimKiem.tbTimKiem.Rows.Add(dr);
            }
            gridTimKiem.DataSource = dsTimKiem.tbTimKiem;
        }

        private void txttk1tiencamtu_KeyPress(object sender, KeyPressEventArgs e)
        {
            myFunction.VerifyNumberInputOnly(sender,e);
        }

        private void txttk1tiencamden_KeyPress(object sender, KeyPressEventArgs e)
        {
            myFunction.VerifyNumberInputOnly(sender, e);
        }

        private void btnkhSearch_Click(object sender, EventArgs e)
        {
            gridKiemHang.DataSource = null;
            dsKiemHang.tbKiemHang.Clear();
            int count = 0;
            long sum = 0;
            
            var fromdate = txtkhtungay.Value.Date;
            var todate = txtkhdenngay.Value.AddDays(1).Date;
            var listGiaoDich = camdochieuduongEntity.GiaoDiches.Where(x => x.NgayCam >= fromdate && x.NgayCam <= todate).ToList();
            foreach (Model.GiaoDich GD in listGiaoDich)
            {  
                if(GD.LoaiGiaoDich == Constants.CamDo && GD.DaChuoc != "X")
                {
                    count++;
                    sum = sum + Convert.ToInt64(GD.TienCam);
                    DateTime NgayCam = GD.NgayCam.Value.Date;
                    DateTime NgayToiHan = NgayCam.AddDays(30);
                    DateTime currDate = DateTime.Now.Date;

                    double SoNgay = (currDate - NgayCam).TotalDays + 1;

                    DataRow dr = dsKiemHang.tbKiemHang.NewRow();
                    dr["IDBienNhan"] = GD.IDBienNhan;
                    dr["KhachHang"] = GD.KhachHang;
                    dr["NgayCam"] = GD.NgayCam;
                    dr["NgayToiHan"] = NgayToiHan;
                    dr["SoNgay"] = SoNgay;
                    dr["LoaiGiaoDich"] = GD.LoaiGiaoDich;
                    dr["TienCam"] = GD.TienCam;
                    dr["MoTa"] = GD.MoTa;
                    dr["DienThoai"] = GD.DienThoai;

                    double SoTienCam = Convert.ToInt64(GD.TienCam);
                    double laisuat = 0;
                    if (SoTienCam >= 10000000)
                    { //Lon hon hoac = 10tr, 2%
                        laisuat = Convert.ToDouble(txtchlaitren10tr.Text);
                    }
                    else
                    {
                        laisuat = Convert.ToDouble(txtchlaiduoi10tr.Text);
                    }
                    var roundTienLai = Math.Round((SoNgay * SoTienCam * laisuat / 100 / 1000), 3);
                    dr["TienLai"] = roundTienLai * 1000;

                    dsKiemHang.tbKiemHang.Rows.Add(dr);
                }   
            }
            gridKiemHang.DataSource = dsKiemHang.tbKiemHang;
            txtkhtongsomon.Text = String.Format("{0:n0}", count);
            txtkhtongtiencam.Text = String.Format("{0:n0}", sum);
        }

        private void btntcSave_Click(object sender, EventArgs e)
        {


            //Save data
            Model.ThuChi thuchi = new Model.ThuChi();

            thuchi.LoaiGiaoDich = cmbtcthuchi.Text;
            thuchi.NgayNhap = DateTime.Parse(txttcngaynhap.Text);
            thuchi.SoTien = Convert.ToInt64(txttcsotien.Text.Replace(",", "")); ;
            thuchi.MoTa = txttcmota.Text;
            camdochieuduongEntity.ThuChis.Add(thuchi);
            camdochieuduongEntity.SaveChanges();
            showThuChi();
        }

        private void btntcSearch_Click(object sender, EventArgs e)
        {
            showThuChi();
        }
        public void showThuChi()
        {
            gridThuChi.DataSource = null;
            dsThuChi.tbThuChi.Clear();
            var fromdate = txttctungay.Value.Date;
            var todate = txttcdenngay.Value.AddDays(1).Date;
            var listThuChi = camdochieuduongEntity.ThuChis.Where(x => x.NgayNhap >= fromdate && x.NgayNhap <= todate).ToList();
            foreach (Model.ThuChi TC in listThuChi)
            {
                DataRow dr = dsThuChi.tbThuChi.NewRow();
                dr["MaThuChi"] = TC.MaThuChi;
                dr["NgayNhap"] = TC.NgayNhap;
                dr["LoaiGiaoDich"] = TC.LoaiGiaoDich;
                dr["SoTien"] = TC.SoTien;
                dr["MoTa"] = TC.MoTa;
                dr["Cancel"] = TC.Cancel;
                dsThuChi.tbThuChi.Rows.Add(dr);

            }
            gridThuChi.DataSource = dsThuChi.tbThuChi;
        }

        private void btntcCancel_Click(object sender, EventArgs e)
        {

            GridView gridView = gridThuChi.FocusedView as GridView;
            var selectId = Convert.ToInt64(gridView.GetFocusedRowCellValue("MaThuChi"));

            //Update Thu Chi Table
            Model.ThuChi TC = camdochieuduongEntity.ThuChis.Find(selectId);
            if (TC.NgayNhap.Value.Date == DateTime.Now.Date) {
                TC.Cancel = "X";
                camdochieuduongEntity.SaveChanges();
                showThuChi();
            }
            else
            {
                MessageBox.Show("Không thể xóa phiếu thu chi ngày quá khứ");
            }

        }

        private void btntk1Print_Click(object sender, EventArgs e)
        {
            GridView gridView = gridTimKiem.FocusedView as GridView;
            var selectId = gridView.GetFocusedRowCellValue("IDBienNhan").ToString();
            string a4printer = ConfigurationManager.AppSettings.Get("a4printer");
            myFunction.PrintToPrinterA4(selectId, a4printer);

            //Update Giao Dich Table
            Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(selectId);
            GD.InBienNhan++;
            camdochieuduongEntity.SaveChanges();
            ////Get new data to Grid
            getGrid1();
            getTimKiem();
        }

        private void btntk1Cancel_Click(object sender, EventArgs e)
        {
            GridView gridView = gridTimKiem.FocusedView as GridView;
            var selectId = gridView.GetFocusedRowCellValue("IDBienNhan").ToString();

            //Update Giao Dich Table
            Model.GiaoDich GD = camdochieuduongEntity.GiaoDiches.Find(selectId);
            if (GD.LoaiGiaoDich == Constants.CamDo)
            {
                GD.Canceled = "X";
                camdochieuduongEntity.SaveChanges();
            }else if (GD.LoaiGiaoDich == Constants.ThayGiay) {
                GD.Canceled = "X";
                camdochieuduongEntity.SaveChanges();
                //reverse thông tin đơn cũ
                Model.GiaoDich GD1 = camdochieuduongEntity.GiaoDiches.Find(GD.ThayTheCho);
                GD1.ThayTheBang = null; //reset thông tin thay giấy
                GD.TienLai = 0; //reset thông tin lãi
                camdochieuduongEntity.SaveChanges();
            } else
            {
                MessageBox.Show("Không thể Huỷ");
            }

            ////Get new data to Grid
            getGrid1();
            getTimKiem();
        }

        private void labelControl11_Click(object sender, EventArgs e)
        {

        }

        private void txtkhdenngay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            Model.CauHinh CH = camdochieuduongEntity.CauHinhs.Find(Constants.CamDo);
            if (CH == null)
            {
                CH.Type = Constants.CamDo;
                CH.LaiTren10Tr = Convert.ToInt64(txtchlaitren10tr.Text);
                CH.LaiDuoi10Tr = Convert.ToInt64(txtchlaiduoi10tr.Text);
                CH.SoNgayApDung = Convert.ToInt16(txtchsongayapdung.Text);
                CH.SoNgayToiHan = Convert.ToInt16(txtchsongaytoihan.Text);
                camdochieuduongEntity.CauHinhs.Add(CH);
            }else
            {
                CH.LaiTren10Tr = Convert.ToDecimal(txtchlaitren10tr.Text);
                CH.LaiDuoi10Tr = Convert.ToDecimal(txtchlaiduoi10tr.Text);
                CH.SoNgayApDung = Convert.ToInt16(txtchsongayapdung.Text);
                CH.SoNgayToiHan = Convert.ToInt16(txtchsongaytoihan.Text);
            }
            camdochieuduongEntity.SaveChanges();
            MessageBox.Show("Lưu Xong");



        }
    }
}
