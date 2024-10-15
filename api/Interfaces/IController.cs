namespace api.Interfaces;


public interface IController<T>
{
    ActionResult<IEnumerable<T>> GetAll(); //GET    
    // ActionResult<T> GetByAtribbute(string atribb, string valor); //GET    
    // ActionResult<T> Create(T entity); // POST
    // IActionResult Update(int id, T entity);
    // IActionResult PartialUpdate(int id, T entity);
    // IActionResult Delete(int id);
    //ActionResult<IEnumerable<T>> GetByAtributo(string atributo, object valor);
}

