namespace api.Interfaces;


public interface IController<T>
{
    ActionResult<IEnumerable<T>> GetAll();
    ActionResult<T> GetById(int id);
    ActionResult<T> Post(T entity);
    IActionResult Put(int id, T entity);
    IActionResult Delete(int id);
    //ActionResult<IEnumerable<T>> GetByAtributo(string atributo, object valor);
}

