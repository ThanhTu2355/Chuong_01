using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BT01
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet();
        DataTable tblKhoa = new DataTable("KHOA");
        DataTable tblSinhVien = new DataTable("SINHVIEN");
        DataTable tblKetQua = new DataTable("KETQUA");
        int stt = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Tạo cấu trúc các Datatable
            TaoCauTrucCacBang();
            MocNoiQuanHeCacBang();
            NhapDuLieuCacBang();
            KhoiTaoComboKhoa();
            btnDau.PerformClick();
        }

        private void TaoCauTrucCacBang()
        {
            //Tạo cấu trúc cho DataTable tương ứng với bảng KHOA
            tblKhoa.Columns.Add("MaKH", typeof(string));
            tblKhoa.Columns.Add("TenKH", typeof(string));
            //Tạo khoá chính cho tblKHOA
            tblKhoa.PrimaryKey = new DataColumn[] { tblKhoa.Columns["MaKH"] };

            //Tạo cấu trúc cho DataTable tương ứng với bảng SINHVIEN
            tblSinhVien.Columns.Add("MaSV", typeof(string));
            tblSinhVien.Columns.Add("HoSV", typeof(string));
            tblSinhVien.Columns.Add("TenSV", typeof(string));
            tblSinhVien.Columns.Add("Phai", typeof(Boolean));
            tblSinhVien.Columns.Add("NgaySinh", typeof(DateTime));
            tblSinhVien.Columns.Add("NoiSinh", typeof(string));
            tblSinhVien.Columns.Add("MaKH", typeof(string));
            tblSinhVien.Columns.Add("HocBong", typeof(double));
            //Tạo khoá chính cho tblSINHVIEN
            tblSinhVien.PrimaryKey = new DataColumn[] { tblSinhVien.Columns["MaSV"] };

            //Tạo cấu trúc cho DataTable tương ứng với bảng KETQUA
            tblKetQua.Columns.Add("MaSV", typeof(string));
            tblKetQua.Columns.Add("MaMH", typeof(string));
            tblKetQua.Columns.Add("Diem", typeof(double));
            //Tạo khoá chính cho tblSINHVIEN
            tblKetQua.PrimaryKey = new DataColumn[] { tblKetQua.Columns["MaSV"], tblKetQua.Columns["MaMH"] };

            //Them cac DataTable vao DataSet, dungf tung lenh
            //ds.Tables.Add(tblKhoa);
            //ds.Tables.Add(tblSinhVien);
            //ds.Tables.Add(tblKetQua);

            //Them dong thoi nhieu Datatable vao DataSet
            ds.Tables.AddRange(new DataTable[] { tblKhoa, tblSinhVien, tblKetQua});
        }
        private void MocNoiQuanHeCacBang()
        {
            //Tao quan he giua tblKhoa vaf tblSinhVien
            ds.Relations.Add("FK_KHOA_SINHVIEN", ds.Tables["KHOA"].Columns["MaKH"], ds.Tables["SINHVIEN"].Columns["MaKH"], true);
            //Tao quan he giua tblSinhVien vaf tblKetQua
            ds.Relations.Add("FK_SINHVIEN_KETQUA", ds.Tables["SINHVIEN"].Columns["MaSV"], ds.Tables["KETQUA"].Columns["MaSV"], true);
            //Loai bo Cacase Delete trong cac quan he
            ds.Relations["FK_KHOA_SINHVIEN"].ChildKeyConstraint.DeleteRule = Rule.None;
            ds.Relations["FK_SINHVIEN_KETQUA"].ChildKeyConstraint.DeleteRule = Rule.None;
        }
        private void NhapDuLieuCacBang()
        {
            NhapLieu_tblKhoa();
            NhapLieu_tblSinhVien();
            NhapLieu_tblKetQua();
        }
        private void NhapLieu_tblKhoa()
        {
            //Nhap lieu cho tblKhoa doc due lieu tu tap tin KHOA.txt
            string[] MangKhoa = File.ReadAllLines(@"..\..\..\data\khoa.txt");
            foreach(string ChuoiKhoa in MangKhoa)
            {
                //Tach Chuoi_Khoa thanh cac thanh phan tuong ung voi MaKH va TenKH
                string[] MangThanhPhan = ChuoiKhoa.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //Tao mot dong moi co cau truc giong voi cau truc cua mot dong trong tblKhoa
                DataRow rkh = tblKhoa.NewRow();
                //Gan gia tri cho cac cot cua dong moi tao ra
                rkh[0] = MangThanhPhan[0];
                rkh[1] = MangThanhPhan[1];
                //Them dong vua tao vao tblKhoa
                tblKhoa.Rows.Add(rkh);
            }
        }
        private void NhapLieu_tblSinhVien()
        {
            //Nhap lieu cho tblSinhVien doc due lieu tu tap tin SINHVIEN.txt
            string[] MangSV = File.ReadAllLines(@"..\..\..\data\sinhvien.txt");
            foreach (string ChuoiSV in MangSV)
            {
                //Tach Chuoi_SV thanh cac thanh phan tuong ung voi cac cot trong tblSinhVien
                string[] MangThanhPhan = ChuoiSV.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                //Tao mot dong moi co cau truc giong voi cau truc cua mot dong trong tblSinhVien
                DataRow rsv = tblSinhVien.NewRow();
                //Gan gia tri cho cac cot cua dong moi tao ra
                for (int i = 0; i < MangThanhPhan.Length; i++)
                    rsv[i] = MangThanhPhan[i];
                //Them dong vua tao vao tblKhoa
                tblSinhVien.Rows.Add(rsv);
            }
        }
        private void NhapLieu_tblKetQua()
        {
            //Nhap lieu cho tblKetQua doc due lieu tu tap tin KETQUA.txt
            string[] MangKQ = File.ReadAllLines(@"..\..\..\data\ketqua.txt");
            foreach (string ChuoiKQ in MangKQ)
            {
                //Tach Chuoi_KQ thanh cac thanh phan tuong ung voi cac cot trong tblKetQua
                string[] MangThanhPhan = ChuoiKQ.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                //Tao mot dong moi co cau truc giong voi cau truc cua mot dong trong tblKetQua
                DataRow rkq = tblKetQua.NewRow();
                //Gan gia tri cho cac cot cua dong moi tao ra
                for (int i = 0; i < MangThanhPhan.Length; i++)
                    rkq[i] = MangThanhPhan[i];
                //Them dong vua tao vao tblKetQua
                tblKetQua.Rows.Add(rkq);
            }
        }
        private void KhoiTaoComboKhoa()
        {
            cboMaKhoa.DisplayMember = "TenKH";
            cboMaKhoa.ValueMember = "MaKH";
            cboMaKhoa.DataSource = tblKhoa;
        }
        private void GanDuLieu(int stt)
        {
            //lay dong du lieu thu stt trong tblsinhVien
            DataRow rsv = tblSinhVien.Rows[stt];
            txtMaSV.Text = rsv["MaSV"].ToString();
            txtHoSV.Text = rsv["HoSV"].ToString();
            txtTenSV.Text = rsv["TenSV"].ToString();
            chkPhai.Checked = (Boolean)rsv["Phai"];
            dtpNgaySinh.Text = rsv["NgaySinh"].ToString();
            txtNoiSinh.Text = rsv["NoiSinh"].ToString();
            cboMaKhoa.SelectedValue = rsv["MaKH"].ToString();
            txtHocbong.Text = rsv["HocBong"].ToString();
            //the hien so thu tu mau tin hien hanh
            lblSTT.Text = (stt + 1) + "/" + tblSinhVien.Rows.Count;
        }

        private void btnDau_Click(object sender, EventArgs e)
        {
            stt = 0;
            GanDuLieu(stt);
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            stt = tblSinhVien.Rows.Count - 1;
            GanDuLieu(stt);
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (stt == 0) return;
            stt --;
            GanDuLieu(stt);
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if(stt ==tblSinhVien.Rows.Count-1) return;
            stt++;
            GanDuLieu(stt);
        }
    }
}
