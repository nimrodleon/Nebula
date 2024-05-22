import Swal from "sweetalert2";

// confirmar tarea a realizar.
export const confirmTask = async () => {
  return Swal.fire({
    title: "¿Estás seguro?",
    text: "¡No podrás revertir esto!",
    icon: "question",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Sí, Aceptar",
    cancelButtonText: "Cancelar"
  });
};

// confirmar borrar item.
export const deleteConfirm = async () => {
  return Swal.fire({
    title: `¿Estás seguro de borrar?`,
    text: "¡No podrás revertir esto!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Sí, bórralo!",
    cancelButtonText: "Cancelar"
  });
};

// confirmar salir de la operación en curso.
export const confirmExit = async () => {
  return await Swal.fire({
    title: "¿Estás seguro?",
    text: "Si sales, perderás los cambios no guardados.",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Sí, salir",
    cancelButtonText: "Cancelar"
  });
};

// acceso denegado.
export const accessDenied = async () => {
  return Swal.fire(
    "Oops...",
    "Acceso denegado!",
    "error"
  );
};

// mensaje caja diaria.
export const errorAlBorrarCajaDiaria = async (num: number) => {
  const msg = num > 1 ? "registros asociados" : "registro asociado";
  return Swal.fire(
    "Oops...",
    `Hay ${num} ${msg} a este documento!`,
    "error"
  );
};

// mensaje de error al borrar.
export const deleteError = async () => {
  return Swal.fire(
    "Oops...",
    "Error, No se puede eliminar el registro!",
    "error"
  );
};
