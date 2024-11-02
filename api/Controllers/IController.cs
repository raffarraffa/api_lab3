namespace api.Controllers;

public interface IController<T>
{
    ActionResult<T> Obtener(int? id); // captura el id del claims o llega pro param (propeiario, inmueble)
    ActionResult<T> Crear(T entity);
    ActionResult<List<T>> ObtenerTodos(); // where desde claims

    ActionResult<List<T>> ObtenerActivos();
    ActionResult<T> Actualizar(T entity);
    ActionResult<bool> EliminadoLogico(int id);
    ActionResult GuardarFile(IFormFile file);

}