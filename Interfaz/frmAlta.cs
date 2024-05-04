using Dominio;
using Negocio;
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
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Interfaz
{
    public partial class frmAlta : Form
    {
        #region Atributos
        private Articulo articulo = null;
        private OpenFileDialog archivoImagen = null;
        private ErrorProvider errorProvider = null;
        #endregion

        #region Constructor
        public frmAlta()
        {
            InitializeComponent();
            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        public frmAlta(Articulo articulo, bool banderaMenuDetalleArticulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            lblAgregarProducto.Text = "Modificar Articulo";
            mostrarMenuDetalleArticulo(banderaMenuDetalleArticulo);
            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        }

        #endregion

       
        #region Metodos
        /// <summary>
        /// Carga el formulario de alta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAlta_Load(object sender, EventArgs e)
        {
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            try
            {
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboMarca.DataSource = marcaNegocio.listarMarcas();
                cboCategoria.DataSource = categoriaNegocio.listarCategorias();

                if (this.articulo != null)
                {               
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;                    
                    cboMarca.SelectedValue = articulo.Marca.Id;                   
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Muestra el form del detalle de un articulo
        /// </summary>
        /// <param name="banderaMenuDetalleArticulo"></param>
        private void mostrarMenuDetalleArticulo(bool banderaMenuDetalleArticulo)
        {
            if (banderaMenuDetalleArticulo)
            {
                btnAceptar.Enabled = false;
                btnAceptar.Visible = false;
                txtCodigo.ReadOnly = true;
                txtNombre.ReadOnly = true;
                txtDescripcion.ReadOnly = true;
                cboMarca.Enabled = false;
                cboCategoria.Enabled = false;
                txtImagenUrl.Enabled = false;
                txtImagenUrl.Visible = false;
                txtPrecio.ReadOnly = true;
                btnCancelar.Text = "Atras";
                lblImagenUrl.Enabled = false;
                lblImagenUrl.Visible = false;
                lblAgregarProducto.Text = "Detalle Producto";
                btnAgregarImagen.Enabled = false;
                btnAgregarImagen.Visible = false;
            }

        }

        /// <summary>
        /// Valida las entradas del usuario en los text box del articulo
        /// </summary>
        /// <returns></returns>
        public bool validarAgregarArticulo()
        {
            bool bandera = false;
            List<TextBox> lista = new List<TextBox>()
            {
                txtCodigo,
                txtNombre,
                txtDescripcion,
                txtPrecio
            };
            
            foreach (TextBox txt in lista)
            {

                if (Validador.CadenaEsInvalida(txt.Text))
                {
                    bandera = true;
                    errorProvider.SetError(txt, "Ningun campo puede quedar vacio, excepto la imagen url");
                }
            }

            if (txtCodigo.Text.Length != 3 || !Regex.IsMatch(txtCodigo.Text, "^[A-Za-z]{1}[0-9]{2}$"))
            {
                bandera = true;

            }

            if (txtNombre.Text.Length < 3 || !Regex.IsMatch(txtNombre.Text, "^[A-Za-z]{2}[A-Za-z\\s\\S]*$"))
            {
                bandera = true;

            }

            if (txtDescripcion.Text.Length < 3 || !Regex.IsMatch(txtDescripcion.Text, "^[A-Za-z]{3}[A-Za-z\\s\\S]*$"))
            {
                bandera = true;

            }

            if (!(decimal.TryParse(txtPrecio.Text, out decimal precio) && precio > 0))
            {
                bandera = true;
            }
            
            return bandera;

        }

        /// <summary>
        /// Cambia de error dependiendo si se eliminan o agregan caracteres-condiciones en el text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text.Length == 3 && Regex.IsMatch(txtCodigo.Text, "^[A-Za-z]{1}[0-9]{2}$"))
            {
                errorProvider.SetError(txtCodigo, null);

            }
            else
            {
                errorProvider.SetError(txtCodigo, "El codigo solo puede tener tres caracteres: " +
                                                  "una letra y dos numeros");
            }

        }

        /// <summary>
        /// Cambia de error dependiendo si se eliminan o agregan caracteres-condiciones en el text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (txtNombre.Text.Length >= 3 && Regex.IsMatch(txtNombre.Text, "^[A-Za-z]{2}[A-Za-z\\s\\S]*$"))
            {
                errorProvider.SetError(txtNombre, null);
            }
            else
            {
                errorProvider.SetError(txtNombre, "El nombre debe tener tres caracteres o mas: " +
                                                  "los primeros dos deben ser letras");
            }
        }

        /// <summary>
        /// Cambia de error dependiendo si se eliminan o agregan caracteres-condiciones en el text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            if (txtDescripcion.Text.Length >= 3 && Regex.IsMatch(txtDescripcion.Text, "^[A-Za-z]{3}[A-Za-z\\s\\S]*$"))
            {
                errorProvider.SetError(txtDescripcion, null);
            }
            else
            {
                errorProvider.SetError(txtDescripcion, "La descripcion debe tener tres caracteres o mas: " +
                                                       "los primeros tres deben ser letras");
            }

        }

        /// <summary>
        /// Cambia de error dependiendo si se eliminan o agregan caracteres-condiciones en el text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPrecio.Text, out decimal precio) && precio > 0)
            {
                errorProvider.SetError(txtPrecio, null);
            }
            else
            {
                errorProvider.SetError(txtPrecio, "El precio solo puede contener numeros y " +
                                                  "deben ser mayores a cero");
            }

        }

        /// <summary>
        /// Boton que agrega permite agregar o modificar un articulo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            try
            {
                if(articulo == null)
                    articulo = new Articulo();

                if (validarAgregarArticulo())
                    return;

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;                            
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (archivoImagen != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                {
                    if(!(File.Exists(ConfigurationManager.AppSettings["images-folder"] + archivoImagen.SafeFileName)))
                        File.Copy(archivoImagen.FileName, ConfigurationManager.AppSettings["images-folder"] + archivoImagen.SafeFileName);
                    articulo.ImagenUrl = ConfigurationManager.AppSettings["images-folder"] + archivoImagen.SafeFileName;
                }
                else
                {
                    articulo.ImagenUrl = txtImagenUrl.Text;
                }

                if (articulo.Id == 0 )
                {
                    articuloNegocio.agregarArticulo(articulo);
                    MessageBox.Show("Articulo agregado correctamente");

                }
                else
                {
                    articuloNegocio.modificarArticulo(articulo);
                    MessageBox.Show("Articulo modificado correctamente");
                }

                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Cierra el formulario de alta al presionar el boton cancelar y vuelve al menu principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Carga una imagen en imagenUrl
        /// </summary>
        /// <param name="imagen"></param>
        public void cargarImagen(string imagen)
        {
            try
            {
                pbxAlta.Load(imagen);


            }
            catch (Exception ex)
            {

                pbxAlta.Load("https://static.wikia.nocookie.net/saintseiya/images/0/0e/Mime_de_benetnasch_eta.jpg/revision/latest/thumbnail/width/360/height/360?cb=20181209234225&path-prefix=es");
            }
        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivoImagen = new OpenFileDialog();
            archivoImagen.Filter = "jpg|*.jpg;|png|*.png";
            if (archivoImagen.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivoImagen.FileName;
                cargarImagen(archivoImagen.FileName);

            }
        }

        #endregion

    }
}
