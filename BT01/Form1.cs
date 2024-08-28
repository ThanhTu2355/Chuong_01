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

            //Them cac DataTable vao DataSet, dung tung lenh
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
            //Tinh tong diem
            txtTongDiem.Text = TongDiem(txtMaSV.Text).ToString();
        }
        private double TongDiem(string msv)
        {
            double kq = 0;
            Object td = tblKetQua.Compute("sum(Diem)", "MaSV='" + msv + "'");
            //Luu y Truong hop SV khong co diem thi phuong thuc tra ve gia tri DBNull
            if (td == DBNull.Value)
                kq = 0;
            else
                kq = Convert.ToDouble(td);
            return kq;
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaSV.ReadOnly = false;
            foreach (Control ctl in Controls)
                if (ctl is TextBox)
                    (ctl as TextBox).Clear();
                else if (ctl is CheckBox)
                    (ctl as CheckBox).Checked = true;
                else if (ctl is ComboBox)
                    (ctl as ComboBox).SelectedIndex = 0;
                else if (ctl is DateTimePicker)
                    (ctl as DateTimePicker).Value = new DateTime(2006,1,1);
            txtMaSV.Focus();
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            txtMaSV.ReadOnly = true;
            GanDuLieu(stt);
        }

        private void btnGhi_Click(object sender, EventArgs e)
        {
            if(txtMaSV.ReadOnly==true)//ghi khi sua
            {
                //Xac dinhj dong can sua
                DataRow rsv = tblSinhVien.Rows.Find(txtMaSV.Text);
                //Tien hanh sua
                rsv["HoSV"] = txtHoSV.Text;
                rsv["TenSV"] = txtTenSV.Text;
                rsv["Phai"] = chkPhai.Checked;
                rsv["NgaySinh"] = dtpNgaySinh.Text;
                rsv["NoiSinh"] = txtNoiSinh.Text;
                rsv["MaKH"] = cboMaKhoa.SelectedValue.ToString();
                rsv["HocBong"] = txtHocbong.Text;
            }
            else//ghi sau khi them
            {
                //Kiem tra khoa chinh co bi trung khong
                DataRow rsv = tblSinhVien.Rows.Find(txtMaSV.Text);
                if(rsv!=null)//Da co SV mang MaSV nay
                {
                    MessageBox.Show("MaSV nay bi trung, moi nhap MaSV khac");
                    txtMaSV.Focus();
                    return;
                }
                rsv = tblSinhVien.NewRow();
                rsv["MaSV"] = txtMaSV.Text;
                rsv["HoSV"] = txtHoSV.Text;
                rsv["TenSV"] = txtTenSV.Text;
                rsv["Phai"] = chkPhai.Checked;
                rsv["NgaySinh"] = dtpNgaySinh.Text;
                rsv["NoiSinh"] = txtNoiSinh.Text;
                rsv["MaKH"] = cboMaKhoa.SelectedValue.ToString();
                rsv["HocBong"] = txtHocbong.Text;
                tblSinhVien.Rows.Add(rsv);
                txtMaSV.ReadOnly = true;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            //Xac dinh dong can huy => su dung ham find
            DataRow rsv = tblSinhVien.Rows.Find(txtMaSV.Text);
            //can kiem tra neu rsv ton tai nhung dong lien quan trong tblKetQua => khong cho xoa. Nguoc lai thi cho xoa
            //Su dung ham GetChilRow de kiem tra nhung dong lien quan co ton tai khong. Gia tri tra ve cua ham la 1 mang
            DataRow[] mangDongLienQuan = rsv.GetChildRows("FK_SINHVIEN_KETQUA");
            if (mangDongLienQuan.Length > 0)//co ton tai nhung dong lien quan trong tblKetQua
                MessageBox.Show("Khong xoa SV duoc vi da co ket qua thi");
            else
            {
                DialogResult tl;
                tl = MessageBox.Show("Xoa sinh vien nay khong", "Can than", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (tl == DialogResult.Yes)
                {
                    rsv.Delete();
                    btnDau.PerformClick();
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Ghi tap tin
            //Luu y: tblSinhVien.Rows => Tap hop dong chu khong phai mang
            //Chuyen thanh mang => ItemArry
            //Chuyen mot mang thanh chuoi => Join
            //Thuat toan ghi mot Database vao tep tin
            //1. Khai bao mot mang chuoi voi moi phan tu tuong ung voi mot dong trong Database
            //2. Duyet qua tap hop Rows cua DataTable va dua tung dong vao mang chuoi voi ham join
            //3. Su dung phuong thuc WriteAllLine de ghi mang chuoi vao tap tin SINHVIEn.TXT
            List<string> mangChuoiSinhVien = new List<string>();
            foreach(DataRow rsv in tblSinhVien.Rows)
            {
                //Bien mang thanh chuoi
                string chuoiDongSinhVien = string.Join("|", rsv.ItemArray);
                //Them chuoi tren vao mangChuoiSinhVien
                mangChuoiSinhVien.Add(chuoiDongSinhVien);
            }
            //Ghi mangChuoiSinhVien vao tep tin
            File.WriteAllLines(@"..\..\..\data\sinhvien.txt",mangChuoiSinhVien);
        }
    }
}
