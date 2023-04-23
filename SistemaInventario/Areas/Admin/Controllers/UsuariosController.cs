using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repository.IRepository;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IUnitWork _unitWork;
        private readonly ApplicationDbContext _db;

        public UsuariosController(IUnitWork unitWork, ApplicationDbContext db)
        {
            _unitWork = unitWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await _unitWork.UsuarioAplicacion.ObtenerTodos();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
            }

            return Json(new { data = usuarioLista });
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _unitWork.UsuarioAplicacion.ObtenerPrimero(u=>u.Id == id);
            if (usuario==null)
            {
                return Json(new { success = false, message = "Error de ususario" });
            }
            if(usuario.LockoutEnd!=null && usuario.LockoutEnd > DateTime.Now)
            {
                //Usuario Bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _unitWork.Guardar();
            return Json(new { success = true, message = "Operacion exitosa" });

        }

        #endregion
    }
}
