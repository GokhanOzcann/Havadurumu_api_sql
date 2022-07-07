using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace son
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        SqlDataAdapter da;

        DataSet ds;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string api = "35e94a0fea42d6ba4a7c413a392728e4";
            
            string connection = "http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=35e94a0fea42d6ba4a7c413a392728e4";

            XDocument weather = XDocument.Load(connection);
            var temp = weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            var pre = weather.Descendants("pressure").ElementAt(0).Attribute("value").Value;
            var gunes = weather.Descendants("sun").ElementAt(0).Attribute("rise").Value;
            var hiz = weather.Descendants("speed").ElementAt(0).Attribute("value").Value;
            var sehir = weather.Descendants("city").ElementAt(0).Attribute("name").Value;
            cmd = new SqlCommand();
            con.Open();
            MessageBox.Show("Baþarýyla eklendi.");
            cmd.Connection = con;
            cmd.CommandText = "insert into durum(sýcaklýk,basýnç,gunes,hýz,sehir) values (" + temp + ",'" + pre + "','" + gunes+ "','" + hiz + "','"+ sehir+"')";
            cmd.ExecuteNonQuery();
            con.Close();
            giris();


            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            giris();    
        }
        void giris()
        {
            //sql baðlantýsýný kurdum.
            con = new SqlConnection("server=localhost; Database=havadurumu; Integrated Security=true;");
            // tablomu seçtim.
            da = new SqlDataAdapter("Select * From durum", con);
            ds = new DataSet();
            con.Open();

            da.Fill(ds, "durum");
            //datagridview adý neyse onu yazdým.

            dataGridView1.DataSource = ds.Tables["durum"];
            con.Close();
        }
    }
}