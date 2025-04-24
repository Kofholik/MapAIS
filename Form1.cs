using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsPresentation;
using GMapMarker = GMap.NET.WindowsForms.GMapMarker;

namespace Apkalikacja2
{
    public partial class Form1 : Form
    {
        private long my_mmsi;
        public Form1()
        {
            InitializeComponent();
        }

        public void odczytywanie_mmsi()
        {
            // Odczytanie MMSI z textboxa  
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Pole MMSI nie może być puste.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                my_mmsi = Convert.ToInt64(textBox1.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wprowadź poprawny numer MMSI.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void rysowanie(int mmsi, double lon, double lat)
        {
            gMapControl11.Position = new PointLatLng(lat, lon);
            gMapControl11.Zoom = 12;
            gMapControl11.MinZoom = 4;
            gMapControl11.MaxZoom = 50;
            GMapOverlay markers = new GMapOverlay("markers");
            if (mmsi == my_mmsi)
            {
                GMapMarker marker =
                new GMarkerGoogle(
                    new PointLatLng(lat, lon),
                    GMarkerGoogleType.blue_dot);
                markers.Markers.Add(marker);
            }
            else
            {
                GMapMarker marker =
                new GMarkerGoogle(
                    new PointLatLng(lat, lon),
                    GMarkerGoogleType.red_dot);
                markers.Markers.Add(marker);
            }
            gMapControl11.Overlays.Add(markers);
        }

        public void dekodowanie(string payload, string checksum)
        {
            //Dekodowanie
            string binary = "";
            foreach (char c in payload)
            {
                int i = c - 48;
                if (i > 40)
                {
                    i -= 8;
                }
                binary += Convert.ToString(i, 2).PadLeft(6, '0');
            }

            //Typ komunikatu
            int msg_type = Convert.ToInt32(binary.Substring(0, 6), 2);
            listBox2.Items.Add("Message type: " + msg_type);

            switch (msg_type)
            {
                case 1:
                    komunikat_1(binary);
                    break;
                case 3:
                    komunikat_1(binary);
                    break;
                case 4:
                    komunikat_4(binary);
                    break;
                case 5:
                    break;
                default:
                    listBox2.Items.Add("Zły typ komunikatu");
                    break;
            }
        }

        //Wpisywanie danych do listobxa
        private void komunikat_1(string binary)
        {
            int mmsi = Convert.ToInt32(binary.Substring(8, 30), 2);
            int status = Convert.ToInt32(binary.Substring(38, 2), 2);
            string[] status_desc = { "Under way using engine", "At anchor", "Not under command", "Restricted manoeuverability" };
            int rot = Convert.ToInt32(binary.Substring(42, 8), 2);
            double cog = Convert.ToDouble(Convert.ToInt64(binary.Substring(116, 12), 2)) / 10;
            double sog = Convert.ToDouble(Convert.ToInt64(binary.Substring(50, 10), 2)) / 10;
            double lon = Convert.ToDouble(Convert.ToInt64(binary.Substring(61, 28), 2)) / 600000;
            double lat = Convert.ToDouble(Convert.ToInt64(binary.Substring(89, 27), 2)) / 600000;
            int true_heading = Convert.ToInt32(binary.Substring(128, 9), 2);
            rysowanie(mmsi, lon, lat);
            listBox2.Items.Add("MMSI: " + mmsi);
            listBox2.Items.Add("Status: " + status);
            listBox2.Items.Add("Status description: " + status_desc[status]);
            listBox2.Items.Add("Lon: " + lon);
            listBox2.Items.Add("Lat: " + lat);
            listBox2.Items.Add("COG: " + cog);
            listBox2.Items.Add("SOG: " + sog);
            listBox2.Items.Add("ROT: " + rot);
            listBox2.Items.Add("True heading: " + true_heading);
        }

        private void komunikat_4(string binary)
        {
            int mmsi = Convert.ToInt32(binary.Substring(8, 30), 2);
            int rok = Convert.ToInt32(binary.Substring(38, 14), 2);
            int miesiac = Convert.ToInt32(binary.Substring(52, 4), 2);
            int dzien = Convert.ToInt32(binary.Substring(56, 5), 2);
            int godzina = Convert.ToInt32(binary.Substring(61, 5), 2);
            int minuta = Convert.ToInt32(binary.Substring(66, 6), 2);
            int secunda = Convert.ToInt32(binary.Substring(72, 6), 2);
            double lon = Convert.ToDouble(Convert.ToInt64(binary.Substring(79, 28), 2)) / 600000;
            double lat = Convert.ToDouble(Convert.ToInt64(binary.Substring(107, 27), 2)) / 600000;

            rysowanie(mmsi, lon, lat);

            listBox2.Items.Add("MMSI: " + mmsi);
            listBox2.Items.Add("Data: " + rok + "-" + miesiac.ToString("D2") + "-" + dzien.ToString("D2"));
            listBox2.Items.Add("Czas: " + godzina.ToString("D2") + ":" + minuta.ToString("D2") + ":" + secunda.ToString("D2"));
            listBox2.Items.Add("Lon: " + lon);
            listBox2.Items.Add("Lat: " + lat);
        }


        //Pobieranie danych z pliku 1
        public void Dane_Plik1()
        {
            listBox1.Items.Clear();
            using (StreamReader reader = new StreamReader("Resources//kom_sensor_ais.txt"))
            {
                string line = reader.ReadLine();
                string payload = "";
                string checksum = "";
                while (line != null)
                {
                    if (line.Contains("AIVDM"))
                    {
                        string[] words = line.Split(',');
                        if (words[1] == "1")
                        {
                            payload = words[5];
                            checksum = words[6];
                            dekodowanie(payload, checksum);
                            listBox1.Items.Add(line);
                        }
                        else
                        {
                            payload += words[5];
                            listBox1.Items.Add(line);
                        }
                    }
                    line = reader.ReadLine();
                }
            }
        }

        //Pobieranie danych z pliku 2
        public void Dane_Plik2()
        {
            listBox1.Items.Clear();
            using (StreamReader reader = new StreamReader("Resources//kom_sensor_ais2.txt"))
            {
                string line = reader.ReadLine();
                string payload = "";
                string checksum = "";
                while (line != null)
                {
                    if (line.Contains("AIVDM"))
                    {
                        string[] words = line.Split(',');
                        if (words[1] == "1")
                        {
                            payload = words[5];
                            checksum = words[6];
                            dekodowanie(payload, checksum);
                            listBox1.Items.Add(line);
                        }
                        else
                        {
                            payload += words[5];
                            listBox1.Items.Add(line);
                        }
                    }
                    line = reader.ReadLine();
                }
            }
        }
        //Przycisk do wyswietlania danych 1
        private void button1_Click_1(object sender, EventArgs e)
        {
            odczytywanie_mmsi();
            Dane_Plik1();
            gMapControl11.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
        }

        //Przycisk do wyswietlania danych 2
        private void button2_Click(object sender, EventArgs e)
        {
            odczytywanie_mmsi();
            Dane_Plik2();
            gMapControl11.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
        }




        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
