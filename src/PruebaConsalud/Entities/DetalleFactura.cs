namespace PruebaConsalud.Entities;

public class Detallefactura
{
    public int Id { get; set; }
    public float CantidadProducto { get; set; }
    public Producto Producto { get; set; }
    public float TotalProducto { get; set; }
}