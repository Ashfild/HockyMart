using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace HockyMart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-GM3N8DP8\FADHIL;Initial Catalog=NewDB;Integrated Security=True");

        public void clearData()
        {
            nama_txt.Clear();
            harga_txt.Clear();
            jumlah_txt.Clear();
            jenis_txt.Clear();
            search_txt.Clear();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("Select * from HockyTable", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }

        private void ClearBtn_Click_1(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        public bool isValid()
        {
            if (nama_txt.Text == String.Empty)
            {
                MessageBox.Show("Nama barang diperlukan", "Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (harga_txt.Text == String.Empty)
            {
                MessageBox.Show("Harga barang diperlukan", "Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (jumlah_txt.Text == String.Empty)
            {
                MessageBox.Show("Jumlah barang diperlukan", "Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (jenis_txt.Text == String.Empty)
            {
                MessageBox.Show("Jenis barang diperlukan", "Gagal", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO HockyTable VALUES (@Nama, @Harga, @Jumlah, @Jenis)", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Nama", nama_txt.Text);
                    cmd.Parameters.AddWithValue("@Harga", harga_txt.Text);
                    cmd.Parameters.AddWithValue("@Jumlah", jumlah_txt.Text);
                    cmd.Parameters.AddWithValue("@Jenis", jenis_txt.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    MessageBox.Show("Berhasil mendaftarkan", "Simpan", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete from HockyTable where ID = " + search_txt.Text+ "", conn);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Barang telah dihapus", "Dihapus", MessageBoxButton.OK, MessageBoxImage.Information);
                conn.Close();
                clearData();
                LoadGrid();
                conn.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Tidak Terhapus" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Update HockyTable set Nama = '"+nama_txt.Text+ "', Harga = '" +harga_txt.Text+ "', Jumlah = '" +jumlah_txt.Text+ "', Jenis = '" +jenis_txt.Text+ "' WHERE ID ='"+search_txt.Text+"'",conn);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Barang telah diperbarui", "Diperbarui", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }finally
            {
                conn.Close();
                clearData();
                LoadGrid();
            }
        }
    }
}
