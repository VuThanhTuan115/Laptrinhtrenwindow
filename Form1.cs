using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            lblQLySV.Text = "Sẵn sàng. Hãy chạy Migration trước khi dùng (xem README.md).";
            await TaiDanhSach();
        }
        private async Task TaiDanhSach()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    List<Student> danhSach = await context.Students.ToListAsync();
                    dgvSinhVien.DataSource = null;
                    dgvSinhVien.DataSource = danhSach;
                    lblQLySV.Text = $"Đã tải {danhSach.Count} sinh viên.";
                    lblQLySV.ForeColor = System.Drawing.Color.DarkGreen;
                }
            }
            catch (Exception ex)
            {
                lblQLySV.Text = "Lỗi: " + ex.Message;
                lblQLySV.ForeColor = System.Drawing.Color.Red;
            }
        }
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtHoTen.Text = row.Cells["FullName"].Value.ToString();
                txtDiem.Text = row.Cells["Grade"].Value.ToString();
            }
        }
        private async void btnTaiLai_Click(object sender, EventArgs e)
        {
            await TaiDanhSach();
        }
        private void dgvSinhVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow?.DataBoundItem is Student sv)
            {
                txtHoTen.Text = sv.FullName;
                txtDiem.Text = sv.Grade.ToString();
            }
        }
        // Thêm sinh viên mới
        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }
            if (!double.TryParse(txtDiem.Text, out double diem))
            {
                MessageBox.Show("Điểm không hợp lệ!");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Students.Add(new Student { FullName = txtHoTen.Text, Grade = diem });
                    await context.SaveChangesAsync();
                }
                lblQLySV.Text = "Đã thêm sinh viên mới!";
                lblQLySV.ForeColor = System.Drawing.Color.DarkGreen;
                txtHoTen.Clear();
                txtDiem.Clear();
                await TaiDanhSach();
            }
            catch (Exception ex)
            {
                lblQLySV.Text = "Lỗi: " + ex.Message;
                lblQLySV.ForeColor = System.Drawing.Color.Red;
            }
        }
        // Sửa thông tin sinh viên
        private async void bttSua_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow?.DataBoundItem is not Student svDangChon)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }
            if (!double.TryParse(txtDiem.Text, out double diemMoi))
            {
                MessageBox.Show("Điểm không hợp lệ!");
                return;
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    Student sv = await context.Students.FindAsync(svDangChon.ID);
                    if (sv != null)
                    {
                        sv.FullName = txtHoTen.Text;
                        sv.Grade = diemMoi;

                        await context.SaveChangesAsync();
                    }
                }
                lblQLySV.Text = "Đã cập nhật thông tin!";
                lblQLySV.ForeColor = System.Drawing.Color.DarkGreen;
                await TaiDanhSach();
            }
            catch (Exception ex)
            {
                lblQLySV.Text = "Lỗi: " + ex.Message;
                lblQLySV.ForeColor = System.Drawing.Color.Red;
            }
        }
        // Xóa sinh viên
        private async void bttXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.CurrentRow?.DataBoundItem is not Student svDangChon)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!");
                return;
            }
            DialogResult ketQua = MessageBox.Show(
            $"Bạn có chắc muốn xóa \"{svDangChon.FullName}\"?",
            "Xác nhận xóa",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );
            if (ketQua != DialogResult.Yes) return;
            try
            {
                using (var context = new AppDbContext())
                {
                    Student sv = await context.Students.FindAsync(svDangChon.ID);
                    if (sv != null)
                    {
                        context.Students.Remove(sv);
                        await context.SaveChangesAsync();
                    }
                }
                lblQLySV.Text = "Đã xóa sinh viên!";
                lblQLySV.ForeColor = System.Drawing.Color.DarkGreen;
                txtHoTen.Clear();
                txtDiem.Clear();
                await TaiDanhSach();
            }
            catch (Exception ex)
            {
                lblQLySV.Text = "Lỗi: " + ex.Message;
                lblQLySV.ForeColor = System.Drawing.Color.Red;
            }
        }
        private void bttTai_Click(object sender, EventArgs e)
        {
           
        }
        // Lọc sinh viên có điểm >= 5 và sắp xếp theo điểm giảm dần
        private async void bttLoc_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    List<Student> ketQua = await context.Students
                    .Where(sv => sv.Grade >= 5)
                    .OrderByDescending(sv => sv.Grade)
                    .ToListAsync();
                    dgvSinhVien.DataSource = null;
                    dgvSinhVien.DataSource = ketQua;
                    lblQLySV.Text = $"Tìm thấy {ketQua.Count} sinh viên đạt (>=5).";
                    lblQLySV.ForeColor = System.Drawing.Color.DarkGreen;
                }
            }
            catch (Exception ex)
            {
                lblQLySV.Text = "Lỗi: " + ex.Message;
                lblQLySV.ForeColor = System.Drawing.Color.Red;
            }
        }
        
        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDiem_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
