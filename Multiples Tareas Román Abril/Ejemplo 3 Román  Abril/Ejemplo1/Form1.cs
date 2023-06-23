using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Ejemplo1
{
    public partial class Form1 : Form
    {
        private string apiURL;
        private HttpClient httpclient;
        public Form1()
        {
            InitializeComponent();
            apiURL = "https://localhost:7004";
            httpclient = new HttpClient();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            
            gif.Visible = true;
            var tarjetas = ObtenerTarjetasDeCredito(5);
            var stopwatch=new Stopwatch();
            stopwatch.Start();

            try
            {
                await ProcesarTarjetas(tarjetas);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show($"Operación finalizada en {stopwatch.ElapsedMilliseconds / 1000.0} segundos");
            gif.Visible = false;
        }
        private async Task Esperar()
        { 
            await Task.Delay(5000);
        }
        private async Task <string> DevolverSaludo(string nombre)
        {
            using (var respuesta=await httpclient.GetAsync($"{apiURL}/saludos/{nombre}"))
            {
                respuesta.EnsureSuccessStatusCode();
                var saludo = await respuesta.Content.ReadAsStringAsync();
                return saludo;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private List<string> ObtenerTarjetasDeCredito(int cantidadDeTarjetas)
        {
            var tarjetas = new List<string>();
            for (int i=0; i< cantidadDeTarjetas; i++)
            {
                tarjetas.Add(i.ToString().PadLeft(16,'0'));
            }
        }
        private async Task ProcesarTarjetas(List<string> tarjetas)
        {
            var tareas = new List<Task>();
            foreach(var tarjeta in tarjetas)
            {
                var json= JsonConvert.SerializeObject(tarjeta);
                var content=new StringContent(json, encoding: Encoding.UTF8, "application/json");
                var respuestaTask = httpclient.PostAsync($"{apiURL}/tarjetas", content);
                tarjetas.Add(respuestaTask);
            }
            await Task.WhenAll(tareas);
        }
    }
}
