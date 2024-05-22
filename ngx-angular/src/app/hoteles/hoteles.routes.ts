import { Routes } from '@angular/router';
import { HabitacionDisponibleComponent } from './pages/habitacion-disponible/habitacion-disponible.component';
import { HabitacionOcupadoComponent } from './pages/habitacion-ocupado/habitacion-ocupado.component';
import { HabitacionLimpiezaComponent } from './pages/habitacion-limpieza/habitacion-limpieza.component';
import { PisosHotelComponent } from './pages/pisos-hotel/pisos-hotel.component';
import { CategoriasHabitacionComponent } from './pages/categorias-habitacion/categorias-habitacion.component';
import { HabitacionHotelComponent } from './pages/habitacion-hotel/habitacion-hotel.component';
import { ProcesarHabitacionComponent } from './pages/procesar-habitacion/procesar-habitacion.component';
import { VerificacionSalidaComponent } from './pages/verificacion-salida/verificacion-salida.component';

export const routes: Routes = [
  { path: "recepcion/habitaciones-disponible", component: HabitacionDisponibleComponent },
  { path: "recepcion/habitaciones-ocupado", component: HabitacionOcupadoComponent },
  { path: "recepcion/habitaciones-limpieza", component: HabitacionLimpiezaComponent },
  { path: "recepcion/procesar-habitacion/:habitacionId", component: ProcesarHabitacionComponent },
  { path: "recepcion/verificacion-salida/:estadiaId", component: VerificacionSalidaComponent },
  { path: "pisos-hotel", component: PisosHotelComponent },
  { path: "categorias-habitacion", component: CategoriasHabitacionComponent },
  { path: "habitacion-hotel", component: HabitacionHotelComponent },
];
