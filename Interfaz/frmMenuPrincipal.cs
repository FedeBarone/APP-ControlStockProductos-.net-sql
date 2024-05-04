using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interfaz
{
    public partial class frmMenuPrincipal : Form
    {
        #region Atributos
        private bool expandirBarraLateral;
        private List<Articulo> listaArticulos;
        #endregion

        #region Constructores
        public frmMenuPrincipal()
        {
            InitializeComponent();

            expandirBarraLateral = true;
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Actualiza el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            cargarArticulo();
            cboCampo.Items.Add("A.Codigo");
            cboCampo.Items.Add("A.Nombre");
            cboCampo.Items.Add("A.Descripcion");

        }

        /// <summary>
        /// Carga los articulos en el formulario
        /// </summary>
        public void cargarArticulo()
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                listaArticulos = articuloNegocio.listarArticulos();
                dgvArticulos.DataSource = listaArticulos;
                ocultarColumnas();
                cargarImagen(listaArticulos[0].ImagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Carga la imagen del articulo en el campo imagenUrl
        /// </summary>
        /// <param name="imagen"></param>
        public void cargarImagen(string imagen)
        {
            try
            {
                pbxImagenArticulos.Load(imagen);


            }
            catch (Exception ex)
            {

                pbxImagenArticulos.Load("https://static.wikia.nocookie.net/saintseiya/images/0/0e/Mime_de_benetnasch_eta.jpg/revision/latest/thumbnail/width/360/height/360?cb=20181209234225&path-prefix=es");
            }
        }

        /// <summary>
        /// Permite que cuando se selecciona un articulo especifico, cambie la imagen al respectivo articulo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo articuloSeleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(articuloSeleccionado.ImagenUrl);

            }


        }

        /// <summary>
        /// Oculta las columnas que se seleccionen
        /// </summary>
        public void ocultarColumnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
        }

        /// <summary>
        /// Agrega un articulo a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            frmAlta frmAlta = new frmAlta();
            frmAlta.ShowDialog();
            cargarArticulo();
        }

        /// <summary>
        /// Modifica un articulo de la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModificarArticulo_Click(object sender, EventArgs e)
        {
            Articulo articuloAModificar;
            if (dgvArticulos.CurrentRow != null)
            {
                articuloAModificar = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmAlta modificarArticulo = new frmAlta(articuloAModificar, false);
                modificarArticulo.ShowDialog();
                cargarArticulo();
            }
            else
            {
                MessageBox.Show("No se encontro articulo para modificar");
            }

        }

        /// <summary>
        /// Elimina un articulo de la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            Articulo articuloAEliminar;

            try
            {
                DialogResult respuesta = MessageBox.Show("Esta seguro de eliminar el articulo?", 
                                                         "Articulo Eliminado", 
                                                         MessageBoxButtons.YesNo, 
                                                         MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    if (dgvArticulos.CurrentRow != null)
                    {
                        articuloAEliminar = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                        articuloNegocio.eliminarArticulo(articuloAEliminar.Id);
                        cargarArticulo();
                    }
                    else
                    {
                        MessageBox.Show("No hay disponinle para eliminar");
                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }        

        }

        private bool validarFiltro()
        {
            bool bandera = false;
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar.");
                bandera = true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar.");
                bandera = true;
            }
            //if (cboCampo.SelectedItem.ToString() == "Número")
            //{
            //    if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
            //    {
            //        MessageBox.Show("Debes cargar el filtro para numéricos...");
            //        return true;
            //    }
            //    if (!(Validador.soloNumeros(txtFiltroAvanzado.Text)))
            //    {
            //        MessageBox.Show("Solo nros para filtrar por un campo numérico...");
            //        return true;
            //    }

            //}

            return bandera;
        }

        /// <summary>
        /// Filtra con distintos criterios articulos de la base de datos 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscarArticulo_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = articuloNegocio.filtrarArticulos(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Filtra de manera rapida articulos de la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada = listaArticulos;
            string filtro = txtFiltroRapido.Text;


            if (filtro.Length >= 3)
                listaFiltrada = listaArticulos.FindAll(x => x.Nombre.ToUpper().
                                Contains(filtro.ToUpper()));


            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "A.Codigo")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnVerDetalleArticulo_Click(object sender, EventArgs e)
        {
            Articulo detalleArticulo;
            if (dgvArticulos.CurrentRow != null)
            {
                detalleArticulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                frmAlta verDetalleArticulo = new frmAlta(detalleArticulo, true);
                verDetalleArticulo.ShowDialog();
            }
            else
            {
                MessageBox.Show("No hay articulo para detallar");
            }

        }

        //MENU PRINCIPAL EXPANSION
        private void tmrFlpExpandir_Tick(object sender, EventArgs e)
        {
            if (expandirBarraLateral)
            {
                flpExpandirMenuPrincipal.Width -= 8;
                if (flpExpandirMenuPrincipal.Width <= 40)
                {
                    expandirBarraLateral = false;
                    tmrFlpExpandir.Stop();

                }

            }
            else
            {
                flpExpandirMenuPrincipal.Width += 8;
                if (flpExpandirMenuPrincipal.Width >= 250)
                {
                    expandirBarraLateral = true;
                    tmrFlpExpandir.Stop();
                }
            }

        }

        private void pbxExpandirMenuPrincipal_Click(object sender, EventArgs e)
        {
            tmrFlpExpandir.Start();

        }

        private void pbxSalirMenuPrincipal_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
     
    }
}
